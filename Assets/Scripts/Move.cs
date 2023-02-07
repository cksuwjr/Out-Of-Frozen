using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public ObjectController controller;
    public float Speed = 40f;

    bool Jump = false;
    float MoveDir = 0;

    private void Update()
    {
        MoveDir = Input.GetAxisRaw("Horizontal");
        //float vetticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetButton("Jump"))
        {
            Jump = true;
        }
    }
    private void FixedUpdate()
    {
        controller.Move(MoveDir, Speed, Jump);
        Jump = false;
    }
}
