using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectController : MonoBehaviour
{

    // 점프 옵션
    [SerializeField] private float JumpForce = 400f;

    // 움직임 옵션
    [Range(0, .3f)] [SerializeField] private float MovementSmoothing = .05f;


    // 바닥 체크
    [SerializeField] private LayerMask WhatIsGround;
    [SerializeField] private Transform GroundCheck;

    const float GroundedRadius = .2f;
    private bool isGround;

    // 기본모듈
    private Rigidbody2D rb;
    private SpriteRenderer sr;


    // 바라보는 방향
    public Vector2 direction = Vector2.zero;
    private Vector2 m_Velocity = Vector2.zero;


    // 이벤트
    [Header("Events")]
    [Space]

    // 이벤트 - 착지
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
