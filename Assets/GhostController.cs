using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    private Ghost ghost;

    private Vector3 previousDirection;
    private Vector3 currentDirection;

    private void Awake()
    {
        this.ghost = GetComponent<Ghost>();
    }

    private Vector3 NewDirection()
    {
        this.previousDirection = this.currentDirection;

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

        if (directions.Count == 0)
        {
            this.currentDirection = -this.currentDirection;
            return this.currentDirection; // move background if deadend, shouldn't happen
        }

        this.currentDirection = directions.ToArray()[Random.Range(0, directions.Count())];

        return this.currentDirection;
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

    internal Vector3 GetMovement()
    {
        return NewDirection();
    }
}
