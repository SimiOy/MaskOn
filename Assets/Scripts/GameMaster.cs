using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;
    //valori ce raman + player prefs
    public int currScore;
    public int highScore;

    public int curLives;
    public int[] costsOfLevels; //called for CostOfLevelUI in MainMenu
    public string[] levelNames;
    public int money;
    public int multiplicationfactorOfMoneyToMakeYouFeelAwesome;

    //volume control
    public int volumeLevel = 1;
    public int sfxLevel = 1;

    public ScenesScores[] _sceneControl;
    private int scenesCount; //no of levels
    private int whatSceneWeIn;

    public int tutorialDone;

    public int testLevel; //0 if not a test level; else index for numbers of test levels


    void Awake()
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

        Time.timeScale = 1f;
        curLives = 3;  

        scenesCount = SceneManager.sceneCountInBuildSettings;
        _sceneControl = new ScenesScores[scenesCount];
        whatSceneWeIn = SceneManager.GetActiveScene().buildIndex -1; // not on menu load but during the play

        LoadGAME();
    }

    private void Update()
    {
        curLives = LivesCounter.livesLeft;
        currScore = Score.scoreCounter;
        // Debug.Log(curLives);
        whatSceneWeIn = SceneManager.GetActiveScene().buildIndex -2;

        if (whatSceneWeIn >= 0)
        {
            highScore = _sceneControl[whatSceneWeIn]._highestScore;
        }

        if (currScore > highScore)
        {
            highScore = currScore;
            _sceneControl[whatSceneWeIn].NewHighscore(highScore);
            //new highscore ui

        }
    }

    void LoadGAME()
    {
        //Already doing this in the awake method of SaveManager
        //SaveManager.instance.Load();

        TransferLoadedDATA();
    }

    void TransferLoadedDATA()
    {
        money = SaveManager.instance.state.money;
        tutorialDone = SaveManager.instance.state.isTutorial;
        volumeLevel = SaveManager.instance.state.isVolume;
        sfxLevel = SaveManager.instance.state.isSFX;
        
        int x = _sceneControl.Length - 2;
        for (int i = 0; i < x; i++)
        {
            int hs = SaveManager.instance.state.levelsHighscores[i];
            int availability = SaveManager.instance.state.levelsUnlocked[i];
            int price = costsOfLevels[i]; //assigned in inspector
            string nm = levelNames[i]; //assigned in inspector
             _sceneControl[i] = new ScenesScores(hs,availability,nm,price);

            Debug.Log("Logging entry on " + _sceneControl[i]._name + " that is " + _sceneControl[i]._isAvailable + " and costs " + _sceneControl[i]._priceToPay);
        }
        
    }
    private void OnDisable()
    {
        SaveGame();
    }
    public void SaveGame()
    {
        SaveManager.instance.InputSaveVariables(money, tutorialDone, volumeLevel, sfxLevel);
        SkinManager.instance.SaveSkins();

        int x = _sceneControl.Length - 1;

        int[] localHighscores = new int[x];
        int[] localUnlockedLevels = new int[x];

        for (int i = 0; i < x - testLevel; i++)
        {
            localHighscores[i] = _sceneControl[i]._highestScore;
            localUnlockedLevels[i] = _sceneControl[i]._isAvailable;
        }

        SaveManager.instance.InputLevelArrays(localUnlockedLevels, localHighscores);

        //actual saving of game
        SaveManager.instance.Save();
    }
    public int HowMuchYouEarned()
    {
        int score = currScore;
        return (int)(score * multiplicationfactorOfMoneyToMakeYouFeelAwesome);//for to see how much you earned
    }

    public void AddMoney(int score)
    {
        //lerp in moneyOutput
        //here just instant implementation
        money += (int)(score * multiplicationfactorOfMoneyToMakeYouFeelAwesome);
        SaveGame();
    }

    public void SubtractMoney(int cost)
    {
        money -= cost;
        SaveGame();
        Debug.Log(money);
    }

    public void UnlockLevel(int levelIndex)
    {
        _sceneControl[levelIndex-1].BecomeAvailable();
        SaveGame();
    }

    public bool IsAvailable(int levelIndex)
    {
        //mainn menu disable button if not availbale trick
        int x;
        x = _sceneControl[levelIndex - 1]._isAvailable;
        //Debug.Log(_sceneControl[levelIndex - 1]._name + " was checked");
        if (x == 1)
            return true;
        else
            return false;
    }

    public void DEBUGMONEY()
    {
        money += 2000;
    }

    public void DEBUGSAVESYSTEM()
    {
        SaveManager.instance.DeleteSave();
    }
}
