using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour {

    [Header("Input Settings")]
    [Range(1f, 10f)]
    public float inputSensitivity = 3f;

    // Private
    private Vector2 newInput;   // Stores the current value from input
    private Animator anim;      // Stores the animator of the Character

    private Vector2 deltaInput; // Fixes the Current value from the input

    private void Start() {
        anim = GetComponent<Animator>();
    }

    private void Update() {
        anim.SetFloat("horizontal", deltaInput.x);
        anim.SetFloat("vertical", deltaInput.y);
    }

    private void FixedUpdate() {
        PrepareControlsInput();
    }

    public void OnMove(InputValue t) {

        newInput = t.Get<Vector2>();

    }

    private void PrepareControlsInput() {

        deltaInput.x = Mathf.MoveTowards(deltaInput.x, newInput.x, Time.deltaTime * inputSensitivity);
        deltaInput.y = Mathf.MoveTowards(deltaInput.y, newInput.y, Time.deltaTime * inputSensitivity);

        deltaInput = Vector2.ClampMagnitude(deltaInput, 1f);

    }

    public void OnInteraction() {
        Debug.Log("OH YEAH");
    }

}
