using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float startTimer;
    private float curTime;

    //ui stuff
    public Image fillImage;
    public Color initColor;
    public Color finalUIColor;

    private int stopAskingMe; //for tut

    private void Awake()
    {
        startTimer = DifficultyLevel.timer;
        stopAskingMe = GameMaster.instance.tutorialDone;

        curTime = startTimer;
    }
    void Update()
    {
        if(curTime>=0)
        {
            float fillPercentage = curTime / startTimer;
            fillImage.fillAmount = fillPercentage;
            fillImage.color = Color.Lerp(finalUIColor, initColor, fillPercentage);
            curTime -= Time.deltaTime;
        }
        else
            if(curTime<=0)
        {
            //menus
            LivesCounter.loseLife = true;
            //im losing a fkuin life every time i spawn a new lane in WTF

            //tut timer
            if(stopAskingMe == 0)
            {
                if(GameMaster.instance.tutorialDone == 0)
                {
                    //havent done it shit
                    TutorialScript.instance.TutTimer();
                    stopAskingMe = 1;
                }
            }

            StartUsage();
        }
    }

    public void StartUsage()
    {
        curTime = startTimer;
    }
}
