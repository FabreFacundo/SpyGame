using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    #region INSPECTOR_ATTRIBUTES
    [Header("Stamina Attributes")]
    [SerializeField] private float _staminaCost = 1f;
    [SerializeField] private float _maxStamina = 10f;
    [SerializeField] private float _minStamina = 3f;
    [SerializeField] private float _maxStaminaDrainFactor = 0.5f;
    [SerializeField] private float _staminaRecoveryFactor = 0.5f;
    [SerializeField] private float _tiredCooldown = 1f;

    [Header("Energy Drink item")]
    [SerializeField] private Item _energyDrinkItem;
    #endregion
    #region INTERNAL_ATTRIBUTES
    private PlayerManager _playerManager;
    private PlayerInputs _inputs;
    private bool _isCoolingDown;
    private float _availableStamina;
    private float _actualStamina;
    private Inventory _inventory;
    #endregion
    #region PROPERTIES

    public float MaxStamina { get { return _maxStamina; } }
    public float MinStamina { get { return _minStamina; } }
    public float ActualStaminaValue { get { return _actualStamina; } }
    public float availableStaminaValue { get { return _availableStamina; } }


    protected float AvailableStamina // esta propieda permite modificar los valores y clampearlos al maximo y al minimo
    {
        get { return _availableStamina; }
        set { _availableStamina = Mathf.Clamp(_availableStamina + value, _minStamina, _maxStamina); }
    }
    protected float ActualStamina // esta propieda permite modificar los valores y clampearlos al maximo y al minimo
    {
        get { return _actualStamina; }
        set { _actualStamina = Mathf.Clamp(_actualStamina + value, 0, _availableStamina); }
    }
    #endregion
    private void Start()
    {
        _inventory = GameManager.instance.Inventory;
        _availableStamina = _maxStamina;
        _actualStamina = _availableStamina;
        _isCoolingDown = false;
        _playerManager = GameManager.instance.PlayerManager;
        _inputs = GameManager.instance.Inputs;

    }

    private void Update()
    {
        // Recupera stamina con el tiempo
        if (ActualStamina < AvailableStamina)
            ActualStamina = _staminaRecoveryFactor * Time.deltaTime;

        if (_inputs.IsConsumeClicked) // Usa una bebida energetica
        {
            if (_inventory.Items.ContainsKey(_energyDrinkItem))
            { 
                if (_inventory.Items[_energyDrinkItem] > 0)
                { 
                    RestoreAvailableStamina(_maxStamina);
                    _actualStamina = _availableStamina;
                    _inventory.RemoveItem(_energyDrinkItem);
                    _playerManager.Movement.HaveStamina = true;
                }
            }

        }

    }
    public void DrainStamina(float Time)
    {
        // Si esta en cooldown no puede gastar stamina
        if (_isCoolingDown) return;

        // Gasta stamina y si se queda sin stamina deshabilita el movimiento por un tiempo
        ActualStamina = -_staminaCost * Time;
        if (_actualStamina <= 0)
        {
            _playerManager.Movement.enabled = false;
            //Animacion de cansansio
            
            Invoke("RestoreMovement", _tiredCooldown); // restaura el movimiento despues de un tiempo
        }
        AvailableStamina = - _maxStaminaDrainFactor * Time; // gasta stamina disponible 
        if (_availableStamina <= _minStamina)
        {
            _playerManager.Movement.HaveStamina = false; // avisa al player movement que no tiene stamina disponible
        }
      

    }

    // Restaura la stamina disponible a un valor especifico (bebidas energeticas)
    public void RestoreAvailableStamina(float stamina)
    {
        _availableStamina = stamina;
    }

    // Restaura el movimiento del jugador y saca el cooldown
    private void RestoreMovement()
    {
        _playerManager.Movement.enabled = true;
        _isCoolingDown = false;
    }
    

}
