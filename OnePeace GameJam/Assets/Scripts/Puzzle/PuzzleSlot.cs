using UnityEngine;

// This script defines the drop targets for the puzzle pieces.

public class PuzzleSlot : MonoBehaviour
{
    [Tooltip("The ID of the piece that belongs in this slot.")]
    public int correctPieceID;

    [Tooltip("The sprite renderer for visual feedback (e.g., changing color when a piece is near).")]
    public SpriteRenderer slotRenderer;

    void Awake()
    {
        if (slotRenderer == null)
        {
            slotRenderer = GetComponent<SpriteRenderer>();
        }
    }

    // Public method to get the position (used by PuzzlePiece to snap to)
    public Vector3 GetSnapPosition()
    {
        // We use the transform.position of the slot as the snap point
        return transform.position;
    }
}