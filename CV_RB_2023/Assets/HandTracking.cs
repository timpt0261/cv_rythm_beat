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
        
        data = data.Remove(0, 1);
        data = data.Remove(data.Length-1, 1);
        if (data == "None") { return;} 
    
        string[] points = data.Split(',');
        int num_points = 21;
        if (points.Length != 64) { num_points = 42; }
        
        for (int i = 0; i < num_points; i++) 
        {
            float x = 5 -  float.Parse(points[i * 3])/100;
            float y = float.Parse(points[i * 3 + 1])/100;
            float z = float.Parse(points[i * 3 + 2])/100;

            handPoints[i].transform.localPosition = new Vector3(x, y, z);
        }


    }
}
