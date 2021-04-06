using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureFlciker : MonoBehaviour
{
    public Material cleanWhite;
    public Material oldWhite;

    void Start()
    {
        Invoke("cleanSet", 2);
    }

    void Update()
    {
        
    }
   void cleanSet()
    {
        GetComponent<Renderer>().material = cleanWhite;
        Invoke("oldSet", 5);
    }
    void oldSet()
    {
        GetComponent<Renderer>().material = oldWhite;
        Invoke("cleanSet", 0.5f);
    }
}
