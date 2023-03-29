using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAM : MonoBehaviour
{
    public GameObject Target;
    [Range(-1f,3f)] public float intensity = 0.25f;
    public float fixUp = 0f;
    // Update is called once per frame
    void Update()
    {
        if (Target == null) Target = GameObject.FindGameObjectWithTag("Player");
        if (Target == null) return;

        transform.position = new Vector3(Target.transform.position.x, ((Target.transform.position.y) * intensity) + fixUp, transform.position.z);
    }
}
