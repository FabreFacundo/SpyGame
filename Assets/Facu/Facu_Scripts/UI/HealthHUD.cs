using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthHUD : MonoBehaviour
{
    [Header("Health HUD References")]
    [SerializeField] private Slider _healthValue;
    

    private PlayerManager _playerManager;
    private TMP_Text _lifesCounter;


    private void Start()
    {
        _lifesCounter = GetComponentInChildren<TMP_Text>();
        _playerManager = GameManager.instance.PlayerManager;
        // setea los valores maximos de las barras de stamina
        _healthValue.maxValue = _playerManager.Health.MaxHealth;


    }


    private void Update()
    {
        _lifesCounter.text ="lifes remaining= " + _playerManager.Lifes.ToString();
        _healthValue.value = _playerManager.Health.HealthValue;
    }


}
