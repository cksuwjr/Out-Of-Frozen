using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : MonoBehaviour
{
    int dir = -1;
    [SerializeField] float Speed = 5f;
    [SerializeField] Rigidbody2D rb;

    private void Start()
    {
        InvokeRepeating("Pattern", Random.Range(0.1f, 1.9f), 1);
    }
    void Update()
    {

        

    }
    public void Pattern()
    {
        int var = Random.Range(0, 5);
        if (var == 1)
        {
            rb.velocity = new Vector3(Speed * dir, rb.velocity.y);
        }
        else if (var == 2)
        {
            
            transform.localScale = new Vector3(dir, 1, 1);
            dir *= -1;
            rb.velocity = new Vector3(Speed * dir, rb.velocity.y);

        }
        else if (var == 3)
        {
            rb.AddForce(new Vector2(0, 10));
        }
        else if (var == 4)
        {
            if(rb.velocity.y !> 0)
                rb.velocity = new Vector3(Speed * dir, 5);
        }
    }
}
