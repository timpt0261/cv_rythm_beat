using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTracking : MonoBehaviour
{
    public UDPReceive udpReceive;
    public GameObject[] handPoints;

    // Update is called once per frame
    void Update()
    {
        string data = udpReceive.data;
        if (data == "None") { return; }


        data = data.Remove(0, 1);
        data = data.Remove(data.Length-1, 1);
    
        string[] points = data.Split(',');

        for (int i = 0; i < 42; i++) 
        {
            float x = 3 - float.Parse(points[i * 3])/100;
            float y = float.Parse(points[i * 3 + 1])/100;
            float z = float.Parse(points[i * 3 + 2])/100;

            handPoints[i].transform.localPosition = new Vector3(x, y, z);
        }


    }
}
