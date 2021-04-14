using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public List<Tile> Neighbors;

    public Tile Forward;
    public Tile Right;
    public Tile Back;
    public Tile Left;

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

        Neighbors = colliders
            .Where(collider => collider.GetComponent<Tile>() != null)
            .Select(collider => collider.GetComponent<Tile>())
            .Distinct()
            .ToList();

        Forward = Neighbors
            .Where(tile => Vector3.Distance(tile.transform.position, transform.position + Vector3.forward) < 0.5f)
            .FirstOrDefault();
        Right = Neighbors
            .Where(tile => Vector3.Distance(tile.transform.position, transform.position + Vector3.right) < 0.5f)
            .FirstOrDefault();
        Back = Neighbors
            .Where(tile => Vector3.Distance(tile.transform.position, transform.position + Vector3.back) < 0.5f)
            .FirstOrDefault();
        Left = Neighbors
            .Where(tile => Vector3.Distance(tile.transform.position, transform.position + Vector3.left) < 0.5f)
            .FirstOrDefault();

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
        return Neighbors
            .Where(tile => tile.transform.position == transform.position + direction)
            .FirstOrDefault();
    }

    public Tile TileDirectlyInFront(Vector3 center, Vector3 direction)
    {
        if (direction == Vector3.forward)
            return this.Neighbors
                .Where(tile => tile.transform.position.z - transform.position.z > 0
                    && Mathf.Abs(tile.transform.position.x - center.x) < 0.1f)
                .FirstOrDefault();

        if (direction == Vector3.right)
            return Neighbors
                .Where(tile => tile.transform.position.x - transform.position.x > 0
                    && Mathf.Abs(tile.transform.position.z - center.z) < 0.1f)
                .FirstOrDefault();

        if (direction == Vector3.back)
            return Neighbors
                .Where(tile => tile.transform.position.z - transform.position.z < 0
                    && Mathf.Abs(tile.transform.position.x - center.x) < 0.1f)
                .FirstOrDefault();

        if (direction == Vector3.left)
            return Neighbors
                .Where(tile => tile.transform.position.x - transform.position.x < 0
                    && Mathf.Abs(tile.transform.position.z - center.z) < 0.1f)
                .FirstOrDefault();

        return null;
    }
}
