using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.UI;
using Unity.Mathematics;

public class UiManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject canvas;
    public Transform livesObject;
    public Image[] lives;

    public GameObject[] ObjectsToTurnOff;

    public GameObject HealthIcons;

    bool isThereLivesObject;
    GameObject lifePrefab;
    Vector2 LifeSpawnPos;

    GameObject deathScreen;
    GameObject ScreenButtons;

    int spaceBetweenLifeImages;

    public GameObject FadeInOutObject;

   //public SceneTransitionFade fade;

   public GameObject LifeImagePrefab;


    //Laita gameobject assign editorissa.  
    //


     void Awake() {
        // if(GameObject.Find("Canvas")== null)return;
            
        //     canvas = GameObject.Find("Canvas");
        // if(GameObject.Find("DeathScreen") != null)    
        //     deathScreen = GameObject.Find("DeathScreen");

        //     isThereLivesObject =false;
        // if(canvas.transform.Find("livesObject")){
        //       }
            // livesObject = canvas.transform.Find("livesObject");
            LifeSpawnPos = new Vector2(livesObject.transform.position.x,livesObject.transform.position.y);
            isThereLivesObject= true;
            Debug.Log("Transform object found" +livesObject);
      
             spaceBetweenLifeImages= 20;

        //if(FadeInOutObject == null){


        //    Debug.LogError("You need to add FadeInOutObject to UiManager");
        //}
        //else{

        //    //fade = FadeInOutObject.GetComponent<SceneTransitionFade>();
        //}


            //Tää pitää olla awakessa että se hakee prefabin
            //ennen kuin pelaaja käyttää sitä start functiossaan
        //    lifePrefab = Resources.Load("LifeImage") as GameObject;


           //tärkee määrittää tässtä 
           //että se määritetään ennen kun player kutsuu sen startissa
          
    }
    void Start(){
            //Tee tän niminen Prefab joka on healtin kuva 
 
    // HideDeathScreen();
 
    
    // CreateLifeImages(3);

    }

  

    public void ShowDeathScreen(){
    //   deathScreen.SetActive(true);
    //   deathScreen.SetActive(!gameObject.activeInHierarchy);

        
        //fade.fadeIn = true;


    

      for(int i = 0; i<ObjectsToTurnOff.Length; i++){

           ObjectsToTurnOff[i].SetActive(true);     
        }



       
    }



    // public void HideDeathScreen(){

    //     // deathScreen.SetActive(false);
    //     for(int i = 0; i<ObjectsToTurnOff.Length; i++){

    //        ObjectsToTurnOff[i].SetActive(false);     
    //     }

    // }

  public void HideLives(){

    HealthIcons.SetActive(false);

  }

  public void ShowLives(){

    HealthIcons.SetActive(true);
    
  }

   



    public void UpdateLives(int health){

        if(isThereLivesObject==false)return;

            for (int i = 0; i < lives.Length; i++)
            {

                if (i <= health - 1)
                {

                    lives[i].enabled = true;

                }
                else if (i > health - 1)
                {
                    lives[i].enabled = false;

                }

            }    
    }

        //Hakee vain healthobjectien määrän
        private void GetLivesImages(Transform livesObject){

            int livesAmount = livesObject.childCount;
            lives = new Image[livesAmount];

            for(int i =0; i< livesAmount ; i++ ){

                lives[i] = livesObject.GetChild(i).GetComponent<Image>();
             
            }
            

        }


        //Luo health objecit ja laittaa ne lifeObjectin sisään
        public void CreateLifeImages(int MaxHealth){

            if(isThereLivesObject == false)return;

            Debug.Log("CreateLifeImages called ");

            for(int i = 0; i < MaxHealth; i++){
             
                
                GameObject prefabClone = Instantiate(LifeImagePrefab);
                Debug.Log("LIfeImage " + prefabClone);
                prefabClone.name = "LifeImage" + i;
                 GameObject.Find("LifeImage" + i).transform.parent = livesObject.transform;
                prefabClone.transform.position = new Vector2(livesObject.position.x + (i * spaceBetweenLifeImages ),livesObject.position.y);
                
               
    
            }
            //päivittää paljon objecteja luotiin
             GetLivesImages(livesObject);

        }


    


}