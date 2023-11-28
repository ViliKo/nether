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
    public Transform currentSpawnPoint;
    public Transform PreviousSpawnPoint;
    public Transform checkPointPosition;
    private Transform currentSpawnTransform;
    public int currentSpawnIndex;
    bool isAtSpawnPoint;
    bool firstSpawnPointFound;




    public float cameraDistanceFromPlayer;


    public Vector3 NewPos;

    public float lerpDuration;

    float value = 0f;
    float timePassed = 0f;
    float time = 0f;

    bool cameraIsLookingRight;
    bool cameraIsLookingLeft;

    

    public CinemachineVirtualCamera currentVirtualCamera;

    
    private void Awake() {

        uiManager = uiObject.GetComponent<UiManager>();

        if(transform.GetComponent<Rigidbody2D>() != null)
            rb = gameObject.GetComponent<Rigidbody2D>();
    
        if(transform.GetComponent<BoxCollider2D>() != null)
            bc = gameObject.GetComponent<BoxCollider2D>();
    
        if(gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>() != null)
            childSpriteRenderer = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();

        spawnPoints = new List<Transform>();
        firstSpawnPointFound= true;
    }


    void Start()
    {
        currentHealth = 5;
        maxHealth = 10;
        isAtSpawnPoint=false;
        currentSpawnIndex = 0;
        cameraIsLookingRight = true;
        cameraDistanceFromPlayer = 2f;
        lerpDuration = 2f;
       // CameraLookatPosition.position = new Vector3(this.transform.position.x -cameraDistanceFromPlayer,this.transform.position.y,0);  qwef fasdf
    }

    // Update is called once per frame
    void Update() {

        if (currentSpawnPoint != null){
            if(firstSpawnPointFound == true){
                MovePlayerToLatestSpawnPoint(currentSpawnPoint);

            }
        }
    }

    
    private void  OnTriggerEnter2D(Collider2D other) {
        if(other == null) return;
        

        //Check jos collideri on damageDealer
        if(other.gameObject.GetComponent<IDamageDealer>() !=null){  

            Die(-other.gameObject.GetComponent<IDamageDealer>().DealDamage());
         
    
        }
    }

    void Die(float damage){
        Debug.Log("Health" + currentHealth);
        
        MovePlayerToLatestSpawnPoint(currentSpawnPoint);
        

    }





   public  void MovePlayerToLatestSpawnPoint(Transform spawnPos){

        transform.position = spawnPos.position;


        if(transform.position == spawnPos.position){
            firstSpawnPointFound =false;
        }            
    }


   IEnumerator SpawnWaitRoutine(){
       
       
        yield return new WaitForSeconds(2f);
        bc.enabled = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        childSpriteRenderer.enabled =true;
          
   }

  
    public void ResetPlayerStats(){

        isAtSpawnPoint=false;
        currentSpawnIndex = 0;
    }
}

