using UnityEngine;
using UnityEngine.UI;
using System;
using Utils.StateMachine;
using StateMachine;
using UnityEngine.SceneManagement;

public class PlayerUI : MonoBehaviour, ICooldownObserver
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;

    private Image dashImage;
    private Image spiritModeImage;

    private float dashCooldownDuration;
    private float spiritCooldownDuration;

    private float dashCooldownTimer;
    private float spiritCooldownTimer;

    private bool dashCooldownTriggered = false;
    private bool spiritCooldownTriggered = false;

    void Start()
    {
        Resume();

        Image[] cooldownImages = GetComponentsInChildren<Image>();

        foreach (Image cooldownImageSearched in cooldownImages)
        {
            if (cooldownImageSearched.gameObject.name == "dash-fill")
                dashImage = cooldownImageSearched;
            if (cooldownImageSearched.gameObject.name == "spirit-fill")
                spiritModeImage = cooldownImageSearched;
        }


        dashImage.fillAmount = 1;
        spiritModeImage.fillAmount = 1;

        CooldownManager.CooldownStarted += OnCooldownStarted;
    }

    void OnDestroy()
    {
        CooldownManager.CooldownStarted -= OnCooldownStarted;
    }

    private void Update()
    {
        CooldownUpdate();
        PauseControl();
    }

    private void CooldownUpdate()
    {
        if (dashCooldownTriggered)
        {
            dashCooldownTimer -= Time.deltaTime;
            UpdateCooldownUI(dashImage, dashCooldownTimer, dashCooldownDuration, ref dashCooldownTriggered);
        }

        if (spiritCooldownTriggered)
        {
            spiritCooldownTimer -= Time.deltaTime;
            UpdateCooldownUI(spiritModeImage, spiritCooldownTimer, spiritCooldownDuration, ref spiritCooldownTriggered);
        }
    }


    void UpdateCooldownUI(Image cooldownImage, float cooldownTimer, float cooldownDuration, ref bool cooldownTriggered)
    {
        float fillAmount = Mathf.Clamp01(1 - (cooldownTimer / cooldownDuration));
        cooldownImage.fillAmount = fillAmount;

        if (cooldownTimer <= 0)
        {
            cooldownTriggered = false;
        }
    }

    public void OnCooldownStarted(Type abilityType, float cooldownTime)
    {
        if (abilityType == typeof(DashState))
        {
            dashCooldownTriggered = true;
            dashCooldownTimer = cooldownTime;
            dashCooldownDuration = cooldownTime;
        }
        if (abilityType == typeof(SpiritModeEnterState))
        {
            spiritCooldownTriggered = true;
            spiritCooldownTimer = cooldownTime;
            spiritCooldownDuration = cooldownTime;
        }

        Debug.Log("triggered from ui");
    }

    private void PauseControl()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}

