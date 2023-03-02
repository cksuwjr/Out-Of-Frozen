using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectController : MonoBehaviour
{
    [SerializeField] private float JumpForce = 400f;

    [Range(0, .3f)] [SerializeField] private float MovementSmoothing = .05f;


    [SerializeField] private LayerMask WhatIsGround;
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
    
    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

    }

    private void FixedUpdate()
    {
        bool wasGrounded = isGround;
        isGround = false;

        Collider2D collider = Physics2D.OverlapCircle(GroundCheck.position, GroundedRadius, WhatIsGround);
        if (collider)
        {
            isGround = true;
            if (!wasGrounded)
                OnLandEvent.Invoke();
        }
    }

    public void Move(float direction, float speed, bool jump)
    {
        
        if (isGround)
        {

        }
        Vector3 targetVelocity = new Vector2(direction * speed, rb.velocity.y);

        if (direction != 0)
            this.direction = new Vector3(direction, 0);
        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, MovementSmoothing);
        if (direction == 1)
            sr.flipX = false;
        else if (direction == -1)
            sr.flipX = true;


        if (isGround && jump)
        {
            isGround = false;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0f, JumpForce));
        }
    }
    public void Attack(Vector3 attackSize, Vector3 attackOffset, string attackDir = "Front", string CC = "None")
    {
        Vector3 attackPos = transform.position + attackOffset;
        if (attackDir == "Front")
            attackPos += new Vector3((((attackSize.x - 2) / 2) * direction.x), 0);
        else if (attackDir == "Back")
            attackPos -= new Vector3((((attackSize.x - 2) / 2) * direction.x), 0);
        else // Middle
            { }

        Debug.Log(direction);
        Debug.Log(attackDir);
        Collider2D[] colliders = Physics2D.OverlapBoxAll(attackPos, attackSize / 2, WhoIsEnemy);
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

    void StepOnObstacle()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(new Vector2(0, 400f));
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.layer == LayerMask.NameToLayer("Unbeatable")) return;

        GameObject Target = collision.gameObject;
        
        if (Target.layer == LayerMask.NameToLayer("Mob"))
        {
            if (rb.velocity.y < 0 && transform.position.y > Target.transform.position.y && !isGround)
            {
                StartCoroutine(Unbeatable(0.3f));
                Target.GetComponent<Hit>().OnHit(gameObject, 1, false, "None");
                //Target.GetComponent<ObjectController>().GetHurt(1);
                StepOnObstacle();
            }
            else
            {
                StartCoroutine(Unbeatable(0.3f));
                GetComponent<ObjectController>().GetHurt(1);
                int dir = Target.transform.position.x > transform.position.x ? 1 : -1;

                SpriteRenderer sr = GetComponent<SpriteRenderer>();
                if (dir == 1)
                    sr.flipX = false;
                else if (dir == -1)
                    sr.flipX = true;

                rb.AddForce(new Vector2(-dir * 275f, 175f));
                

            }
        }
    }
    virtual public void GetHurt(int damage)
    {
        anim.SetTrigger("Hit");

        stat.Hp -= damage;
        if (stat.Hp <= 0)
            Die();
    }

    virtual public void Die()
    {

        //Instantiate(gameObject, transform.position, Quaternion.identity);
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        rb.velocity = Vector3.zero;
        rb.AddForce(new Vector2(0, 400f));
        GetComponent<SpriteRenderer>().flipY = true;
        GetComponent<BoxCollider2D>().isTrigger = true;
        Destroy(GetComponent<Move>());
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
