using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectController : MonoBehaviour
{

    // ���� �ɼ�
    [SerializeField] private float JumpForce = 400f;

    // ������ �ɼ�
    [Range(0, .3f)] [SerializeField] private float MovementSmoothing = .05f;


    // �ٴ� üũ
    [SerializeField] private LayerMask WhatIsGround;
    [SerializeField] private Transform GroundCheck;

    [SerializeField] private LayerMask WhoIsEnemy;

    const float GroundedRadius = .2f;
    public bool isGround;

    // �⺻���
    private Rigidbody2D rb;
    private SpriteRenderer sr;


    // �ٶ󺸴� ����
    public Vector2 direction = Vector2.zero;
    private Vector2 m_Velocity = Vector2.zero;


    // �̺�Ʈ
    [Header("Events")]
    [Space]

    // �̺�Ʈ - ����
    public UnityEvent OnLandEvent;
    
    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

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
        /*
        if(rb.velocity.y > 0)
        {
            Debug.Log("����: " + rb.velocity.y);

            
        }
        else Debug.Log("����: " + rb.velocity.y);
        */
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
        //Debug.Log("��ü��: " + colliders.Length);
        foreach(Collider2D i in colliders)
        {
            GameObject Deffender = i.gameObject;
            Hit isHitted = i.GetComponent<Hit>();
            Attack iAttacked = GetComponent<Attack>();
            if (isHitted)
            {
                if (iAttacked.isAttackTarget(Deffender)) // ���ݴ���ΰ�? �Ǵ�
                {
                    if (isHitted.isHitTarget()) // ���� �ǰݴ������ ��밡 ���� �Ǵ�
                    {
                        int damage = iAttacked.CalcDamage(gameObject, Deffender, 50); // ������ ��� 1 (������ ����)
                        isHitted.OnHit(gameObject, damage, false, CC); // ������ ��� 2 (ȸ���ΰ� �����ΰ�)
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
        GameObject Target = collision.gameObject;
        
        if (Target.layer == LayerMask.NameToLayer("Mob"))
        {
            if (rb.velocity.y <= -1f && transform.position.y > Target.transform.position.y && !isGround)
            {
                //Debug.Log("����!: " + rb.velocity.y);
                Target.GetComponent<Rat>().Die();
                StepOnObstacle();
            }
            else
            {
                Die(); 
            }
        }
    }

    public void Die()
    {
        Instantiate(gameObject, transform.position, Quaternion.identity); // ����
        //CancelInvoke("Pattern");
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        //anim.SetTrigger("Hit");
        //Destroy(anim);
        rb.velocity = Vector3.zero;
        rb.AddForce(new Vector2(0, 400f));
        GetComponent<SpriteRenderer>().flipY = true;
        GetComponent<BoxCollider2D>().isTrigger = true;
        Destroy(gameObject, 2);
    }


    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(transform.position + new Vector3(direction.x, 0, 0), new Vector3(5,1));
    }
    */
}
