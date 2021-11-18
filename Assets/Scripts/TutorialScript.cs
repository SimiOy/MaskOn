using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    int tutDone;
    private Animator anim;

    private bool leftDone, rightDone, timerDone;

    public static TutorialScript instance;

    private void Awake()
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
    }
    void Start()
    {
        tutDone = GameMaster.instance.tutorialDone;
       // tutDone = 0;
        if(tutDone == 1)
        {
            //true we have played before
            this.gameObject.SetActive(false);
        }
        else
        {
            //we havent done before
            anim = GetComponent<Animator>();
        }
    }

    private void Update()
    {
        if(leftDone && rightDone && timerDone)
        {
            //all tuts done
            tutDone = 1;
            GameMaster.instance.tutorialDone = 1;
        }
    }

    public int TutRight()
    {
        if(rightDone == false)
        {
            anim.SetTrigger("right");
            rightDone = true;
            return 1;
        }
        return 0;
    }

    public int TutLeft()
    {
        if (leftDone == false)
        {
            anim.SetTrigger("left");
            leftDone = true;
            return 1;
        }
        return 0;
    }

    public void TutTimer()
    {
        if (timerDone == false)
        {
            anim.SetTrigger("timer");
            timerDone = true;
        }
    }

    public void TutFinished()
    {
        timerDone = leftDone = rightDone = true;
    }
}
