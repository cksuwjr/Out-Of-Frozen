using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("HitBox")) return;
   
        if (collision.transform.parent.gameObject.layer == transform.parent.gameObject.layer) { return; }


        GameObject Target = collision.transform.parent.gameObject;
        Debug.Log(transform.parent.name + "가 충돌: " + Target.name + transform.parent.gameObject.layer + "/" + Target.layer);
        Rigidbody2D rb = transform.parent.GetComponent<Rigidbody2D>();
        ObjectController oc = transform.parent.GetComponent<ObjectController>();
        if (rb.velocity.y < 0 && transform.parent.position.y > Target.transform.position.y && !oc.isGround)
        {
            oc.StepOnObstacle(Target);
        }
        else
        {
            oc.SteppedByObstacle(Target);
        }
    }
    */
}
