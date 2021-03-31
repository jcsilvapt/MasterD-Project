using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiTest : MonoBehaviour {

    public void LoadWithLoader() {
        GameManager.ChangeScene(1, true);
    }

    public void LoadWithoutLoader() {
        GameManager.ChangeScene(1, false);
    }

}
