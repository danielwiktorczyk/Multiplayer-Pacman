using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private List<Tile> neighbors;

    private void Awake()
    {
        Calibrate();
    }

    void Start()
    {
        GetNeighbors();
    }

    private void GetNeighbors()
    {
        var colliders = Physics.OverlapSphere(transform.position, 1f);

        this.neighbors = colliders
            .Where(collider => collider.GetComponent<Tile>() != null)
            .Select(collider => collider.GetComponent<Tile>())
            .Distinct()
            .ToList();
    }

    private void Calibrate()
    {
        this.transform.position = new Vector3(
            GetClosestHalf(this.transform.position.x),
            0,
            GetClosestHalf(this.transform.position.z));
    }

    private float GetClosestHalf(float x) => x - (x % 0.5f);

    public Tile TileAtOffset(Vector3 direction)
    {
        return this.neighbors
            .Where(tile => tile.transform.position == transform.position + direction)
            .FirstOrDefault();
    }

    public Tile TileDirectlyInFront(Vector3 center, Vector3 direction)
    {
        if (direction == Vector3.forward)
            return this.neighbors
                .Where(tile => tile.transform.position.z - transform.position.z > 0
                    && Mathf.Abs(tile.transform.position.x - center.x) < 0.1f)
                .FirstOrDefault();

        if (direction == Vector3.right)
            return this.neighbors
                .Where(tile => tile.transform.position.x - transform.position.x > 0
                    && Mathf.Abs(tile.transform.position.z - center.z) < 0.1f)
                .FirstOrDefault();

        if (direction == Vector3.back)
            return this.neighbors
                .Where(tile => tile.transform.position.z - transform.position.z < 0
                    && Mathf.Abs(tile.transform.position.x - center.x) < 0.1f)
                .FirstOrDefault();

        if (direction == Vector3.left)
            return this.neighbors
                .Where(tile => tile.transform.position.x - transform.position.x < 0
                    && Mathf.Abs(tile.transform.position.z - center.z) < 0.1f)
                .FirstOrDefault();

        return null;
    }
}
