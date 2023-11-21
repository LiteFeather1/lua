using UnityEngine;

public class FireListener : ParticleStoppedCallBack
{
    [SerializeField] private Health _health;

    private void Awake()
    {
        _health.OnDamageEffectApplied += OnEffectApplied;
        _health.OnDamageEffectRemoved += OnEffectRemoved;

        _health.OnDeath += Died;
    }

    private void OnDestroy()
    {
        _health.OnDamageEffectApplied -= OnEffectApplied;
        _health.OnDamageEffectRemoved -= OnEffectRemoved;

        _health.OnDeath -= Died;
    }

    private void OnEffectApplied(int effectId)
    {
        if (effectId != (int)IDamageEffect.DamageEffectID.FIRE_ID)
            return;

        _ps.Play();
    }

    private void OnEffectRemoved(int effectId)
    {
        if (effectId != (int)IDamageEffect.DamageEffectID.FIRE_ID)
            return;

        _ps.Stop();
    }

    public void Died() => _ps.Stop();
}
