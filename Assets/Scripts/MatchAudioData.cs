using UnityEngine;

[CreateAssetMenu(menuName = "Data/MatchAudioData", fileName = "New MatchAudioData")]
public class MatchAudioData : ScriptableObject
{
    [field: SerializeField] public AudioClip PopSound { get; private set; }
    [field: SerializeField] public AudioClip CreateSound { get; private set; }
    [field: SerializeField] public AudioClip SelectSound { get; private set; }
    //[field: SerializeField] public AudioClip SwapSound { get; private set; }
    [field: SerializeField] public AudioClip ExplodeSound { get; private set; }
}
