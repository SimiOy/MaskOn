using TMPro;
using UnityEngine;

public class IgnoreThisMoney : MonoBehaviour
{
    TextMeshProUGUI _moneyText;
    void Update()
    {
        _moneyText = GetComponent<TextMeshProUGUI>();
        _moneyText.text = "$"+GameMaster.instance.money.ToString();
    }

}
