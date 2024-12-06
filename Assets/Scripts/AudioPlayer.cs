using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _minPitch = 0.9f;
    [SerializeField] private float _maxPitch = 1.2f;

    private float _defaultPitch;

    private void Awake()
    {
        _defaultPitch = _audioSource.pitch;
    }

    public void PlaySound(AudioClip clip)
    {
        _audioSource.pitch = Random.Range(_minPitch, _maxPitch);
        _audioSource.PlayOneShot(clip);
        _audioSource.pitch = _defaultPitch;
    }
}
