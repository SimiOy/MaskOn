using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public GameObject loadingBar;
    public TextMeshProUGUI infoText;
    public Slider pbar;
    private float timeElapsed = 0f;
    private float waitForTime = 2.8f;
    private bool loadingDone;
    private void Awake()
    {

        //LoadGame();
        loadingDone = false;
        loadingBar.gameObject.SetActive(true);
    }

    private void Update()
    {
        if(loadingDone == false)
        {

            if (timeElapsed <= waitForTime)
            {
                float x = (float)(timeElapsed / waitForTime);
                pbar.value = x;

                if(x>0 && x<0.2f)
                {
                    infoText.text = "Getting saved info...";
                }
                else
                    if (x > 0.2f && x < 0.4f)
                {
                    infoText.text = "Configuring data...";
                }
                else
                    if (x > 0.4f && x < 0.6f)
                {
                    infoText.text = "Getting maps...";
                }
                else
                    if (x > 0.6f && x < 0.8f)
                {
                    infoText.text = "Getting skins...";
                }
                else
                    if(x>0.8f && x < 1f)
                {
                    infoText.text = "Resolving dependencies...";
                }
            }
            else
            {
                 loadingDone = true;
                 StartCoroutine(LoadLevel(1)); //<-to main menu
            }

            timeElapsed += Time.deltaTime;
        }
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        yield return new WaitForSeconds(0.3f);

        SceneManager.LoadScene(levelIndex);
        //loadingBar.gameObject.SetActive(false);
    }

    /*
    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

    public void LoadGame()
    {
        loadingBar.gameObject.SetActive(true);
        //just the first scene
        scenesLoading.Add(SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive));
        for (int i = 1; i < x; i++)
        {
            scenesLoading.Add(SceneManager.LoadSceneAsync(i, LoadSceneMode.Additive));
        }
        StartCoroutine(GetSceneLoadProgress());
    }

    float totalSceneProgress;
    public IEnumerator GetSceneLoadProgress()
    {
        for (int i = 0; i < scenesLoading.Count; i++)
        {
            while(!scenesLoading[i].isDone)
            {
                totalSceneProgress = 0;
                foreach(AsyncOperation operation in scenesLoading)
                {
                    totalSceneProgress += operation.progress;
                }

                totalSceneProgress = (totalSceneProgress / scenesLoading.Count) * 100f;

                pbar.value = Mathf.RoundToInt(totalSceneProgress);

                Debug.Log(Mathf.RoundToInt(totalSceneProgress));

                yield return null;
            }
        }

        yield return new WaitForSeconds(2f);

        loadingBar.gameObject.SetActive(false);
    }
    */
}
