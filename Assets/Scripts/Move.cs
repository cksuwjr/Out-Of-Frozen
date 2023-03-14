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
        if (Input.GetKey(KeyCode.R))
        {
            Run = true;
        }
    }
    private void FixedUpdate()
    {
        if (!Run)
            controller.Move(MoveDir, Speed, Jump);
        else
            controller.Move(MoveDir, RunSpeed, Jump);
        GameObject background = GameObject.FindGameObjectWithTag("Background");


        if (background)
            background.GetComponent<Background>().MoveBackground((transform.position.x - bepos.x) * Time.fixedDeltaTime);
        bepos = transform.position;
        

        Jump = false;
        Run = false;
    }
}
