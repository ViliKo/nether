using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CompositeStateRunner;

public class EnemyPoolable : MonoBehaviour
{
    // Store initial state and position information
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private bool initialState;

    private AIBaseController _aiBaseController;
    private Collider2D _collider;
    
   

    private void Start()
    {
        // Capture initial state and position when the enemy is instantiated
        CaptureInitialState();

        _aiBaseController = GetComponent<AIBaseController>();
        _collider = GetComponent<Collider2D>();
    }

    public void CaptureInitialState()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialState = gameObject.activeSelf;
    }

    public void ResetToInitialState()
    {
        // Reset the enemy to its initial state and position
        _collider.enabled = true;
        _aiBaseController.SetSearchState();
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        gameObject.SetActive(initialState);

        Debug.Log("I am resetted");
    }
}
