using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.instance;
    }

    public void StartGame()
    {
        _gameManager.LoadGame();
    }

    public void ReturnMainMenu()
    {
        _gameManager.LoadMainMenu();
    }
}
