using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private IDataService dataService = new JsonDataService();
    public PlayerState playerState; // Assume this is set and initialized elsewhere
    public int currentLevel = 0; // Assume this is updated based on level progression
    private bool encryptionEnabled = false;

    public int currentSlot = -100;


    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();

                if (instance == null)
                {
                    GameObject obj = new GameObject("game-manager");
                    instance = obj.AddComponent<GameManager>();
                }
            }

            return instance;
        }
    }





    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //_startPoint = GameObject.FindGameObjectWithTag("StartPoint").transform;

    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


    // Method to switch to a new level
    public void SwitchToLevel(int newLevel)
    {
        // Update the current level
        currentLevel = newLevel;

        // Identify the current save slot (you may adjust this logic based on your game's design)
        int currentSaveSlotIndex = currentSlot;

        // Ensure the current save slot index is valid
        if (currentSaveSlotIndex >= 0 && currentSaveSlotIndex < playerState.saveSlots.Count)
        {
            // Update the current level in the selected save slot
            playerState.saveSlots[currentSaveSlotIndex].currentLevel = currentLevel;

            // Save the updated player state
            SavePlayerState();
        }
        else
        {
            Debug.LogError("Invalid save slot index");
        }
    }

    private void SavePlayerState()
    {
        dataService.SaveData("/player-state.json", playerState, encryptionEnabled);
    }
}