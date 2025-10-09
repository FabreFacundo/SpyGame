using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private string _mainMenuSceneName = "MainMenuScene";
    [SerializeField] private string _gameSceneName = "GameScene";
    [SerializeField] private string _winSceneName = "WinScene";
    [SerializeField] private string _loseSceneName = "LoseScene";



    private Vector3 _playerStartPosition;
    private CheckPointManager _checkPointManager;
    private PlayerManager _playerManager;
    private PlayerInputs _inputs;
    private Inventory _inventory;
    private EnemyManager _enemyManager;
    private UIManager _uiManager;

    public string MainMenuSceneName => _mainMenuSceneName;
    public string GameSceneName => _gameSceneName;
    public string WinSceneName => _winSceneName;
    public string LoseSceneName => _loseSceneName;
    public PlayerManager PlayerManager => _playerManager;
    public UIManager UIManager => _uiManager;
    public Inventory Inventory => _inventory;
    public CheckPointManager CheckPointManager => _checkPointManager;
    public PlayerInputs Inputs { get { return _inputs; } }
    public Transform PlayerSpawnPoint => _playerSpawnPoint;
    public EnemyManager EnemyManager { get { return _enemyManager; } set { _enemyManager = value; } }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            _inputs = GetComponent<PlayerInputs>();
            _inventory = GetComponent<Inventory>();
            _checkPointManager = GetComponent<CheckPointManager>();
            _uiManager = GetComponent<UIManager>();
            _enemyManager = GetComponent<EnemyManager>();
            _playerManager = GetComponent<PlayerManager>();
            _playerStartPosition = _playerSpawnPoint.position;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    
 


    public void SetGameStatus()
    {
        _playerManager.enabled = true;
        _checkPointManager.enabled = true;
        _inventory.enabled = true;
        _enemyManager.enabled = true;
        _uiManager.enabled = true;
        _inputs.enabled = true;
        _playerStartPosition = _playerSpawnPoint.position;
        _checkPointManager.ResetAll();
        _enemyManager.FindEnemies();
        _playerManager.PlayerObject.transform.position = _playerStartPosition;

    }
    public void SetMenuStatus()
    {
        _playerManager.enabled = false;
        _checkPointManager.enabled = false;
        _inventory.enabled = false;
        _enemyManager.enabled = false;
        _uiManager.enabled = false;
        _inputs.enabled = false;
        _inventory.ResetInventory();

    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadGame()
    {
        SceneManager.sceneLoaded += (scene, mode) => SetGameStatus();
        SceneManager.sceneLoaded -= _checkPointManager.UpdateCheckPoint;
        _playerSpawnPoint.position = _playerStartPosition;
        _playerManager.Lifes = _playerManager.MaxLifes;
        _inputs.ChangeCursorLockState(CursorLockMode.Locked);
        Time.timeScale = 1;
        LoadScene(_gameSceneName);
    }

    public void LoadMainMenu()
    {
        SceneManager.sceneLoaded += (scene, mode) => SetMenuStatus();
        _inputs.ChangeCursorLockState(CursorLockMode.None);
        LoadScene(_mainMenuSceneName);
    }

    public void LoadWinScene()
    {
        SceneManager.sceneLoaded += (scene, mode) => SetMenuStatus();
        _inputs.ChangeCursorLockState(CursorLockMode.None);
        LoadScene(_winSceneName);
    }

    public void LoadLoseScene()
    {
        SceneManager.sceneLoaded += (scene, mode) => SetMenuStatus();
        _inputs.ChangeCursorLockState(CursorLockMode.None);
        LoadScene(_loseSceneName);
    }
    public void SetCheckpoint(Vector3 checkpointPosition)
    {
        _playerSpawnPoint.position = checkpointPosition;
    }
    public void LoadCheckpoint()
    {
        SceneManager.sceneLoaded -= (scene, mode) => SetGameStatus();
        SceneManager.sceneLoaded += _checkPointManager.UpdateCheckPoint;
        LoadScene(SceneManager.GetActiveScene().name);
    }

}
