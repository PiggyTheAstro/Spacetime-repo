using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadScript : MonoBehaviour
{
    float factor;
    void Start() 
    { 
        factor = 1.0f / transform.localScale.x; 
    }

    void Update() 
    { 
        GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(Camera.main.transform.position.x * factor, Camera.main.transform.position.y*factor)); 
    }
}
