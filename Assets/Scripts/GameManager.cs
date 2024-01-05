using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager instance
    {
        get
        {
            if (_instance is null)
            {
                _instance = (GameManager)FindObjectOfType(typeof(GameManager));
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }
    private Player _player;
    public Player player
    {
        get
        {
            if(_player == null)
            {
                _player = (Player)FindObjectOfType(typeof(Player));
            }
            return _player;
        }
    }
    private UIManager _uiManager;
    public UIManager uiManager
    {
        get
        {
            if (_uiManager == null)
            {
                _uiManager = (UIManager)FindObjectOfType(typeof(UIManager));
                DontDestroyOnLoad(_uiManager.gameObject);
            }
            return _uiManager;
        }
    }


    private void Awake()
    {
        if (_instance != this)
            Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            uiManager.Pause();
    }
}
