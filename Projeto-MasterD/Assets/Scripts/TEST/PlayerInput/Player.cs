using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour {

    public Vector2 normalInput;
    public Vector2 newInput;
    private Animator anim;
    public float dumpTemp = 0.25f;
    public float speed = 1f;



    private void Start() {
        anim = GetComponent<Animator>();
    }

    private void Update() {
        normalInput.x = Input.GetAxis("Horizontal");
        normalInput.y = Input.GetAxis("Vertical");
    }

    public void OnMove(InputValue t) {

        var value = t.Get<Vector2>();
        float vX = 0f; ;
        float vY = 0f;
        if (value.x > 0) { vX += 1f; }
        else if(value.x < 0) { vX -=1f; }

        vX = Mathf.Clamp(vX, -1, 1);

        if(value.y > 0) { vY += 1f; }
        else if(value.y < 0) { vY -= 1f; }

        newInput.x = vX;
        newInput.y = vY;

        anim.SetFloat("horizontal", vX);
        anim.SetFloat("vertical", vY);

    }

    public void OnInteraction() {
        Debug.Log("OH YEAH");
    }

}
