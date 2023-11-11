using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // Start is called before the first frame update
  

    public List<CinemachineVirtualCamera> VirtualCameraList;

    public CinemachineVirtualCamera currentCamera;


    public Player playerScript;

    private void Awake() {
     
        if(GameObject.FindWithTag ("Player") != null){
                playerScript = GameObject.FindWithTag ("Player").GetComponent<Player>();
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        for(int i = 0; i < VirtualCameraList.Count; i++){

            if(VirtualCameraList[i].enabled){
                currentCamera = VirtualCameraList[i];

            }


        }
        Debug.Log("CUrretn camera " + currentCamera); 
        
    }




        public void SwapCamerasHorizontal(CinemachineVirtualCamera cameraFromLeft,CinemachineVirtualCamera cameraFromRight, Vector2 triggerExitDirection ){
                                            //toimii vain horizontaalisesti
            if(currentCamera == cameraFromLeft && triggerExitDirection.x > 0){

                cameraFromRight.enabled = true;

                cameraFromLeft.enabled = false;

                currentCamera = cameraFromRight;
                playerScript.currentVirtualCamera = cameraFromRight;

            }


             if(currentCamera == cameraFromRight  && triggerExitDirection.x < 0){

                Debug.Log("player current camera" + cameraFromRight + playerScript.currentVirtualCamera);
                cameraFromLeft.enabled = true;

                cameraFromRight.enabled = false;

                currentCamera = cameraFromLeft;
                playerScript.currentVirtualCamera = cameraFromLeft;

            }


        }




}
