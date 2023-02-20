using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSpawner : MonoBehaviour
{
    [SerializeField] Vector3 SpawnPosition = Vector3.zero;

    [SerializeField] GameObject Fear;

    public void Spawn(string EffectName, float time)
    {
        GameObject appear = null;
        if (EffectName == "Fear")
            appear = Fear;

        if (appear == null) return;
        GameObject me = Instantiate(appear, transform.position + SpawnPosition, Quaternion.identity);
        Destroy(me, time);
        StartCoroutine(FloatingMyself(me));
    }
    
    IEnumerator FloatingMyself(GameObject appear)
    {
        while (true && appear)
        {
            appear.transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            appear.transform.position = transform.position + SpawnPosition;
            yield return null;
        }

    }
}
