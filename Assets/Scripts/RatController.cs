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

    public override void DieEvent()
    {
        for (int i = 0; i < Spawncount; i++)
        {
            GameObject Spawned = Instantiate(Rat, transform.position, Quaternion.identity);
            int force = -FloatingRange + (((FloatingRange * 2) / Spawncount) * i);

            int direction;
            if (force > 0)
                direction = -1;
            else
                direction = 1;


            if (direction != 0)
                transform.localScale = new Vector3(direction, 1, 1);

            Spawned.GetComponent<Rigidbody2D>().AddForce(new Vector2(force, Random.Range(100f, 300f)));
        }
        Destroy(gameObject);
    }
}
