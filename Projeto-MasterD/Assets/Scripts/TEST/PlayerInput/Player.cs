using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{

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

    //change animations
    public bool flashLight = false;
    public GameObject lighter; // lanterna para testes
    public bool pickitup = false;

    // Private
    private Vector2 move, look;   // Stores the current value from input
    private Animator anim;      // Stores the animator of the Character
    private bool canMove = true;
    public bool ableToPeak = false;

    private Vector2 deltaInput; // Fixes the Current value from the input

    private void Start()
    {
        anim = GetComponent<Animator>();
        lighter.SetActive(false);
    }

    #region INPUTS

    public void OnMove(InputValue t)
    {
        move = t.Get<Vector2>();
    }

    public void OnInteraction()
    {
        ableToPeak = !ableToPeak;
    }

    public void OnLook(InputValue value)
    {
        look = value.Get<Vector2>();
    }

    public void OnFreeLook()
    {
        freeLook = !freeLook;
    }

    #endregion



    private void PrepareControlsInput()
    {

        deltaInput.x = Mathf.MoveTowards(deltaInput.x, move.x, Time.deltaTime * inputSensitivity);
        deltaInput.y = Mathf.MoveTowards(deltaInput.y, move.y, Time.deltaTime * inputSensitivity);

        deltaInput = Vector2.ClampMagnitude(deltaInput, 1f);

    }

    private void Update()
    {
        // Checks if the player can move
        if (canMove)
        {

            #region Follow Transform Rotation

            followTransform.transform.rotation *= Quaternion.AngleAxis(look.x * rotationPower, Vector3.up);

            #endregion

            #region Follow Transform Vertical Rotation

            followTransform.transform.rotation *= Quaternion.AngleAxis(-look.y * rotationPower, Vector3.right);

            var angles = followTransform.transform.localEulerAngles;
            angles.z = 0;

            var angle = followTransform.transform.localEulerAngles.x;

            if (angle > 180 && angle < 340)
            {
                angles.x = 340;
            }
            else if (angle < 180 && angle > 40)
            {
                angles.x = 40;
            }

            followTransform.transform.localEulerAngles = angles;

            #endregion

            #region Transform Rotation

            nextRotation = Quaternion.Lerp(followTransform.transform.rotation, nextRotation, Time.deltaTime * rotationPower);

            if (freeLook)
            {
                nextPosition = transform.position;
                anim.SetFloat("horizontal", 0);
                anim.SetFloat("vertical", 0);
                return;
            }

            transform.rotation = Quaternion.Euler(0, followTransform.transform.rotation.eulerAngles.y, 0);

            followTransform.transform.localEulerAngles = new Vector3(angles.x, 0, 0);

            #endregion

            #region Animator Control

            anim.SetFloat("horizontal", deltaInput.x);
            anim.SetFloat("vertical", deltaInput.y);
         
            #endregion

        }
        else
        {
            anim.SetFloat("horizontal", 0);
            anim.SetFloat("vertical", 0);
        }

        #region Animations Change
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (flashLight == false)
            {
                anim.SetBool("FlashLight", true);
                flashLight = true;
                lighter.SetActive(true);
            }
            else
            {
                anim.SetBool("FlashLight", false);
                flashLight = false;
                lighter.SetActive(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            anim.speed = 1.2f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            anim.speed = 1f;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            anim.Play("PickItUp");
        }
       

        #endregion
    }

    private void FixedUpdate()
    {
        PrepareControlsInput();
    }

    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
    }

}
