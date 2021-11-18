using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Spawning : MonoBehaviour
{
    private GameObject personInLine;
    List<GameObject> peoplePrefabs = new List<GameObject>();
    public GameObject spawnGameObject; //ca sa pot baga foruri in el lol
    public GameObject[] gbArray = new GameObject[5];
    Transform[] spArray = new Transform[5];
    public int[] availableArray = new int[5];

    public bool startMethod;
    int i = 0;

    private bool newTemp;

    public Temperature tAux;
    public float temperatura;

    public Timer timeRelatedIssues;

    private int stopAskingMe; //for tut
    private bool thisObj;

    private void Awake()
    {
        stopAskingMe = GameMaster.instance.tutorialDone;
        if (gameObject.name == "SpawnArea")
            thisObj = true;
        else
            thisObj = false;
    }
    private void Start()
    {
        peoplePrefabs = new List<GameObject>();
        int xv = SkinManager.instance.boughtSkins.Count;
        for (int i = 0; i < xv; i++)
        {
            Debug.Log("im here "+ i);
            peoplePrefabs.Add(SkinManager.instance.boughtSkins[i]);
            peoplePrefabs[i].GetComponent<MoveScript>().enabled = true;
            peoplePrefabs[i].GetComponent<moveSkins>().enabled = false;
            if(peoplePrefabs[i].GetComponent<Crowdmovement>() !=null)
            peoplePrefabs[i].GetComponent<Crowdmovement>().enabled = false;

            peoplePrefabs[i].gameObject.SetActive(true);
        }

        newTemp = true;

        foreach(Transform child in transform)
        {
            personInLine = peoplePrefabs[(int)(Random.Range(0, peoplePrefabs.Count))];
            GameObject go = (GameObject)Instantiate(personInLine, child.transform.position,Quaternion.identity,spawnGameObject.transform);
            go.name = i.ToString();
            go.GetComponent<MoveScript>().destination = child.transform;
            spArray[i] = child;
            gbArray[i] = go;
            availableArray[i] = 0;
            i++;
        }
    }

    private void Update()
    {
        personInLine = peoplePrefabs[(int)(Random.Range(0, peoplePrefabs.Count))];

        if (startMethod)
        {
            availableArray[0] = 1;
            startMethod = false;
        }

        for (int i = 0; i < availableArray.Length-1; i++)
        {
            if(availableArray[i]==1)
            {
                moveSensei(i + 1);
                availableArray[i] = 0;
            }
        }

        
        if(availableArray[availableArray.Length-1]==1 && availableArray.Length<=5) 
        {
            GameObject go = (GameObject)Instantiate(personInLine, spArray[spArray.Length-1].transform.position, Quaternion.identity, spawnGameObject.transform);
            availableArray[availableArray.Length - 1] = 0;
            gbArray[gbArray.Length - 1] = go;
            go.name = (gbArray.Length - 1).ToString();
            go.GetComponent<MoveScript>().destination = spArray[spArray.Length - 1].transform;

            gbArray[0].name = (int.Parse(gbArray[0].name) - 1).ToString();

            newTemp = true;
        }

        //updating the game

        if(gbArray[0].name =="0" && newTemp)
        {
            newTemp = false;
            startTimer();
        }
        
    }

    void moveSensei(int k)
    {
        availableArray[k] = 1;
        GameObject aux = gbArray[k];
        aux.GetComponent<MoveScript>().startMove = true;
        aux.GetComponent<MoveScript>().destination = spArray[k - 1];
        if(k>=2)
        gbArray[k].name = (int.Parse(gbArray[k].name) - 1).ToString();
        gbArray[k - 1] = gbArray[k];
    }

    private int k = 0;
    private float t;
    void startTimer()
    {
        temperatura = Random.Range(35f, 40f);
        temperatura = Mathf.Round(temperatura * 10f) / 10f;
        tAux.temp = temperatura;
        //in helpmeimstupid fac comparatia

        if (thisObj)
        {
            t = temperatura;

            if (stopAskingMe == 0)
            {
                if (GameMaster.instance.tutorialDone == 0)
                {
                    //always have to check cuz it changes in my tutroial scirpt

                    //no tutorial done
                    if (t <= 37.3f)
                    {
                        //right
                        k += TutorialScript.instance.TutRight();
                    }
                    else
                        if (t > 37.3f)
                    {
                        //stanga
                        k += TutorialScript.instance.TutLeft();
                    }
                }


                if (k == 2)
                {
                    //did both letft and right
                    stopAskingMe = 1;
                }
            }
        }

        //add timer
        timeRelatedIssues.StartUsage();
    }

}
