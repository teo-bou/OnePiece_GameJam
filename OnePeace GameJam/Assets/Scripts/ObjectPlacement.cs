using UnityEngine;

public class ObjectPlacement : MonoBehaviour
{
    // Assign your Prefab in the Inspector
    public GameObject objectToPlace;
    public int maxObjectCount = 5;
    private int objectCount = 1;
    // Public array to store positions of instantiated clones. Visible in the Inspector.
    public Vector3[] clonePositions;
    public GameObject finalPuzzlePiece;

    void Start()
    {
        maxObjectCount = maxObjectCount + 1;
        // Initialize the positions array to the maximum number of objects allowed.
        clonePositions = new Vector3[maxObjectCount];
        clonePositions[0] = finalPuzzlePiece.transform.position;
    }

    void Update()
    {
        // Check if the left mouse button is pressed down (index 0)
        if (Input.GetMouseButtonDown(0))
        {
            PlaceObject();
        }
    }

    void PlaceObject()
    {
        // 1. Get the current mouse position in screen coordinates (pixels)
        Vector3 mouseScreenPosition = Input.mousePosition;

        // 2. Convert the screen coordinates to world coordinates.
        // This is necessary because objects exist in the world, but the mouse position is reported in screen pixels.
        // The z-coordinate is often set to the camera's near clip plane by default, but we need to ensure it's 
        // appropriate for a 2D plane (usually z=0 in a 2D project).

        // We use Camera.main.ScreenToWorldPoint()
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        // 3. IMPORTANT for 2D: Reset the Z-coordinate to 0 (or your desired Z-depth)
        // This prevents the object from being placed far away on the Z-axis.
        worldPosition.z = -10f; // Assuming your camera is at z = -10 looking towards z = 0

        // 4. Instantiate the object at the calculated world position
        if (objectToPlace != null)
        {
            if (objectCount >= maxObjectCount)
            {
                Debug.Log("Maximum object count reached. Cannot place more objects.");
                return;
            }
            Instantiate(objectToPlace, worldPosition, Quaternion.identity);
            // Store the placed object's position in the public array for later use
            if (clonePositions != null && objectCount < clonePositions.Length)
            {
                clonePositions[objectCount] = worldPosition;
            }
            objectCount++;

            // Log for verification
            Debug.Log($"Placed object at: {worldPosition}");
        }
        else
        {
            Debug.LogError("Object To Place Prefab is not assigned in the Inspector!");
        }
    }
}