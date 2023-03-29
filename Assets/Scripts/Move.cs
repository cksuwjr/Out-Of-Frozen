using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public ObjectController controller;
    public float Speed = 40f;
    public float RunSpeed = 40f;

    bool Jump = false;
    bool Run = false;
    float MoveDir = 0;

    Vector3 bepos = Vector3.zero;
    private void Update()
    {
        if (!controller.Movable) return;


        MoveDir = Input.GetAxisRaw("Horizontal");
        //float vetticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetButton("Jump"))
        {
            Jump = true;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Run = true;
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            Run = false;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.velocity = new Vector3(rb.velocity.x, -12, 0);
        }
    }
    private void FixedUpdate()
    {
        if (Run)
            controller.Move(MoveDir, RunSpeed, Jump);
        else
            controller.Move(MoveDir, Speed, Jump);
        GameObject background = GameObject.FindGameObjectWithTag("Background");


        if (background)
            background.GetComponent<Background>().MoveBackground((transform.position.x - bepos.x) * Time.fixedDeltaTime);
        bepos = transform.position;
        

        Jump = false;
        //Run = false;
    }
}
