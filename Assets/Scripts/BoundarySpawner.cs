using UnityEngine;

public class BoundarySpawner : MonoBehaviour
{
    public GameObject boundarySegmentPrefab;
    private Vector2 boundaryRange;

    public void Init(Vector2 gameFieldRange)
    {
        boundaryRange = gameFieldRange;
        SpawnBoundarySegments();
    }

    private void SpawnBoundarySegments()
    {
        int minX = (int)boundaryRange.x;
        int maxX = (int)boundaryRange.y;
        int minY = (int)boundaryRange.x;
        int maxY = (int)boundaryRange.y;

        // Spawn horizontal boundary segments
        for (float x = minX; x <= maxX; x++)
        {
            SpawnBoundarySegment(new Vector3(x, minY, 0));
            SpawnBoundarySegment(new Vector3(x, maxY, 0));
        }

        // Spawn vertical boundary segments
        for (float y = minY + 1; y < maxY; y++)
        {
            SpawnBoundarySegment(new Vector3(minX, y, 0));
            SpawnBoundarySegment(new Vector3(maxX, y, 0));
        }
    }

    private void SpawnBoundarySegment(Vector3 position)
    {
        GameObject newSegment = Instantiate(boundarySegmentPrefab, position, Quaternion.identity);
        newSegment.transform.SetParent(transform);
    }
}
