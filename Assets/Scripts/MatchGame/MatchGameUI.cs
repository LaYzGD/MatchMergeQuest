using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MatchGameUI : MonoBehaviour
{
    [SerializeField] private GameObject _rewardPanel;
    [SerializeField] private TextMeshProUGUI _xpText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private Slider _xpBar;

    private LevelUpData _levelUpData;
    private MatchSystem _matchSystem;

    private int _maxValue;

    [Inject]
    public void Construct(LevelUpData data, MatchSystem matchSystem) 
    {
        _levelUpData = data;
        _matchSystem = matchSystem;
        _xpBar.interactable = false;
        _xpBar.wholeNumbers = true;
        _maxValue = _levelUpData.StartRequiredXP;
        _xpBar.maxValue = _maxValue;
        _levelText.text = _levelUpData.StartingLevel.ToString();
        UpdateXpBarCurrentValue(0);
    }

    public void LevelUp(int level)
    {
        _levelText.text = level.ToString();
        _maxValue += _levelUpData.GetNextStep(level);
        _xpBar.maxValue = _maxValue; 
        ShowRewardPanel(true);
    }

    public void ShowRewardPanel(bool flag) 
    {
        _rewardPanel.SetActive(flag);
        _matchSystem.IsRewardPanelShown = flag;
    }

    public void UpdateXpBarCurrentValue(int xp)
    {
        _xpBar.value = xp;
        _xpText.text = $"{xp}/{_maxValue}";
    }
}
