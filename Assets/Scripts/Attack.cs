using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Attack : MonoBehaviour
{
    // ¿Ã∫•∆Æ
    [Header("Events")]
    [Space]
    public UnityEvent OnAttackEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private void Awake()
    {
        if (OnAttackEvent == null)
            OnAttackEvent = new UnityEvent();
    }




    public ObjectController controller;
    string attack = "";
    void Start()
    {
        if (!controller)
            controller = GetComponent<ObjectController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            attack = "Basic";
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            attack = "Fear";
        }
    }
    private void FixedUpdate()
    {
        if (attack == "Basic")
        {
            controller.Attack(new Vector3(5, 1), Vector3.zero);
        }
        else if (attack == "Fear")
        {
            controller.Attack(new Vector3(5, 1), Vector3.zero, "Middle", "Fear");
        }
        attack = "None";
    }
    public int CalcDamage(GameObject attacker, GameObject deffender, int damage, string attackInfo = "")
    {
        int attackdamage = damage;
        return attackdamage;
    }
    public bool isAttackTarget(GameObject deffender)
    {
        return true;
    }
}
