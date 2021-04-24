using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Profile Data", menuName ="Game/Create Game Profile")]
[System.Serializable]
public class SO_PlayerData : ScriptableObject {

    [Header("Current Status")]
    public int currentScene;
    public bool hasKey;
    public bool hasLantern;
    public bool gateOpen;
    public Vector3 currentPosition;

}
