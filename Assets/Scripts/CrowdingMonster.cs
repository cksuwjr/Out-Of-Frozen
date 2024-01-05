using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdingMonster : Monster
{
    [SerializeField] GameObject individual;
    [SerializeField] int spawnCount;
    public override void TrampledOn(GameObject Target) // Áþ¹âÈ÷´Ù.
    {
        if (Target.GetComponent<ObjectController>().die) return;
        controller.invincibility(0.2f);
    }
    public override void Die()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            var indivisual = Instantiate(individual, transform.position, Quaternion.identity);
            indivisual.GetComponent<ObjectController>().SmallJump();
        }
        Destroy(gameObject);
    }
}
