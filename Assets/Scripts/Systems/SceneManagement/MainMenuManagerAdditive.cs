using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;
using Packages.Rider.Editor;


public class MainMenuManagerAdditive : MonoBehaviour
{


    [Header("Scenes to load")] 
    [SerializeField] public SceneField _persistentStuff;
    [SerializeField] public SceneField _MainMenuScene;

    [SerializeField] private GameObject[] _hideObjects;

    [SerializeField] private GameObject _loadingBar;
    [SerializeField] public SceneField _level1;
    [SerializeField] public SceneField _level2;
    [SerializeField] public SceneField _level3;
 
    [SerializeField] private Image _loadBarImage;

    [SerializeField] private  float timeWaitBeforeFade;

    [SerializeField] private GameObject soundManagerObject;


    [SerializeField] private float VolumeFadeSpeed;
     //private SoundManager soundManagerScript;

    [SerializeField] private bool  FadeOutVolume;

    [SerializeField]  private bool FadeInVolume;




     //private Sound audioClip;



    private bool fadeOutIsDone;

    SceneTransitionFade FadeScript;
    [SerializeField]public GameObject FadeOutCanvasGroup;
    bool doneloading;
    private List<AsyncOperation> _scenesToLoad = new List<AsyncOperation>();

    private void Awake() {
        _loadingBar.SetActive(false);
  
        doneloading = false;
        FadeScript = FadeOutCanvasGroup.GetComponent<SceneTransitionFade>();
        //soundManagerScript = soundManagerObject.GetComponent<SoundManager>();
        timeWaitBeforeFade= 2f;


        VolumeFadeSpeed = 1f;
    }



    public void StartGame(){

        HideMenu();
        FadeOutVolume = true;

        _scenesToLoad.Add( SceneManager.LoadSceneAsync(_level1, LoadSceneMode.Additive));
        _scenesToLoad.Add( SceneManager.LoadSceneAsync(_level2, LoadSceneMode.Additive));  
        _scenesToLoad.Add( SceneManager.LoadSceneAsync(_level3, LoadSceneMode.Additive));  
        StartCoroutine(LoadBarProgress());
    }


      private void Update() {
          if(doneloading){
            Timer();
          }
      }




    public void QuitEditor(){
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    private void HideMenu(){

        for(int i= 0; i < _hideObjects.Length; i++){

            _hideObjects[i].SetActive(false);

        }    
    }


    IEnumerator WaitBeforeUnload(){

  
        yield return new WaitForSeconds(timeWaitBeforeFade);
        FadeScript.fadeOut = true;
        doneloading = true;

    }




    private IEnumerator LoadBarProgress(){

        float loadProgress = 0f;
        _loadingBar.SetActive(true);

        for(int i= 0; i<_scenesToLoad.Count; i++ ){

            while(!_scenesToLoad[i].isDone ){

                loadProgress +=_scenesToLoad[i].progress;


                _loadBarImage.fillAmount = loadProgress /_scenesToLoad.Count;
                yield return null;
            }
     
            _loadingBar.SetActive(false);
      
        }
        StartCoroutine(WaitBeforeUnload());
    }


     private void Timer(){
        if(FadeScript.fadeOut == false){
            doneloading =false;

            SceneManager.UnloadSceneAsync(_MainMenuScene);
        }
     }

}


