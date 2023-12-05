using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    private float horizontalDistance;
    private float verticalDistance;
    public float maxOffsetX = 4f;
    public float maxOffsetY = 2f;




    public List<CinemachineVirtualCamera> VirtualCameraList;
    public CinemachineVirtualCamera currentCamera;
    //public Player playerScript;
    CinemachineFramingTransposer transposer;

    private void Awake() {
        //if(GameObject.FindWithTag ("Player") != null)
               // playerScript = GameObject.FindWithTag ("Player").GetComponent<Player>();


    }


    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < VirtualCameraList.Count; i++){

            if(VirtualCameraList[i].enabled){
                currentCamera = VirtualCameraList[i];
                if(currentCamera != null)
                {
                    if (transposer == null)
                    {
                        transposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

                    }
                }
            }
        }


        if (Mathf.Abs(Input.GetAxis("CameraHorizontal")) > 0 || Mathf.Abs(Input.GetAxis("CameraVertical")) > 0)
        {

            // Adjust the sensitivity by multiplying with a smaller factor
            float horizontalInput = Input.GetAxis("CameraHorizontal") * 0.5f;
            float verticalInput = Input.GetAxis("CameraVertical") * 0.5f;

            horizontalDistance += horizontalInput;
            verticalDistance += verticalInput;

            // Adjust the maximum offset based on your preference

            // Apply clamping with the adjusted values
            transposer.m_TrackedObjectOffset.x = Mathf.Clamp(horizontalDistance, -maxOffsetX, maxOffsetX);
            transposer.m_TrackedObjectOffset.y = Mathf.Clamp(verticalDistance, -maxOffsetY, maxOffsetY);
        }
        else
        {
            // Reset distances and offsets when there is no input
            horizontalDistance = 0;
            verticalDistance = 0;
            transposer.m_TrackedObjectOffset.x = 0;
            transposer.m_TrackedObjectOffset.y = 0;
        }
    }

    public void SwapCamerasHorizontal(CinemachineVirtualCamera cameraFromLeft,CinemachineVirtualCamera cameraFromRight, Vector2 triggerExitDirection ){
                                        //toimii vain horizontaalisesti
        if(currentCamera == cameraFromLeft && triggerExitDirection.x > 0){

            cameraFromRight.enabled = true;
            cameraFromLeft.enabled = false;

            currentCamera = cameraFromRight;
            //playerScript.currentVirtualCamera = cameraFromRight;

        }

        if(currentCamera == cameraFromRight  && triggerExitDirection.x < 0)
        {

            //Debug.Log("player current camera" + cameraFromRight + playerScript.currentVirtualCamera);
            cameraFromLeft.enabled = true;

            cameraFromRight.enabled = false;

            currentCamera = cameraFromLeft;
            //playerScript.currentVirtualCamera = cameraFromLeft;

        }
    }
}
