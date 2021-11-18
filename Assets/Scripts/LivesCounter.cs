using UnityEngine;
using UnityEngine.UI;

public class LivesCounter : MonoBehaviour
{
    public static int livesLeft;
    public static bool loseLife;

    public Color badUI;
    Color originalColor;
    public Image[] livesUI;

    public PauseMenu deathUI;

    private void Start()
    {
        loseLife = false;
        livesLeft = GameMaster.instance.curLives;
        originalColor = livesUI[0].color;
        CheckLives();
    }

    private void Update()
    {
        if (livesLeft <= 0)
        {
            if (PauseMenu.hadAd == false)
                deathUI.RetryAdsMenu();
            else
                deathUI.DeathMenu();
        }

        if(loseLife)
        {
            livesLeft--;
            //ui
            livesUI[livesLeft].color = badUI;

            GameObject.FindObjectOfType<AudioManager>().Play("Wrong");

            loseLife = false;
        }

        CheckLives();
    }

    void CheckLives()
    {
        switch(livesLeft)
        {
            case 3:
                    livesUI[0].color = originalColor;
                    livesUI[1].color = originalColor;
                    livesUI[2].color = originalColor;
                break;
            case 2:
                livesUI[0].color = originalColor;
                livesUI[1].color = originalColor;
                livesUI[2].color = badUI;
                break;
            case 1:
                livesUI[0].color = originalColor;
                livesUI[1].color = badUI;
                livesUI[2].color = badUI;
                break;
            case 0:
                livesUI[0].color = badUI;
                livesUI[1].color = badUI;
                livesUI[2].color = badUI;
                break;
             default:
                Debug.Log("lives counter not matching.");
                break;
        }
    }
}
