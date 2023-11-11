using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class HealthObject : MonoBehaviour
{
    
    [Header("Healt amount")]
    public int HealthAmount;

    void Start()
    {
        HealthAmount = 1;
    }

    public int GiveHealth() => HealthAmount;
    

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag != "Player")return;

        other.gameObject.GetComponent<Player>().UpdateHealth(GiveHealth());
        Destroy(gameObject);

    }


}

