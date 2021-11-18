using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    public Transform destination;
    public bool startMove;
    private float speedToMove;
    public bool endOfJourney;
    Animator animcontroller;

    //the defalut move speed is 2 so i divide curr move speed by 2 to get the multiplication effect to apply on my walk anim

    private void Awake()
    {
        speedToMove = DifficultyLevel.timeToMove;
    }
    private void Start()
    {
        startMove = false;
        endOfJourney = false;
        animcontroller = GetComponent<Animator>();
    }
    void FixedUpdate()
    {

        if(startMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination.position, speedToMove * Time.fixedDeltaTime);

            // Determine which direction to rotate towards
            Vector3 targetDirection = destination.position - transform.position;

            // The step size is equal to speed times frame time.
            float singleStep = speedToMove * Time.fixedDeltaTime;

            // Rotate the forward vector towards the target direction by one step
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

            // Calculate a rotation a step closer to the target and applies rotation to this object
            transform.rotation = Quaternion.LookRotation(newDirection);

            animcontroller.SetTrigger("Walk");
            animcontroller.SetFloat("SpeedMult",(float)(DifficultyLevel.timeToMove / 2));
        }

        //when gets close
        if (Vector3.Distance(transform.position, destination.position) < 0.2f)
        {
            startMove = false;
           // Debug.Log("reached destination " + this.gameObject.name);
            if (endOfJourney)
                Destroy(gameObject);
        }

        if(!startMove)
        {
            animcontroller.SetTrigger("Idlee");
        }
    }
}
