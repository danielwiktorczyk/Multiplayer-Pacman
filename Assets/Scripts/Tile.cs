using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public List<Tile> Neighbors;
    public bool SpawnPelletAbove = true;

    [SerializeField] private GameObject pelletToSpawn;
    [SerializeField] private GameObject speedPelletToSpawn;

    public Tile WarpTile;
    public bool warpUp;
    public bool warpRight;
    public bool warpDown;
    public bool warpLeft;

    public Tile Forward;
    public Tile Right;
    public Tile Back;
    public Tile Left;

    public bool SpawnSpeedPellet;

    private void Awake()
    {
        Calibrate();
    }

    void Start()
    {
        GetNeighbors();

        SpawnPellet();
    }

    private void SpawnPellet()
    {
        if (!this.SpawnPelletAbove)
            return;

        var pelletToSpawn = SpawnSpeedPellet ? this.speedPelletToSpawn : this.pelletToSpawn;
        var pellet = Instantiate(pelletToSpawn.gameObject, this.transform);
        pellet.transform.position = this.transform.position;
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

        if (WarpTile != null)
        {
            if (this.warpUp)
                Forward = WarpTile;
            else if (this.warpRight)
                Right = WarpTile;
            else if (this.warpDown)
                Back = WarpTile;
            else if (this.warpLeft)
                Left = WarpTile;
        }
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
        if (direction == Vector3.forward)
            return Forward;

        if (direction == Vector3.right)
            return Right;

        if (direction == Vector3.back)
            return Back;

        if (direction == Vector3.left)
            return Left;

        return null;
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
