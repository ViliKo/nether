using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Cinemachine;
public class LevelManager : MonoBehaviour
{

    public string persistentScreenName;
    public StateManager stateManager;
    public GameObject cameraManagerObject;

    private GameObject player;
    private List<Scene> scenes;
    private bool listHasPlayer;
    private CameraManager cameraManager;
    private CheckpointManager _checkpointManager;


    //public SceneField checkPointScene;

    private void Awake() {
        _checkpointManager = GameObject.Find("checkpoint-manager").GetComponent<CheckpointManager>();

        if (cameraManagerObject != null){
            cameraManager = cameraManagerObject.GetComponent<CameraManager>();
       }
    }

    private void Start()
    {
        listHasPlayer = false;
        scenes = new List<Scene>();
        SpawnPlayer();
    }



     

    private void SpawnPlayer(){
        // ma tekisin tasta paan
        GetScenes();
        listHasPlayer = CheckIfPersitenSceneHasLoaded();

        if (!listHasPlayer)
            StartCoroutine(LoadLevel(persistentScreenName));
        else
            PlayerSetup();
    }

    private IEnumerator LoadLevel(string sceneName)
    {
        var asyncLoadLevel = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!asyncLoadLevel.isDone)
            yield return null;

        PlayerSetup();
        
        SetCheckPointScene();
        listHasPlayer = false;
    }



    public void PlayerSetup()
    {
        if (GameObject.FindWithTag("Player"))
        {
            player = GameObject.FindWithTag("Player");
            GivePlayerTransformToAllCamerasAtScene(player);
            SetStartCheckpoint();
        }
    }


    public void GivePlayerTransformToAllCamerasAtScene(GameObject cameraLookAtSpot){            
        for(int i = 0; i < cameraManager.VirtualCameraList.Count; i++){
                    
            if(cameraManager.VirtualCameraList[i].GetComponent<CinemachineVirtualCamera>().Follow != cameraLookAtSpot.transform){

                cameraManager.VirtualCameraList[i].GetComponent<CinemachineVirtualCamera>().Follow = cameraLookAtSpot.transform;
                Debug.Log("Im in setCamera" + cameraManager.VirtualCameraList[i].GetComponent<CinemachineVirtualCamera>().Follow);

            } else {

                cameraManager.VirtualCameraList[i].GetComponent<CinemachineVirtualCamera>().Follow = cameraLookAtSpot.transform;
            }
        }
    }

    private void SetStartCheckpoint()
    {
        player.transform.position = _checkpointManager.GetActiveCheckpointPosition();
    }


    private void GetScenes()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
            scenes.Add(SceneManager.GetSceneAt(i));

        Debug.Log("All scenes at start" + scenes.Count);

    }

    public bool CheckIfPersitenSceneHasLoaded()
    {
        for (int i = 0; i < scenes.Count; i++)
        {
            if (scenes[i].name == persistentScreenName)
                return true;

        }
        return false;
    }


    public void SetCheckPointScene()
    {
        if (GameObject.Find("StateManager"))
        {
            //stateManager = GameObject.Find("StateManager").GetComponent<StateManager>();
            //stateManager._checkpointScene = checkPointScene;
        }
    }
}