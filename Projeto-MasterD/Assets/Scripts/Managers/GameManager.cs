﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    // Singleton
    public static GameManager instance;

    [Header("Load System")]

    [SerializeField] GameObject loadCanvas;
    [SerializeField] Slider loadSlider;

    [Header("Developer Settings")]

    [Tooltip("Enable this to show the Splash Screen on Play")]
    [SerializeField] bool showSplashScreen = true;

    [Tooltip("Enable this to show the Title Screen on Play")]
    [SerializeField] bool showTitleScreen = true;

    [Tooltip("Enable this to Skip Menu")]
    [SerializeField] bool forceGame = false;

    [Header("RUNTIME VISUALIZATION DON'T CHANGE")]

    [SerializeField] bool isPaused = false;
    [SerializeField] GameState activeState;

    // Private
    private GameState currentState = GameState.None;



    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitialSettings();
        } else {
            Destroy(gameObject);
        }
    }

    private void InitialSettings() {
        loadCanvas.SetActive(false);
    }


    private void Update() {

        if (currentState == activeState) {
            return;
        } else {
            currentState = activeState;

            switch (currentState) {
                case GameState.SplashScreen:
                    //TODO: SelectScene;
                    break;
                case GameState.TitleScreen:
                    //TODO: SelectScene;
                    break;
                case GameState.MainMenu:
                    //TODO: SelectScene;
                    break;
                case GameState.Game:
                    //TODO: SelectScene;
                    break;
                default:
                    break;
            }
        }
    }


    #region WRAPPERS

    /// <summary>
    /// Function that Changes the current Scene.
    /// Use allowLoadScreen if the scene you'r loading needs pre-load.
    /// </summary>
    /// <param name="sceneIndex">ID of the Scene to be loaded</param>
    /// <param name="allowLoadScreen">If True, the loading Screen will appear</param>
    public static void ChangeScene(int sceneIndex, bool allowLoadScreen) {
        if (instance != null) {
            instance._changeScene(sceneIndex, allowLoadScreen);
        }
    }

    /// <summary>
    /// Function that Pauses the Game
    /// </summary>
    public static void Pause() {
        if(instance != null) {
            instance._pause(true);
        }
    }

    /// <summary>
    /// Function that Resumes the Game
    /// </summary>
    public static void Resume() {
        if(instance != null) {
            instance._pause(false);
        }
    }

    /// <summary>
    /// Function that Closes the Game
    /// </summary>
    public static void QuitGame() {
        if(instance != null) {
            instance.CloseGame();
        }
    }


    #endregion

    #region SCENE RELATED

    /// <summary>
    /// Function that Changes the current Scene.
    /// Use allowLoadScreen if the scene you'r loading needs pre-load.
    /// </summary>
    /// <param name="index">ID of the Scene to be loaded</param>
    /// <param name="isToLoad">If True, the loading Screen will appear</param>
    private void _changeScene(int index, bool isToLoad) {
        if (isToLoad) {
            loadCanvas.SetActive(true);
            StartCoroutine(LoadScreen(index));
        } else {
            SceneManager.LoadScene(index);

        }
    }

    #endregion


    #region LOGIC

    /// <summary>
    /// Function that Controls the Pause System
    /// </summary>
    /// <param name="value">True to Pause; False to Resume</param>
    private void _pause(bool value) {
        isPaused = value;
        Time.timeScale = isPaused ? 0 : 1;
        Cursor.visible = isPaused ? true : false;
    }

    /// <summary>
    /// Function that Closes the Game
    /// </summary>
    private void CloseGame() {
        //TODO: Possivelmente adicionar o sistema de Guardar Dados ao Fechar o Jogo...
        Application.Quit();
    }

    #endregion


    #region COROUTINES

    /// <summary>
    /// COROUTINE: Shows the LoadScreen when enable
    /// </summary>
    /// <param name="sceneIndex"></param>
    /// <returns></returns>
    IEnumerator LoadScreen(int sceneIndex) {

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone) {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            loadSlider.value = progress;

            yield return null;
        }

        loadSlider.value = 0;
        loadCanvas.SetActive(false);
    }

    #endregion
}