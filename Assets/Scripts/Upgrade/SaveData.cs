using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public string sceneName;
    public string lastManaBlockID;
    public float playerX, playerY;
    public int mana;
    public List<string> triggeredManaBlocks = new List<string>();
    public int maxMana;
    public int selectionLimit;
    public int shell;
}
