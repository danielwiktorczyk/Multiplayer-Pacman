using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] float speed = 3f;

    private Tile currentTile;
    private List<Tile> closestTiles;

    public Vector3 CurrentDirection;
    private Vector3 bufferedDirection;

    public bool IsPaused;

    private GhostController ghostController;

    private float cool;

    public Tile CurrentTile()
    {
        return this.currentTile;
    }

    private void Awake()
    {
        this.IsPaused = true;
        this.ghostController = GetComponent<GhostController>();
    }

    void Start()
    {
        var colliders = Physics.OverlapSphere(this.transform.position, 3f);
        this.currentTile = colliders
            .Where(collider => collider.GetComponent<Tile>() != null
                && collider.gameObject != this.gameObject)
            .Select(collider => collider.GetComponent<Tile>())
            .OrderBy(tile => Distance2D(transform.position, tile.transform.position))
            .First();
        this.closestTiles = new List<Tile>
        {
            this.currentTile
        };

        transform.position = new Vector3
        (
            this.currentTile.transform.position.x,
            transform.position.y,
            this.currentTile.transform.position.z
        );

        //GetMovementFromController();
    }

    void Update()
    {
        Move();
        UpdateClosestTile();
    }

    private void UpdateClosestTile()
    {
        this.currentTile = closestTiles
            .OrderBy(tile => Distance2D(transform.position, tile.transform.position))
            .First();
    }

    public float Distance2D(Vector3 a, Vector3 b)
    {
        return Mathf.Sqrt(Mathf.Pow((a.x - b.x), 2) + Mathf.Pow((a.z - b.z), 2));
    }

    private void Move()
    {
        StopAtJunction();

        if (this.IsPaused)
            GetMovementFromController();

        this.transform.position += this.CurrentDirection * Time.deltaTime * this.speed;
    }

    private void GetMovementFromController()
    {
        this.CurrentDirection = this.ghostController.GetMovement();
        this.IsPaused = false;
    }

    private void StopAtJunction()
    {
        if (this.IsPaused)
            return;

        if (this.cool > 0)
        {
            this.cool -= Time.deltaTime;
            return;
        }
        this.cool = 0.5f;

        var directionToCurrentTile = transform.position - this.currentTile.transform.position;
        if (directionToCurrentTile.normalized != this.CurrentDirection)
            return;

        var directions = this.currentTile.Neighbors
            .Where(tile => tile != currentTile)
            .Select(tile => tile.transform.position - currentTile.transform.position)
            .Select(direction => Normalize(direction))
            .ToArray();

        if (directions.Length < 2)
            return;

        CurrentDirection = Vector3.zero;
        transform.position = new Vector3
        (
            this.currentTile.transform.position.x,
            transform.position.y,
            this.currentTile.transform.position.z
        );
        
        this.IsPaused = true;
    }

    public bool IsOppositeDirection(Vector3 a, Vector3 b)
    {
        return a == Vector3.forward && b == Vector3.back
                    || a == Vector3.back && b == Vector3.forward
                    || a == Vector3.right && b == Vector3.left
                    || a == Vector3.left && b == Vector3.right;
    }

    private void OnTriggerEnter(Collider other)
    {
        var tile = other.GetComponent<Tile>();

        if (tile != null)
            this.closestTiles.Add(tile);
    }

    private void OnTriggerExit(Collider other)
    {
        var tile = other.GetComponent<Tile>();

        if (tile != null)
            this.closestTiles.Remove(tile);
    }

    private Vector3 Normalize(Vector3 direction)
    {
        var normalized = direction.normalized;
        return new Vector3(Mathf.Round(normalized.x),
            0,
            Mathf.Round(normalized.z
            )
        );
    }
}
