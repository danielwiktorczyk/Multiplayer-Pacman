using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pacman : MonoBehaviour
{
    [SerializeField] float speed = 3f;

    private Tile currentTile;
    private List<Tile> closestTiles;

    private Vector3 currentDirection;
    private Vector3 bufferedDirection;

    private bool isAtJunction;

    private bool pausedOnStart;

    private void Awake()
    {
        this.pausedOnStart = true;
    }

    internal void BufferDirection(Vector3 bufferedDirection)
    {
        this.bufferedDirection = bufferedDirection;
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
    }

    void Update()
    {
        Move();
    }

    private void FixedUpdate()
    {
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
        StopWhenNoTileInFront();

        UpdateCurrentDirectionFromBuffer();

        this.transform.position += this.currentDirection * Time.deltaTime * this.speed;
    }

    private void StopWhenNoTileInFront()
    {
        var destinationTile = this.currentTile.TileAtOffset(this.currentDirection);

        if (destinationTile != null)
            return;

        var destinationTilePosition = this.currentTile.transform.position + this.currentDirection;

        if (Distance2D(transform.position, destinationTilePosition) > 1f)
            return;

        this.currentDirection = Vector3.zero;
    }

    private void UpdateCurrentDirectionFromBuffer()
    {
        if (this.bufferedDirection != Vector3.zero && this.pausedOnStart)
        {
            HandleDirectionDirectlyAfterStart();
            return;
        }

        if (this.bufferedDirection == Vector3.zero)
            return;

        if (IsOppositeDirection())
        {
            if (this.currentTile.TileDirectlyInFront(transform.position, this.bufferedDirection) is null)
                return;
        }
        else
        {
            if (Distance2D(transform.position, this.currentTile.transform.position) > 0.01f)
                return; // must be at a junction! So return

            if (this.currentTile.TileAtOffset(this.bufferedDirection) is null)
                return;

            transform.position = new Vector3
            (
                this.currentTile.transform.position.x,
                transform.position.y,
                this.currentTile.transform.position.z
            );
        }

        this.currentDirection = this.bufferedDirection;
        this.bufferedDirection = Vector3.zero;
    }

    private void HandleDirectionDirectlyAfterStart()
    {
        this.currentDirection = this.bufferedDirection;
        this.bufferedDirection = Vector3.zero;

        this.pausedOnStart = false;
    }

    private bool IsOppositeDirection()
    {
        return this.currentDirection == Vector3.forward && this.bufferedDirection == Vector3.back
                    || this.currentDirection == Vector3.back && this.bufferedDirection == Vector3.forward
                    || this.currentDirection == Vector3.right && this.bufferedDirection == Vector3.left
                    || this.currentDirection == Vector3.left && this.bufferedDirection == Vector3.right;
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

}
