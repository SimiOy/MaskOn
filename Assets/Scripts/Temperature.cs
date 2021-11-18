using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Temperature : MonoBehaviour
{
    public float temp;
    TextMeshProUGUI toDisplay;

    private void Start()
    {
        toDisplay = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        toDisplay.text = temp.ToString();
    }
}
