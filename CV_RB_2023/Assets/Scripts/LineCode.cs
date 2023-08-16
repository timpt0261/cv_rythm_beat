using UnityEngine;

public class LineCode : MonoBehaviour
{
    public Transform origin;
    public Transform destination;
    LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }

    void Update()
    {
        if (origin && destination)
        {
            lineRenderer.SetPosition(0, origin.position);
            lineRenderer.SetPosition(1, destination.position);
        }
    }
}
