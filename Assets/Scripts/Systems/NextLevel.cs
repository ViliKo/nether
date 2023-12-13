using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class NextLevel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // Adjust the tag according to your player's tag
        {
            Debug.Log("Im here heeloll");
            GameManager.Instance.LoadScene("LastRise");
        }
    }
}
