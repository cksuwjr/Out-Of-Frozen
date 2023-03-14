using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class ObjectController : MonoBehaviour
{
    [SerializeField] private float JumpForce = 400f;

    [Range(0, .3f)] [SerializeField] private float MovementSmoothing = .05f;


    [SerializeField] public LayerMask WhatIsGround;
    [SerializeField] private Transform GroundCheck;

    [SerializeField] private LayerMask WhoIsEnemy;

    [SerializeField] private Status stat;

    const float GroundedRadius = .2f;
    public bool isGround;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

    public Vector2 direction = Vector2.zero;
    private Vector2 m_Velocity = Vector2.zero;

    public bool Movable = true;

    private int mylayer;
    Coroutine UnbeatableCo;
    [SerializeField] Image UnbeatableGage_Image;
    int jumpstack = 0;
    float UnbeatableTime = 0;

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
        sr = GetComponent<SpriteRenderer>();
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
        bool wasGrounded = isGround;
        isGround = false;

        Collider2D collider = Physics2D.OverlapCircle(GroundCheck.position, GroundedRadius, WhatIsGround);
        if (collider)
        {
            isGround = true;
            jumpstack = 0;
            if (!wasGrounded)
            {
                anim.SetBool("Jump", false);
                OnLandEvent.Invoke();
            }
        }


        if(rb.velocity.y < -12f)
        {
            rb.velocity = new Vector2(rb.velocity.x, -12f);
        }
    }

    public void Move(float direction, float speed, bool jump)
    {
        
        if (isGround)
        {

        }
        Vector3 targetVelocity = new Vector2(direction * speed, rb.velocity.y);
        if (direction != 0)
        {
            this.direction = new Vector3(direction, 0);
            anim.SetBool("Move", true);
            if (speed >= 4)
                anim.SetBool("Run", true);
            else
                anim.SetBool("Run", false);
        }
        else
        {
            anim.SetBool("Move", false);
            anim.SetBool("Run", false);
        }
        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, MovementSmoothing);
        
        if(direction != 0)
            SetDirection(direction);
        
        if (isGround && jump)
        {
            anim.SetBool("Jump", true);
            isGround = false;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0f, JumpForce));
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
                        isHitted.OnHit(gameObject, damage, false, CC);
                    }
                }
            }

        }
    }

    public void StepOnObstacle(GameObject Target)
    {
        anim.SetTrigger("Hit");
        if (UnbeatableTime <= 0.3f * jumpstack)
        {
            jumpstack += 1;
            GetHeal(jumpstack * 0.08f);
            if (UnbeatableCo != null)
                StopCoroutine(UnbeatableCo);
            UnbeatableCo = StartCoroutine("Unbeatable", 0.3f * jumpstack);
        }
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(new Vector2(0, 400f));
        Target.GetComponent<Hit>().OnHit(gameObject, stat.AttackPower, false, "None");
    }
    public void SteppedByObstacle(GameObject Target)
    {
        if (UnbeatableCo != null)
            StopCoroutine(UnbeatableCo);

        if (tag == "Player")
            UnbeatableCo = StartCoroutine("Unbeatable",0.5f);
        else
            UnbeatableCo = StartCoroutine("Unbeatable",0.3f);
        GetHurt(Target.GetComponent<Status>().AttackPower);
        int dir = Target.transform.position.x > transform.position.x ? 1 : -1;

        if (dir != 0)
            SetDirection(dir);

        rb.AddForce(new Vector2(-dir * 10, 150f));
        Vector2.SmoothDamp(rb.velocity, new Vector2(-dir * 155f, 5), ref m_Velocity, MovementSmoothing);


    }
    public void SetDirection(float dir)
    {
        transform.localScale = new Vector3(dir, 1, 1);
        if (UnbeatableGage_Image)
        {
            UnbeatableGage_Image.transform.parent.localScale = new Vector3(dir, 1, 1);
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
        OnHurtEvent.Invoke();
    }
    public void Die()
    {
        OnDieEvent.Invoke();        
    }
    virtual public void DieEvent()
    {
        //Instantiate(gameObject, transform.position, Quaternion.identity);
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        rb.velocity = Vector3.zero;
        rb.AddForce(new Vector2(0, 400f));
        anim.SetTrigger("Hit");
        GetComponent<SpriteRenderer>().flipY = true;

        Destroy(GetComponent<BoxCollider2D>());
        Destroy(transform.Find("Hitbox").gameObject);
        Destroy(GetComponent<Move>());
        Destroy(GetComponent<Bat>());
        Destroy(gameObject, 2);
        
    }

    // 무적
    IEnumerator Unbeatable(float time)
    {
        gameObject.layer = LayerMask.NameToLayer("Unbeatable");
        UnbeatableTime = time;
        while (UnbeatableTime > 0)
        {
            UnbeatableTime -= Time.fixedDeltaTime;
            if (UnbeatableGage_Image)
                UnbeatableGage_Image.fillAmount = UnbeatableTime / time;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        gameObject.layer = mylayer;
    }

    public void Ouch()
    {
        StopCoroutine("SetUnMovable");
        StartCoroutine("SetUnMovable",0.15f);
    }
    IEnumerator SetUnMovable(float time)
    {
        Movable = false;
        yield return new WaitForSeconds(time);
        Movable = true;
    }


    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(transform.position + new Vector3(direction.x, 0, 0), new Vector3(5,1));
    }
    */
}
