using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour, IDamageDealer
{
    // Start is called before the first frame update

    public int damage;
 
    public int DealDamage() => damage;
    

    void Start()
    {
        damage = 1;
    }


}
