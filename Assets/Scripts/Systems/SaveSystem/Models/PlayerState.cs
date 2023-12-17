using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerState
{
    public int currentLevel = 0;

    public List<SaveSlot> saveSlots = new List<SaveSlot>()
    {
        new SaveSlot(), // Slot 1
        new SaveSlot(), // Slot 2
        new SaveSlot(), // Slot 3
        new SaveSlot(), // Slot 4
    };

}

[Serializable]
public class SaveSlot
{
    public int currentLevel = 0;
    // Add more save slot information here
}