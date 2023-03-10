using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;

    [SerializeField] float Speed = 5f;

    [SerializeField] Vector2 X_minmax = Vector2.zero;
    [SerializeField] Vector2 Y_minmax = Vector2.zero;

    int X_dir = 1;
    int Y_dir = 1;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        float x = transform.position.x;
        float y = transform.position.y;

        if (X_minmax.x > x)
            X_dir = 1;
        else if(X_minmax.y < x)
            X_dir = -1;

        
        if (X_dir != 0)
            transform.localScale = new Vector3(X_dir, 1, 1);


        if (Y_minmax.x > y)
            Y_dir = 1;
        else if (Y_minmax.y < y)
            Y_dir = -1;




        rb.velocity = new Vector2(X_dir * Speed, Y_dir * Speed);
        

    }
    public void Hitted()
    {
        StartCoroutine(Slow(2f, 2f));
    }
    IEnumerator Slow(float howmuch, float time)
    {
        
        float bespeed = 3;
        Speed = howmuch;
        yield return new WaitForSeconds(time);
        Speed = bespeed;
    }
    public void Scared(GameObject FromWho)
    {
        rb.velocity = Vector2.zero;

        if (FromWho)
        {
            int dir;
            dir = FromWho.transform.position.x > transform.position.x ? 1 : -1;

            if (dir != 0)
                transform.localScale = new Vector3(dir, 1, 1);

            rb.velocity = new Vector3(-dir * 0.5f, rb.velocity.y);
        }

        float stuntime = 1f;
        StartCoroutine(Slow(0.5f, stuntime));
        GetComponent<EffectSpawner>().Spawn("Fear", stuntime);

    }
}
