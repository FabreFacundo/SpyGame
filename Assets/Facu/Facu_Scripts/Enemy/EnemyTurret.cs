using UnityEngine;

public class EnemyTurret : Enemy
{
    [SerializeField] Transform _shootPosition;
    [SerializeField] float _shootDistance = 10f;
    [SerializeField] float _damage = 5f;
    [SerializeField] ParticleSystem _muzzleFlash;

    [Header("Rates in shots per second")]
    [SerializeField][Range(1,300)] float _attackRate = 2f;
    

    private PlayerManager _playerManager;
    private bool _isAttacking;
    private LayerMask _playerLayer;
    private int _shotsFired;
    private float _burstRate;
    private bool _cooldown;
    private float _attackRateValue;


    protected override void Start()
    {
        base.Start();
        _playerManager = GameManager.instance.PlayerManager;   
        _playerLayer = _playerManager.PlayerLayer;
        _attackRateValue = 1f / _attackRate;
        _burstRate = _attackRate / 2;
    }
    public override void Attack()
    {
        if (!_isAttacking)
        {
            _isAttacking = true;
           InvokeRepeating(nameof(Burst), 0f, _burstRate);
        }
       
    }
    private void Shoot()
    {
        _muzzleFlash.Emit(1);
        if (Physics.Raycast(_shootPosition.position, _shootPosition.forward, out RaycastHit _hit, _shootDistance, _playerLayer))
        {
            _playerManager.Health.TakeDamage(_damage);
        }
        _shotsFired++;
    }
    private void Burst()
    {
        if (_cooldown) return;
        if(_agent.ActualState != Enemy_agent.ENEMY_STATE.ATTACKING)
        {
            _isAttacking = false;
            CancelInvoke(nameof(Shoot));
            CancelInvoke(nameof(Burst));
            return;
        }

        if (_shotsFired >= _attackRate)
        {

            CancelInvoke(nameof(Shoot));
            Invoke(nameof(ResetAttack), _burstRate);
            _cooldown = true;
            return;
        }
        InvokeRepeating(nameof(Shoot), 0,_attackRateValue);


    }
    private void ResetAttack()
    {
        _shotsFired = 0;
        _cooldown = false;
    }

    public override void Neutralize()
    {
        base.Neutralize();
        transform.rotation=Quaternion.LookRotation(Vector3.down);
    }



}
