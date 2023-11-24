using UnityEngine;

public class FogController : MonoBehaviour
{
    public bool enableFog = true;
    public float fogDensity = 0.02f;
    public Color fogColor = Color.black;

    void Start()
    {
        RenderSettings.fog = enableFog;
        RenderSettings.fogDensity = fogDensity;
        RenderSettings.fogColor = fogColor;
    }
}

