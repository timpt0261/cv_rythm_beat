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
            Debug.Log("Player is Colliding with Detector " + detector_ID);
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
            float minutes = Mathf.FloorToInt(timeHolding / 60);
            float seconds = Mathf.FloorToInt(timeHolding % 60);
            Debug.Log("Player holded on Detector:"+ detector_ID +  " for " + minutes + " : " + seconds);

        }
    }
}
