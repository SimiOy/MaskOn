using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static int scoreCounter;
    TextMeshProUGUI UiDisplay;

    [SerializeField]
    private bool highscore=false;

    void Start()
    {
        //scoreCounter = 0;
        //reset every time
        UiDisplay = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (!highscore) //not to make 2 scripts
            UiDisplay.text = scoreCounter.ToString();
        else
            if (highscore)
        {
                UiDisplay.text = GameMaster.instance.highScore.ToString();
        }

    }
}
