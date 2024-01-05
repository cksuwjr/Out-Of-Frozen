using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Creature
{
    int jumpstack = 0;
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
                if (controller.invincibleTime <= 0.3f * jumpstack)
                {
                    jumpstack += 1;
                    controller.GetHeal(jumpstack * 0.08f);
                    controller.invincibility(0.3f * jumpstack);
                }
                Trample(Target);
                GameManager.instance.uiManager.SetPlayerHeartUI();
            }
            else
            {
                if(gameObject.layer != LayerMask.NameToLayer("Unbeatable"))
                    TrampledOn(Target);
                GameManager.instance.uiManager.SetPlayerHeartUI();
            }
        }
    }
    public void SetJumpStack(int value)
    {
        jumpstack = value;
    }
}
