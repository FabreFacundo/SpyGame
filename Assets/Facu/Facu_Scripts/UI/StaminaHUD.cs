using UnityEngine;
using UnityEngine.UI;   

public class StaminaHUD : MonoBehaviour
{
    [Header("Stamina HUD References")]
    [SerializeField] private Slider _staminaValue;
    [SerializeField] private Slider _staminaDepleted;

    private PlayerManager _playerManager;

    
    private void Start()
    {
        _playerManager = GameManager.instance.PlayerManager;
        // setea los valores maximos de las barras de stamina
        _staminaValue.maxValue = _playerManager.Stamina.MaxStamina;
        _staminaDepleted.maxValue = _playerManager.Stamina.MaxStamina - _playerManager.Stamina.MinStamina;
    }

    
    private void Update()
    {
        _staminaValue.value = _playerManager.Stamina.ActualStaminaValue;
        _staminaDepleted.value = _playerManager.Stamina.MaxStamina - _playerManager.Stamina.availableStaminaValue;
    }



}
