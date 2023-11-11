using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;



[RequireComponent(typeof(BoxCollider2D))]
public class CameraSwitchTrigger : MonoBehaviour
{


    //Jos halutaan et vaihtuu myös ylös ja alas pitää muokata
    public CinemachineVirtualCamera cameraRight;
    public CinemachineVirtualCamera cameraLeft;

   public  CameraManager cameraManager;

    private BoxCollider2D _collider;


    private void Start() {
        _collider = GetComponent<BoxCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){



        }
    }

    private void OnTriggerExit2D(Collider2D other) {

           if(other.gameObject.tag == "Player"){
                //voit tulla tähän oveen oikealta monella eri kameralla.
                // jos on vaik eri reittejä joista tulee mukaan eri kamera.
                //pitää hakee edellisestä scenestä tää 
                // if(cameraRight == null ){
                //     cameraRight = other.gameObject.GetComponent<Player>().currentVirtualCamera;   
                //     Debug.Log("Current virtual camera from playe" + cameraRight);
                // }
    
        
                Vector2 ExitDirection = (other.transform.position - _collider.bounds.center).normalized;

                 cameraManager.SwapCamerasHorizontal(cameraLeft,cameraRight,ExitDirection);     
            
             }
    
    }



}
