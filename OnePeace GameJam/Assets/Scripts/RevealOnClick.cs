using UnityEngine;

public class RevealOnClick : MonoBehaviour
{
    public GameObject objectToReveal;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {

            // Reveal the object
            if (objectToReveal != null)
            {
                objectToReveal.SetActive(true);
            }

        }

    }
}
