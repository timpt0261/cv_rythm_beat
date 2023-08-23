using UnityEngine;

public class CalculateHandBounds : MonoBehaviour
{
    [SerializeField]
    private Transform collider_transform;
    [SerializeField]
    private Transform[] points;
    private BoxCollider boxCollider;

    [Range(0.1f, 2.0f)]
    [SerializeField]
    private float size_estimate = 0.1f;

    [Range(0.1f, 2.0f)]
    [SerializeField]
    private float center_estimate = 0.1f;

    // Start is called before the first frame update
    private void Start()
    {
        collider_transform = GetComponent<Transform>();
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    private void Update()
    {
        Transform[] bounds = GetBounds(points);

        Vector3 size = new Vector3(
            Mathf.Abs(bounds[3].position.x - bounds[2].position.x) * size_estimate,
            Mathf.Abs(bounds[0].position.y - bounds[1].position.y) * size_estimate,
            Mathf.Abs(bounds[5].position.z - bounds[4].position.z) * size_estimate
        );

        // Calculate the point closest to the middle
        Vector3 center = new Vector3(
            ((bounds[3].position.x + bounds[2].position.x) / 2) * center_estimate,
            ((bounds[0].position.y + bounds[1].position.y) / 2) * center_estimate,
            ((bounds[5].position.z + bounds[4].position.z) / 2) * center_estimate
            );
        
        // Apply the size and center adjustments
        boxCollider.size = size;
        boxCollider.center = center;

        collider_transform.position = points[9].position;
        collider_transform.rotation = points[9].rotation;
    }

    private Transform[] GetBounds(Transform[] points)
    {
        Transform topBound = points[0];
        Transform bottomBound = points[0];
        Transform leftBound = points[0];
        Transform rightBound = points[0];
        Transform depthPositive = points[0];
        Transform depthNegative = points[0];

        foreach (Transform point in points)
        {
            if (point.position.z > depthPositive.position.z)
                depthPositive = point;

            if (point.position.z < depthNegative.position.z)
                depthNegative = point;

            if (point.position.y > topBound.position.y)
                topBound = point;

            if (point.position.y < bottomBound.position.y)
                bottomBound = point;

            if (point.position.x < leftBound.position.x)
                leftBound = point;

            if (point.position.x > rightBound.position.x)
                rightBound = point;
        }

        Transform[] outputs = new Transform[] { topBound, bottomBound, leftBound, rightBound, depthPositive, depthNegative };
        return outputs;
    }
}
