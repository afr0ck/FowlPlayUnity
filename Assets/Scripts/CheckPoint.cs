using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    // Public variables
    public Renderer meshRend;

    public Material chkPntOff;
    public Material chkPntOn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void checkPointOn()
    {
        CheckPoint[] chkPnts = FindObjectsOfType<CheckPoint>();
        foreach(CheckPoint i in chkPnts)
        {
            i.checkPointOff();
        }

        meshRend.material = chkPntOn;
    }

    public void checkPointOff()
    {
        meshRend.material = chkPntOff;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            FindObjectOfType<HealthManager>().saveCheckPoint(transform.position);

            checkPointOn();
        }
    }
}
