using System.Collections.Generic;
using UnityEngine;

// This script manages the overall state of the game, 
// tracks placed pieces, and detects the win condition.

public class GameManager : MonoBehaviour
{
    // Singleton pattern for easy access from other scripts
    public static GameManager Instance;

    // List to keep track of all pieces in the scene.
    private List<PuzzlePiece> allPieces = new List<PuzzlePiece>();
    public GameObject Dialogs;
    public string nextSceneName = "DialogPtitFrere";
    public KeyCode next = KeyCode.Space;
    // Counter for correctly placed pieces.
    private int placedPieceCount = 0;
    private bool won = false;

    void Update()
    {
        if (won && Input.GetKeyDown(next))
        {

            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);


        }
    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Called by each PuzzlePiece upon initialization
    public void RegisterPiece(PuzzlePiece piece)
    {
        allPieces.Add(piece);
    }

    // Called by PuzzlePiece when it successfully snaps into a slot
    public void PiecePlaced(bool isCorrectlyPlaced)
    {
        if (isCorrectlyPlaced)
        {
            placedPieceCount++;
        }
        else
        {
            placedPieceCount--; // Should only happen if a piece is moved again
        }

        // Check for win condition
        if (placedPieceCount >= allPieces.Count)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        Dialogs.SetActive(true);
        won = true;

    }
}