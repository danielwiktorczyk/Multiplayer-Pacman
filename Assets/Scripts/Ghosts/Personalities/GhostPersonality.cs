using System.Collections.Generic;
using UnityEngine;

public abstract class GhostPersonality : MonoBehaviour
{    
    public abstract List<Vector3> GetDirections(Ghost ghost, Pacman pacman);
}