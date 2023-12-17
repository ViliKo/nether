using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public List<Transform> checkpoints = new List<Transform>();
    private Transform activeCheckpoint;


    private void Start()
    {


        Debug.Log("I found the player");
        

    }


    public void ActivateCheckpoint(Transform checkpoint)
    {
        activeCheckpoint = checkpoint;
    }

    public Vector3 GetActiveCheckpointPosition()
    {
        return activeCheckpoint.position;
    }
}
