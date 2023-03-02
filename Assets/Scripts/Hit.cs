using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hit : MonoBehaviour
{
    [Header("Events")]
    [Space]
    public UnityEvent OnHitEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private void Awake()
    {
        if (OnHitEvent == null)
            OnHitEvent = new UnityEvent();
        
    }








    public void OnHit(GameObject attacker, int damage, bool isCritical = false, string attackInfo = "", int hitCount = 1)
    {
        if (GetComponent<Rat>() && attackInfo == "Fear")
        {
            GetComponent<Rat>().Hit(attacker);
            
        }
        else
            GetComponent<ObjectController>().GetHurt(1);
    }
    public bool isHitTarget()
    {
        return true;
    }
}
