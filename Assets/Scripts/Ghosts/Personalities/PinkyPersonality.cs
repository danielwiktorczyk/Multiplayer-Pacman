using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PinkyPersonality : GhostPersonality
{
    public override List<Vector3> GetDirections(Ghost ghost, Pacman pacman)
    {
        var origin = ghost.transform.position;
        var target = pacman.transform.position;

        // Pinky likes to ambush. Aim for BEYOND pacman to cut them off! 
        var ambushOffset = pacman.transform.forward * 5f;

        var directions = new List<Vector3>
        {
            Vector3.forward,
            Vector3.right,
            Vector3.back,
            Vector3.left,
        };

        foreach (var direction in directions)
            Debug.DrawLine(origin + direction, target + ambushOffset);

        directions = directions
            .OrderBy(direction => Vector3.Distance(origin + direction, target + ambushOffset))
            .ToList();

        return directions;
    }
}
