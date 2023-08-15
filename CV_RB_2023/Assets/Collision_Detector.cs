using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision_Detector : MonoBehaviour
{
    [SerializeField]
    private int detector_num = 1;
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision);
    }
}
