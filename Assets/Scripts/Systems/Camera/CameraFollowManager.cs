using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFollowManager : MonoBehaviour
{

    public Transform playerTransform;
    public float followSpeed = 5f;
    public float peekSpeed = 2f;
    public float peekTime = 1f;

    private CinemachineFreeLook freeLookComponent;

    private void Start()
    {
        freeLookComponent = GetComponent<CinemachineFreeLook>();
    }

    private void Update()
    {
        FollowPlayer();
        PeekDirection();
    }

    private void FollowPlayer()
    {
        if (playerTransform != null)
        {
            transform.position = Vector3.Lerp(transform.position, playerTransform.position, followSpeed * Time.deltaTime);
        }
    }

    private void PeekDirection()
    {
        float horizontalInput = Input.GetAxis("RightStickHorizontal");

        if (Mathf.Abs(horizontalInput) > 0.1f)
        {
            // Adjust the rig height for peeking
            freeLookComponent.m_Orbits[1].m_Height = Mathf.Lerp(freeLookComponent.m_Orbits[1].m_Height, 4f, Time.deltaTime * peekSpeed);

            // Rotate the rig based on stick input
            freeLookComponent.m_XAxis.m_InputAxisValue = horizontalInput;
        }
        else
        {
            // Reset the rig height when stick is neutral
            freeLookComponent.m_Orbits[1].m_Height = Mathf.Lerp(freeLookComponent.m_Orbits[1].m_Height, 0f, Time.deltaTime * peekSpeed);

            // Reset the X-axis input when stick is neutral
            freeLookComponent.m_XAxis.m_InputAxisValue = Mathf.Lerp(freeLookComponent.m_XAxis.m_InputAxisValue, 0f, Time.deltaTime * peekSpeed);
        }
    }
}
