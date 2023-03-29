using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : ObjectController
{
    [SerializeField] GameObject ChildMonster;
    [SerializeField] int DieSpawncount;
    [SerializeField] int Spawncount;
    [Range(0, 200f)] [SerializeField] private int FloatingRange = 0;
    private void Start()
    {
          
    }
    public void DecreaseScale()
    {
        float xscale = 0.2f;
        if (transform.localScale.x > 0)
            xscale = 0.2f;
        else
            xscale = -0.2f;
        transform.localScale -= new Vector3(xscale, 0.2f, 0);
    }
    public void SpawnMob()
    {
        for (int i = 0; i < Spawncount; i++)
        {
            GameObject Spawned = Instantiate(ChildMonster, transform.position, Quaternion.identity);
            int force = -FloatingRange + (((FloatingRange * 2) / Spawncount) * i);

            int direction;
            if (force > 0)
                direction = -1;
            else
                direction = 1;


            if (direction != 0)
                Spawned.transform.localScale = new Vector3(direction, 1, 1);

            Spawned.GetComponent<Rigidbody2D>().AddForce(new Vector2(force, Random.Range(100f, 300f)));
        }
    }
    public override void DieEvent()
    {
        for (int i = 0; i < DieSpawncount; i++)
        {
            GameObject Spawned = Instantiate(ChildMonster, transform.position, Quaternion.identity);
            int force = -FloatingRange + (((FloatingRange * 2) / Spawncount) * i);

            int direction;
            if (force > 0)
                direction = -1;
            else
                direction = 1;


            if (direction != 0)
                Spawned.transform.localScale = new Vector3(direction, 1, 1);

            Spawned.GetComponent<Rigidbody2D>().AddForce(new Vector2(force, Random.Range(100f, 300f)));
        }
        Destroy(gameObject);
    }
}
