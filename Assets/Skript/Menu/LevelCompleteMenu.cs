using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelCompleteMenu : MonoBehaviour
{
    public GameObject levelCompleteCanvas;
    public Button exitButton;
    public Button nextLevelButton;
    public GameObject player; // Rename to follow naming conventions

    private void Start()
    {
        // Attach event handlers to buttons
        exitButton.onClick.AddListener(ExitGame);
        nextLevelButton.onClick.AddListener(NextLevel);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object colliding with the congratulationTrigger is the player
        if (other.gameObject == player)
        {
            // Display Canvas with UI elements
            levelCompleteCanvas.SetActive(true);
        }
    }

    private void ExitGame()
    {

        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Menu");
    }

    private void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}