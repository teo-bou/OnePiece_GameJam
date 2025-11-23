using UnityEngine;

// This script handles the drag-and-drop mechanics for a puzzle piece.

[RequireComponent(typeof(BoxCollider2D))]
public class PuzzlePiece : MonoBehaviour
{
    [Tooltip("The unique ID of this puzzle piece.")]
    public int pieceID;

    [Tooltip("The maximum distance (in world units) to snap to the slot.")]
    public float snapThreshold = 0.5f;

    private Vector3 initialPosition;
    private Camera mainCamera;
    private bool isPlaced = false;

    void Start()
    {
        // Cache the main camera reference
        mainCamera = Camera.main;
        // Store the initial position for potential resets
        initialPosition = transform.position;

        // Register this piece with the GameManager
        GameManager.Instance.RegisterPiece(this);
    }

    // --- DRAG MECHANICS (Using legacy Input System) ---

    private void OnMouseDown()
    {
        // When clicked, start dragging and ensure the piece is not marked as placed
        if (isPlaced)
        {
            isPlaced = false;
            // Notify the manager that this piece is no longer correctly placed
            GameManager.Instance.PiecePlaced(false);
        }
    }

    private void OnMouseDrag()
    {
        // Convert mouse position to world coordinates
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = mainCamera.nearClipPlane;
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);

        // Keep Z position consistent for 2D sorting
        worldPos.z = transform.position.z;

        // Move the piece to the mouse position
        transform.position = worldPos;
    }

    private void OnMouseUp()
    {
        // Check for the correct drop target upon release
        CheckForSnap();
    }

    // --- SNAPPING LOGIC ---

    private void CheckForSnap()
    {
        // Find all PuzzleSlot components in the scene
        PuzzleSlot[] slots = FindObjectsOfType<PuzzleSlot>();
        PuzzleSlot correctSlot = null;

        // 1. Identify the correct slot for this piece
        foreach (PuzzleSlot slot in slots)
        {
            if (slot.correctPieceID == pieceID)
            {
                correctSlot = slot;
                break;
            }
        }

        if (correctSlot == null)
        {
            Debug.LogError($"Piece ID {pieceID} cannot find its corresponding slot!");
            return;
        }

        // 2. Calculate the distance to the correct slot
        Vector3 slotPosition = correctSlot.GetSnapPosition();
        float distance = Vector3.Distance(transform.position, slotPosition);

        // 3. Check the snap threshold
        if (distance <= snapThreshold)
        {
            // Snap the piece into the exact center of the slot
            transform.position = slotPosition;
            isPlaced = true;
            Debug.Log($"Piece {pieceID} snapped into place.");

            // Notify the Game Manager of successful placement
            GameManager.Instance.PiecePlaced(true);
        }
        else
        {
            // If the piece is dropped too far, return it to its initial spot (optional, but good UX)
            // For a more advanced game, you might leave it where it was dropped.
            transform.position = initialPosition;
            isPlaced = false;
        }
    }
}