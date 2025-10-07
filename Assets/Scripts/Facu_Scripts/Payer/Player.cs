using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Stats attributes")]
    [SerializeField] private float _health = 100;
    [SerializeField] private float _maxHealth = 50f;
    [SerializeField] private GameObject _GFX;
    [SerializeField] private GameObject _gunGFX;

    private PlayerManager _playerManager;

    public float HealthValue => _health;
    public float MaxHealth => _maxHealth;

    public GameObject GFX => _GFX;
    public GameObject GunGFX => _gunGFX;

    private void Awake()
    { 
        if(GameManager.instance == null) return;
        if(GameManager.instance.PlayerManager == null) return;
        if (GameManager.instance.PlayerManager.PlayerObject != gameObject)
            GameManager.instance.PlayerManager.SetPlayer(gameObject);
    }

    private void Start()
    {
        transform.position = GameManager.instance.PlayerSpawnPoint.position;
        _health = _maxHealth;
        _playerManager = GameManager.instance.PlayerManager;
    }


    //solo para debug

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(10);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal(10);
        }
    }


    public void TakeDamage(float damage)
    {
        _health -= damage;
        if(_health <= 0)
        {
            if(_playerManager.Lifes>0)
            {
                _playerManager.Lifes--;
                _playerManager.Health.Heal(_maxHealth);
                GameManager.instance.LoadCheckpoint();
            }
            else
            {
                GameManager.instance.LoadLoseScene();
            }
        }
    }
    public void Heal(float health)
    {
        _health += health;
        _health = Mathf.Clamp(_health, 0, _maxHealth);
    }

}
