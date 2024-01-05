using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class UIManager : MonoBehaviour
{
    [SerializeField] Image PlayerHeartUI;
    [SerializeField] Image PauseUI;
    [SerializeField] GameObject PlayerUI;
    //[SerializeField] Image invincible_Image;

    private void Awake()
    {
        if (GameManager.instance.uiManager != this)
            Destroy(gameObject);
    }
    public void SetPlayerHeartUI()
    {
        Status stat = GameObject.Find("Player").GetComponent<Status>();
        PlayerHeartUI.fillAmount = stat.Hp / stat.MaxHp;
        if (stat.Hp <= 0)
            SetPlayerUIFalse();
    }
    //public void SetinvincibleGageUI()
    //{

    //}
    public void SetPlayerUIFalse()
    {
        PlayerUI.gameObject.SetActive(false);
    }
    public void SetPlayerUITrue()
    {
        PlayerUI.gameObject.SetActive(true);
    }

    public void Pause()
    {
        if (PauseUI.gameObject.activeSelf) return;
        SetPlayerUIFalse();
        PauseUI.gameObject.SetActive(true);
        Time.timeScale = 0;
    }
    public void Resume()
    {
        SetPlayerUITrue();
        PauseUI.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    public void Replay()
    {
        Time.timeScale = 1;
        PauseUI.gameObject.SetActive(false);
        LoadingSceneManager.ReloadScene();
    }
}
