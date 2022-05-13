using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondPickup : MonoBehaviour
{
    public int diamondVal;

    public GameObject pickUp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            FindObjectOfType<GameManager>().addDiamond(diamondVal);

            Instantiate(pickUp, transform.position, transform.rotation);

            Destroy(gameObject);
        }
    }
}
