using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveSkins : MonoBehaviour
{
    private Vector3 initialPos;
    private Vector3 finalPos;

    private void Awake()
    {
        initialPos = new Vector3(0f,58.7f,0f);
        finalPos = new Vector3(8f, 58.7f, 0f);
        //we get these in corellation to where the shop skinDisplayObject is
    }

    private void Update()
    {
        if(finalPos != null)
        {
            transform.position = Vector3.Lerp(transform.position, finalPos, 0.5f);

            if(Mathf.Abs(Vector3.Distance(finalPos,transform.position )) <= 0.1f)
            {
                transform.position = finalPos;
            }
        }
    }

    private void OnDisable()
    {
        transform.position = initialPos;
    }
}

