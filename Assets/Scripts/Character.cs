using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] float speed = 3f;

    private Tile currentTile;
    private List<Tile> closestTiles;

    public Vector3 CurrentDirection;
    public Vector3 DirectionBeforePause;
    private Vector3 bufferedDirection;

    private bool isPaused;

    public Tile CurrentTile()
    {
        return this.currentTile;
    }

    private void Awake()
    {
        this.isPaused = true;
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

        this.transform.position += this.CurrentDirection * Time.deltaTime * this.speed;
    }

    private void StopWhenNoTileInFront()
    {
        if (this.isPaused)
            return;

        var destinationTile = this.currentTile.TileAtOffset(this.CurrentDirection);

        if (destinationTile != null)
            return;

        var destinationTilePosition = this.currentTile.transform.position + this.CurrentDirection;

        if (Distance2D(transform.position, destinationTilePosition) > 0.99f)
            return;

        DirectionBeforePause = CurrentDirection;
        CurrentDirection = Vector3.zero;
        transform.position = new Vector3
        (
            this.currentTile.transform.position.x,
            transform.position.y,
            this.currentTile.transform.position.z
        );
        this.isPaused = true;
    }

    private void UpdateCurrentDirectionFromBuffer()
    {
        if (this.bufferedDirection != Vector3.zero && this.isPaused)
        {
            HandleDirectionOnPause();
            return;
        }

        if (this.bufferedDirection == Vector3.zero)
            return;

        if (IsOppositeDirection(this.CurrentDirection, this.bufferedDirection))
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

        this.CurrentDirection = this.bufferedDirection;
        this.bufferedDirection = Vector3.zero;
    }

    private void HandleDirectionOnPause()
    {
        this.CurrentDirection = this.bufferedDirection;
        this.bufferedDirection = Vector3.zero;

        this.isPaused = false;
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

}