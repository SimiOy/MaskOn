using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectedVisualiser : MonoBehaviour
{
    public bool isInfected;
    private bool willGetinfected;

    //use crowd spanwer as static references for ease of common values
    [Range(0f,1f)]
    float firstInfectedChance;
    [Range(0f, 0.5f)]
    float giveDeasesRate;

    private Material newMats;

    public PerlinControl _manager;

    [Range(0.2f,3f)]
    float timeSeen;

    private void Start()
    {
        _manager = (PerlinControl)FindObjectOfType(typeof(PerlinControl));
        firstInfectedChance = CrowdSpawne.instance.initialPopulationInfectionRate;
        giveDeasesRate = CrowdSpawne.instance.humanSpreadRate;

        willGetinfected = false;

        if (Random.Range(0f, 1f) <= firstInfectedChance)
        {
            isInfected = true;
            CrowdSpawne.instance.humansInfected++;
        }
        else
            isInfected = false;
    }

    void ShowInfected()
    {
        timeSeen = CrowdSpawne.instance.timeMatChange;

        if(isInfected)
        {
            StartCoroutine(DisplayInfected());
        }
    }

    IEnumerator DisplayInfected()
    {
        GameObject target = this.gameObject;
        foreach(Transform tr in target.gameObject.transform)
        {
            if(tr.gameObject.activeSelf)
            {
                if(tr.gameObject.GetComponent<SkinnedMeshRenderer>() != null)
                {
                    Material oldMat = tr.gameObject.GetComponent<SkinnedMeshRenderer>().material;
                    tr.gameObject.GetComponent<SkinnedMeshRenderer>().material = newMats;
                    yield return new WaitForSeconds(timeSeen);
                    tr.gameObject.GetComponent<SkinnedMeshRenderer>().material = oldMat;
                }
            }
        }
    }

     void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("human"))
        {
            if(isInfected)
            {
                //the giver
                if(Random.Range(0f,1f) <= giveDeasesRate)
                {
                    collision.gameObject.GetComponent<InfectedVisualiser>().willGetinfected = true;
                }
                else
                {
                    //do not infect
                }
            }
        }
    }

    public void ApplyInfectrion(Material newMat,bool ovrd)
    {
        newMats = newMat;
        //only if not yet infected

        if(ovrd)
        {
            //has to be ovewritten
            willGetinfected = true;
        }

        if(isInfected == false)
        {
            isInfected = willGetinfected;
            if(willGetinfected)
            {
                //increase counetrs
                CrowdSpawne.instance.humansInfected++;
            }
        }

        ShowInfected();
    }
    void OnMouseDown()
    {
        if(_manager.startSelectingInfected == true)
        {
            // particles
            //update counters
            int taps = CrowdSpawne.instance.noTaps;
            if(taps > 0)
            {
                CrowdSpawne.instance.noTaps--;
                if(isInfected)
                {
                    //had issues and nailed it fine
                    CrowdSpawne.instance.humansInfected--;
                    CrowdSpawne.instance.humansAlive--;

                    //tutorial right
                    _manager.TutStuffNottoAssignOtherRepresentatives(2);
                }
                else
                {
                    //wrong assumption
                    GameObject.FindObjectOfType<AudioManager>().Play("Wrong");
                    CrowdSpawne.instance.humansAlive--;

                }
                Destroy(gameObject);    
            }
            else
            {
                //taps lower
                _manager.TutStuffNottoAssignOtherRepresentatives(3);  
            }

            int infectedNo = CrowdSpawne.instance.humansInfected;
            if(infectedNo == 0 || (infectedNo > CrowdSpawne.instance.humansAlive))
            {
                Debug.Log("game finished");
                CrowdSpawne.instance.GameEnded();
            }
        }
    }

}
