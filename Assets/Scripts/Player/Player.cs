using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using UnityEditor.SceneManagement;
using Cinemachine;



public class Player : MonoBehaviour
{


    Rigidbody2D rb;
    BoxCollider2D bc;
    SpriteRenderer childSpriteRenderer;

    // Start is called before the first frame update

    [Header("Lives")]
    public int currentHealth;
    public int maxHealth;

    public UiManager uiManager;
    public GameObject uiObject;
 

    public Scene currentScene;

    public List<Transform> spawnPoints;
    public int currentSpawnIndex;
    bool isAtSpawnPoint;
    bool firstSpawnPointFound;
    public Transform currentSpawnPoint;
    public Transform PreviousSpawnPoint;
    //public Transform CameraLookatPosition;

    public Transform checkPointPosition;
    Transform currentSpawnTransform;

    public bool FadeOutVolume;
    public bool FadeInVolume;


    public  float VolumeFadeSpeed;



    public float cameraDistanceFromPlayer;


    public Vector3 NewPos;

    public float lerpDuration;

    float value = 0f;
    float timePassed = 0f;
    float time = 0f;

    bool cameraIsLookingRight;
    bool cameraIsLookingLeft;

    

    public CinemachineVirtualCamera currentVirtualCamera;

    public GameObject CameraFollowObject;
    private CameraFollowObject cameraFollowObjectScript;
    
    private void Awake() {

        uiManager = uiObject.GetComponent<UiManager>();

        if(transform.GetComponent<Rigidbody2D>() != null)
            rb = gameObject.GetComponent<Rigidbody2D>();
    
        if(transform.GetComponent<BoxCollider2D>() != null)
            bc = gameObject.GetComponent<BoxCollider2D>();
    
        if(gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>() != null)
            childSpriteRenderer = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
    
        if (CameraFollowObject != null)
            cameraFollowObjectScript = CameraFollowObject.GetComponent<CameraFollowObject>();
        

        spawnPoints = new List<Transform>();
        firstSpawnPointFound= true;

        Debug.Log(cameraFollowObjectScript + "this script is not found");

    }


    void Start()
    {
        currentHealth = 5;
        maxHealth = 10;
        Init();
        isAtSpawnPoint=false;
        currentSpawnIndex = 0;
        VolumeFadeSpeed = 0.5f;


         cameraIsLookingRight = true;
       


        cameraDistanceFromPlayer = 2f;
        lerpDuration = 2f;
       // CameraLookatPosition.position = new Vector3(this.transform.position.x -cameraDistanceFromPlayer,this.transform.position.y,0);  qwef fasdf


    }

    // Update is called once per frame
    void Update() {
        SetCameraLookAtPosition();


        if (currentSpawnPoint != null){
            if(firstSpawnPointFound == true){
                MovePlayerToLatestSpawnPoint(currentSpawnPoint);

        }
    }

   

        // if(GameObject.Find("SpawnPoint" + currentSpawnIndex) != null){

        //     currentSpawnTransform = GameObject.Find("SpawnPoint" + currentSpawnIndex).transform;

        //     if(!spawnPoints.Contains(currentSpawnTransform)){
                    
        //             spawnPoints.Add(currentSpawnTransform);

        //     }
            
        // }
        //     Debug.Log("CurrentSpawnPoint = " + currentSpawnPoint);

    }

    



    private void  OnTriggerEnter2D(Collider2D other) {
        if(other == null) return;
        

        //Check jos collideri on damageDealer
        if(other.gameObject.GetComponent<IDamageDealer>() !=null){  

            UpdateHealth(-other.gameObject.GetComponent<IDamageDealer>().DealDamage());
         
            // PlayGetHitSound();
            Debug.Log("Damage from damageDealer: " + currentHealth);
            // soundManager.sounds[6].source.time = 0.8f;
            // soundManager.Play("Jump");
        }
      

    }

    void Die(){
        Debug.Log("Health" + currentHealth);
        if(currentHealth > 0 ){
            MovePlayerToLatestSpawnPoint(currentSpawnPoint);
        }
        if(currentHealth <= 0){

            Destroy(gameObject);
          
            // GameManager.instance.GameOver();
            //  soundManager.Play("Creepy1");
             
            // HoldPlayer();
            // GetSoundClip();
           
        //    FadeInVolume = true;
            
        //    MovePlayerToLatestSpawnPoint(checkPointPosition);
           //scriptit pois päältä 
           //

        }

    }


    public void UpdateHealth(int healthAdded){

        if(currentHealth + healthAdded > maxHealth)
            currentHealth = maxHealth;
        else
            currentHealth += healthAdded;
            
        uiManager.UpdateLives(currentHealth);
             

        if( healthAdded<0)
            Die();



        Debug.Log("player health is: " + currentHealth);
    }



