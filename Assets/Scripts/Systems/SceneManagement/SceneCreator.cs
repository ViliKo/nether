using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class SceneCreator : MonoBehaviour
{
    [SerializeField] private SceneField[]_scenesToLoad;
    [SerializeField] private SceneField[]_scenesToUnload;


    private void OnTriggerEnter2D(Collider2D other) {
        if(other.transform.tag != "Player") return;

        LoadScenes();
        UnloadScenes();
    }



    private void LoadScenes(){

        for(int i = 0; i < _scenesToLoad.Length; i++){

            bool isSceneLoaded = false;

            for(int j = 0; j < SceneManager.sceneCount; j++){

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


                if(_scenesToUnload.Length != 0){
                    if(loadedScene.name == _scenesToUnload[i].SceneName){
                        SceneManager.UnloadSceneAsync(_scenesToUnload[i]);
                    }   

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
    }
}
