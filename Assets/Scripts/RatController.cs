using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatController : ObjectController
{
    [SerializeField] GameObject Rat;
    [SerializeField] int Spawncount;
    [Range(0, 200f)] [SerializeField] private int FloatingRange = 0;
    private void Start()
    {
          
    }

    public override void Die()
    {
        for (int i = 0; i < Spawncount; i++)
        {
            GameObject Spawned = Instantiate(Rat, transform.position, Quaternion.identity);
            int force = -FloatingRange + (((FloatingRange * 2) / Spawncount) * i);
            SpriteRenderer sr = Spawned.GetComponent<SpriteRenderer>();
            if (force > 0)
                sr.flipX = false;
            else 
                sr.flipX = true;

            Spawned.GetComponent<Rigidbody2D>().AddForce(new Vector2(force, 200f));
        }
        Destroy(gameObject);
    }
}
