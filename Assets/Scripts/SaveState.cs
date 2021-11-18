using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveState
{
    public int money = 0;
    public int isTutorial = 0;
    public int isVolume = 1;
    public int isSFX = 1;

    public int[] levelsHighscores = { 0, 0, 0, 0,0,0};
    public int[] levelsUnlocked = { 1, 0, 0, 0,0 ,0};

    //max 100 skins imo
    public int[] allSkins = new int[100];
}
