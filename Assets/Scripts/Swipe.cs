using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour
{
    private bool tap, swipeLeft, swipeRight, swipeUp, swipeDown;
    private bool isSelected;
    private bool isDragging = false;
    private Vector2 startTouch, swipeDelta;

    HelperSwipe nextTarget;
    private void Start()
    {
        nextTarget = GetComponent<HelperSwipe>();
    }

    private void Update()
    {
        tap = swipeLeft = swipeRight = swipeDown = swipeUp = false;

        //normal mouse stuff
        if(Input.GetMouseButtonDown(0))
        {
            tap = true;
            isDragging = true;
            startTouch = Input.mousePosition;
        }
        else
            if(Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            Reset();
        }

        //mobile stuff
        if(Input.touches.Length > 0)
        {
            if(Input.touches[0].phase == TouchPhase.Began)
            {
                tap = true;
                isDragging = true;
                startTouch = Input.touches[0].position;
            }
            else
                if(Input.touches[0].phase ==TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                isDragging = false;
                Reset();
            }
        }

        //pentru doar primu tont (pc)
        if (Input.GetMouseButtonDown(0))
        {

            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                if (hitInfo.transform.gameObject.CompareTag("Current"))
                {
                    if(hitInfo.transform.gameObject.name=="0")
                    {
                        isSelected = true;
                        nextTarget.objectToMove = hitInfo.transform.gameObject.transform;
                        hitInfo.transform.gameObject.name = "cacaexilat";
                    }
                }
                else
                {
                    Debug.Log("nopz");
                }
            }
            else
            {
                Debug.Log("No hit");
            }
        }

        //tap for mobile selctiveness
        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hitInfo;
            if (Physics.Raycast(raycast, out hitInfo))
            {
                if (hitInfo.transform.gameObject.CompareTag("Current"))
                {
                    if (hitInfo.transform.gameObject.name == "0")
                    {
                        isSelected = true;
                        nextTarget.objectToMove = hitInfo.transform.gameObject.transform;
                        hitInfo.transform.gameObject.name = "cacaexilat";
                    }
                }
                else
                {
                    Debug.Log("nopz");
                }
            }
            else
            {
                Debug.Log("No hit");
            }
        }


        //delta mafs for where is the fukin drag
        swipeDelta = Vector2.zero;
        if(isDragging)
        {
            if(Input.touches.Length > 0)
            {
                swipeDelta = Input.touches[0].position - startTouch;
            }
            else
                if(Input.GetMouseButton(0))
            {
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
            }
        }

        //sunt peste zona moarta; activez?
        if(swipeDelta.magnitude > 100) //in pixeli il fac variabila
        {
            //unde?
            float x = swipeDelta.x;
            float y = swipeDelta.y;

            if(Mathf.Abs(x) > Mathf.Abs(y))
            {
                //stg sau drpt
                if (x < 0)
                {
                    swipeLeft = true;
                }
                else
                {
                    swipeRight = true;
                }
            }
            else
            {
                //sus sau jos
                if(y<0)
                {
                    swipeDown = true;
                }
                else
                {
                    swipeUp = true;
                }
            }

            Reset();
        }
    }

    private void Reset()
    {
        startTouch = swipeDelta = Vector2.zero;
        isDragging = false;
        isSelected = false;
    }


    public Vector2 SwipeDelta { get { return swipeDelta; } }
    public bool SwipeLeft { get { return swipeLeft; } }
    public bool SwipeRight { get { return swipeRight; } }
    public bool SwipeUp { get { return swipeUp; } }
    public bool SwipeDown { get { return swipeDown; } }

    public bool Tap { get { return tap; } }

    public bool Selected { get { return isSelected; } }
}
