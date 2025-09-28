using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string _speedParameterName;
    [SerializeField] private string _crouchParameterName;
    [SerializeField] private string _crawlParameterName;
    [SerializeField] private string _attackTriggerName;
    [SerializeField] private string _granadeTriggerName;

    private PlayerManager _playerManager;

    private void Start()
    {
        _playerManager = GameManager.instance.PlayerManager;
    }

    public void ChangePlayerSpeed(float newValue)
    {
        _animator.SetFloat(_speedParameterName,Mathf.Clamp(newValue,0,2));
    }


    public void ChangeStanceValue(int stanceStep)
    {
        switch(stanceStep)
        {
            case 0:
                _animator.SetBool(_crouchParameterName, false);
                _animator.SetBool(_crawlParameterName, false);
                break;
            case 1:
                _animator.SetBool(_crouchParameterName, true);
                _animator.SetBool(_crawlParameterName, false);
                break;
            case 2:
                _animator.SetBool(_crawlParameterName, true);
                break;

        }
    }

    public void ChangeAnimationSpeed(float newValue)
    {
        _animator.speed = newValue;

    }

    public void InvertAnimation()
    {
        _animator.speed *= -1;
    }
    public void SetAttackTrigger()
    {
        ChangeStanceValue(0);
        _animator.SetTrigger(_attackTriggerName);
    }

    public void BeginShooting()
    {
        _playerManager.Attack.Attack();
    }
    public void EndedShooting()
    {
        _playerManager.Movement.enabled = true;
        _playerManager.Attack.IsAttacking = false;
        _playerManager.GFX.transform.Rotate(0,90,0);
        _playerManager.GunGFX.SetActive(false);
    }
    public void SetGranadeTrigger()
    {
        _animator.SetTrigger(_granadeTriggerName);
    }

    public void BeginStandingUp()
    {
        _playerManager.Movement.enabled = false;
        _animator.speed = 1;
    }

    public void EndedStandedUp()
    {
        _playerManager.Movement.enabled = true;
    } 


}
