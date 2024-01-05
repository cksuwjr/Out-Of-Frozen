using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    protected ObjectController controller;
    private void Awake()
    {
        controller = GetComponent<ObjectController>();
    }
    public virtual void TrampledOn(GameObject Target) // Áþ¹âÈ÷´Ù.
    {
        if (controller.die) return;

        if (Target.GetComponent<ObjectController>().die) return;
        controller.invincibility(0.5f);
        GetComponent<Hit>().OnHit(Target, Target.GetComponent<Status>().AttackPower, false, "None");
        controller.Knockback(Target.transform.position.x > transform.position.x ? 1 : -1);
    }
    public void Trample(GameObject Target)
    {
        if (Target.GetComponent<ObjectController>().die) return;
        controller.AnimatorSetTrigger("Hit");
        
        controller.SmallJump();
        Target.GetComponent<Hit>().OnHit(gameObject, controller.stat.AttackPower, false, "None");
        Target.GetComponent<Creature>().TrampledOn(gameObject);
    }
    public virtual void Die()
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
        controller.SmallJump();
        controller.AnimatorSetTrigger("Hit");

        GetComponent<SpriteRenderer>().flipY = true;

        GetComponent<BoxCollider2D>().isTrigger = true;
        controller.Movable = false;
        Destroy(gameObject, 2);

    }
}
