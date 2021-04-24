using UnityEngine;

[System.Serializable]
public class SaveProfile {

    public int currentScene;        // Saves the current scene that the player is currently on.
    public bool hasKey;             // Saves if the player has the Key.
    public bool hasLantern;         // Saves if the player has the lantern.
    public bool gateOpen;           // Saves the current state of the Gate.
    public float x, y, z;           // Saves the current position of the player.
}
