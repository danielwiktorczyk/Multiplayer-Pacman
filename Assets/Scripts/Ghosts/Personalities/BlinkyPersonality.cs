using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlinkyPersonality : GhostPersonality
{
    public override List<Vector3> GetDirections(Ghost ghost, Pacman pacman)
    {
        var origin = ghost.transform.position;
        var target = pacman.transform.position;

        // Blinky just goes straight for the player. No funny business here

        var directions = new List<Vector3>
        {
            Vector3.forward,
            Vector3.right,
            Vector3.back,
            Vector3.left,
        };

        foreach (var direction in directions)
            Debug.DrawLine(origin + direction, target);

        directions = directions
            .OrderBy(direction => Vector3.Distance(origin + direction, target))
            .ToList();

        return directions;
    }
}
