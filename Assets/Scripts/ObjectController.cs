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

    const float GroundedRadius = .2f;
    private bool isGround;

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
        
    }

    public void Move(float direction, float speed, bool jump)
    {
        if (isGround)
        {

        }
        Vector3 targetVelocity = new Vector2(direction * speed, 0);

        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, MovementSmoothing);
        if (direction == 1)
            sr.flipX = false;
        else if(direction == -1)
            sr.flipX = true;


        if (isGround && jump)
        {
            isGround = false;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0f, JumpForce));
        }
    }

}
