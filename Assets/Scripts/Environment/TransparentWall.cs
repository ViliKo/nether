using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentWall : MonoBehaviour
{
    public Material transparentMaterial;
    private Material originalMaterial;


    void Start()
    {
        // Save the original material
        originalMaterial = GetComponent<SpriteRenderer>().material;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UpdateMaterial();
        }
    }


    private void UpdateMaterial()
    {
        // Use transparent material if the player is inside, otherwise use the original material
        Material targetMaterial = transparentMaterial;
        GetComponent<SpriteRenderer>().material = targetMaterial;
    }
}
