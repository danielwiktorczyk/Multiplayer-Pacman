using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Pacman : MonoBehaviour
{
    [SerializeField] float speed = 3f;

    private Tile currentTile;
    private List<Tile> closestTiles;

    public Vector3 CurrentDirection;
    public Vector3 DirectionBeforePause;
    private Vector3 bufferedDirection;

    private bool isPaused;

    public int Score;
    [SerializeField] private Text scoreText;

    private Vector3 startingPosition;
    private Quaternion startingRotation;

    private AudioManager audioManager;
    private PhotonView photonView;

    public Tile CurrentTile()
    {
        return this.currentTile;
    }

    private void Awake()
    {
        this.isPaused = true;

        this.startingPosition = transform.position;
        this.startingRotation = transform.rotation;

        this.audioManager = FindObjectOfType<AudioManager>();
        this.photonView = GetComponent<PhotonView>();

        if (this.scoreText is null)
        {
            if (this.CompareTag("player1"))
                this.scoreText = GameObject.FindGameObjectWithTag("p1score").GetComponent<Text>();
            else
                this.scoreText = GameObject.FindGameObjectWithTag("p2score").GetComponent<Text>();
        }

        this.closestTiles = new List<Tile>();
    }

    internal void BufferDirection(Vector3 bufferedDirection)
    {
        this.bufferedDirection = bufferedDirection;
    }

    void Start()
    {
        if (!this.photonView.IsMine)
            return;

        Respawn();
    }

    void Update()
    {
        if (!this.photonView.IsMine)
            return;

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
        if (this.CurrentDirection != Vector3.zero)
            this.transform.rotation = Quaternion.LookRotation(this.CurrentDirection, Vector3.up);
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
            if (!WouldMovePastCenterThisUpdate())
                return;

            //if (Distance2D(transform.position, this.currentTile.transform.position) > 0.01f)
            //    return; // must be at a junction! So return

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

    private bool WouldMovePastCenterThisUpdate()
    {
        var currentPosition = transform.position;
        var nextPosition = currentPosition
                        + this.CurrentDirection * Time.deltaTime * this.speed;
        var center = this.currentTile.transform.position;

        if (this.CurrentDirection == Vector3.forward)
            return center.z > currentPosition.z && center.z < nextPosition.z;

        if (this.CurrentDirection == Vector3.right)
            return center.x > currentPosition.x && center.x < nextPosition.x;

        if (this.CurrentDirection == Vector3.back)
            return center.z < currentPosition.z && center.z > nextPosition.z;

        if (this.CurrentDirection == Vector3.left)
            return center.x < currentPosition.x && center.x > nextPosition.x;


        return false;
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
        if (other.CompareTag("Pellet"))
        {
            CollectPellet(other.GetComponent<Pellet>());
            return;
        }

        if (other.CompareTag("Tile"))
        {
            if (!this.photonView.IsMine)
                return;
            EnterNewTile(other);
            return;
        }

    }

    private void EnterNewTile(Collider other)
    {
        var tile = other.GetComponent<Tile>();

        if (tile != null)
            this.closestTiles.Add(tile);

        if (tile.WarpTile != null)
        {
            this.currentTile = tile.WarpTile;
            this.closestTiles = new List<Tile> { this.currentTile };
            transform.position = new Vector3
            (
                this.currentTile.transform.position.x + this.CurrentDirection.x,
                transform.position.y,
                this.currentTile.transform.position.z + this.CurrentDirection.z
            );
        }
    }

    private void CollectPellet(Pellet pellet)
    {
        if (pellet.IsRemovedFromPlay())
            return;

        pellet.OnPickedUp();

        if (this.photonView.IsMine)
            this.audioManager.PlayPelletSound();

        if (pellet.SpeedBoost)
            BoostSpeed();

        Score += 1;
        this.scoreText.text = $"P1 Score : {Score}";
    }

    private void BoostSpeed()
    {
        this.speed *= 2;

        StartCoroutine(RevertSpeedIn(5f));
    }

    private IEnumerator RevertSpeedIn(float time)
    {
        yield return new WaitForSeconds(time);

        this.speed /= 2;

        yield return null;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Tile"))
            RemoveTileFromClosestTiles(other);
    }

    private void RemoveTileFromClosestTiles(Collider other)
    {
        if (!this.photonView.IsMine)
            return;

        var tile = other.GetComponent<Tile>();

        if (tile != null)
            this.closestTiles.Remove(tile);
    }

    public void GetSpooked()
    {
        if (!this.photonView.IsMine)
            return;

        this.audioManager.PlaySpookedSound();

        Respawn();
    }

    public void Respawn()
    {
        this.isPaused = true;

        transform.position = this.startingPosition;
        transform.rotation = this.startingRotation;

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

        this.CurrentDirection = Vector3.zero;
    }
}
