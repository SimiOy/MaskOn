using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenesScores 
{
    public int _highestScore = 0;
    public int _isAvailable = 0;
    public int _priceToPay;
    public string _name;//scene index
    
    public ScenesScores(int hs, int availability, string nm, int price)
    {
        _highestScore = hs;
        _isAvailable = availability;
        _name = nm;
        _priceToPay = price;
    }

    public void BecomeAvailable()
    {
        if(GameMaster.instance.money >= _priceToPay)
        {
            _isAvailable = 1;
            Debug.Log(_name + " becama available");
            //ui
            GameMaster.instance.SubtractMoney(_priceToPay);
        }
    }

    public void NewHighscore(int hs)
    {
        Debug.Log(_name + " has a new highscore");
        _highestScore = hs;
    }
}
