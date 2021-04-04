using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PeekCameraController : MonoBehaviour
{
    //Brain Reference
    public CinemachineBrain brain;

    //Peek Camera Reference
    private CinemachineVirtualCamera peekCamera;

    //Constraints
    public float leftConstraint;
    public float rightConstraint;
    public float topConstraint;
    public float bottomConstraint;

    //Auxiliar Variables
    private float horizontalRotation;
    private float verticalRotation;

    //Camera Sensitivity
    public float cameraSensitivity;

    private void Start()
    {
        //Get References
        brain = Camera.main.GetComponent<CinemachineBrain>();
        peekCamera = GetComponent<CinemachineVirtualCamera>();

        horizontalRotation = 0;
        verticalRotation = 0;
    }

    private void Update()
    {
        if((CinemachineVirtualCamera) brain.ActiveVirtualCamera == peekCamera)
        {
            horizontalRotation += Input.GetAxisRaw("Mouse X") * cameraSensitivity * Time.deltaTime;
            verticalRotation -= Input.GetAxisRaw("Mouse Y") * cameraSensitivity * Time.deltaTime;

            transform.localEulerAngles = new Vector3(Mathf.Clamp(verticalRotation, topConstraint, bottomConstraint), Mathf.Clamp(horizontalRotation, leftConstraint, rightConstraint), 0);
        }
    }
}
