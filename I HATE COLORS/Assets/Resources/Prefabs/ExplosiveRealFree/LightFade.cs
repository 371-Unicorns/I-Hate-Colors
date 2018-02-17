using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE What is this for? It is not used anywhere? (David 2/16)
public class LightFade : MonoBehaviour
{

    public Light light;
    // Use this for initialization
    void Start()
    {
        light = GetComponent<Light>();
        light.color = Color.yellow;
    }

    // Update is called once per frame
    void Update()
    {
        light.range = Mathf.Lerp(light.range, 0, Time.deltaTime);

    }
}
