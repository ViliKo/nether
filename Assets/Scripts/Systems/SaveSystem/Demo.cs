using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;

public class Demo : MonoBehaviour
{
    private PlayerStats playerStats = new PlayerStats();
    private IDataService dataService = new JsonDataService();
    private bool encryptionEnabled = true;

    public void SerializeJson()
    {
        if (dataService.SaveData("/player-stats.json", playerStats, encryptionEnabled))
        {
            
            try
            {
                PlayerStats data = dataService.LoadData<PlayerStats>("/player-stats.json", encryptionEnabled);

                Debug.Log("Loaded text" + JsonConvert.SerializeObject(data));
            }
            catch (Exception e)
            {
                Debug.Log($"There was an error: {e.Message} {e.StackTrace}");
            }
        } 
        else
        {
            Debug.LogError("Could not save file! Show something on the Ui about it");
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SerializeJson();
        }
    }
}

