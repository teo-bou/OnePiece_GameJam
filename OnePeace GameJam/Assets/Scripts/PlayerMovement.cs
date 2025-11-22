using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject objectPlacementManager;
    public KeyCode launch = KeyCode.Space;
    private bool launched = false;
    private ObjectPlacement objectPlacementScript;
    private Vector3[] clonePositions;
    private int targetIndex = -1;
    private const float reachThreshold = 0.01f;
    public Animator animator;

    void Start()
    {
        if (objectPlacementManager != null)
        {
            objectPlacementScript = objectPlacementManager.GetComponent<ObjectPlacement>();
            if (objectPlacementScript != null)
            {
                clonePositions = objectPlacementScript.clonePositions;
            }
        }
    }

    void Update()
    {
        if (objectPlacementScript != null)
        {
            clonePositions = objectPlacementScript.clonePositions;
        }

        if (Input.GetKeyDown(launch) && !launched)
        {
            launched = true;
            animator.SetFloat("Speed", 1);
        }

        if (launched)
        {

            targetIndex = FindClosestIndex();
            Vector3 current = transform.position;
            Vector3 target = clonePositions[targetIndex];

            Vector3 direction = target - current;
            transform.position = Vector3.MoveTowards(current, target, moveSpeed * Time.deltaTime);

            // Mirror sprite when moving left by flipping localScale.x
            if (Mathf.Abs(direction.x) > 0.0001f)
            {
                Vector3 ls = transform.localScale;
                ls.x = Mathf.Abs(ls.x) * (direction.x < 0f ? -1f : 1f);
                transform.localScale = ls;
            }
            if (Vector3.Distance(current, target) < reachThreshold)
            {
                if (targetIndex == 0)
                {// Reached final puzzle piece
                    Debug.Log("Reached final puzzle piece!");
                    launched = false;
                }
                if (clonePositions != null && targetIndex >= 0 && targetIndex < clonePositions.Length)
                {
                    clonePositions[targetIndex] = Vector3.zero;
                    targetIndex = -1;
                }
            }

        }
        else
        {
            animator.SetFloat("Speed", 0);
        }
    }
    private int FindClosestIndex()
    {
        if (clonePositions == null || clonePositions.Length == 0) return -1;

        int bestIndex = -1;
        float minDistSqr = float.MaxValue;
        Vector3 current = transform.position;

        for (int i = 0; i < clonePositions.Length; i++)
        {
            Vector3 pos = clonePositions[i];
            if (pos == Vector3.zero) continue;

            float dsq = (pos - current).sqrMagnitude;
            if (dsq < minDistSqr)
            {
                minDistSqr = dsq;
                bestIndex = i;
            }
        }

        return bestIndex;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Collided with obstacle! You lose!");
            launched = false;
            animator.SetBool("Clashed", true);

        }
        if (collision.gameObject.CompareTag("Puzzle"))
        {
            Debug.Log("Reached final puzzle piece! You win!");
            Destroy(collision.gameObject);
            launched = false;
        }
        if (collision.gameObject.CompareTag("Coin"))
        {
            Debug.Log("Picked up a coin!");
            Destroy(collision.gameObject);
        }
    }

}
