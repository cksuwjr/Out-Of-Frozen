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


    [SerializeField] Transform UnderCheck;
    [SerializeField] Transform FrontCheck;

    Dictionary<string, float> act = new Dictionary<string, float>();
    public string NowAct = "";


    private void StateRegister()
    {
        // 상태추가
        act.Add("Go", 55f);
        act.Add("GoAnother", 30f);
        act.Add("Stop", 5f);
        act.Add("StopJump", 5f);
        act.Add("GoJump", 5f);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (!controller)
            controller = GetComponent<ObjectController>();

        StateRegister();
        
        InvokeRepeating("Pattern", 0, 3);
    }
    void Update()
    {
        if (!UnderCheck) return;
        Collider2D collider = Physics2D.OverlapCircle(UnderCheck.position, .1f, controller.WhatIsGround);

        if (!collider && controller.isGround)
        {
            Pattern("GoAnother");
        }
        collider = Physics2D.OverlapCircle(FrontCheck.position, .1f, controller.WhatIsGround);
        if (collider && controller.isGround)
        {
            Pattern("GoAnother");
        }

    }
    
    

    
    public void Pattern()
    {
        StopCoroutine("Act");
        NowAct = SelectAct();
        StartCoroutine("Act");
    }
    public void Pattern(string what = "")
    {
        StopCoroutine("Act");
        if (what == "")
            NowAct = SelectAct();
        else
            NowAct = what;
        StartCoroutine("Act");
    }
    string SelectAct()
    {
        float total = 0;
        foreach (float value in act.Values)
        {
            total += value;
        }
        float r = Random.Range(0, total);

        string Act = "";
        for (int i = 0; i < act.Count; i++)
        {
            float min = 0;
            float max = 0;

            int count = 0;

            foreach (float value in act.Values)
            {
                if (count == i) { max += min; max += value; break; }
                min += value;
                count++;
            }
            if (min <= r && r < max)
            {
                count = 0;
                foreach (string n in act.Keys)
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
            case "Go":
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
                        Pattern();
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
                        Pattern();

                    }
                }

        }
    }
    public void Hit(GameObject FromWho)
    {
        CancelInvoke("Pattern");
        StopCoroutine("Act");
        rb.velocity = Vector2.zero;

        if (FromWho)
        {
            dir = FromWho.transform.position.x > transform.position.x ? 1 : -1;

            if (dir != 0)
                transform.localScale = new Vector3(dir, 1, 1);

            rb.velocity = new Vector3(-dir * 0.5f, rb.velocity.y);
        }

        anim.SetTrigger("Hit");
        float stuntime = 1f * (1-Tenasious);
        GetComponent<EffectSpawner>().Spawn("Fear", stuntime);

        InvokeRepeating("Pattern", stuntime, 3);
        //Invoke("Pattern", stuntime);       
    }
    
}
