using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
[RequireComponent(typeof(Status))]
[RequireComponent(typeof(Rigidbody2D))]


public class ObjectController : MonoBehaviour
{
    // private
    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField] private float JumpForce = 400f;

    [Range(0, .3f)] [SerializeField] private float MovementSmoothing = .05f;


    [SerializeField] private Transform GroundCheck;
    [SerializeField] private LayerMask WhoIsEnemy;

    // public
    public Status stat;

    [SerializeField] public LayerMask WhatIsGround;

    public bool isGround;
    // public use


    public Vector2 direction = Vector2.zero;

    public bool Movable = true;
    public bool Jumpable = true;

    private int mylayer;
    Coroutine invincibility_Coroutine;
    [SerializeField] Image invincible_Image;
    public float invincibleTime = 0;

    public bool die = false;

    // 경사로 처리

    public bool isSlope = false;
    Vector2 perp = Vector2.zero;




    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;
    public UnityEvent OnDieEvent;
    public UnityEvent OnHurtEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
        if (OnDieEvent == null)
            OnDieEvent = new UnityEvent();
        if (OnHurtEvent == null)
            OnHurtEvent = new UnityEvent();
        mylayer = gameObject.layer;
    }

    private void FixedUpdate()
    {
        if (!GroundCheck) return;
        FallSpeedCheck();
        SlopCheck();
        JumpCheck();

        //Debug.Log(isGround);
    }

    // ============================= Checking(Update) =================================
    private void JumpCheck()
    {
        bool wasGrounded = isGround;
        isGround = false;


        Collider2D collider = Physics2D.OverlapBox(GroundCheck.position, GroundCheck.localScale, 0, WhatIsGround);

        if (isSlope)
        {
            if (collider)
            {
                isGround = true;
                if (!wasGrounded)
                {
                    anim.SetBool("Jump", false);
                    OnLandEvent.Invoke();
                }
            }
            else
            {
                isGround = false;
                if (!wasGrounded)
                    anim.SetBool("Jump", true);
            }
        }
        else
        {
            if (collider && rb.velocity.y <= 0.5f)
            {
                isGround = true;
                if (!wasGrounded)
                {
                    anim.SetBool("Jump", false);
                    OnLandEvent.Invoke();
                }
            }
            else
            {
                if (!wasGrounded)
                    anim.SetBool("Jump", true);
            }
        }

    }
    private void FallSpeedCheck()
    {
        if(rb.velocity.y < -12f)
            rb.velocity = new Vector2(rb.velocity.x, -12f);
    }
    
    private void SlopCheck()
    {
        Vector3 pos = transform.position;
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider)
        {
            pos += new Vector3(boxCollider.offset.x * transform.localScale.y, boxCollider.offset.y * transform.localScale.y);
            pos += new Vector3(boxCollider.size.x * 0.5f * direction.x * transform.localScale.y, 0);
        }
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, 1f, WhatIsGround);
        Vector2 n = Vector2.Perpendicular(hit.normal);
        float angle = Vector2.Angle(hit.normal, Vector2.up);



        if (angle == 0)
        {
            pos = transform.position;
            pos -= new Vector3(0.4f * direction.x, 0);
            hit = Physics2D.Raycast(pos, Vector2.down, 1f, WhatIsGround);
            angle = Vector2.Angle(hit.normal, Vector2.up);
            n = Vector2.Perpendicular(hit.normal);
            if (angle == 0)
                isSlope = false;
            
        }
        else
        {
            isSlope = true;
            perp = n;
        }

        //Debug.Log("a" + hit.point);
        //Debug.Log("b" + hit.normal);
        Debug.DrawLine(hit.point, hit.point + hit.normal, Color.red);
        Debug.DrawLine(hit.point, hit.point + n, Color.green);
    }
    /// =========================================================================================================================
    public void Move(float direction, float speed, bool jump)
    {
        //Debug.Log(direction + "/" + speed + "/");
        
        if (isGround)
        {
            OnLandEvent.Invoke();
        }
        if (direction == 0)
            rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        else
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        Vector3 targetVelocity = new Vector2(direction * speed, rb.velocity.y);
        if (isSlope)
        {
            float targetx;
            float targety = (perp.y / 4f) * -direction * rb.gravityScale * (speed + 1);
            if (targety > 0)
                targetx = -perp.x * direction * speed * rb.gravityScale * 0.5f; // speed = 2.5f
            else
                targetx = direction * (0.1f * speed) * rb.gravityScale;
            targetVelocity = new Vector2(targetx, targety);
;       }
        if (direction != 0)
        {
            this.direction = new Vector3(direction, 0);
            anim.SetBool("Move", true);
            if (speed >= 4 && isGround)
                anim.SetBool("Run", true);
            else
                anim.SetBool("Run", false);
        }
        else
        {
            anim.SetBool("Move", false);
            anim.SetBool("Run", false);
        }

        Vector2 m_Velocity = Vector2.zero;
        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, MovementSmoothing);

        if (isGround && isSlope && targetVelocity.y > 0)
        {
            rb.MovePosition(rb.position + new Vector2(targetVelocity.x, targetVelocity.y) * Time.fixedDeltaTime);
            rb.velocity = new Vector2(rb.velocity.x, 0);
            isGround = false;
        }
        
        if(direction != 0)
            SetDirection(direction);
        
        if (isGround && jump && Jumpable)
        {
            anim.SetBool("Jump", true);
            anim.SetTrigger("Jumpt");
            isGround = false;
            rb.velocity = new Vector2(rb.velocity.x, 0);

            float jumpforce = JumpForce;
            if (speed >= 4) jumpforce += 50f;
            if (isSlope) jumpforce *= 1.2f;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0f, jumpforce));
        }
    }
    public void Attack(Vector3 attackSize, Vector3 attackOffset, string attackDir = "Front", string CC = "None")
    {
        Vector3 attackPos = transform.position + attackOffset;
        
        if (attackDir == "Front")
            attackPos += new Vector3((((attackSize.x) / 2) * direction.x), 0);
        else if (attackDir == "Back")
            attackPos -= new Vector3((((attackSize.x) / 2) * direction.x), 0);
        else // Middle
            { }
        
        if (CC == "Fear")
            anim.SetTrigger("Bark");

        
        Collider2D[] colliders = Physics2D.OverlapBoxAll(attackPos, attackSize, 0, WhoIsEnemy);
        foreach(Collider2D i in colliders)
        {
            GameObject Deffender = i.gameObject;
            Hit isHitted = i.GetComponent<Hit>();
            Attack iAttacked = GetComponent<Attack>();
            if (isHitted)
            {
                if (iAttacked.isAttackTarget(Deffender))
                {
                    if (isHitted.isHitTarget())
                    {
                        int damage = iAttacked.CalcDamage(gameObject, Deffender, 1);
                        if (CC == "Fear")
                            damage = 0;
                        isHitted.OnHit(gameObject, damage, false, CC);
                    }
                }
            }

        }
    }

    
    
    public void SetDirection(float dir)
    {
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * dir, transform.localScale.y, transform.localScale.z);
        if (invincible_Image)
        {
            invincible_Image.transform.parent.localScale = new Vector3(dir, 1, 1);
        }

    }

    virtual public void GetHurt(float damage)
    {
        anim.SetTrigger("Hit");
        stat.Hp -= damage;
        OnHurtEvent.Invoke();
        if (stat.Hp <= 0)
            Die();
    }
    virtual public void GetHeal(float value)
    {
        if ((stat.Hp + value) <= stat.MaxHp)
            stat.Hp += value;
        else
            stat.Hp = stat.MaxHp;
    }
    // 무적
    IEnumerator Unbeatable(float time)
    {
        gameObject.layer = LayerMask.NameToLayer("Unbeatable");
        invincibleTime = time;
        while (invincibleTime > 0)
        {
            invincibleTime -= Time.fixedDeltaTime;
            if (invincible_Image)
                invincible_Image.fillAmount = invincibleTime / time;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        gameObject.layer = mylayer;
    }

    public void Ouch()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
        StopCoroutine("SetUnMovable");
        StartCoroutine("SetUnMovable", 0.315f * (1 - stat.Tenasious));
    }
    IEnumerator SetUnMovable(float time)
    {
        Movable = false;
        yield return new WaitForSeconds(time);
        Movable = true;
    }
    public void Die()
    {
        die = true;
        if(gameObject == GameManager.instance.player.gameObject)
            GameManager.instance.uiManager.SetPlayerUIFalse();
        OnDieEvent.Invoke();
    }

    public void Knockback(float dir)
    {
        if (dir != 0)
            SetDirection(dir);

        rb.AddForce(new Vector2(-dir * 10, 150f));

        Vector2 m_Velocity = Vector2.zero;
        Vector2.SmoothDamp(rb.velocity, new Vector2(-dir * 155f, 5), ref m_Velocity, MovementSmoothing);
    }
    public void SmallJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        float jumpforce = 400f;
        if (isSlope) jumpforce *= 1.5f;
        rb.AddForce(new Vector2(0, jumpforce));
    }

    ////////////////////////////////////////////////////
    public void invincibility(float time)
    {
        if (invincibility_Coroutine != null)
            StopCoroutine(invincibility_Coroutine);

        invincibility_Coroutine = StartCoroutine("Unbeatable", time);

    }
    public void AnimatorSetTrigger(string what)
    {
        anim.SetTrigger(what);
    }

    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(transform.position + new Vector3(direction.x, 0, 0), new Vector3(5,1));
    }
    */
}
