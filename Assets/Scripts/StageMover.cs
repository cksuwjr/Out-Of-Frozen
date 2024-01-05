using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMover : MonoBehaviour
{
    [SerializeField] string next;
    [TextArea]
    [SerializeField] string tip;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            LoadingSceneManager.SetTipText(tip);
            LoadingSceneManager.LoadScene(next);
        }
    }
}
