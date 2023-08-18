using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    double timeInstantiated;
    public float assignedTime;

    // Start is called before the first frame update
    void Start()
    {
        timeInstantiated = Song_Manager.GetAudioSourceTime();
        
    }

    // Update is called once per frame
    void Update()
    {
        double timesinceInstantiated = Song_Manager.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timesinceInstantiated / (Song_Manager.Instance.noteTime * 2));

        if (t > 1)
        {
            Destroy(gameObject);
        }
        else 
        {
            transform.localPosition = Vector3.Lerp(Vector3.forward * Song_Manager.Instance.noteSpawnZ, Vector3.forward * Song_Manager.Instance.noteDespawnZ, t);
        }
    }
}
