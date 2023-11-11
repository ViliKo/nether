using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    // Start is called before the first frame update


   public GameObject player;

//    public float   lerpValue;

//     float time;
//    float timePassed;
   public float duration;


    private Coroutine _LerpTurnRoutine;

    void Start()
    {

         duration = 0.5f;
          
    }


    //  public void GetPlayerStuff(GameObject player){

    //       if(player != null){

    //     playerController7 = player.GetComponent<PlayerController7>();
          
    //        
           
    //   }else{

    //     Debug.LogError("There is no player object attacthed to cameraFolloWObjectScript");

    //   }

    //  }   


    // Update is called once per frame
        //täytyy olla fixed updatessa että liike on smooth;
    private void FixedUpdate() {

        if(player == null){
            
            return;
        
        
        }else{
        transform.position = player.transform.position;

        }
    }
    void Update()
    {
         
       



                 

            // if(!hasLerpedToRight){
            //           if(timePassed < duration){

            //             time = timePassed/duration;
            //              lerpValue =  Mathf.Lerp(transform.rotation.y,0f,time);
            //             timePassed += Time.deltaTime;

            //            transform.eulerAngles = new Vector3(transform.rotation.x,lerpValue,0);
                                                
            //             }else{
            //                 hasLerpedToRight = true;
            //                 time = 0f;
            //                 timePassed = 0f;
            //             }            
                       

            // }

            //          Debug.Log("face direction" + playerController7.isFacingRight + lerpValue);
                   

        //     if(!hasLerpedToLeft){
        //                     if(timePassed < duration){

        //                         time = timePassed/duration;
        //                         lerpValue = Mathf.Lerp(transform.rotation.y,-180f,time);
        //                         timePassed += Time.deltaTime;

        //                         transform.eulerAngles = new Vector3(transform.rotation.x,lerpValue,0);
                                                        
        //                         }else{
        //                             hasLerpedToLeft = true;
        //                             time = 0f;
        //                             timePassed = 0f;
        //                         }            
                       

        //                     }

        //              Debug.Log("face direction" + playerController7.isFacingRight + lerpValue);
        // }
       
        
    }


          public void CallRotationRoutine(){

                _LerpTurnRoutine = StartCoroutine(LerpRoutine());
                Debug.Log("coroutine " + _LerpTurnRoutine);
                
          }  

        private float GetPlayerRotation(){

            if(PlayerController.Player.dir == 1){


                return -180f;
            }else{

                return 0f;
            }

        }



        IEnumerator LerpRoutine(){

            float rotation = GetPlayerRotation();
            float startRotation = transform.localEulerAngles.y;
            float timePassed = 0f;
          
            float lerpValue = 0f;
              while(timePassed < duration){
                    
                //    time = timePassed/duration;
                   timePassed += Time.deltaTime;
                  
                   lerpValue = Mathf.Lerp(startRotation,rotation,(timePassed/duration));
                  
                   
                   
                transform.eulerAngles = new Vector3(transform.rotation.x,lerpValue,0);

                // transform.rotation = quaternion.Euler(0f,lerpValue,0f);
                yield return null;
              }  
               
            
          
        }
    


}
