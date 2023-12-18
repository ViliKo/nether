using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateLevel : MonoBehaviour
{
    public int newLevel = -100;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            GameManager.Instance.SwitchToLevel(newLevel-1);
    }
}
