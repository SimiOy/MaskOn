using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperSwipe : MonoBehaviour
{
    public Swipe swipeController;

    public Transform sickLane;
    public Transform healthyLane;

    public Transform objectToMove;

    private bool isSelected = false;
    private bool goRight, goLeft;

    public Spawning[] spPlace;

    private void Awake()
    {
        goRight = goLeft = isSelected = false;
    }
    void Update()
    {
        if(swipeController.SwipeLeft)
        {
            goLeft = true;
        }
        if(swipeController.SwipeRight)
        {
            goRight = true;
        }

        if(swipeController.Selected)
        {
            isSelected = true;
        }

        //where to go
        if(isSelected) //a lane was chosen and do movement
        {

            //objectToMove.GetComponent<Animator>().SetTrigger("Highlight");
            if(gameObject.transform!=null)
            {
                if(goRight)
                {
                    Reset();
                    objectToMove.GetComponent<MoveScript>().destination=healthyLane;
                    //Spawning.startMethod = true; asta merge in toti nu e bine
                    objectToMove.GetComponentInParent<HelpMeImstupid>().whereTo = 1;
                    objectToMove.GetComponentInParent<HelpMeImstupid>().startNow = true;
                    objectToMove.GetComponent<MoveScript>().startMove = true;
                    objectToMove.name = "noTouchy";
                    objectToMove.GetComponent<MoveScript>().endOfJourney = true;

                    //disable colliders
                    objectToMove.gameObject.GetComponent<MeshCollider>().enabled = false;

                    //function to change parent and opacity

                    Detach(objectToMove.gameObject);

                }else
                    if(goLeft)
                {
                    Reset();
                    objectToMove.GetComponent<MoveScript>().destination = sickLane;
                    //Spawning.startMethod = true;
                    objectToMove.GetComponentInParent<HelpMeImstupid>().whereTo = 0;
                    objectToMove.GetComponentInParent<HelpMeImstupid>().startNow = true;
                    objectToMove.GetComponent<MoveScript>().startMove = true;
                    objectToMove.name = "noTouchy";
                    objectToMove.GetComponent<MoveScript>().endOfJourney = true;

                    //disable colliders
                    objectToMove.gameObject.GetComponent<MeshCollider>().enabled = false;

                    Detach(objectToMove.gameObject);

                }
            }

        }
    }

    void Detach(GameObject gb)
    {
        gb.transform.parent = null;
        //doesnt work yet 
        Color color = gb.GetComponentInChildren<MeshRenderer>().material.color;
        color.a -= 0.3f;
        gb.GetComponentInChildren<MeshRenderer>().material.color = color;
    }

    private void Reset()
    {
        isSelected = goLeft = goRight = false;
    }
}
