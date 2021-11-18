using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance { set; get; }
    public SaveState state; //im calling this now

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }

        Load();
    }

    public void InputSaveVariables(int money, int tutorial, int volume, int sfx)
    {
        //saving simple variables
        state.money = money;
        state.isTutorial = tutorial;
        state.isVolume = volume;
        state.isSFX = sfx;
    }

    public void InputLevelArrays(int[] unlockedScenes, int[] highScoresPerScene)
    {
        //saving long arrays
        //might add skins if i feel good

        int x = unlockedScenes.Length; //same as highScoresPerScene Length due to its natural build

        for (int i = 0; i < x; i++)
        {
            state.levelsHighscores[i] = highScoresPerScene[i];
            state.levelsUnlocked[i] = unlockedScenes[i];
        }
    }

    public void InputSkinsArray(int[] skins)
    {
        int x = skins.Length;
        for (int i = 0; i < x; i++)
        {
            //we are inputing whether or not they are unlocked
            state.allSkins[i] = skins[i];
        }
    }

    //save the whole script to player pref
    public void Save()
    {
        PlayerPrefs.SetString("save",DeseralizationHelperSave.Serialize<SaveState>(state));
    }

    //load state from player prefs
    public void Load()
    {
        if(PlayerPrefs.HasKey("save"))
        {
            state = DeseralizationHelperSave.Deserialize<SaveState>(PlayerPrefs.GetString("save"));

            //if new levels have been added
            //dont delete all save
            //append to existing one
            // WARNING DO NOT DELETE OLD LEVELS CANT FIX THAT CRAP
            int x = (SceneManager.sceneCountInBuildSettings - 1);
            int a = state.levelsHighscores.Length;
            if (a < x)
            {
                int[] substituteArray1 = new int[a];
                int[] substituteArray2 = new int[a];
                for (int i = 0; i < a; i++)
                {
                    substituteArray1[i] = state.levelsHighscores[i];
                    substituteArray2[i] = state.levelsUnlocked[i];
                }
                state = new SaveState();
                for (int y = 0; y < a; y++)
                {
                    state.levelsHighscores[y] = substituteArray1[y];
                    state.levelsUnlocked[y] = substituteArray2[y];
                }
                Save();
            }
        }
        else
        {
            state = new SaveState();
            Save();

            Debug.Log("No save file found. creating a new one");
        }

        Debug.Log(DeseralizationHelperSave.Serialize<SaveState>(state));
    }

    public void DeleteSave()
    {
        PlayerPrefs.DeleteKey("save");
        Load();
    }
}
