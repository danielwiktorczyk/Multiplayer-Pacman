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

        this.pacman.BufferDirection(this.bufferedDirection);
    }

    private void UpdateDirection()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            this.bufferedDirection = Vector3.forward;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            this.bufferedDirection = Vector3.right;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            this.bufferedDirection = Vector3.back;

        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            this.bufferedDirection = Vector3.left;
        }
    }
}
