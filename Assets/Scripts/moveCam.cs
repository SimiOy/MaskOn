using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveCam : MonoBehaviour
{
    public GameObject[] buildingsArray;
    public int[] ChangeRotation;
    public Vector3 shopCoordinates;
    public static bool startLerp;
    public static int currIndex=0;

    float t = 0f;
    [SerializeField] [Range(0f,4f)] float lerpTime;

    private static Vector3 currentLerpCoord;

    private bool isShopOpened;

    private int old_index;

    private void Start()
    {
        startLerp = false;
        isShopOpened = false;

        currIndex = 0;
        currentLerpCoord = buildingsArray[currIndex].transform.position;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, ChangeRotation[currIndex], transform.eulerAngles.z);
    }

    private void Update()
    {
        if(startLerp)
        {
            transform.position = Vector3.Lerp(transform.position, currentLerpCoord, lerpTime * Time.deltaTime);

            transform.eulerAngles =new Vector3(transform.eulerAngles.x, Mathf.LerpAngle(transform.eulerAngles.y, ChangeRotation[currIndex], lerpTime * Time.deltaTime),transform.eulerAngles.z);

            t = Mathf.Lerp(t, 1f, lerpTime * Time.deltaTime);
            if(t>.99f)
            {
                t = 0f;
                transform.position = currentLerpCoord;
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, ChangeRotation[currIndex], transform.eulerAngles.z);

                startLerp = false;
            }
        }
        if(isShopOpened == false)
        {
            currentLerpCoord = buildingsArray[currIndex].transform.position;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, ChangeRotation[currIndex], transform.eulerAngles.z);
        }
    }

    public void GoToShop()
    {
        isShopOpened = true;
        gameObject.transform.GetComponentInChildren<Animator>().SetBool("ShopCam", isShopOpened);
        currentLerpCoord = shopCoordinates;
        old_index = currIndex;
        currIndex = 0;
        startLerp = true;
    }

    public void ReturnFromShop()
    {
        isShopOpened = false;
        gameObject.transform.GetComponentInChildren<Animator>().SetBool("ShopCam", isShopOpened);
        currentLerpCoord = buildingsArray[currIndex].transform.position;
        currIndex = old_index;
        startLerp = true;
    }

    public static void ChangeIndex(int _index)
    {
        currIndex += _index;
    }
}
