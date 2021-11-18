using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public float transitionTime = 0.2f;

    public GameObject volumeBtnOn;
    public GameObject volumeBtnOff;

    public GameObject sfxBtnOn;
    public GameObject sfxBtnOff;

    public GameObject buttonNext;
    public GameObject buttonPrev;

    public GameObject playBUtton;

    public TextMeshProUGUI _textLevelName;
    public TextMeshProUGUI _textLevelCost;

    private bool _vol;
    private bool _sfx;
    private int volumePref;
    private int sfxPref;

    public static int levelIndex=2;
    int scenesCount;

    public TMP_Dropdown m_dropdown_quality;

    public GameObject startBtnTestLvlsGMS;

    public GameObject _cardPrefabOBJ;

    private void Awake()
    {

        scenesCount = SceneManager.sceneCountInBuildSettings;
        scenesCount -= GameMaster.instance.testLevel; //not counting main menu

        levelIndex = 2;
        //min& max locks
        if (levelIndex == scenesCount - 1)
        {
            buttonNext.GetComponent<Button>().interactable = false;
            buttonPrev.GetComponent<Button>().interactable = true;
        }
        else
            if (levelIndex == 2)
        {
            buttonPrev.GetComponent<Button>().interactable = false;
            buttonNext.GetComponent<Button>().interactable = true;
        }
        else
        {
            buttonNext.GetComponent<Button>().interactable = true;
            buttonPrev.GetComponent<Button>().interactable = true;
        }

        //is level bought
        if (GameMaster.instance.IsAvailable(levelIndex - 1) == false)
        {
            playBUtton.GetComponent<Button>().interactable = false;
        }
        else
            playBUtton.GetComponent<Button>().interactable = true;


        //get ad
        AdMobScript.instance.RequestRewardBasedVideo();

        int y = PlayerPrefs.GetInt("Quality");
        m_dropdown_quality.value = y;
        SetQuality(y);

        if(GameMaster.instance.money <100)
        {
            //disable
            startBtnTestLvlsGMS.GetComponent<Button>().interactable = true;
        }
        else
        {
            startBtnTestLvlsGMS.GetComponent<Button>().interactable = true;
        }    

        SkipAnimationAtStart();
    }

    void SkipAnimationAtStart()
    {
        this.GetComponent<Animator>().Play("Home", 0, 0.40f);
    }

    private void Start()
    {
        volumePref = GameMaster.instance.volumeLevel;
        sfxPref = GameMaster.instance.sfxLevel;

        GameObject.FindObjectOfType<AudioManager>().VolumeChange("SpecialEvent", 0f);
        //GameObject.FindObjectOfType<AudioManager>().VolumeChange("BgMusic", 0.2f);
        GameObject.FindObjectOfType<AudioManager>().Play("BgMusic");

        if (volumePref == 1)
        {
            _vol = true;
            volumeBtnOn.SetActive(true);
            volumeBtnOff.SetActive(false);
            GameObject.FindObjectOfType<AudioManager>().VolumeChange("BgMusic", 0.2f);
        }
        else
            if (volumePref == 0)
        {
            _vol = false;
            volumeBtnOn.SetActive(false);
            volumeBtnOff.SetActive(true);
            GameObject.FindObjectOfType<AudioManager>().VolumeChange("BgMusic", 0);
        }


        if (sfxPref == 1)
        {
            _sfx = true;
            sfxBtnOn.SetActive(true);
            sfxBtnOff.SetActive(false);
            GameObject.FindObjectOfType<AudioManager>().SFX_volume(_sfx);
        }
        else
            if (sfxPref == 0)
        {
            _sfx = false;
            sfxBtnOn.SetActive(false);
            sfxBtnOff.SetActive(true);
            GameObject.FindObjectOfType<AudioManager>().SFX_volume(_sfx);
        }

        //announcments in anim home at end
        // ActivateCardTutorial(0);

        _cardPrefabOBJ.GetComponent<CardTutDisplay>().IndexSetter(0);

    }

    public void OptionsMenu()
    {
        GameObject.FindObjectOfType<AudioManager>().Play("Click");
    }

    public void VolumeButton()
    {
        _vol = !_vol;
        if(_vol)
        {
            volumeBtnOn.SetActive(true);
            volumeBtnOff.SetActive(false);
            //to do music control
            GameMaster.instance.volumeLevel = 1;
            GameObject.FindObjectOfType<AudioManager>().VolumeChange("BgMusic", 0.2f);
        }
        else
            if(!_vol)
        {
            volumeBtnOn.SetActive(false);
            volumeBtnOff.SetActive(true);

            GameMaster.instance.volumeLevel = 0;
            GameObject.FindObjectOfType<AudioManager>().VolumeChange("BgMusic", 0);
        }
    }

    public void SFXButton()
    {
        _sfx = !_sfx;
        if (_sfx)
        {
            sfxBtnOn.SetActive(true);
            sfxBtnOff.SetActive(false);
            //to do music control
            GameMaster.instance.sfxLevel = 1;
            GameObject.FindObjectOfType<AudioManager>().SFX_volume(_sfx);
        }
        else
            if (!_sfx)
        {
            sfxBtnOn.SetActive(false);
            sfxBtnOff.SetActive(true);

            GameMaster.instance.sfxLevel = 0;
            GameObject.FindObjectOfType<AudioManager>().SFX_volume(_sfx);
        }
    }

    public void PlayGame()
    {
        GameObject.FindObjectOfType<AudioManager>().Play("Click");
        //resetting progress
        LivesCounter.livesLeft = 3;
        GameMaster.instance.currScore = 0;
        Score.scoreCounter = 0;

        //load ads in advance
        //AdMobScript.instance.RequestRewardBasedVideo();

        //not saved shit add warning
        if(GameMaster.instance.IsAvailable(levelIndex - 1))
        {
            //Debug.Log("i play " + levelIndex);
            StartCoroutine(LoadLevel(levelIndex)); //1->for Level
        }
    }

    public void ExtraMoneyAD()
    {
        AdMobScript.instance._typeOfAD = 2; //extra money
        AdMobScript.instance.ShowVideoRewardAd();
    }

    public void changeLevel(bool add)
    {
        if (add && levelIndex<scenesCount-1)
        {
            levelIndex++;

            ChangeUi(levelIndex);
            
            //call transitions
            moveCam.ChangeIndex(1);
            moveCam.startLerp = true;


        }
        else
            if(!add && levelIndex>1)
        {
            levelIndex--;

            ChangeUi(levelIndex);

            //call tansitions
            moveCam.ChangeIndex(-1);
            moveCam.startLerp = true;
        }

        //min& max locks
        if(levelIndex==scenesCount-1)
        {
            buttonNext.GetComponent<Button>().interactable = false;
            buttonPrev.GetComponent<Button>().interactable = true;
        }
        else
            if(levelIndex == 2)
        {
            buttonPrev.GetComponent<Button>().interactable = false;
            buttonNext.GetComponent<Button>().interactable = true;
        }
        else
        {
            buttonNext.GetComponent<Button>().interactable = true;
            buttonPrev.GetComponent<Button>().interactable = true;
        }

        //is level bought
        if (GameMaster.instance.IsAvailable(levelIndex - 1) == false)
        {
            playBUtton.GetComponent<Button>().interactable = false;
        }
        else
            playBUtton.GetComponent<Button>().interactable = true;

       // Debug.Log("I want to select Buildlevel " + levelIndex);
    }

    void ChangeUi(int levelIndex)
    {
        int i = levelIndex - 2;
        if (GameMaster.instance._sceneControl[i]._isAvailable == 0) //locked
        {

            _textLevelName.text = "Unlock " + GameMaster.instance.levelNames[i] + " for";
            int cost = GameMaster.instance.costsOfLevels[i];
             _textLevelCost.text = "$" + cost.ToString();
        }
        else
        {
            _textLevelName.text = GameMaster.instance.levelNames[i];
            _textLevelCost.text = "Unlocked";
        }
    }

    public void Buylevel()
    {
        int whichOne;
        whichOne = levelIndex - 1; //the previewed level
        //ad cube grey-scale to simulate not bought
        GameMaster.instance.UnlockLevel(whichOne);
        Debug.Log("buying level..." + whichOne);
        //is level bought
        if (GameMaster.instance.IsAvailable(levelIndex - 1) == false)
        {
            playBUtton.GetComponent<Button>().interactable = false;
        }
        else
            playBUtton.GetComponent<Button>().interactable = true;
    }
    public void QuitGame()
    {

        GameObject.FindObjectOfType<AudioManager>().Play("Click");

        GameMaster.instance.SaveGame();

        Application.Quit();
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        int x = m_dropdown_quality.value;

        PlayerPrefs.SetInt("Quality", x);
    }

    public void TestLevel()
    {
        if (GameMaster.instance.money < 100)
        {
            //disable
            startBtnTestLvlsGMS.GetComponent<Button>().interactable = true;
        }
        else
        {
            startBtnTestLvlsGMS.GetComponent<Button>().interactable = true;
            GameMaster.instance.SubtractMoney(100);
            AudioManager am = GameObject.FindObjectOfType<AudioManager>();
            am.VolumeChange("BgMusic", 0f);
            am.VolumeChange("SpecialEvent", 0.175f);
            am.Play("SpecialEvent");

            //index of level special
            StartCoroutine(LoadLevel(6));
        }
    }

    public void ActivateCardTutorial(int cardIndex)
    {
        Debug.Log("called yes");
        _cardPrefabOBJ.GetComponent<CardTutDisplay>().IndexSetter(cardIndex);
        _cardPrefabOBJ.SetActive(true);
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        //transition.SetTrigger("Start");
        //Debug.Log("menuu");
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }
}
