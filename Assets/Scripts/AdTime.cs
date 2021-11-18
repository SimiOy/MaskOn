using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdTime : MonoBehaviour
{
    float startTimer;
    private float curTime;

    //ui stuff
    public Image fillImage;
    public Color initColor;
    public Color finalUIColor;

    public PauseMenu gb;

    public bool retry;

    private void OnEnable()
    {
        startTimer = 5f;
        curTime = startTimer;
    }
    void Update()
    {
            if(curTime>=0)
            {
                float fillPercentage = curTime / startTimer;
                fillImage.fillAmount = fillPercentage;
                fillImage.color = Color.Lerp(finalUIColor, initColor, fillPercentage);
                curTime -= Time.unscaledDeltaTime;
            }
            else
                if(curTime<=0)
            {
                if (retry)
                {
                    //menus
                    gb.DeathMenu();
                }
                else
                    gb.ClosePopUP();
            }
    }
}
