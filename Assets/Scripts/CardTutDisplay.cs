using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardTutDisplay : MonoBehaviour
{
    public CardTutObject[] _card;
    public TextMeshProUGUI _text;
    private float lastTime;
    int cardIndex;

    private void OnEnable()
    {
        GetData();
        CradSetter(cardIndex);
    }

    public void CradSetter(int index)
    {
        Debug.Log(_card[index].used);
        if (_card[index].used == false)
        {
            _text.text = _card[index].text;
            lastTime = _card[index].lastForHowLong;
            _card[index].used = true;
            SaveData();
            StartCoroutine(WaitTillDestruction(lastTime));
        }
        else
            if(_card[cardIndex].used)
        {
            SaveData();
            this.gameObject.SetActive(false);
        }
    }

    public void IndexSetter(int ind)
    {
        cardIndex = ind;
    }

    void GetData()
    {
        if (PlayerPrefs.GetInt(_card[cardIndex].name) == 0)
            _card[cardIndex].used = false;
        else
        {
            Debug.Log("aiciii");
            _card[cardIndex].used = true;
        }
    }

    void SaveData()
    {
        int x = 0;
        if (_card[cardIndex].used == false)
            x = 0;
        else
            x = 1;

        PlayerPrefs.SetInt(_card[cardIndex].name, x);
    }

    IEnumerator WaitTillDestruction(float time)
    {
        //pause and unpause time during collection of tutroials

        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(time);
        this.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
