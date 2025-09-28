using UnityEngine;

public class SmokeGranade : MonoBehaviour
{
    [SerializeField] private ParticleSystem _smokeParticles;
    [SerializeField] private float _explodeDelay;
    [SerializeField] private float _disipationDelay;
    [SerializeField] private Collider _obstacle;

    private Collider _collider;
    private Rigidbody _rb;
    private bool _exploded;
    public void Trhow()
    {
        _collider = GetComponent<Collider>();
        _rb = GetComponent<Rigidbody>();
        Invoke(nameof(Explode),_explodeDelay);
    }
    private void Update()
    {
        if (_exploded)
            if (!_smokeParticles.isPlaying)
                StopSmoke();
    }
    private void Explode()
    {
        _obstacle.enabled = true;
        _smokeParticles.Play();
        _rb.isKinematic = true;
        _collider.isTrigger=true;
        _exploded = true;
    }

    private void StopSmoke()
    {
        Destroy(gameObject);
    }

}
