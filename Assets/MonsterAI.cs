using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    protected ObjectController controller;

    protected int dir = -1;
    
    protected Rigidbody2D rb;
    protected Animator anim;
    protected Status status;

    [SerializeField] Transform UnderCheck;
    [SerializeField] Transform FrontCheck;

    Dictionary<string, float> act = new Dictionary<string, float>();
    public string NowAct = "";

    public void AddAct(string stateName, float percentage)
    {
        act.Add(stateName, percentage);
    }
    protected virtual void StateRegister()
    {
        // 상태추가
        AddAct("Go", 55f);
        AddAct("GoAnother", 30f);
        AddAct("Stop", 5f);
        AddAct("StopJump", 5f);
        AddAct("GoJump", 5f);
    }

    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        status = GetComponent<Status>();
        controller = GetComponent<ObjectController>();

        StateRegister();

        StartCoroutine("Act");
        InvokeRepeating("ChangeAct", Random.Range(0.01f,0.05f), 3);
    }
    public void ChangeAct()
    {
        NowAct = SelectAct();
    }
    public void ObstacleCheck()
    {
        if (controller.isSlope) return;
        if (!UnderCheck) return;

        Collider2D collider = Physics2D.OverlapCircle(UnderCheck.position, .1f, controller.WhatIsGround);

        if (!controller.isSlope)
        {
            if (!collider && controller.isGround)
                NowAct = "GoAnother";
            collider = Physics2D.OverlapCircle(FrontCheck.position, .1f, controller.WhatIsGround);
            if (collider && controller.isGround)
                NowAct = "GoAnother";
        }
    }
    IEnumerator Act()
    {
        bool Rest = false;
        while (!Rest)
        {
            ObstacleCheck();

            if (controller.Movable)
            {
                if (NowAct == "Go")
                    controller.Move(dir, status.Speed, false);
                else if (NowAct == "GoAnother")
                {
                    dir *= -1;
                    controller.Move(dir, status.Speed, false);
                    NowAct = "Go";
                }
                else if (NowAct == "Stop")
                {
                    Rest = true;
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    controller.Move(0, 0, false);
                    StartCoroutine("Rest", 3);
                }
                else if (NowAct == "StopJump")
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    controller.Move(0, 0, true);
                    NowAct = "Stop";
                }
                else if (NowAct == "GoJump")
                {
                    controller.Move(dir, status.Speed, true);
                    NowAct = "Stop";
                }
                else if (NowAct == "Hit")
                {
                    NowAct = "Wait";
                }
            }
            yield return null;
        }
    }
    IEnumerator Rest(float time)
    {
        StopCoroutine("Act");
        yield return new WaitForSeconds(time);
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

    public virtual void Hit(GameObject FromWho)
    {
        Debug.Log("아야");
        CancelInvoke("ChangeAct");
        NowAct = "Hit";

        rb.velocity = Vector2.zero;

        if (FromWho)
        {
            dir = FromWho.transform.position.x > transform.position.x ? 1 : -1;

            if (dir != 0)
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * dir, transform.localScale.y, transform.localScale.z);

            rb.velocity = new Vector3(-dir * 0.5f, rb.velocity.y);
        }

        anim.SetTrigger("Hit");
        float stuntime = 1f * (1-controller.stat.Tenasious);
        GetComponent<EffectSpawner>().Spawn("Fear", stuntime);

        InvokeRepeating("ChangeAct", stuntime, 3);

        //Invoke("Pattern", stuntime);       
    }

}
