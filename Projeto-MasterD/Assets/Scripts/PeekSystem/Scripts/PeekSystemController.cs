using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PeekSystemController : MonoBehaviour
{
    //Brain Reference
    private CinemachineBrain brain;

    //Camera in that Area
    public CinemachineVirtualCamera cameraToBeActive;

    //Player Camera
    public CinemachineVirtualCamera cameraPlayer;

    //UI Reference
    public GameObject UIPeek;

    public bool canTransition;

    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        //Get References
        brain = Camera.main.gameObject.GetComponent<CinemachineBrain>();
        cameraPlayer = (CinemachineVirtualCamera) brain.ActiveVirtualCamera;

        //Set Current Active Camera
        canTransition = false;

        ActivateUI(false);
    }

    private void Update()
    {
        if (canTransition == true && !brain.IsBlending && Input.GetKeyDown(KeyCode.E))
        {
            if ((CinemachineVirtualCamera)brain.ActiveVirtualCamera == cameraPlayer)
            {
                int lowerPriority = cameraToBeActive.Priority;
                cameraToBeActive.Priority = cameraPlayer.Priority;
                cameraPlayer.Priority = lowerPriority;

                playerController.SetCanMove(false);
                playerController.StopPlayer();
            }
            else if ((CinemachineVirtualCamera)brain.ActiveVirtualCamera == cameraToBeActive)
            {
                int lowerPriority = cameraToBeActive.Priority;
                cameraToBeActive.Priority = cameraPlayer.Priority;
                cameraPlayer.Priority = lowerPriority;

                playerController.SetCanMove(true);
            }
        }
    }

    /// <summary>
    /// Activates or Deactivates Peek UI
    /// </summary>
    /// <param name="isActive">true - Activates UI, false - Deactivates UI</param>
    private void ActivateUI(bool isActive)
    {
        UIPeek.SetActive(isActive);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            canTransition = true;
            playerController = other.GetComponent<PlayerController>();

            ActivateUI(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            canTransition = false;
            playerController = null;

            ActivateUI(false);
        }
    }
}
