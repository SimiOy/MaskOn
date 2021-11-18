using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool isPaused = false;
    public static bool hadAd = true;
   // public Animator transition;
    public float transitionTime = 1f;

    public static PauseMenu _deathMenu;

    public GameObject PauseButton;
    public GameObject PauseMenuUI;
    public GameObject DeathMenuUI;
    public GameObject RetryAdsUI;
    public GameObject AreYouSureUI;
    public GameObject DoubleUI;

    public GameObject MoneyHolderForAds;

    public GameObject otherCanvas;

    public Timer[] _timers;

    public Text _noInternet;

    readonly float chanceToGetRewardedAd = 0.65f;

    private void Awake()
    {
        PauseButton.SetActive(true);
        hadAd = true;
    }

    private void Start()
    {
        //asign ad mob script the current pause menu items
        AdMobScript.instance._pausemenu = this.gameObject.GetComponent<PauseMenu>();
        AdMobScript.instance._notInternetText = _noInternet;
    }

    public void PauseGame()
    {
        GameObject.FindObjectOfType<AudioManager>().Play("Click");

        PauseMenuUI.SetActive(true);
        otherCanvas.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void RetryAdsMenu()
    {
        Time.timeScale = 0f;

        //load ads manager
        AdMobScript.instance.RequestRewardBasedVideo();

        RetryAdsUI.SetActive(true);
        PauseButton.SetActive(false);
        otherCanvas.SetActive(false);
        //isPaused = true;
    }

    public void DeathMenu()
    {
        RetryAdsUI.SetActive(false);
       // MoneyHolderForAds.SetActive(false);
        DeathMenuUI.SetActive(true);
        PauseButton.SetActive(false);
        otherCanvas.SetActive(false);
        Time.timeScale = 0f;
        //isPaused = true;

        GameMaster.instance.SaveGame();

        bool getAd = ChanceToGetRewardedAD(chanceToGetRewardedAd);
        if (getAd)
        {
            DoubleRewardsMenu();
        }
        else
        {
            //interstitial
            AdMobScript.instance.ShowInterstititalAd();
        }
    }

    bool ChanceToGetRewardedAD(float x)
    {
        //higher x bigger chance for rewarded menu

        if(Random.Range(0f,1f) <= x)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ContinueButton()
    {
        hadAd = true;
        //ads

        //pause timers

        //RetryAdsUI.SetActive(false);

        AdMobScript.instance._typeOfAD = 0; //extra life
        AdMobScript.instance.ShowVideoRewardAd();
    }

    public void RewardYes()
    {
        RetryAdsUI.SetActive(false);
        //pausing the game so it doenst start with issues
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        //how many lives restored
        LivesCounter.livesLeft = 1;

        //reset timers for being nice boi
        for (int i = 0; i < _timers.Length; i++)
        {
            if (_timers[i].gameObject.activeSelf)
            {
                //if it is turned on
                _timers[i].StartUsage();
            }
        }
    }

    public void RewardNo()
    {
        DeathMenu();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        //hadAd = false;

        GameObject.FindObjectOfType<AudioManager>().Play("Click");

        //reset score
        Score.scoreCounter = 0;
        LivesCounter.livesLeft = 3;

        //isPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void ResumeGame()
    {
        GameObject.FindObjectOfType<AudioManager>().Play("Click");

        AreYouSureUI.SetActive(false);
        PauseMenuUI.SetActive(false);
        RetryAdsUI.SetActive(false);
        PauseButton.SetActive(true);
        otherCanvas.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void AreYouSureYouWantToExit()
    {
        AreYouSureUI.SetActive(true);
        PauseMenuUI.SetActive(false);
        RetryAdsUI.SetActive(false);
        PauseButton.SetActive(false);
        otherCanvas.SetActive(false);
        Time.timeScale = 0f;
        //isPaused = true;
    }

    public void LoadMenu()
    {
        // Debug.Log("menuu");
        GameObject.FindObjectOfType<AudioManager>().Play("Click");

        GameMaster.instance.SaveGame();

        Time.timeScale = 1f;
        StartCoroutine(LoadLevel(1)); //1->for MainMenu
    }

    //called on death
    void DoubleRewardsMenu()
    {
        //DeathMenuUI.SetActive(false);
        MoneyHolderForAds.SetActive(false);
        DoubleUI.SetActive(true);

        //load ad function
        AdMobScript.instance.RequestRewardBasedVideo();
    }

    public void ClosePopUP()
    {
        DoubleUI.SetActive(false);
        MoneyHolderForAds.SetActive(true);
    }

    public void AcceptAd()
    {
        //display ad
        DoubleUI.SetActive(false);
        AdMobScript.instance._typeOfAD = 1; //double coins
        AdMobScript.instance.ShowVideoRewardAd(); 
    }

    public void DoubledCoinsSucces()
    {
        //double reward

        //if rewarded
        //GameMaster.instance.currScore *= 2;
        RepairMonetText();
        ClosePopUP();
       //MoneyHolderForAds.SetActive(true);
    }

    public void QuitGame()
    {
        GameObject.FindObjectOfType<AudioManager>().Play("Click");

        GameMaster.instance.SaveGame();

        Application.Quit();
    }

    void RepairMonetText()
    {
        foreach(Transform gb in MoneyHolderForAds.gameObject.transform)
        {
            GameObject go = gb.gameObject;
            go.GetComponent<MoneyOutput>()._isDouble = true;
        }
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        //transition.SetTrigger("Start");
        //Debug.Log("menuu");
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }

    IEnumerator WaitMe()
    {
        yield return new WaitForSecondsRealtime(1f);
    }
}
