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
        var directions = this.ghost.CurrentTile().Neighbors
            .Where(tile => tile != currentTile)
            .Select(tile => tile.transform.position - currentTile.transform.position)
            .Select(direction => Normalize(direction))
            .ToArray();
        Debug.Log(directions.Length);

        if (this.currentDirection != Vector3.zero)
            directions = directions
                .Where(direction => !this.ghost.IsOppositeDirection(this.currentDirection, direction))
                .ToArray();
        Debug.Log(directions.Length);

        this.currentDirection = directions[Random.Range(0, directions.Length)];

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
        Debug.Log("GetMovement!");
        return NewDirection();
    }
}
