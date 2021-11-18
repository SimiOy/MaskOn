using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneyOutput : MonoBehaviour
{
    TextMeshProUGUI _textMoneyTotal;
    TextMeshProUGUI _textHowMuchYouEarned;
    public bool what;
    float lerp = 0f;
    public float duration = 2f;
    private int finalScore;
    private int startingScore;

    public bool _isDouble;

    private void Awake()
    {
        _isDouble = false;
    }
    private void OnEnable()
    {
            if (what)
            {
                _textMoneyTotal = GetComponent<TextMeshProUGUI>();
                startingScore = GameMaster.instance.money;
                GameMaster.instance.AddMoney(GameMaster.instance.currScore);
                finalScore = GameMaster.instance.money;
                GetComponentInParent<Animator>().SetTrigger("StartClash");
            }
            else
            {
                _textHowMuchYouEarned = GetComponent<TextMeshProUGUI>();
                int earned = GameMaster.instance.HowMuchYouEarned();
                if(_isDouble)
            {
                earned *= 2;
                _textHowMuchYouEarned.text = "+ $" + earned.ToString();
                _isDouble = false;

            }
                else
            {
                _textHowMuchYouEarned.text = "+ $" + earned.ToString();
            }
            }
    }

    private void Update()
    {
            if(what == true) //just for the total money thingy tired and lazy to make two scripts lol
            if (startingScore < finalScore)
                SmoothMoney();
    }

    public void SmoothMoney()
    {
        lerp += Time.unscaledDeltaTime / duration;
        startingScore = (int)Mathf.Lerp(startingScore, finalScore, lerp);
        if(finalScore - startingScore < 0.01f)
        {
            startingScore = finalScore;
            lerp = 0f;
        }
        _textMoneyTotal.text = "$" + startingScore.ToString();
    }
}
