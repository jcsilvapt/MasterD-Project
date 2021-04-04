using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour {

    [Header("Input Settings")]
    [Range(1f, 10f)]
    [SerializeField] float inputSensitivity = 3f;

    [Header("Mouse Settings")]
    [SerializeField] float rotationPower = 1f;
    [SerializeField] float rotationLerp = 1f;
    [SerializeField] Camera camera;
    [SerializeField] GameObject followTransform;

    [SerializeField] Vector3 nextPosition;
    [SerializeField] Quaternion nextRotation;

    private bool freeLook = false;

    // Private
    private Vector2 move, look;   // Stores the current value from input
    private Animator anim;      // Stores the animator of the Character

    private Vector2 deltaInput; // Fixes the Current value from the input

    private void Start() {
        anim = GetComponent<Animator>();
    }

    #region INPUTS

    public void OnMove(InputValue t) {
        move = t.Get<Vector2>();
    }

    public void OnInteraction() {
        Debug.Log("OH YEAH");
    }

    public void OnLook(InputValue value) {
        look = value.Get<Vector2>();
    } 

    public void OnFreeLook() {
        freeLook = !freeLook;
    }

    #endregion



    private void PrepareControlsInput() {

        deltaInput.x = Mathf.MoveTowards(deltaInput.x, move.x, Time.deltaTime * inputSensitivity);
        deltaInput.y = Mathf.MoveTowards(deltaInput.y, move.y, Time.deltaTime * inputSensitivity);

        deltaInput = Vector2.ClampMagnitude(deltaInput, 1f);

    }

    private void Update() {

        #region Follow Transform Rotation

        followTransform.transform.rotation *= Quaternion.AngleAxis(look.x * rotationPower, Vector3.up);

        #endregion

        #region Follow Transform Vertical Rotation

        followTransform.transform.rotation *= Quaternion.AngleAxis(-look.y * rotationPower, Vector3.right);

        var angles = followTransform.transform.localEulerAngles;
        angles.z = 0;

        var angle = followTransform.transform.localEulerAngles.x;

        if(angle > 180 && angle < 340) {
            angles.x = 340;
        }
        else if(angle < 180 && angle > 40) {
            angles.x = 40;
        }

        followTransform.transform.localEulerAngles = angles;

        #endregion

        nextRotation = Quaternion.Lerp(followTransform.transform.rotation, nextRotation, Time.deltaTime * rotationPower);

        if(freeLook) {
            nextPosition = transform.position;
            return;
        }

        transform.rotation = Quaternion.Euler(0, followTransform.transform.rotation.eulerAngles.y, 0);

        followTransform.transform.localEulerAngles = new Vector3(angles.x, 0, 0);

        anim.SetFloat("horizontal", deltaInput.x);
        anim.SetFloat("vertical", deltaInput.y);
    }

    private void FixedUpdate() {
        PrepareControlsInput();
    }



}
