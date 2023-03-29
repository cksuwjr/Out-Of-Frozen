using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{

    public enum Type
    {
        Fixed,
        CAMAttached,
        OutoMove,
    }

    private Renderer render;
    private float offset;
    public float Speed = 1f;
    public Type type;
    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<Renderer>();
        //if (transform.parent.tag == "MainCamera")
        //    type = Type.CAMAttached;
    }
    private void FixedUpdate()
    {
        if (type == Type.OutoMove)
        {
            offset += Speed * Time.fixedDeltaTime;
            render.material.mainTextureOffset = new Vector2(offset, 0);
        }
    }
    // Update is called once per frame
    public void MoveBackground(float x)
    {
        if (type == Type.Fixed) return;
        if (type == Type.OutoMove) return;
        if (type == Type.CAMAttached)
        {
            offset += x * Speed;
            render.material.mainTextureOffset = new Vector2(offset, 0);
        }
    }
}
