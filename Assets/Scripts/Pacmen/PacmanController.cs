using Photon.Pun;
using UnityEngine;

public class PacmanController : MonoBehaviour
{
    private Pacman pacman;

    private Vector3 bufferedDirection;

    private PhotonView photonView;
    private GameManager gameManager;

    private void Awake()
    {
        this.pacman = GetComponent<Pacman>();
        this.photonView = GetComponent<PhotonView>();
        
        this.gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (!this.photonView.IsMine 
            || !this.gameManager.IsGameStarted)
            return;

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
            && (newDirection != this.pacman.CurrentDirection || this.bufferedDirection == Vector3.zero))
        {
            this.bufferedDirection = newDirection;
            this.pacman.BufferDirection(this.bufferedDirection);
        }
    }
}
