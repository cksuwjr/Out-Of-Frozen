using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public ObjectController controller;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("HitBox")) return;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Unbeatable")) return;
        if (!GetComponent<BoxCollider2D>()) return;

        GameObject Target = collision.transform.parent.gameObject;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (Target.layer == LayerMask.NameToLayer("Mob"))
        {
            if (rb.velocity.y < 0 && transform.position.y > Target.transform.position.y && !controller.isGround)
            {
                controller.StepOnObstacle(Target);
            }
            else
            {
                if(gameObject.layer != LayerMask.NameToLayer("Unbeatable"))
                    controller.SteppedByObstacle(Target);
            }
        }
    }


}
