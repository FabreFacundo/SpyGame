using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class ControlMenu : MonoBehaviour
{
    [SerializeField] private Toggle _mouseXInvert;
    [SerializeField] private Toggle _mouseYInvert;
    [SerializeField] private Toggle _movementXInvert;
    [SerializeField] private Toggle _movementYInvert;
    [SerializeField] private Slider _mouseXSensirivity;
    [SerializeField] private Slider _mouseYSensirivity;


    private PlayerInputs _inputs;

    

    
    private void Start()
    {
         _inputs = GameManager.instance.Inputs;

        _mouseXInvert.isOn = _inputs.IsMouseXAxisInverted;
        _mouseYInvert.isOn = _inputs.IsMouseYAxisInverted;
        _movementXInvert.isOn = _inputs.IsMouseXAxisInverted;
        _movementYInvert.isOn = _inputs.IsMouseXAxisInverted;
        _mouseXSensirivity.minValue = _inputs.MinSensitivity;
        _mouseYSensirivity.minValue = _inputs.MinSensitivity;
        _mouseXSensirivity.value = _inputs.MouseHorizontalSensitivity;
        _mouseXSensirivity.maxValue = _inputs.MaxSensitivity;
        _mouseYSensirivity.maxValue = _inputs.MaxSensitivity;
        _mouseYSensirivity.value = _inputs.MouseVerticalSensitivity;
        _mouseYSensirivity.minValue = _inputs.MinSensitivity;
        _mouseXSensirivity.onValueChanged.AddListener(UpdateXSensitivity);
        _mouseYSensirivity.onValueChanged.AddListener(UpdateYSensitivity);
        _mouseXInvert.onValueChanged.AddListener(UpdateMouseXinvert);
        _mouseYInvert.onValueChanged.AddListener(UpdateMouseYinvert);
        _movementXInvert.onValueChanged.AddListener(UpdateMouvementXinvert);
        _movementYInvert.onValueChanged.AddListener(UpdateMouvementYinvert);
    }

    public void UpdateXSensitivity(float newValue)
    {
        _inputs.MouseHorizontalSensitivity = newValue;
    }
    public void UpdateYSensitivity(float newValue)
    {
        _inputs.MouseVerticalSensitivity = newValue;
    }
    public void UpdateMouseXinvert(bool newValue)
    {
        _inputs.IsMouseXAxisInverted = newValue;
    }
    public void UpdateMouseYinvert(bool newValue)
    {
        _inputs.IsMouseYAxisInverted = newValue;
    }
    public void UpdateMouvementXinvert(bool newValue)
    {
        _inputs.IsXAxisInverted = newValue;
    }
    public void UpdateMouvementYinvert(bool newValue)
    {
        _inputs.IsYAxisInverted = newValue;
    }

    private void OnDisable()
    {
        UIManager manager = GameManager.instance.UIManager;
        manager.ActiveMenu(manager.InGameMenu);
    }
    private void OnDestroy()
    {
        _mouseXSensirivity.onValueChanged.RemoveListener(UpdateXSensitivity);
        _mouseYSensirivity.onValueChanged.RemoveListener(UpdateYSensitivity);
        _mouseXInvert.onValueChanged.RemoveListener(UpdateMouseXinvert);
        _mouseYInvert.onValueChanged.RemoveListener(UpdateMouseYinvert);
        _movementXInvert.onValueChanged.RemoveListener(UpdateMouvementXinvert);
        _movementYInvert.onValueChanged.RemoveListener(UpdateMouvementYinvert);

    }
}
