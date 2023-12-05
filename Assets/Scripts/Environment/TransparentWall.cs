using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentWall : MonoBehaviour
{
    public Material transparentMaterial;
    private Material originalMaterial;
    private bool isPlayerInside = false;

    void Start()
    {
        // Save the original material
        originalMaterial = GetComponent<SpriteRenderer>().material;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            UpdateMaterial();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            UpdateMaterial();
        }
    }

    private void UpdateMaterial()
    {
        // Use transparent material if the player is inside, otherwise use the original material
        Material targetMaterial = isPlayerInside ? transparentMaterial : originalMaterial;
        GetComponent<SpriteRenderer>().material = targetMaterial;
    }
}
