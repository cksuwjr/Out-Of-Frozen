using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : MonoBehaviour
{
    int dir = -1;
    
    [SerializeField] float Speed = 5f;
    [SerializeField] float Tenasious = 0f;
    [SerializeField] bool Jumpable = true;
    private Rigidbody2D rb;
    private Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        InvokeRepeating("Pattern", Random.Range(0.1f, 1.9f), 1.5f);
    }
    void Update()
    {

    }
    public void Pattern()
    {
        Dictionary<string, float> act = new Dictionary<string, float>();
        act.Add("Go", 55f);
        act.Add("GoAnother", 30f);
        act.Add("Stop", 5f);
        act.Add("StopJump", 5f);
        act.Add("GoJump", 5f);

        float total = 0;
        foreach (float value in act.Values) // 가중치 전체 값 계산
        {
            total += value;
        }
        float r = Random.Range(0, total);

        string actname = "";
        for(int i = 0; i < act.Count; i++)
        {
            float min = 0;
            float max = 0;

            int count = 0;

            foreach(float value in act.Values) // 최대 최소 값 구하기
            {
                if (count == i) { max += min; max += value; break; }
                min += value;
                count++;
            }
            //Debug.Log(i + "번째: min(" + min + "), max(" + max + ")");
            if(min <= r && r < max) // 최대 최소값에 랜덤값이 포함되나 각자 계산 하고 포함될 경우 활동 적용
            {
                //Debug.Log("찾았다" + i);
                count = 0;
                foreach(string n in act.Keys)
                {
                    if (count == i) { actname = n; break; }
                    count++;

                }
                //Debug.Log("내 활동네임은 " + actname);

                break;
            }
        }

        if (actname == "Go")
        {
            transform.localScale = new Vector3(dir, 1, 1);
            rb.velocity = new Vector3(Speed * dir, rb.velocity.y);
        }
        else if(actname == "GoAnother")
        {
            dir *= -1;
            transform.localScale = new Vector3(dir, 1, 1);
            rb.velocity = new Vector3(Speed * dir, rb.velocity.y);
        }
        else if(actname == "Stop")
        {
            rb.velocity = new Vector3(0, rb.velocity.y);
        }
        else if(actname == "StopJump" && Jumpable)
        {
            rb.velocity = Vector2.zero;
            if (rb.velocity.y! > -0.1f)
                rb.AddForce(new Vector2(0, 200));
        }
        else if(actname == "GoJump" && Jumpable)
        {
            rb.velocity = Vector2.zero;
            if (rb.velocity.y! > -0.1f)
                rb.AddForce(new Vector2(Speed * dir, 200));
        }
    }
    public void Hit(GameObject FromWho)
    {
        CancelInvoke("Pattern");
        rb.velocity = Vector2.zero;

        if (FromWho)
        {
            dir = FromWho.transform.position.x > transform.position.x ? 1 : -1;
            transform.localScale = new Vector3(dir, 1, 1);
            //일정거리 넉백
            rb.velocity = new Vector3(-dir * 0.5f, rb.velocity.y);
        }

        anim.SetTrigger("Hit");
        float stuntime = 1f * (1-Tenasious);
        GetComponent<EffectSpawner>().Spawn("Fear", stuntime);

        InvokeRepeating("Pattern", stuntime, 1.5f);
    }
    

    public void Die()
    {
        CancelInvoke("Pattern");
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        anim.SetTrigger("Hit");
        //Destroy(anim);
        rb.velocity = Vector3.zero;
        rb.AddForce(new Vector2(0, 200f));
        GetComponent<SpriteRenderer>().flipY = true;
        GetComponent<BoxCollider2D>().isTrigger = true;
        Destroy(gameObject, 2);
    }
}
