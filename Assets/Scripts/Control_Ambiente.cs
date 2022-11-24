using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control_Ambiente : MonoBehaviour
{
    public float velocityRotation;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation",Time.time*velocityRotation);

    }
}
