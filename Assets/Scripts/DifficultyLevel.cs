using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyLevel : MonoBehaviour
{
    public static float timeToMove=2f;
    public static float timer=5f;
    public static int pointsMultiplier = 1;

    private int i = 0;

    public float[] cubeSpeed;
    public float[] timeDiff;
    public int[] NoLanes;
    public int[] PointMatter;

    public int _increasedPayouts;

    public GameObject[] gbRow2;
    public GameObject[] gbRow3;

    private void Awake()
    {
        i = 0;
        actualizareNivel(i);
        i++;
    }

    private void Start()
    {
        GameMaster.instance.multiplicationfactorOfMoneyToMakeYouFeelAwesome = _increasedPayouts;
    }

    private void Update()
    {
        //gets harder
        //Debug.Log(i);
        if(Score.scoreCounter >= i*10)
        {
            actualizareNivel(i);
            if (i + 1 < cubeSpeed.Length)
                i++;
            //in case they didnt get all tuts we consider them pros
            TutorialScript.instance.TutFinished();
        }
    }

    void actualizareNivel(int k)
    {

            timeToMove = cubeSpeed[k];
            timer = timeDiff[k];
            pointsMultiplier = PointMatter[k];

            if(k<=NoLanes.Length)
        {
            if(NoLanes[k]==2)
            {
                for (int j = 0; j < gbRow2.Length; j++)
                {
                    gbRow2[j].SetActive(true);
                    //LivesCounter.livesLeft++;
                }
            }
                if (NoLanes[k] == 3)
             {
                for (int j = 0; j < gbRow3.Length; j++)
                {
                    gbRow3[j].SetActive(true);
                   // LivesCounter.livesLeft++;
                }
             }
        }
    }
}
