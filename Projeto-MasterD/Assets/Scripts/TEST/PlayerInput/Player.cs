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

    [Header("IK Settings")]
    [SerializeField] Transform ikObjective;
    [SerializeField] Vector3 rightShoulderRot;
    [SerializeField] Vector3 rightUpperArmRot;
    [SerializeField] Vector3 rightLowerArmRot;
    [SerializeField] Vector3 rightHandRot;

    [Header("Light Settings")]
    [SerializeField] GameObject lightIdle;
    [SerializeField] GameObject lightInUse;

    [Header("Developer Settings")]
    [Tooltip("Set true to display debug rays in Edit Window")]
    [SerializeField] bool displayDebugRays = false;
    [Tooltip("Set true to fix and Hold Aim System")]
    [SerializeField] bool staticAim = false;
    [Tooltip("Shows if the player is currently Aiming")]
    [SerializeField] bool isAiming = false;


    private Vector3 aimPos;


    public bool hasFlashLight = false;
    public GameObject lighter; // lanterna para testes
    public bool pickitup = false;

    // Private
    private bool freeLook = false;
    private Vector2 move, look;   // Stores the current value from input
    private Animator anim;      // Stores the animator of the Character
    private bool canMove = true;
    public bool ableToPeak = false;

    private Vector2 deltaInput; // Fixes the Current value from the input

    private void Start() {
        anim = GetComponent<Animator>();
        // Disables all the Lights at the beginning
        lightIdle.SetActive(false);
        lightInUse.SetActive(false);
        //lighter.SetActive(false);
    }

    #region INPUTS

    public void OnMove(InputValue t) {
        move = t.Get<Vector2>();
    }

    public void OnInteraction() {
        ableToPeak = !ableToPeak;
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
        // Checks if the player can move
        if (canMove) {

            #region Follow Transform Rotation

            followTransform.transform.rotation *= Quaternion.AngleAxis(look.x * rotationPower, Vector3.up);

            #endregion

            #region Follow Transform Vertical Rotation

            followTransform.transform.rotation *= Quaternion.AngleAxis(-look.y * rotationPower, Vector3.right);

            var angles = followTransform.transform.localEulerAngles;
            angles.z = 0;

            var angle = followTransform.transform.localEulerAngles.x;

            if (angle > 180 && angle < 340) {
                angles.x = 340;
            } else if (angle < 180 && angle > 40) {
                angles.x = 40;
            }

            followTransform.transform.localEulerAngles = angles;

            #endregion

            #region Transform Rotation

            nextRotation = Quaternion.Lerp(followTransform.transform.rotation, nextRotation, Time.deltaTime * rotationPower);

            if (freeLook) {
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

        } else {
            anim.SetFloat("horizontal", 0);
            anim.SetFloat("vertical", 0);
        }


        #region Animations Change

        if (staticAim) {
            hasFlashLight = true;
        }

        if (hasFlashLight) {

            isAiming = Input.GetButton("Fire2") || staticAim;

            // When Player has a FlashLight sets the idle flashlight on
            lightIdle.SetActive(!isAiming);
            lightInUse.SetActive(isAiming);

            if (isAiming) {

                anim.SetBool("FlashLight", isAiming || staticAim);

                Ray r = Camera.main.ScreenPointToRay(new Vector2((Screen.width / 2) + 50, Screen.height / 2 + 50));

                RaycastHit hitInfo;

                aimPos = Vector3.zero;

                if (Physics.Raycast(r, out hitInfo, 20)) {
                    aimPos = hitInfo.point;
                } else {
                    aimPos = r.origin + r.direction * 20;
                }

                if (displayDebugRays) {
                    float size = 0.2f;
                    Vector3 up = Vector3.up * size;
                    Vector3 sd = Vector3.right * size;
                    Vector3 fw = Vector3.forward * size;
                    Debug.DrawLine(aimPos + up, aimPos - up, Color.red);
                    Debug.DrawLine(aimPos + sd, aimPos - sd, Color.red);
                    Debug.DrawLine(aimPos + fw, aimPos - fw, Color.red);
                    Debug.DrawLine(r.origin, aimPos, Color.red);
                }
            } else {
                anim.SetBool("FlashLight", false);
            }
        }




        /*
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
        */
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            anim.speed = 1.2f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)) {
            anim.speed = 1f;
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            anim.Play("PickItUp");
        }


        #endregion
    }



    private void FixedUpdate() {
        PrepareControlsInput();
    }
    private void LateUpdate() {

        if (isAiming) {
            #region ARM
            Transform upperArm = anim.GetBoneTransform(HumanBodyBones.RightUpperArm);
            Transform lowerArm = anim.GetBoneTransform(HumanBodyBones.RightLowerArm);
            Transform shoulder = anim.GetBoneTransform(HumanBodyBones.RightShoulder);

            shoulder.LookAt(ikObjective);
            upperArm.LookAt(ikObjective);
            lowerArm.LookAt(ikObjective);

            upperArm.rotation = upperArm.rotation * Quaternion.Euler(rightUpperArmRot);
            lowerArm.rotation = lowerArm.rotation * Quaternion.Euler(rightLowerArmRot);
            shoulder.rotation = shoulder.rotation * Quaternion.Euler(rightShoulderRot);
            #endregion

            #region HAND

            Transform hand = anim.GetBoneTransform(HumanBodyBones.RightHand);

            hand.LookAt(ikObjective);

            hand.rotation = hand.rotation * Quaternion.Euler(rightHandRot);

            #endregion
        }

    }

    public void SetCanMove(bool canMove) {
        this.canMove = canMove;
    }

}
