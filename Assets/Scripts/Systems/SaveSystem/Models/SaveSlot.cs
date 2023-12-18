using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SaveSlot
{
    public int currentLevel = 0;
    public bool isInitiated = false;

    // Add more save slot information here

    public SaveSlot()
    {
    }
}