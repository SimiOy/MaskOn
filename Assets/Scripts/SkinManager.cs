using System;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using System.Collections;
using UnityEditor;

public class SkinManager : MonoBehaviour
{
    public SkinClass[] skins;

    public static SkinManager instance;

    public List<GameObject> boughtSkins = new List<GameObject>();

    int[] availableSkins;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        GetListOfSkins();
        SortSkins();
        GenerateBought();
    }

    public string SkinName(int id)
    {
        return skins[id]._name;
    }

    public int SkinCost(int id)
    {
        return skins[id]._cost;
    }

    public int SkinAvailable(int id)
    {
        return skins[id]._available;
    }

    public void GenerateBought()
    {
        int y = skins.Length;
        for (int i = 0; i < y; i++)
        {
            if(skins[i]._available == 1)
            {
                //create bought skin collection
                boughtSkins.Add(skins[i]._prefab);
                //Debug.Log(skins[i]._prefab.name);
            }
        }
    }

    public void BuySkin(int id)
    {
        if (GameMaster.instance.money >= skins[id]._cost)
        {
            skins[id]._available = 1;
            availableSkins[skins[id]._id] = 1;

            Debug.Log("bought " + skins[id]._id);
            GameObject.FindObjectOfType<AudioManager>().Play("Buy");
            //ui
            GameMaster.instance.SubtractMoney(skins[id]._cost);
        }
        GenerateBought();

        //DebuggerSkins();
    }

    public void SortSkins()
    {
        Array.Sort(skins,delegate (SkinClass x, SkinClass y) 
        { 
            return y._available.CompareTo(x._available); 
        });
        Debug.Log("sorted the skins");
        //sort by bought and not bought
    }


    void GetListOfSkins()
    {
        int skinsCount = skins.Length;
        availableSkins = new int[skinsCount];
        for (int i = 0; i < skinsCount; i++)
        {
            int x = skins[i]._id;
            availableSkins = SaveManager.instance.state.allSkins;
            skins[i]._available = availableSkins[x];

            //debugging
            if (skins[i]._cost == 0) //should be available from start
            {
                skins[i]._available = 1;
                availableSkins[x] = 1;
            }
           // Debug.Log(skins[x]._name + " is " + skins[x]._available);
        } 
    }

    //I DONT UNDERSTAND HOW IT'S WORKING BUT IT CERTAINLY IS DON'T CHANGE ANYTHING IF YOU VALUE YOUR LIFE

    public void CheckSkins()
    {
        SaveSkins();
    }
    private void OnDisable()
    {
        CheckSkins();
    }

    public void SaveSkins()
    {
        SaveManager.instance.InputSkinsArray(availableSkins);
    }

    void DebuggerSkins()
    {
        int x = availableSkins.Length;
        for (int i = 0; i < x; i++)
        {
            Debug.Log(availableSkins[i]);
        }
    }
}
