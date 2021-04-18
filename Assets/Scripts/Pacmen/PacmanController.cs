using UnityEngine;

public class PacmanController : MonoBehaviour
{
    private Pacman pacman;

    private Vector3 bufferedDirection;

    private void Awake()
    {
        this.pacman = GetComponent<Pacman>();
    }

    void Update()
    {
        UpdateDirection();
    }

    private void UpdateDirection()
    {
        var newDirection = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            newDirection = Vector3.forward;
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            newDirection = Vector3.right;
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            newDirection = Vector3.back;
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            newDirection = Vector3.left;

        if (newDirection != Vector3.zero 
            && (newDirection != this.bufferedDirection || this.bufferedDirection == Vector3.zero))
        {
            this.bufferedDirection = newDirection;
            this.pacman.BufferDirection(this.bufferedDirection);
        }
    }
}
