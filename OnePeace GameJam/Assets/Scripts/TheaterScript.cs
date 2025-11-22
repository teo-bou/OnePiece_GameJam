using UnityEngine;

public class TheaterScript : MonoBehaviour
{
    public GameObject SpawnPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player has entered the theater area.");
            collision.gameObject.transform.position = SpawnPoint.transform.position;
        }
    }
}
