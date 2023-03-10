using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Image PlayerHeartUI;
    [SerializeField] GameObject PlayerUI;
    public void SetPlayerHeartUI()
    {
        Status stat = GameObject.Find("Player").GetComponent<Status>();
        PlayerHeartUI.fillAmount = stat.Hp / stat.MaxHp;
    }
    public void SetPlayerUIFalse()
    {
        PlayerUI.gameObject.SetActive(false);
    }
}
