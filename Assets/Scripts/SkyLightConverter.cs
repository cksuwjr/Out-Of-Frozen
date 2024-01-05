using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SkyLightConverter : MonoBehaviour
{
    Camera camera;
    public Light2D light;
    public Light2D backgroundLight;

    [SerializeField] float startX;
    [SerializeField] float endX;

    [SerializeField] float nowMaxX;

    public Gradient skyColor;

    [SerializeField] float startFlood;
    [SerializeField] float endFlood;

    [SerializeField] float startIntensity;
    [SerializeField] float endIntensity;

    private void Awake()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        nowMaxX = startX;
        light.intensity = startIntensity;

        Convert();
    }
    public void Convert()
    {
        nowMaxX = GameManager.instance.player.transform.position.x;
        float flood = nowMaxX / (endX - startX);

        Color c;
        c = skyColor.Evaluate(flood);
        camera.backgroundColor = new Color(c.r, c.g, c.b);

        //Debug.Log(flood);
        if (startFlood < flood && flood < endFlood)
        {
            if (light)
            {
                light.intensity = startIntensity + (endIntensity - startIntensity) * ((flood - startFlood) / (endFlood - startFlood));
            }
            //Debug.Log(light.intensity);
            if (backgroundLight) backgroundLight.intensity = light.intensity;
        }
        else if (endFlood <= flood)
        {
            if (light) light.intensity = endIntensity;
            if (backgroundLight) backgroundLight.intensity = light.intensity;
        }
    }
    private void Update()
    {
        if (nowMaxX >= GameManager.instance.player.transform.position.x) return;
        Convert();
    }
}
