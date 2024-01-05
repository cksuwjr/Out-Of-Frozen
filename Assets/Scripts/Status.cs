using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{

    [SerializeField] protected float _MaxHp = 1f;
    [SerializeField] protected float _Hp = 1f;
    [SerializeField] protected int _AttackPower = 1;
    [SerializeField] protected float _Tenasious = 0f;
    [SerializeField] protected float _Speed = 5f;

    public float MaxHp { get { return _MaxHp; } set { _MaxHp = value; } }
    public float Hp { get { return _Hp; } set { _Hp = value; } }
    public int AttackPower { get { return _AttackPower; } set { _AttackPower = value; } }
    public float Tenasious { get { return _Tenasious; } set { _Tenasious = value; } }
    public float Speed { get { return _Speed; } set { _Speed = value; } }
}
