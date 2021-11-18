using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

public class DisplaySkins : MonoBehaviour
{
    public GameObject parentobj;
    public GameObject cam;

    public List<GameObject> skinsPrefabs = new List<GameObject>();

    public Vector3 onePlace;
    private int currentSkin;

    [SerializeField] private Button nextBtn;
    [SerializeField] private Button prevButton;

    [SerializeField] private TextMeshProUGUI _SkinName;
    [SerializeField] private TextMeshProUGUI _SkinCost;

    [SerializeField] private Button _buySkinBtn;

    void Start()
    {
        for (int i = 0; i < SkinManager.instance.skins.Length; i++)
        {
            skinsPrefabs.Add(SkinManager.instance.skins[i]._prefab);
            // so they dont start fukin moving around
            //already sorted in skinManager by bought or not
            skinsPrefabs[i].GetComponent<MoveScript>().enabled = false;
            skinsPrefabs[i].GetComponent<moveSkins>().enabled = true;
        }
        // InstantiateLinePrefabs();
        InstantiatePrefabsOneSpot();
        ChangeSkin(0);
    }

    //acces call functions name and cost in SkinManager non-destructable
    //thorugh here so i dont mess up

    void InstantiatePrefabsOneSpot()
    {
        int pieceCount = skinsPrefabs.Count;
        for (int i = 0; i < pieceCount; i++)
        {
            Instantiate(skinsPrefabs[i], onePlace, new Quaternion(0,180,0,0), parentobj.transform);
            skinsPrefabs[i].gameObject.SetActive(false);
        }
        skinsPrefabs[0].gameObject.SetActive(true);
    }

    void SelectSkin(int _index)
    {
        _SkinName.text = SkinManager.instance.SkinName(_index);
        _SkinCost.text = "$"+SkinManager.instance.SkinCost(_index).ToString();

        prevButton.interactable = (_index != 0);
        int pieceCount = parentobj.transform.childCount;
        nextBtn.interactable = (_index != (pieceCount-1));
        for (int i = 0; i < pieceCount; i++)
        {
            parentobj.transform.GetChild(i).gameObject.SetActive(i == _index);
            //parentobj.transform.GetChild(i).gameObject.transform.LookAt(cam.transform);
        }
    }

    public void ChangeSkin(int _change)
    {
        currentSkin += _change;
        SelectSkin(currentSkin);
       // Debug.Log(currentSkin);

        //check if can buy
        if(SkinManager.instance.SkinAvailable(currentSkin) == 0)
        {
            //if i Have money
            _buySkinBtn.gameObject.SetActive(true);

            if(SkinManager.instance.SkinCost(currentSkin) <= GameMaster.instance.money)
            {
                _buySkinBtn.interactable = true;
                //i can buy
            }
            else
            {
                _buySkinBtn.interactable = false;
                //i cant buy
            }
        }
        else
        {
            //already bought
            _buySkinBtn.gameObject.SetActive(false);
        }
    }

    public void BuySkin()
    {
        SkinManager.instance.BuySkin(currentSkin);
    }


    private void Update()
    {
        //check if can buy
        if (SkinManager.instance.SkinAvailable(currentSkin) == 0)
        {
            //if i Have money
            _buySkinBtn.gameObject.SetActive(true);

            if (SkinManager.instance.SkinCost(currentSkin) <= GameMaster.instance.money)
            {
                _buySkinBtn.interactable = true;
                //i can buy
            }
            else
            {
                _buySkinBtn.interactable = false;
                //i cant buy
            }
        }
        else
        {
            //already bought
            _buySkinBtn.gameObject.SetActive(false);
        }
    }
}
