using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        }
        else
        {
            anim.SetBool("Move", false);
        }
        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, MovementSmoothing);
        
        if(direction != 0)
            transform.localScale = new Vector3(direction, 1, 1);
        /*
        if (direction == 1)
            sr.flipX = false;
        else if (direction == -1)
            sr.flipX = true;
        */

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
        StartCoroutine(Unbeatable(0.3f));
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(new Vector2(0, 450f));
        Target.GetComponent<Hit>().OnHit(gameObject, stat.AttackPower, false, "None");
    }
    public void SteppedByObstacle(GameObject Target)
    {
        if(tag == "Player")
            StartCoroutine(Unbeatable(0.5f));
        else
            StartCoroutine(Unbeatable(0.3f));
        GetHurt(Target.GetComponent<Status>().AttackPower);
        int dir = Target.transform.position.x > transform.position.x ? 1 : -1;

        if (dir != 0)
            transform.localScale = new Vector3(dir, 1, 1);

        Vector2.SmoothDamp(rb.velocity, new Vector2(-dir * 2.75f, 5), ref m_Velocity, MovementSmoothing);


    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("HitBox")) return; 
        if (collision.gameObject.layer == LayerMask.NameToLayer("Unbeatable")) return;
        if (!GetComponent<BoxCollider2D>()) return;

        GameObject Target = collision.transform.parent.gameObject;

        if (Target.layer == LayerMask.NameToLayer("Mob") && gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (rb.velocity.y < 0 && transform.position.y > Target.transform.position.y && !isGround)
            {
                StepOnObstacle(Target);
            }
            else
            {
                SteppedByObstacle(Target);
            }
        }
    }
    
    virtual public void GetHurt(int damage)
    {
        anim.SetTrigger("Hit");

        stat.Hp -= damage;
        OnHurtEvent.Invoke();
        if (stat.Hp <= 0)
            Die();
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
        LayerMask layer = gameObject.layer;
        gameObject.layer = LayerMask.NameToLayer("Unbeatable");
        yield return new WaitForSeconds(time);
        gameObject.layer = layer;
    }

    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(transform.position + new Vector3(direction.x, 0, 0), new Vector3(5,1));
    }
    */
}
