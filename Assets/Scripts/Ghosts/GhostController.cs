using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    // Strategy pattern
    [SerializeField] GhostPersonality ghostPersonality;

    private Ghost ghost;

    private Vector3 currentDirection;

    private Pacman player1;
    private Pacman player2;
    private Pacman closestPlayer;

    private void Awake()
    {
        this.ghost = GetComponent<Ghost>();


        this.closestPlayer = this.player1;
    }

    private void Update()
    {
        if (this.player1 is null || this.player2 is null)
        {
            TryToFindPlayers();
            return;
        }


        DetermineClosestPlayer();
    }

    private void TryToFindPlayers()
    {
        if (this.player1 is null)
        {
            var player1GO = GameObject.FindGameObjectWithTag("player1");
            if (player1GO is null)
                return;
            this.player1 = player1GO.GetComponent<Pacman>();
        }

        if (this.player2 is null)
        {
            var player2GO = GameObject.FindGameObjectWithTag("player2");
            if (player2GO is null)
                return;
            this.player2 = player2GO.GetComponent<Pacman>();
        }
    }

    private void DetermineClosestPlayer()
    {
        if (this.player2 is null)
        {
            // i.e. single player mode

            closestPlayer = player1;
            return;
        }

        var distanceP1 = Vector3.Distance(transform.position, this.player1.transform.position);
        var distanceP2 = Vector3.Distance(transform.position, this.player2.transform.position);

        this.closestPlayer = distanceP1 < distanceP2 ? player1 : player2;
    }

    private Vector3 NewDirection()
    {
        var availableDirections = AvailableDirections();

        if (availableDirections.Count == 0)
        {
            this.currentDirection = -this.currentDirection;
            return this.currentDirection; // move background if deadend, but it shouldn't happen
        }

        this.currentDirection = DetermineBestDirectionFromBehaviour(availableDirections);

        if (this.currentDirection == Vector3.zero)
        {
            //Debug.LogWarning("No directions were provided. Maybe pacman was null? Choosing an arbiturary one instead");
            //this.currentDirection = GetRandomDirection(availableDirections);
            this.currentDirection = Vector3.zero;
        }

        return this.currentDirection;
    }

    private Vector3 DetermineBestDirectionFromBehaviour(List<Vector3> availableDirections)
    {
        var desiredDirections = this.ghostPersonality.GetDirections(this.ghost, this.closestPlayer);

        if (desiredDirections is null)
            return Vector3.zero;

        while (desiredDirections.Count() > 0)
        {
            var consideredDirection = desiredDirections.First();

            if (availableDirections.Contains(consideredDirection))
            {
                //Debug.Log("Taking the desired direction");
                //Debug.Log(consideredDirection);
                return consideredDirection;
            }

            //Debug.Log("Unable to take the desired direction");
            //Debug.Log(consideredDirection);
            desiredDirections.Remove(consideredDirection);
        }

        return Vector3.zero;
    }

    private static Vector3 GetRandomDirection(List<Vector3> directions)
    {
        return directions.ToArray()[UnityEngine.Random.Range(0, directions.Count())];
    }

    private List<Vector3> AvailableDirections()
    {
        var currentTile = this.ghost.CurrentTile();
        var center = currentTile.transform.position;
        var directions = new List<Vector3>
        {
            Vector3.forward,
            Vector3.right,
            Vector3.back,
            Vector3.left,
        };

        if (this.currentDirection != Vector3.zero)
            directions = directions
                .Where(direction => !this.ghost.IsOppositeDirection(this.currentDirection, direction))
                .ToList();

        if (currentTile.Forward is null)
            directions.Remove(Vector3.forward);

        if (currentTile.Right is null)
            directions.Remove(Vector3.right);

        if (currentTile.Back is null)
            directions.Remove(Vector3.back);

        if (currentTile.Left is null)
            directions.Remove(Vector3.left);
        return directions;
    }

    private Vector3 ToCardinalDirection(Vector3 direction)
    {
        var normalized = direction.normalized;
        return new Vector3(Mathf.Round(normalized.x),
            0,
            Mathf.Round(normalized.z
            )
        );
    }

    internal Vector3 GetMovement()
    {
        if (this.player1 is null)
            return Vector3.zero;

        return NewDirection();
    }
}
