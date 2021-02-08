using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Underwater : MonoBehaviour {

    public Color underWaterColor;
    public float fogDensity;

    void Start() {
        RenderSettings.fog = true;
        RenderSettings.fogColor = underWaterColor;
        RenderSettings.fogDensity = fogDensity;
        RenderSettings.skybox = null;
    }
}
