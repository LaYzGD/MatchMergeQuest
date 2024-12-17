using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    [Header("Data")]
    [SerializeField] private LevelUpData _levelUpData;
    [SerializeField] private MatchSystemData _matchSystemData;
    [SerializeField] private MatchAudioData _matchAudioData;
    [SerializeField] private InventorySlotData _inventorySlotData;
    [Space]
    [Header("Logic")]
    [SerializeField] private MatchSystem _matchSystem;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private MatchComboUI _comboUI;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private AudioPlayer _audioPlayer;
    [SerializeField] private Points _points;
    public override void InstallBindings()
    {
        InstallData();
        InstallMonoBeh();
    }

    private void InstallData()
    {
        Container.BindInterfacesAndSelfTo<LevelUpData>().FromInstance(_levelUpData).AsSingle();
        Container.BindInterfacesAndSelfTo<MatchSystemData>().FromInstance(_matchSystemData).AsSingle();
        Container.BindInterfacesAndSelfTo<MatchAudioData>().FromInstance(_matchAudioData).AsSingle();
        Container.BindInterfacesAndSelfTo<InventorySlotData>().FromInstance(_inventorySlotData).AsSingle();
    }

    private void InstallMonoBeh()
    {
        Container.BindInterfacesAndSelfTo<MatchSystem>().FromInstance(_matchSystem).AsSingle();
        Container.BindInterfacesAndSelfTo<Camera>().FromInstance(_mainCamera).AsSingle();
        Container.BindInterfacesAndSelfTo<MatchComboUI>().FromInstance(_comboUI).AsSingle();
        Container.BindInterfacesAndSelfTo<InputReader>().FromInstance(_inputReader).AsSingle();
        Container.BindInterfacesAndSelfTo<AudioPlayer>().FromInstance(_audioPlayer).AsSingle();
        Container.BindInterfacesAndSelfTo<Points>().FromInstance(_points).AsSingle();
    }
}
