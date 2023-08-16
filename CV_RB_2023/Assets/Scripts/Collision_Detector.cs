using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision_Detector : MonoBehaviour
{
    public bool active = false;
    [SerializeField] 
    private int detector_ID = 1;
    
    private Material material;
    private float timeHolding = 0;

    private void Start()
    {
        active = false;
        material = GetComponent<Material>();
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Player") && active == false) 
        {
            Debug.Log("Player is Colliding with Detector");
            active = true;
            timeHolding = 0;

        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && active == true)
        {
            timeHolding += Time.deltaTime;

        }

    }

    private void OnCollisionExit (Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && active == true)
        {
            active = false;
            Debug.Log("Player holded on Deteector:"+ detector_ID +  " for " + timeHolding);

        }
    }
}
