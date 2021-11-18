using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CGMenu : MonoBehaviour
{
    public GameObject normalUI;
    public GameObject settingsUI;
    public GameObject victoryUi;

    public GameObject optionsBtn;

    public TextMeshProUGUI victoryText;

    bool pausedGame;
    public float humansFormula, infectionRateFormula, visualTimeFormula;

    public float aVar,bVar,cVar;

    public TextMeshProUGUI textPayout;

    public GameObject restartCstlyBtn;
    public GameObject restartFreeBtn;

    public Slider humansSavedSlider;
    public Slider moneyEarnedSlider;
    public Slider moneyTotalSlider;

    public TextMeshProUGUI NoHumans;
    public TextMeshProUGUI NoMoneyEarned;
    public TextMeshProUGUI NoCurrentMoney;

    public bool hasAdPlayed;
    public bool adHasFailed;

    private float initMoney;

    public bool hasDoneTut;
    public GameObject _cardPrefabOBJ;

    int x;


    private void Start()
    {
        /*
        AudioManager am = GameObject.FindObjectOfType<AudioManager>();
        am.VolumeChange("BgMusic",0f);
        am.VolumeChange("SpecialEvent", 0.175f);
        am.Play("SpecialEvent");
        */

        hasAdPlayed = false;
        adHasFailed = false;
        //presets
        humansFormula = 30f;
        infectionRateFormula =0.016f;
        visualTimeFormula =1.8f;

        pausedGame = true;
        normalUI.SetActive(!pausedGame);
        settingsUI.SetActive(pausedGame);
        optionsBtn.SetActive(false);
        victoryUi.SetActive(false);

        textPayout.text =  CalculatePayout();

        x = _cardPrefabOBJ.GetComponent<CardTutDisplay>()._card.Length;
    }

    private void Update()
    {
        if(pausedGame)
        {
            textPayout.text = CalculatePayout();
        }

        if(adHasFailed)
        {
            //dunno yet
            adHasFailed = false;
        }

        if(hasAdPlayed)
        {
            RestartFree();
            hasAdPlayed = false;
        }
    }

    public void ToggleMenus()
    {
        pausedGame = !pausedGame;
        if (pausedGame == true)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
        //x true means game playing, x false means game paused
        normalUI.SetActive(!pausedGame);
        settingsUI.SetActive(pausedGame);
        victoryUi.SetActive(false);
    }

    public void UWonMenu()
    {
        Time.timeScale = 0f;
        pausedGame = true;
        normalUI.SetActive(false);
        settingsUI.SetActive(false);
        victoryUi.SetActive(true);

        //requesting ad if we need it
        AdMobScript.instance.RequestRewardBasedVideo();

        int remainingHumans = CrowdSpawne.instance.humansAlive;;

        victoryText.text = "You saved " + remainingHumans.ToString() +" humans!";

        SliderCorrections();
    }

    void SliderCorrections()
    {
        int initHumans = GameObject.FindObjectOfType<PerlinControl>().totalHumans;
        int remainingHumans = CrowdSpawne.instance.humansAlive;
        int percentageSaved = (int)(((float)remainingHumans / (float)initHumans) * 100);
        string x = percentageSaved.ToString("F0");
        humansSavedSlider.maxValue = initHumans;
        humansSavedSlider.minValue = 0;
        //humansSavedSlider.value = remainingHumans;
        NoHumans.text = x+"%";
        StartCoroutine(AnimateSliderOverTime(2, 0, remainingHumans,0));

        //anim
        int moneyEarned =(int)(((float)remainingHumans / (float)initHumans) * initMoney);
        Debug.Log(moneyEarned);
        moneyEarnedSlider.maxValue = moneyEarned;
        moneyEarnedSlider.minValue = 0;
        //moneyEarnedSlider.value = moneyEarned;
        NoMoneyEarned.text = "$" + moneyEarned.ToString();
        StartCoroutine(AnimateSliderOverTime(2, 0, moneyEarned,1));

        int curMoney = GameMaster.instance.money;
        moneyTotalSlider.minValue = curMoney;
        moneyTotalSlider.maxValue = (curMoney + moneyEarned);
        // moneyTotalSlider.value = (curMoney + moneyEarned);
        int var = curMoney + moneyEarned;
        NoCurrentMoney.text = "$" + (curMoney + moneyEarned).ToString();
        StartCoroutine(AnimateSliderOverTime(2, curMoney, var, 2));

        GameMaster.instance.money += moneyEarned;

    }

    IEnumerator AnimateSliderOverTime(float seconds, float minV, float maxV, float index)
    {
        float animationTime = 0f;
        while (animationTime < seconds)
        {
            animationTime += Time.unscaledDeltaTime;
            float lerpValue = animationTime / seconds;
            if (index == 0)
            {
                humansSavedSlider.value = Mathf.Lerp(minV, maxV, lerpValue);
            }
            else
            if (index == 1)
            {
                moneyEarnedSlider.value = Mathf.Lerp(minV, maxV, lerpValue); 
            }
            else
            if (index == 2)
            {
                moneyTotalSlider.value = Mathf.Lerp(minV, maxV, lerpValue); 
            }
            else
                yield return null;

            yield return null;
        }
    }

    public void StartGame()
    {
        pausedGame = false;
        normalUI.SetActive(!pausedGame);
        settingsUI.SetActive(pausedGame);
        optionsBtn.SetActive(false);
        victoryUi.SetActive(false);
    }

    public void RestartCostly()
    {
        if(GameMaster.instance.money >= 100)
        {
            GameMaster.instance.money -= 100;
            GameMaster.instance.SaveGame();
            //presets

            pausedGame = true;
            normalUI.SetActive(!pausedGame);
            settingsUI.SetActive(pausedGame);
            optionsBtn.SetActive(false);
            victoryUi.SetActive(false);

            textPayout.text = CalculatePayout();
        }
        else
        {
            //disable button
            restartCstlyBtn.GetComponent<Button>().interactable = false;
        }
    }

    public void GetAdToRestart()
    {
        AdMobScript.instance._typeOfAD = 3; //play again in crwod gen
        AdMobScript.instance.ShowVideoRewardAd();
    }

    void RestartFree()
    {
        GameMaster.instance.SaveGame();
        //presets
        humansFormula = 30f;
        infectionRateFormula = 0.016f;
        visualTimeFormula = 1.8f;

        pausedGame = true;
        normalUI.SetActive(!pausedGame);
        settingsUI.SetActive(pausedGame);
        optionsBtn.SetActive(false);
        victoryUi.SetActive(false);

        textPayout.text = CalculatePayout();
    }

    public void ReturnToMainMenu()
    {
        // Debug.Log("menuu");
        AudioManager am = GameObject.FindObjectOfType<AudioManager>();
        am.Play("Click");
        am.VolumeChange("BgMusic", 0f);
        am.VolumeChange("SpecialEvent", 0.175f);
        am.Play("SpecialEvent");


        GameMaster.instance.SaveGame();

        Time.timeScale = 1f;
        SceneManager.LoadScene(1); //1-< for main menu
    }

    string CalculatePayout()
    {
        string txt;
        float x;
        x = aVar * humansFormula + bVar * (infectionRateFormula * 10) - cVar * visualTimeFormula;
        txt = x.ToString("F0");
        initMoney = x;
        return ("$"+txt);
    }

    public void ActivateCardTutorial(int cardIndex)
    {
        if (hasDoneTut == false)
        {
            _cardPrefabOBJ.GetComponent<CardTutDisplay>().IndexSetter(cardIndex);
            _cardPrefabOBJ.SetActive(true);
        }
        if(cardIndex >= x)
        {
            //stop doing stuff
            hasDoneTut = true;
        }
    }

}
