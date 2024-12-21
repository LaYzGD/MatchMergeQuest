using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MatchGameUI : MonoBehaviour
{
    [SerializeField] private GameObject _rewardPanel;
    [SerializeField] private TextMeshProUGUI _xpText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _rewardText;
    [SerializeField] private Slider _xpBar;
    [SerializeField] private Button _rewardButton;

    private LevelUpData _levelUpData;
    private MatchSystem _matchSystem;

    private int _maxValue;
    private int _rewardAmount;

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
        _rewardText.gameObject.SetActive(false);
        SetRewardButtonInteractable(false);
        UpdateXpBarCurrentValue(0);
    }

    public void LevelUp(int level)
    {
        _levelText.text = level.ToString();
        _maxValue += _levelUpData.GetNextStep(level);
        _xpBar.maxValue = _maxValue;

        _rewardAmount++;
        _rewardText.gameObject.SetActive(true);
        _rewardText.text = $"{_rewardAmount}";

        SetRewardButtonInteractable(true);
    }

    public void RemoveReward()
    {
        _rewardAmount--;

        if (_rewardAmount <= 0)
        {
            _rewardAmount = 0;
            SetRewardButtonInteractable(false);
            ShowRewardPanel(false);
        }

        _rewardText.text = $"{_rewardAmount}";
    }

    public void ShowRewardPanel(bool flag) 
    {
        _rewardPanel.SetActive(flag);
        _matchSystem.IsUIPanelShown = flag;
    }

    public void UpdateXpBarCurrentValue(int xp)
    {
        _xpBar.value = xp;
        _xpText.text = $"{xp}/{_maxValue}";
    }

    private void SetRewardButtonInteractable(bool flag)
    {
        _rewardButton.interactable = flag;
    }
}
