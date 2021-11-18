using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpMeImstupid : MonoBehaviour
{
    public bool startNow;

    public Spawning thisShit;

    public int whereTo; //1 dreapta 0 stanga
    public float temperatura;
    void Start()
    {
        whereTo = 2;
        startNow = false;

        //iau temperatrura
        temperatura = thisShit.temperatura;
    }

    void Update()
    {
       //iau temperatrura
       temperatura = thisShit.temperatura;

        if(startNow)
        {
            //misc
            thisShit.GetComponent<Spawning>().startMethod = true;
            Compare();
            startNow = false;

        }
    }

    void Compare()
    {
        if (whereTo == 1) //healthy
        {
            if (temperatura <= 37.3) //corect
            {
                Score.scoreCounter += DifficultyLevel.pointsMultiplier;
            }
            else
                LivesCounter.loseLife = true;
        }
        else
            if (whereTo == 0) //sick
        {
            if (temperatura > 37.3) //corect
            {
                Score.scoreCounter += DifficultyLevel.pointsMultiplier;
            }
            else
                LivesCounter.loseLife = true;
        }
    }
}
