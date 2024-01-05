using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Monster
{
    public override void TrampledOn(GameObject Target)
    {
        StartCoroutine(Slow(1f, 2f));
    }
    IEnumerator Slow(float howmuch, float time)
    {

        float bespeed = controller.stat.Speed;
        controller.stat.Speed = howmuch;
        yield return new WaitForSeconds(time);
        controller.stat.Speed = bespeed;
    }
}
