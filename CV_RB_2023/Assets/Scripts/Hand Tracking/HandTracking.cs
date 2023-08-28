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
        if (data == "None" || data.Length<2) { return; }



        data = data.Remove(0, 1);
        data = data.Remove(data.Length-1, 1);
    
        string[] points = data.Split(',');
        Debug.Log(points);
        print($"points, {points.Length}, {points[points.Length-1]}");


        for (int i = 0; i < points.Length - 1; i+=3) 
        {
            float x = 3 - float.Parse(points[i])/100;
            float y = float.Parse(points[i + 1])/100;
            float z = float.Parse(points[i + 2])/100;

            handPoints[i/3].transform.localPosition = new Vector3(x, y, z);
        }

        bool img_has_thumb_up = false;
        if(int.Parse(points[points.Length - 1])==1){
            img_has_thumb_up = true;
        }

        Debug.Log("img_has_thumb_up: "+img_has_thumb_up);

        // thumbs up or not is detected...
    }
}
