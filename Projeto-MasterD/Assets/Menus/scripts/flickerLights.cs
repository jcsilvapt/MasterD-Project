using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flickerLights : MonoBehaviour
{
   public float flickerSpeed;
    public Light luz;
    private void Start()
    {
        luz.enabled = true;
    }
    void Update()
    {
    
        if (luz.enabled == false)
        {
            StartCoroutine(FlickerIT());
        }
        else
        {
            luz.enabled = false;
        }
    }
    IEnumerator FlickerIT()
    {

        yield return new WaitForSeconds(flickerSpeed);
        luz.enabled = true;
    }
}
