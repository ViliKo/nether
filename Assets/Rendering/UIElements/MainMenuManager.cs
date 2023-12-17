using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    public void StartGame()
    {
        Debug.Log("Im here");
        SceneManager.LoadScene("GodPortal2"); // Replace "GameScene" with your actual game scene name
    }

    public void QuitGame()
    {
        // Note: This won't work in the Unity Editor. It's meant for standalone builds.
        Application.Quit();
    }

}
