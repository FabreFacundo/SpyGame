using UnityEngine;

public class PlayerManager : MonoBehaviour
{


    #region INSPECTOR_ATTRIBUTES
    [SerializeField] private GameObject _playerObject;
    [SerializeField] private int _maxLifes = 3;
    [SerializeField] private string _hideLayerName = "Invisible";
    #endregion

    #region INTERNAL_ATTRIBUTES
    private int _playerLayer;
    private string _playerTag = "Player";
    private Renderer[] _playerVisualRenderers;
    private GameObject _GFX;
    private GameObject _gunGFX;
    private PlayerMovement _movement;
    private PlayerStealth _stelth;
    private Player _player;
    private PlayerAttack _attack;
    private PlayerStamina _stamina;
    private ShoulderCam _cameraMovement;
    private PlayerAnimation _Animation;
    private Rigidbody _rigidBody;
    private Collider _activeCollider;

    private int _currentLifes;
    #endregion

    #region PROPERTIES
    public int PlayerLayer => _playerLayer;
    public string PlayerTag => _playerTag;
    public Renderer[] Renderers => _playerVisualRenderers;
    public Collider ActiveCollider { get { return _activeCollider; } set { _activeCollider = value; } }
    public Rigidbody Rigid_Body => _rigidBody;
    public GameObject PlayerObject => _playerObject;
    public int MaxLifes => _maxLifes;
    public int Lifes { get { return _currentLifes; } set { _currentLifes = value; } }
    public GameObject GunGFX => _gunGFX;
    public GameObject GFX => _GFX;
    public PlayerMovement Movement => _movement;
    public PlayerStealth Stelth => _stelth;
    public PlayerAttack Attack => _attack;
    public PlayerStamina Stamina => _stamina;
    public Player Health => _player;
    public ShoulderCam CameraMovement => _cameraMovement;
    public PlayerAnimation Animation => _Animation;
    public string HideLayerName => _hideLayerName;
    #endregion
    private void Awake()
    {
        if(PlayerObject != null)
            SetPlayer(PlayerObject);
        _currentLifes = _maxLifes;
    }
    public void SetPlayer(GameObject playerObject)
    {
        _playerObject = playerObject;
        _movement = _playerObject.GetComponent<PlayerMovement>();
        _stelth = _playerObject.GetComponent<PlayerStealth>();
        _Animation = _playerObject.GetComponentInChildren<PlayerAnimation>();
        //_cameraMovement = Camera.main.GetComponent<cameraMovement>();
        _attack = _playerObject.GetComponent<PlayerAttack>();
        _stamina = _playerObject.GetComponent<PlayerStamina>();
        _player = _playerObject.GetComponent<Player>();
        _rigidBody = _playerObject.GetComponent<Rigidbody>();
        _GFX = _player.GFX;
        _playerLayer = _GFX.layer;
        _playerTag = _GFX.tag;
        _playerVisualRenderers = _GFX.GetComponentsInChildren<Renderer>();
        _gunGFX = _player.GunGFX;

    }

    public bool CompareLayerMask(int layer)
    {
        return (1 << layer & _playerLayer) != 0;
    }
    public bool CompareLayer(int layer)
    {
        return _playerLayer == layer;
    }

}
