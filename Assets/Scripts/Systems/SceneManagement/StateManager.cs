using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.PackageManager;

public class StateManager : MonoBehaviour
{
       
    [SerializeField] public SceneField _checkpointScene;
    [SerializeField] public Transform  _checkpointPosition;
    [SerializeField] public List<Scene> UnloadSceneArray;   
    GameObject lookatpos;
    public GameObject player;
    //Player playerScript;
    public GameObject UiManagerObject;
    //CheckPoint checkpointScript;
    //UiManager uiManager;
    public GameObject cinemachineObject;
    //WhoToLookAt whoToLookAt;
    GameObject cameraLookAtSpot;
    LevelManager levelManager;
    GameObject levelManagerObject;
    Scene loadedScene;
    public List<GameObject> sceneObjectArray;
    public GameObject SoundManager;
    //SoundManager soundManagerScript;



    private void Start() {        
        UnloadSceneArray = new List<Scene>();
    }
    private void Update() {
       
        if(lookatpos != null){
            //whoToLookAt.LevelLookAtSpot = lookatpos;
        }
        Debug.Log("look at " + lookatpos);
    
    }
        

    public void LoadCurrentCheckpointScene(){

        UnloadScenes();
        loadScenes();            
        FindCheckPointFromNewScene();

    }

  





    public void FindCheckPointFromNewScene(){
        Scene s = SceneManager.GetSceneByName(_checkpointScene);





    }





    public void FindCameraPositionFromNewScene(){

                  

    }





            //TÄä sama on myös player scriptissä
            //minkä takii vois käyttää sitä
    public void TakePlayerToCheckpointPosition(){

  

            //playerScript.transform.position = _checkpointPosition.position;


            //if(playerScript.transform.position == _checkpointPosition.position){
                       
            //    StartCoroutine(SpawnWaitRoutine());
            //}    

                
    }


    IEnumerator SpawnWaitRoutine(){
       
       
        yield return new WaitForSeconds(3f);

        
        //mieti parempaa paikkaa
        //player.GetComponent<Player>().ResetPlayerStats();
        //player.GetComponent<BoxCollider2D>().enabled = true;
        //player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        //player.GetComponentInChildren<SpriteRenderer>().enabled = true;  

        // soundManagerScript.Play("Chill");

          
    }


    private void UnloadScenes(){

     
        for(int j= 0; j < SceneManager.sceneCount; j++){

            loadedScene = SceneManager.GetSceneAt(j);

            if(loadedScene.name != "PersistentData"){
                SceneManager.UnloadSceneAsync(loadedScene);
            }     
        }
    }


    private  IEnumerator UnLoadLevelCoroutine (List <Scene> UnloadScenes){

               
        for( int i= 0; i<UnloadScenes.Count; i++){

            var asyncLoadLevel = SceneManager.UnloadSceneAsync(UnloadScenes[i]);

            while (!asyncLoadLevel.isDone){

                //lataa pois vielä sceneä
                yield return null;
                    
            }   
        }   
    }



    private void loadScenes(){

        Debug.Log("scenes to unload list" );
        StartCoroutine(LoadLevelCoroutine(_checkpointScene));
    }






    private  IEnumerator LoadLevelCoroutine (string sceneName){
        var asyncLoadLevel = SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Additive);

        while (!asyncLoadLevel.isDone){
            yield return null;
        }
        FindCheckPointFromNewScene();
    }


    public void LoadMainMenu(){
        //unload all scenes and then load main menu;

        for(int j= 0; j < SceneManager.sceneCount; j++){

            Scene loadedScene = SceneManager.GetSceneAt(j);
            SceneManager.UnloadSceneAsync(loadedScene);
        }   

        SceneManager.LoadScene("MainMenu");

    }
}
