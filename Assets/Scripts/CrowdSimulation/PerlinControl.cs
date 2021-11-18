using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PerlinControl : MonoBehaviour
{
    public int width = 256;
    public int height = 256;
    public int stepsForNoOfMagnets = 1;
    public float yScaleOffset;
    public float xScaleOffset;

    public float scale = 20f;
    public float perlinOffsetX = 100f;
    public float perlinOffsetY = 100f;
    public RawImage img;

    [Range(0f,1f)]
    public float magnetPowerNeeded;
    [Range(0f,4f)]
    public float magnetIntensity;

    MagnetObjects[] magnets;

    List<GameObject> peoplePrefabs = new List<GameObject>();

    [Range(1,100)]
    public int totalHumans;
    public int interactionsPerHuman;

    public GameObject crowdParent; //not using it, errors in local to world space conversions
    private GameObject[] spawnedHumans;

    private bool startShiftingPerlin;
    private bool startCheckingRoundEnded;
    public bool startSelectingInfected;
    public float timeToSelectInfected = 10f;
    private float timeElapsedSinceSelection;
    private float timeElapsed;
    public float shiftTime = 2f;
    [Range(0f,0.4f)]
    public float shiftVolume;

    private float countingDowntoStart;
    private bool startedCountingDown;
    private float elapsedForstart;

    public Material infectedMat;

    [Range(0f,3f)]
    public float acceptenceRadius;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI timerToClick;
    public TextMeshProUGUI descriptiveText;

    public TextMeshProUGUI totalHumansTExt;
    public CGMenu menuVAr;
    public GameObject uiAnimatedPIc;


    private void Start()
    {
        img = img.GetComponent<RawImage>();
        img.material.mainTexture = GenerateTexture();

        spawnedHumans = new GameObject[totalHumans];
        startShiftingPerlin = false;
        startCheckingRoundEnded = false;

        Time.timeScale = 0f;

        TotalHumansSetter(30);

        GetHumansArray();
        EnablingStuff(false);
        //in the beggining
       // StartCoroutine(StartingBusiness());
    }

    void Update()
    {
        img.texture = GenerateTexture();

        FindNearbyHumans();

        if(startShiftingPerlin)
        {
            if(timeElapsed <= shiftTime)
            {
                 StartCoroutine(ShiftingPerlin());
            }
            else
            {
                startCheckingRoundEnded = true;
                startShiftingPerlin = false;
            }

            timeElapsed += Time.deltaTime;
        }

        if(startCheckingRoundEnded)
        {
            CheckRoundEnded();
        }

        if(startSelectingInfected)
        {
            if(timeElapsedSinceSelection <= timeToSelectInfected)
            {
                //i m slecting hell yeah
                timerToClick.text = (timeToSelectInfected - timeElapsedSinceSelection).ToString("F2");
            }
            else
            {
                EnablingStuff(false);
                StartCoroutine(RestartBusiness());
                startSelectingInfected = false;
            }

            timeElapsedSinceSelection += Time.deltaTime;
        }

        if(startedCountingDown)
        {
            if(elapsedForstart<=countingDowntoStart)
            {
                if((countingDowntoStart - elapsedForstart) > 1)
                {
                    timerText.text = (countingDowntoStart - elapsedForstart).ToString("F0");    
                }
                else
                {
                    timerText.text = "GO";
                }
            }
            else
            {
                timerText.gameObject.SetActive(false);
                startedCountingDown = false;
            }
            elapsedForstart += Time.deltaTime;
        }
    }

    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);

        magnets = new MagnetObjects[width * height];

        for (int i=0, x = 0; x < width; x+=stepsForNoOfMagnets)
        {
            for (int y = 0; y < height; y+=stepsForNoOfMagnets)
            {
                Color color = CalculateColor(x, y, i);
                texture.SetPixel(x, y, color);

                magnets[i].positions = new Vector3(x + xScaleOffset, -0.6f, y + yScaleOffset);

                i++;
            }
        }

        texture.Apply();
        return texture;
    }

    Color CalculateColor(int x, int y, int magnetsIndices)
    {
        float xCord = (float)x / width * scale + perlinOffsetX;
        float yCord = (float)y / height * scale + perlinOffsetY;

        float sample = Mathf.PerlinNoise(xCord, yCord);
        magnets[magnetsIndices].perlinIntensity = sample;

        return new Color(sample, sample, sample);
    }

    public void RegeneratePerlinCoordinates()
    {
        perlinOffsetX = Random.Range(0f, 300f);
        perlinOffsetY = Random.Range(0f, 300f);

        SpawnHumans();

        FindNearbyHumans();
    }

    public void BeginMovement()
    {

        perlinOffsetX = Random.Range(0f, 300f);
        perlinOffsetY = Random.Range(0f, 300f);
        scale = Random.Range(2f, 6f);

        startShiftingPerlin = true;
        timeElapsed = 0f;
        //Needs fixing later

        foreach (GameObject go in spawnedHumans)
        {
            if (go != null)
            go.GetComponent<Crowdmovement>().StartMovement(acceptenceRadius);
        }
    }

    IEnumerator ShiftingPerlin()
    {
        perlinOffsetX += Random.Range(0, shiftVolume);
        perlinOffsetY += Random.Range(0, shiftVolume);

        yield return new WaitForSeconds(0.8f);
    }

    IEnumerator StartingBusiness()
    {
        yield return new WaitForSeconds(1.4f);
        for (int i = 0; i < totalHumans; i++)
        {
            if(i!=(totalHumans-1))
            if (spawnedHumans[i] != null)
            {
                spawnedHumans[i].GetComponent<InfectedVisualiser>().ApplyInfectrion(infectedMat,false);
                spawnedHumans[i].GetComponent<Crowdmovement>().ClearAll();
            }
            if(i == totalHumans -1)
            {
                //last one
                spawnedHumans[i].GetComponent<InfectedVisualiser>().ApplyInfectrion(infectedMat,true); //override and make him infected
                spawnedHumans[i].GetComponent<Crowdmovement>().ClearAll();
            }
        }
        countingDowntoStart = 5f;
        elapsedForstart = 0f;
        timerText.gameObject.SetActive(true);
        timerText.GetComponent<Animator>().SetTrigger("Jump");
        startedCountingDown = true;

        //tutorial maybe
        menuVAr.ActivateCardTutorial(0);

        yield return new WaitForSeconds(5f);
        //restart crowd
        BeginMovement();
        startSelectingInfected = false;
    }

    IEnumerator RestartBusiness()
    {
        Highlighht();
        yield return new WaitForSeconds(0.8f);
        //restart crowd
        BeginMovement();
    }

    void FindNearbyHumans()
    {
        if (magnets == null)
            return;
        for (int i = 0; i < magnets.Length; i++)
        {
            if (magnets[i].perlinIntensity >= magnetPowerNeeded)
            {
                CheckInSphere(magnets[i].positions, magnets[i].perlinIntensity * magnetIntensity);
            }
        }
    }

    void CheckInSphere(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);

        if (hitColliders == null)
            return;

        foreach (Collider human in hitColliders)
        {
            if(human.CompareTag("human"))
            {
               // Debug.Log("found" + human.name + center);
               human.gameObject.GetComponent<Crowdmovement>().SetDestinations(center,interactionsPerHuman);
            }
        }
    }

    void GetHumansArray()
    {
        peoplePrefabs = new List<GameObject>();

        for (int i = 0; i < CrowdSpawne.instance.humans.Count; i++)
        {
            peoplePrefabs.Add(CrowdSpawne.instance.humans[i]);

            if(peoplePrefabs[i].GetComponent<Crowdmovement>() == null)
            {
                peoplePrefabs[i].AddComponent<Crowdmovement>();
                peoplePrefabs[i].GetComponent<Crowdmovement>().enabled = true;
            }
            else
            {
                peoplePrefabs[i].GetComponent<Crowdmovement>().enabled = true;
            }

            peoplePrefabs[i].gameObject.SetActive(true);
            peoplePrefabs[i].gameObject.GetComponent<Crowdmovement>().ClearAll();
        }

        SpawnHumans();
    }

    void SpawnHumans()
    {
        DestroyOldHumans();

        for (int i = 0; i < totalHumans; i++)
        {
            int x = (int)Random.Range(0, peoplePrefabs.Count);
            Vector3 where = new Vector3((float)Random.Range(0, width) + xScaleOffset, -0.6f, (float)Random.Range(0, height) + yScaleOffset);
            GameObject go = (GameObject) Instantiate(peoplePrefabs[x], where, Quaternion.identity);
            spawnedHumans[i] = go;
            go.GetComponent<Crowdmovement>().ClearAll();
        }
    }

    void DestroyOldHumans()
    {
        if (spawnedHumans==null)
            return;

        for (int i = 0; i < spawnedHumans.Length; i++)
        {
            Destroy(spawnedHumans[i]);
        }
        spawnedHumans = new GameObject[totalHumans];

    }

    public void CheckRoundEnded()
    {
        for (int i = 0; i < totalHumans; i++)
        {
            if(spawnedHumans[i] != null) //fkin slow FIX PLS
            if(spawnedHumans[i].GetComponent<Crowdmovement>().startMove == true)
            {
                //still moving
                return;
            }
        }
        //nobody moving
        Debug.Log("round ended");

        startCheckingRoundEnded = false; //stop looking in update

        //start selecting process

        menuVAr.ActivateCardTutorial(1);

        SelectionTime();

    }

    public void Highlighht()
    {
        for (int i = 0; i < totalHumans; i++)
        {
            if (spawnedHumans[i] != null)
            {
                spawnedHumans[i].GetComponent<InfectedVisualiser>().ApplyInfectrion(infectedMat,false);
                spawnedHumans[i].GetComponent<Crowdmovement>().ClearAll();
            }
        }
    }

    void SelectionTime()
    {
        CrowdSpawne.instance.noTaps = CrowdSpawne.instance.humansInfected;
        EnablingStuff(true);
        timeElapsedSinceSelection = 0f;
        descriptiveText.text = "Select the infected ones and quaranteen them to stop the spread!";
        startSelectingInfected = true;
    }

    public void EnablingStuff(bool tDo)
    {
        timerToClick.gameObject.SetActive(tDo);
        descriptiveText.gameObject.SetActive(tDo);
    }

    public void ResetButton()
    {
        //called by ui button

        Time.timeScale = 1f;
        startSelectingInfected = false;
        startShiftingPerlin = false;
        startCheckingRoundEnded = false;

        RegeneratePerlinCoordinates();

        CrowdSpawne.instance.ResetAll();

        StartCoroutine(StartingBusiness());
    }

    public void TotalHumansSetter(float n) //in ui
    {
        menuVAr.humansFormula = n;
        totalHumans = (int)n;
        totalHumansTExt.text = totalHumans.ToString();
    }

    private void OnDrawGizmos()
    {
        if (magnets == null)
            return;
        for (int i = 0; i < magnets.Length; i++)
        {
            if(magnets[i].perlinIntensity >= magnetPowerNeeded)
            Gizmos.DrawSphere(magnets[i].positions, magnetIntensity * magnets[i].perlinIntensity);
        }
    }

    public void TutStuffNottoAssignOtherRepresentatives(int i)
    {
        menuVAr.ActivateCardTutorial(i);
        if(i == 3)
        {
            //for them taps lol
            uiAnimatedPIc.GetComponent<Animator>().SetTrigger("Notify");
        }
    }
}

struct MagnetObjects
{
    public Vector3 positions;
    public float perlinIntensity;
}