    public void SetCameraLookAtPosition()
    {

        if (PlayerController.Player.dir == 1 && !cameraIsLookingRight)
        {
            //tää pitää varmaan callaa vaan kerran.
            cameraIsLookingRight = true;
            cameraFollowObjectScript.CallRotationRoutine();
        }
        else if (PlayerController.Player.dir == -1 && cameraIsLookingRight)
        {
            cameraIsLookingRight = false;
            cameraFollowObjectScript.CallRotationRoutine();
        }
    }


    // public void LerpWithTime(float start, float end , float duration){

    //     float value = 0f;
    //     float timePassed = 0f;
    //     float time = 0f;

    //     if(timePassed < duration){

    //         time = timePassed/duration;
    //       value =  Mathf.Lerp(start,end,time);   
    //         timePassed += Time.deltaTime;
    //     }else{

    //         value = end;
    //     }

    // }


    //  private void GetSoundClip(){

    //     for(int i= 0; i<soundManager.sounds.Length; i++){

    //         if(soundManager.sounds[i].name =="Creepy1"){

    //             soundClip =soundManager.sounds[i];
    //             Debug.Log("sound clip " + soundClip.name);

    //         }

    //     }



    //  }




    public void HoldPlayer(){

        bc.enabled = false;
        rb.bodyType = RigidbodyType2D.Static;
        rb.velocity = Vector2.zero;
        childSpriteRenderer.enabled = false;

    }

    public void HoldPlayerForSeconds(float time){
        bc.enabled = false;
        rb.bodyType = RigidbodyType2D.Static;
        rb.velocity = Vector2.zero;
            

        StartCoroutine( PlayerHoldRoutine(time));
    }   


    IEnumerator PlayerHoldRoutine(float waitTime){

        yield return new WaitForSeconds(waitTime);

        bc.enabled = false;
        rb.bodyType = RigidbodyType2D.Static;
        rb.velocity = Vector2.zero;
           

    }





    // vois ehkä olla parempi laittaa kaikki pelaajan audio functiot yhteen scriptiin 

   public  void MovePlayerToLatestSpawnPoint(Transform spawnPos){

        // Debug.Log("SPawnpoints: " + spawnPoints[0]);
        // int latestItemInList = spawnPoints.Count -1 ;
        //last spawnpoint ja currentSpawnPoint;

                //bc.enabled = false;
                //rb.bodyType = RigidbodyType2D.Static;
                //rb.velocity = Vector2.zero;
               

                // transform.position =spawnPoints[currentSpawnIndex].position;


                //   if(transform.position == spawnPoints[currentSpawnIndex].position){

                //         StartCoroutine(SpawnWaitRoutine());
                //  }    


        transform.position = spawnPos.position;


        if(transform.position == spawnPos.position){
            firstSpawnPointFound =false;
            //StartCoroutine(SpawnWaitRoutine());
        }    

        
    }



   IEnumerator SpawnWaitRoutine(){
       
       
        yield return new WaitForSeconds(2f);
        bc.enabled = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        childSpriteRenderer.enabled =true;
          
   }

    // void PlayGetHitSound(){

    //         soundManager.Play("GetHit");
    //         soundManager.sounds[1].source.time =2.5f;
    //         soundManager.sounds[1].source.SetScheduledEndTime(AudioSettings.dspTime + (3f -2.5f));

    // }


    void Init(){

        Debug.Log(uiManager + " saatko tämän");
        //Pakko kutsua tässä järjestyksessä 
        //CreateLifeImages luo niin paljon life kuvia kuin on maximi health;
        //tälleen pystytään vaihtaa helposti maximi elämää
        uiManager.CreateLifeImages(maxHealth);
        //kattoo niiden määrän ja updatee
        uiManager.UpdateLives(currentHealth);
        
    }

    public void ResetPlayerStats(){

        currentHealth = 5;
        maxHealth = 10;
        Init();
        isAtSpawnPoint=false;
        currentSpawnIndex = 0;

    }



    //      private void FadeOutMusic(Sound sound){

    //if(sound != null)
    //        if(FadeOutVolume  == true){


    //             if( sound.volume > 0) {

    //               sound.volume -= VolumeFadeSpeed * Time.deltaTime;


    //                  if(sound.volume <= 0){


    //                   FadeOutVolume =false;
    //                  }

    //             }

    //         } 
    //  }



    //      private void FadeInMusic(Sound sound){

    //if(sound != null)
    //        if(FadeInVolume  == true){


    //             if( sound.volume < 1) {

    //               sound.volume += VolumeFadeSpeed * Time.deltaTime;


    //                  if(sound.volume >= 1){


    //                   FadeInVolume =false;
    //                  }

    //             }

    //         } 
    //}








}

