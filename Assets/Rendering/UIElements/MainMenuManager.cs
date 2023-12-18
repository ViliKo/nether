using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{

    private PlayerState playerState;
    private IDataService dataService = new JsonDataService();
    private bool encryptionEnabled = false;

    #region Objects

    [Header("Main Menu")]
    public GameObject mainMenu;
    public GameObject mainMenuFirstSelected;
    [Header("Save Slots")]
    public GameObject saveSlots;
    public GameObject saveSlotsFirstSelected;
    [Header("Options")]
    public GameObject options;
    public GameObject optionsFirstSelected;
    [Header("Errors")]
    public GameObject errorDialog;
    public GameObject errorDialogFirstSelected;
    public GameObject error;
    [Header("Choice")]
    public GameObject choiceDialog;
    public GameObject choiceDialogFirstSelected;
    public GameObject info;
    public Button yesButton;
    public Button noButton;

    private bool _saveFlag;
    private bool _loadFlag;

    #endregion


    void Start()
    {
        _saveFlag = false;
        _loadFlag = false;
        playerState = null;

        LoadPlayerState();

        EventSystem eventSystem = FindObjectOfType<EventSystem>();
        StandaloneInputModule standaloneInputModule = FindObjectOfType<StandaloneInputModule>();

        if (eventSystem != null && standaloneInputModule != null)
        {
            eventSystem.firstSelectedGameObject = mainMenuFirstSelected;
            standaloneInputModule.inputActionsPerSecond = 10;
        }


        // vaitoehto eventti
    }

    #region PanelLogic
    public void ActivateSaveGamePanel()
    {
        _saveFlag = true;

        mainMenu.SetActive(false);
        saveSlots.SetActive(true);

        EventSystem.current.SetSelectedGameObject(saveSlotsFirstSelected);
    }

    public void ActivateLoadGamePanel()
    {
        _loadFlag = true;

        mainMenu.SetActive(false);
        saveSlots.SetActive(true);

        EventSystem.current.SetSelectedGameObject(saveSlotsFirstSelected);

    }
    public void ReturnToMainMenu()
    {
        _saveFlag = false;
        _loadFlag = false;

        saveSlots.SetActive(false);
        options.SetActive(false);
        errorDialog.SetActive(false);
        choiceDialog.SetActive(false);

        mainMenu.SetActive(true);


        EventSystem.current.SetSelectedGameObject(mainMenuFirstSelected);
    }

    public void ChangeOptions()
    {
        mainMenu.SetActive(false);
        options.SetActive(true);

        EventSystem.current.SetSelectedGameObject(optionsFirstSelected);
    }

    public void ThrowError(string message)
    {
        mainMenu.SetActive(false);
        saveSlots.SetActive(false);
        options.SetActive(false);

        errorDialog.SetActive(true);

        error.GetComponent<TextMeshProUGUI>().text = message;

        EventSystem.current.SetSelectedGameObject(errorDialogFirstSelected);
    }

    public void AskChoice(string message, Action yesChoice, Action noChoice)
    {
        mainMenu.SetActive(false);
        saveSlots.SetActive(false);
        options.SetActive(false);

        choiceDialog.SetActive(true);

        info.GetComponent<TextMeshProUGUI>().text = message;

        EventSystem.current.SetSelectedGameObject(choiceDialogFirstSelected);

        yesButton.onClick.AddListener(new UnityEngine.Events.UnityAction(yesChoice));
        noButton.onClick.AddListener(new UnityEngine.Events.UnityAction(noChoice));

    }

    #endregion

    public void SaveOrLoadGame()
    {
        int slotIndex = GetSlotIndexFromButtonIndex(EventSystem.current.currentSelectedGameObject.transform.GetSiblingIndex());
        Debug.Log(slotIndex);
        Debug.Log("This is the save flag: " + _saveFlag);
        Debug.Log("This is the load flag: " + _loadFlag);

        if (_saveFlag)
        {
            StartNewGame(slotIndex);
        }
        if (_loadFlag)
        {
            LoadGame(slotIndex);
        }
    }

    private void StartGame(int selectedSlotIndex)
    {
        GameManager.Instance.currentSlot = selectedSlotIndex;
        GameManager.Instance.playerState = playerState;

        int startingLevel = playerState.currentLevel + 1; // Adjust as needed

        // Assuming level scenes are named "Level1", "Level2", etc.
        string levelSceneName = "Level" + startingLevel;

        SceneManager.LoadScene(levelSceneName);

    }

    private void StartNewGame(int selectedSlotIndex)
    {
        if (selectedSlotIndex >= 0 && selectedSlotIndex < playerState.saveSlots.Count)
        {
            if (playerState.saveSlots[selectedSlotIndex].isInitiated)
            {
                // Callback function for "Yes" choice
                Action yesChoice = () =>
                {
                    playerState.saveSlots[selectedSlotIndex].isInitiated = true;
                    playerState.saveSlots[selectedSlotIndex].currentLevel = 0;
                    SavePlayerState();
                    StartGame(selectedSlotIndex);
                };

                Action noChoice = () =>
                {
                    ReturnToMainMenu();
                };

                // Ask the choice and provide the callback
                AskChoice("Are you sure you want to overwrite the data", yesChoice, noChoice);
            } else
            {
                playerState.saveSlots[selectedSlotIndex].isInitiated = true;
                SavePlayerState();
                StartGame(selectedSlotIndex);
            }
        }
    }

    private void SavePlayerState()
    {
        dataService.SaveData("/player-state.json", playerState, encryptionEnabled);
    }

    private void LoadGame(int selectedSlotIndex)
    {
        if (selectedSlotIndex >= 0 && selectedSlotIndex < playerState.saveSlots.Count)
        {
            Debug.Log(playerState.saveSlots[selectedSlotIndex].ToString());

            if (!playerState.saveSlots[selectedSlotIndex].isInitiated) 
            {
                ThrowError("Save not initialized");
                return;
            };

            ApplySaveSlotData(playerState.saveSlots[selectedSlotIndex]);

            StartGame(selectedSlotIndex);
        }
    }


    private void ApplySaveSlotData(SaveSlot saveSlot)
    {
        
        playerState.currentLevel = saveSlot.currentLevel;

    }

    private void LoadPlayerState()
    {
        // Load the player state from the file
        PlayerState loadedPlayerState = dataService.LoadData<PlayerState>("/player-state.json", encryptionEnabled);
        Debug.Log($"Loaded PlayerState: {JsonUtility.ToJson(loadedPlayerState)}");


        if (loadedPlayerState != null)
        {
            // Replace the existing playerState with the loaded
            playerState = loadedPlayerState;
            Debug.Log($"Loaded PlayerState: {JsonUtility.ToJson(playerState)}");
        }
        else
        {
            // Handle the case where loading fails (e.g., the file doesn't exist)
            Debug.LogWarning("Failed to load PlayerState. Creating a new instance.");
            playerState = new PlayerState();
        }
    }

    private int GetSlotIndexFromButtonIndex(int buttonIndex) => buttonIndex;
    


    public void QuitGame() => Application.Quit();
    

}
