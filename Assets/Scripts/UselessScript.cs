using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UselessScript : MonoBehaviour
{
    public GameObject emergencyUI;
    private int i = 0;
    private void OnEnable()
    {
        i = 0;
        InvokeRepeating("RepeatAnim", 0f, 0.20f);
    }

    void RepeatAnim()
    {
        if(i >= 3)
        {
            this.gameObject.SetActive(false);
        }
        emergencyUI.GetComponent<Animator>().SetTrigger("Highlighted");
        i++;
    }
}
