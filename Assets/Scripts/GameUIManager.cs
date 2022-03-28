using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    private PlayerControls playerControls;
    private PlayerManager playerManager;

    [Header("Menu")]
    public GameObject menuWallpaper;
    public GameObject menuOptions;

    [Header("Start")]
    public GameObject startButton;
    public GameObject restartButton;

    [Header("Ammo UI")]
    public GameObject ammoUI;

    [Header("Help Option")]
    public GameObject helpOption;

    [Header("Player Health")]
    public Slider healthSlider;

    [Header("Game")]
    public GameObject gameLose;
    public GameObject gameWin;
    public GameObject gameText;
    public GameObject difficultyOption;

    [Header("Game Objective")]
    public GameObject gameObjective;

    [Header("Flags")]
    public bool pauseInput = false;
    public int isPause = 0;
    static private int loadScreen = 0;
    private bool isHelp = false;
    private bool isStart = false;

    private void Awake()
    {
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();

        //Debug.Log("Load scene times: " + loadScreen);
        if (loadScreen == 0)
        {
            StartPanel();
            loadScreen++;
            isStart = true;
        }
        else
        {
            StartButton();
        }
    }

    private void Update()
    {
        MenuPanel(); 
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            // on presseing ESC key
            playerControls.PlayerActions.Menu.performed += i => pauseInput = true;
            // on let go ESC key
            playerControls.PlayerActions.Menu.canceled += i => pauseInput = false;
        }
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void StartPanel()
    {
        Time.timeScale = 0;
        menuWallpaper.gameObject.SetActive(true);
        menuOptions.gameObject.SetActive(true);
        ammoUI.gameObject.SetActive(false);
        startButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(false);
        helpOption.gameObject.SetActive(false);
        healthSlider.gameObject.SetActive(false);
        gameLose.gameObject.SetActive(false);
        gameText.gameObject.SetActive(true);
        difficultyOption.gameObject.SetActive(false);
    }

    private void MenuPanel()
    {
        if (pauseInput && isPause == 0 && !isHelp && !isStart)
        {
            menuOptions.gameObject.SetActive(true);
            healthSlider.gameObject.SetActive(false);
            Time.timeScale = 0;
            pauseInput = false;
            isPause = 1;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            return;
        }
        if(pauseInput && isPause == 1 && !isHelp && !isStart)
        {
            menuOptions.gameObject.SetActive(false);
            healthSlider.gameObject.SetActive(true);
            Time.timeScale = 1;
            pauseInput = false;
            isPause = 0;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            return;
        }
    }

    public void StartButton()
    {
        Time.timeScale = 1;
        menuOptions.gameObject.SetActive(false);
        ammoUI.gameObject.SetActive(true);
        startButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(true);
        helpOption.gameObject.SetActive(false);
        difficultyOption.gameObject.SetActive(true);
    }

    public void GameWon()
    {
        menuOptions.gameObject.SetActive(true);
        healthSlider.gameObject.SetActive(false);
        Time.timeScale = 0;
        pauseInput = false;
        isPause = 1;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void RestartButton()
    {
        menuOptions.gameObject.SetActive(false);
        helpOption.gameObject.SetActive(false);
        gameWin.gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        difficultyOption.gameObject.SetActive(true);
        menuWallpaper.gameObject.SetActive(true);
    }

    public void HelpButton()
    {
        menuOptions.gameObject.SetActive(false);
        helpOption.gameObject.SetActive(true);
        gameWin.gameObject.SetActive(false);
        isHelp = true;

    }

    public void BackButton()
    {
        menuOptions.gameObject.SetActive(true);
        helpOption.gameObject.SetActive(false);
        isHelp = false;
    }

    public void GameEnd()
    {
        menuOptions.gameObject.SetActive(true);
        healthSlider.gameObject.SetActive(false);
        gameLose.gameObject.SetActive(true);
        gameText.gameObject.SetActive(false);
        Time.timeScale = 0;
        pauseInput = false;
        isPause = 1;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void EasyButton()
    {
        playerManager.gameDifficulty = 0.05f;
        difficultyOption.gameObject.SetActive(false);
        menuWallpaper.gameObject.SetActive(false);
        healthSlider.gameObject.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isStart = false;
        gameObjective.gameObject.SetActive(true);
        StartCoroutine(GameObjective());
    }

    public void MediumButton()
    {
        playerManager.gameDifficulty = 0.1f;
        menuWallpaper.gameObject.SetActive(false);
        difficultyOption.gameObject.SetActive(false);
        healthSlider.gameObject.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isStart = false;
        gameObjective.gameObject.SetActive(true);
        StartCoroutine(GameObjective());
    }

    public void HardButton()
    {
        playerManager.gameDifficulty = 0.2f;
        difficultyOption.gameObject.SetActive(false);
        menuWallpaper.gameObject.SetActive(false);
        healthSlider.gameObject.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isStart = false;
        gameObjective.gameObject.SetActive(true);
        StartCoroutine(GameObjective());
    }

    IEnumerator GameObjective()
    {
        yield return new WaitForSeconds(10);
        gameObjective.gameObject.SetActive(false);
    }

}
