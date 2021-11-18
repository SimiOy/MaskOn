using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CrowdSpawne : MonoBehaviour
{
    public List<GameObject> humans = new List<GameObject>();
    public static CrowdSpawne instance;

    [Range(0.01f, 0.5f)]
    public float initialPopulationInfectionRate;
    [Range(0f, 0.1f)]
    public float humanSpreadRate;

    public int humansAlive;
    public int humansInfected;
    public int noTaps;

    public TextMeshProUGUI haText;
    public TextMeshProUGUI hiText;
    public TextMeshProUGUI clicksLeftText;

    public TextMeshProUGUI infoSSlider;
    public TextMeshProUGUI infoHUSlider;

    public CGMenu gameOver;

    [Range(0.2f, 3f)]
    public float timeMatChange;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        humansAlive = GameObject.FindObjectOfType<PerlinControl>().totalHumans;
        humansInfected = 0;
        noTaps = 0;
    }

    private void Start()
    {
        //init values
        ChangeNoticeTime(1.8f);
        ChangeSpreadRate(0.016f);
    }

    public void ResetAll()
    {
        humansAlive = GameObject.FindObjectOfType<PerlinControl>().totalHumans;
        humansInfected = 0;
        noTaps = humansInfected;
    }

    public void ChangeNoticeTime(float x) //in ui
    {
        //just the cg menu init var
        gameOver.visualTimeFormula = x;
        //Debug.Log(x);
        timeMatChange = x;
        infoHUSlider.text = x.ToString("F3");
    }

    public void ChangeSpreadRate(float x) //in ui
    {
        gameOver.infectionRateFormula = x;
        //Debug.Log(x);
        humanSpreadRate = x;
        infoSSlider.text = x.ToString("F3");
    }

    private void LateUpdate()
    {
        haText.text = humansAlive.ToString();
        hiText.text = humansInfected.ToString();
        clicksLeftText.text = (noTaps).ToString();
    }

    public void GameEnded()
    {
        GameObject.FindObjectOfType<PerlinControl>().startSelectingInfected = false;
        GameObject.FindObjectOfType<PerlinControl>().EnablingStuff(false);
        gameOver.UWonMenu();
    }
    public void AdEnded()
    {
        gameOver.hasAdPlayed = true;
    }

    public void AdFailed()
    {
        gameOver.adHasFailed = true;
    }
}
