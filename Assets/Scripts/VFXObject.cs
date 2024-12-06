using System;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class VFXObject : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    private Action<VFXObject> _action;

    public void SetCallbackAction(Action<VFXObject> action)
    {
        _action = action;
    }

    public void Play()
    {
        _particleSystem.Play();
    }

    private void OnParticleSystemStopped()
    {
        _action(this);
    }
}
