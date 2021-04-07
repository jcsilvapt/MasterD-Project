using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
public class buttons : MonoBehaviour
{
    public CinemachineVirtualCamera cam1;
    public CinemachineVirtualCamera cam2;
    public CinemachineVirtualCamera cam3;
    public CinemachineVirtualCamera cam4;
    public void StartGame()
    {
        if (cam1)
        {
            cam1.Priority = 0;
            cam2.Priority = 1;
            cam3.Priority = 0;
            cam4.Priority = 0;
        }
    }
    public void Options()
    {
        if (cam2)
        {
            cam1.Priority = 0;
            cam2.Priority = 0;
            cam3.Priority = 1;
            cam4.Priority = 0;
        }
    }
    public void Back()
    {
        if (cam3)
        {
            cam1.Priority = 0;
            cam2.Priority = 1;
            cam3.Priority = 0;
            cam4.Priority = 0;
        }
    }
    public void Credits()
    {
        if (cam2)
        {
            cam1.Priority = 0;
            cam2.Priority = 0;
            cam3.Priority = 0;
            cam4.Priority = 1;
        }
    }
    public void Quit()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void MainMenu()
    {
        //adicionar o numero da cena quando tiver organizado
    }
    public void NewGame()
    {
        GameManager.ChangeScene(1, true);
    }
}
