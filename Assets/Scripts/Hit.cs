using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hit : MonoBehaviour
{
    // 이벤트
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
        //Debug.Log($"{gameObject.name}가 {attacker.name}로부터 {damage}의 피해를 입었습니다.");
        if (GetComponent<Rat>() && attackInfo == "Fear")
        {
            GetComponent<Rat>().Hit(attacker);
            
        }
        else
            Debug.Log($"{gameObject.name}가 {attacker.name}로부터 {damage}의 피해를 입었습니다.");
    }
    public bool isHitTarget()
    {
        return true;
    }
}
