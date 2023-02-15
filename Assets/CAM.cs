using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAM : MonoBehaviour
{
    public GameObject Target;
    // Update is called once per frame
    void Update()
    {
        if (Target == null) return;

        transform.position = new Vector3(Target.transform.position.x, transform.position.y, transform.position.z);
    }
}
