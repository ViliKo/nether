using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    private PlayerState playerState;
    private IDataService dataService = new JsonDataService();
    private bool encryptionEnabled = true;

    public GameObject mainMenu;
    public GameObject mainMenuFirstSelected;
    public GameObject saveSlots;
    public GameObject saveSlotsFirstSelected;
    public GameObject options;
    public GameObject optionsFirstSelected;

    private bool _saveFlag;
    private bool _loadFlag;


    void Start()
    {
        _saveFlag = false;
        _loadFlag = false;

        LoadPlayerState();

        EventSystem eventSystem = FindObjectOfType<EventSystem>();
        StandaloneInputModule standaloneInputModule = FindObjectOfType<StandaloneInputModule>();

        if (eventSystem != null && standaloneInputModule != null)
        {
            eventSystem.firstSelectedGameObject = mainMenuFirstSelected;
            standaloneInputModule.inputActionsPerSecond = 10;
        }
    }

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



    public void SaveOrLoadGame()
    {
        int slotIndex = GetSlotIndexFromButtonIndex(EventSystem.current.currentSelectedGameObject.transform.GetSiblingIndex());
        Debug.Log(slotIndex);
        Debug.Log("This is the save flag: " + _saveFlag);
        Debug.Log("This is the load flag: " + _loadFlag);

        if (_saveFlag)
        {
            SaveGame(slotIndex);
        }
        if (_loadFlag)
        {
            LoadGame(slotIndex);
        }
    }

    public void ReturnToMainMenu()
    {
        _saveFlag = false;
        _loadFlag = false;

        saveSlots.SetActive(false);
        options.SetActive(false);
        mainMenu.SetActive(true);
        

        EventSystem.current.SetSelectedGameObject(mainMenuFirstSelected);
    }

    public void ChangeOptions()
    {
        mainMenu.SetActive(false);
        options.SetActive(true);

        EventSystem.current.SetSelectedGameObject(optionsFirstSelected);
    }

    private void StartGame()
    {
        int startingLevel = playerState.currentLevel + 1; // Adjust as needed

        // Assuming level scenes are named "Level1", "Level2", etc.
        string levelSceneName = "Level" + startingLevel;

        SceneManager.LoadScene(levelSceneName);

    }


    private void SaveGame(int selectedSlotIndex)
    {
        if (selectedSlotIndex >= 0 && selectedSlotIndex < playerState.saveSlots.Count)
        {
            playerState.saveSlots[selectedSlotIndex] = GetCurrentSaveSlotData();
            Debug.Log("this is the current save slot info: " + playerState.saveSlots[selectedSlotIndex]);
            SavePlayerState();
        }

        StartGame();
    }
    private SaveSlot GetCurrentSaveSlotData()
    {
        SaveSlot saveSlot = new SaveSlot
        {
            currentLevel = playerState.currentLevel,
        };
        Debug.Log(saveSlot + " Does save slot created exist");
        return saveSlot;
    }

    private void SavePlayerState()
    {
        dataService.SaveData("/player-state.json", playerState, encryptionEnabled);
    }

    private void LoadGame(int selectedSlotIndex)
    {
        if (selectedSlotIndex >= 0 && selectedSlotIndex < playerState.saveSlots.Count)
        {
            ApplySaveSlotData(playerState.saveSlots[selectedSlotIndex]);
        }

        StartGame();
    }



    private void ApplySaveSlotData(SaveSlot saveSlot)
    {
        
        playerState.currentLevel = saveSlot.currentLevel;
    }



    private void LoadPlayerState()
    {
        playerState = dataService.LoadData<PlayerState>("/player-state.json", encryptionEnabled);
    }

    private int GetSlotIndexFromButtonIndex(int buttonIndex)
    {
        return buttonIndex;
    }


    public void QuitGame()
    {
        Application.Quit();
    }

}
