using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyPoolingManager : MonoBehaviour
{

    public static EnemyPoolingManager Instance;



    private EnemyPoolable[] enemies;

    private void Awake()
    {
        Instance = this;
        InitializeEnemyPools();
        //Debug.Log(enemyPools.Values);
    }

    private void InitializeEnemyPools()
    {
        enemies = FindObjectsOfType<EnemyPoolable>();
    }

    public void ResetAllEnemiesToInitialState()
    {
        Debug.Log(": Im trying to reset something myself");

        foreach (var enemy in enemies)
        {
            // Get the EnemyPoolable component and reset to initial state
            enemy.ResetToInitialState();
        }
    }

}
