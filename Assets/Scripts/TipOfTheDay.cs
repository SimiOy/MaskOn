using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TipOfTheDay : MonoBehaviour
{
    TextMeshProUGUI _text;

    public string[] _messages;
    private void OnEnable()
    {
        _text = GetComponent<TextMeshProUGUI>();
        StartCoroutine(WaitCorutine());
    }

    IEnumerator WaitCorutine()
    {
        string rand = _messages[Random.Range(0, _messages.Length)];
        _text.text = rand;
        yield return new WaitForSecondsRealtime(7f);
        _text.text = "";
    }
}
