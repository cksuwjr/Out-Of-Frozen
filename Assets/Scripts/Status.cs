using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{

    [SerializeField] protected float _MaxHp = 100f;
    [SerializeField] protected float _Hp = 100f;

    public float MaxHp { get { return _MaxHp; } set { _MaxHp = value; } }
    public float Hp { get { return _Hp; } set { _Hp = value; } }
}
