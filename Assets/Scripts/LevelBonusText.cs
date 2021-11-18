using TMPro;
using UnityEngine;

public class LevelBonusText : MonoBehaviour
{
    TextMeshProUGUI _text;
    private void OnEnable()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _text.text = "X" + GameMaster.instance.multiplicationfactorOfMoneyToMakeYouFeelAwesome.ToString();
    }
}
