using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Crowdmovement : MonoBehaviour
{
    public List<Vector3> destination = new List<Vector3>();
    public int index;
    private Vector3 currDestination;
    private float speedToMove;
    Animator animcontroller;

    public bool startMove = false;

    int numberOfInteractions;
    float checkInRadius;

    //the defalut move speed is 2 so i divide curr move speed by 2 to get the multiplication effect to apply on my walk anim
    private void Start()
    {
        index = 0;
        numberOfInteractions = 0;
        destination = new List<Vector3>();
        animcontroller = GetComponent<Animator>();
        speedToMove = Random.Range(2f, 6f);
        animcontroller.SetFloat("SpeedMult", speedToMove / 2);

    }

     void FixedUpdate()
    {
        if(startMove && (index <= destination.Count - 1))
        {
            
            transform.position = Vector3.MoveTowards(transform.position, currDestination, speedToMove * Time.fixedDeltaTime);

            // Determine which direction to rotate towards
            Vector3 targetDirection = currDestination - transform.position;

            // The step size is equal to speed times frame time.
            float singleStep = speedToMove * Time.fixedDeltaTime;

            // Rotate the forward vector towards the target direction by one step
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

            // Calculate a rotation a step closer to the target and applies rotation to this object
            transform.rotation = Quaternion.LookRotation(newDirection);

            animcontroller.SetTrigger("Walk");

            //when gets close
            if (Vector3.Distance(transform.position, currDestination) < 0.2f)
            {

                if (index + 1 > destination.Count - 1)
                {
                    //maybe call function again to repeat itself
                    startMove = false;
                }
                else
                {
                    destination.RemoveAt(index);
                    //index++;
                    currDestination = LoopTroughAllDestination(index);
                }
            }
        }
        else
        {
            animcontroller.SetTrigger("Idlee");
        }
    }

    private Vector3 LoopTroughAllDestination(int i)
        {
            return ( new Vector3((float)Random.Range(-0.95f, 0.95f) + destination[i].x, destination[i].y, (float)Random.Range(-0.95f, 0.95f) + destination[i].z));
        }

    public void ClearAll()
    {
        index = 0;
        numberOfInteractions = 0;
        startMove = false;
        destination = new List<Vector3>();
    }

    public void SetDestinations(Vector3 pointDestination, int setInteractionNumber)
    {
        if (numberOfInteractions <= setInteractionNumber)
        {
            if (destination.Contains(pointDestination))
            {
                //no more adding it infinite loops
            }
            else
            {
                if(numberOfInteractions == 0)
                {
                    //first steps
                    destination.Add(pointDestination);
                    numberOfInteractions++;
                    return;
                }
                else
                if(Mathf.Abs(Vector3.Distance(destination[destination.Count - 1],pointDestination)) > checkInRadius)
                {
                    destination.Add(pointDestination);
                    numberOfInteractions++;
                }
            }
        }
        else
            return;
    }

    public void StartMovement(float treshHouldRadius)
    {
        index = 0;
        checkInRadius = treshHouldRadius;
        currDestination = LoopTroughAllDestination(index);

        startMove = true;
    }
}

        