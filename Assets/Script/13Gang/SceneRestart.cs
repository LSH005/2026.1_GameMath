using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneRestart : MonoBehaviour
{
    public KeyCode key = KeyCode.R;

    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            RestartCurrentScene();
        }
    }

    private void RestartCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}