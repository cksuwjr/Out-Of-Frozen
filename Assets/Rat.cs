using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : MonoBehaviour
{
    public ObjectController controller;

    int dir = -1;
    
    [SerializeField] float Speed = 5f;
    [SerializeField] float Tenasious = 0f;
    [SerializeField] bool Jumpable = true;
    private Rigidbody2D rb;
    private Animator anim;

    public string NowAct = "";

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (!controller)
            controller = GetComponent<ObjectController>();


        InvokeRepeating("Pattern", Random.Range(0.1f, 1.9f), 3f);
    }
    void Update()
    {

    }
    
    

    string SelectAct(Dictionary<string, float> actsorts)
    {
        float total = 0;
        foreach (float value in actsorts.Values) 
        {
            total += value;
        }
        float r = Random.Range(0, total);

        string Act = "";
        for(int i = 0; i < actsorts.Count; i++)
        {
            float min = 0;
            float max = 0;

            int count = 0;

            foreach(float value in actsorts.Values) 
            {
                if (count == i) { max += min; max += value; break; }
                min += value;
                count++;
            }
            if(min <= r && r < max)
            {
                count = 0;
                foreach(string n in actsorts.Keys)
                {
                    if (count == i) { Act = n; break; }
                    
                        
                    count++;

                }

                break;
            }
        }
        return Act;
    }
    IEnumerator Act()
    {
        switch (NowAct)
        {
            case "Go" :
                {
                    bool jump = false;
                    while (true)
                    {
                        controller.Move(dir, Speed, jump);
                        yield return new WaitForSeconds(Time.fixedDeltaTime);
                    }
                }
            case "GoAnother":
                {
                    dir *= -1;

                    bool jump = false;
                    while (true)
                    {
                        controller.Move(dir, Speed, jump);
                        yield return new WaitForSeconds(Time.fixedDeltaTime);
                    }
                }
            case "Stop":
                {
                    bool jump = false;
                    while (true)
                    {
                        controller.Move(0, 0, jump);
                        yield return new WaitForSeconds(Time.fixedDeltaTime);
                    }
                }
            case "StopJump":
                {
                    if (!Jumpable) break;
                    bool jump = true;
                    while (true)
                    {
                        controller.Move(0, 0, jump);
                        yield return new WaitForSeconds(Time.fixedDeltaTime);
                        //CancelInvoke("Pattern");
                        Invoke("Pattern",0);
                    }
                }
            case "GoJump":
                {
                    if (!Jumpable) break;

                    bool jump = true;
                    while (true)
                    {
                        controller.Move(dir, Speed, jump);
                        yield return new WaitForSeconds(Time.fixedDeltaTime);
                        //CancelInvoke("Pattern");
                        Invoke("Pattern", 0);
                    }
                }

        }
    }
    public void Pattern()
    {
        StopCoroutine("Act");
        Dictionary<string, float> act = new Dictionary<string, float>();
        act.Add("Go", 55f);
        act.Add("GoAnother", 30f);
        act.Add("Stop", 5f);
        act.Add("StopJump", 5f);
        act.Add("GoJump", 5f);

        NowAct = SelectAct(act);
        StartCoroutine("Act");
    }
    public void Hit(GameObject FromWho)
    {
        CancelInvoke("Pattern");
        StopCoroutine("Act");
        rb.velocity = Vector2.zero;

        if (FromWho)
        {
            dir = FromWho.transform.position.x > transform.position.x ? 1 : -1;

            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (dir == 1)
                sr.flipX = false;
            else if (dir == -1)
                sr.flipX = true;

            rb.velocity = new Vector3(-dir * 0.5f, rb.velocity.y);
        }

        anim.SetTrigger("Hit");
        float stuntime = 1f * (1-Tenasious);
        GetComponent<EffectSpawner>().Spawn("Fear", stuntime);

        InvokeRepeating("Pattern", stuntime, 1.5f);
    }
    
}
