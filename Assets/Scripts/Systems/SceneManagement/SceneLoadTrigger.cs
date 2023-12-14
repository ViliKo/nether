using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;


public class SceneLoadTrigger : MonoBehaviour
{

    [SerializeField] private SceneField[] _scenesToLoad;
    [SerializeField] private SceneField[] _scenesToUnload;
    [SerializeField] private string nextCameraPosition;
    [SerializeField] private string nextSpawnPosition;
    [SerializeField] private float CameraOrthoSize;
    [SerializeField] private string NextLevelManager;
    private GameObject newCameraPosition;
    private GameObject newSpawnPosition;
    public int cameraPriorityIndex; 
    public int SpawnPointIndex;
    public int newOrthoSize;
    GameObject cinemachine;
    GameObject player;
    LevelManager levelManager;  
    Vector2 playerEnterVector;
    float distance;
    float distanceSigned;
    //StateManager stateManager;


    void Start(){}

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.transform.tag != ("Player")) return;
        LoadScenes();
        
        player = other.gameObject;
        playerEnterVector = (player.transform.position - this.transform.position).normalized;
    }

    private void LoadScenes(){
        for(int i = 0; i < _scenesToLoad.Length; i++){

            bool isSceneLoaded = false;
            for(int j= 0; j < SceneManager.sceneCount; j++){

                    Scene loadedScene = SceneManager.GetSceneAt(j);

                    if(loadedScene.name == _scenesToLoad[i].SceneName){
                        isSceneLoaded = true;
                        break;
                    }
            }

            if(!isSceneLoaded){
                Debug.Log("I want to load scene");
                StartCoroutine(LoadLevel(_scenesToLoad[i]));
            }
        }
    }
    private void UnloadScenes(){

        for(int i = 0; i < _scenesToLoad.Length; i++){

            for(int j= 0; j < SceneManager.sceneCount; j++){

                 Scene loadedScene = SceneManager.GetSceneAt(j);
                 if(loadedScene.name == _scenesToUnload[i].SceneName){
                    SceneManager.UnloadSceneAsync(_scenesToUnload[i]);
                 }   


            }

        }
            
    }



    private  IEnumerator LoadLevel (string sceneName){
        var asyncLoadLevel = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!asyncLoadLevel.isDone){

            //lataa vielä sceneä
            yield return null;
            
        }


        SetNextCameraPos(CameraOrthoSize);
        SetNewCheckpointPos();
    
    }
    

    private  void SetNextCameraPos(float orthosize){


    }


    private void SetNewCheckpointPos(){
        if(GameObject.Find("StateManager")){

            //stateManager = GameObject.Find("StateManager").GetComponent<StateManager>();
            levelManager = GameObject.Find(NextLevelManager).GetComponent<LevelManager>();
            //stateManager._checkpointScene = levelManager.checkPointScene;
            //Debug.Log("Next Checkpoint: " + stateManager._checkpointScene);
            
        }
    }
}
