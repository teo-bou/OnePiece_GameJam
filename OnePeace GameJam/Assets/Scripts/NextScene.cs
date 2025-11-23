using UnityEngine;

public class NextScene : MonoBehaviour
{
    public KeyCode next = KeyCode.Space;
    public string sceneName = "Puzzle1";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(next))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
}
