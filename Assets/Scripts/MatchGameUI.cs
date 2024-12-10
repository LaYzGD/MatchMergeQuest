using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchGameUI : MonoBehaviour
{
    [SerializeField] private GameObject _rewardPanel;
    [SerializeField] private TextMeshProUGUI _xpText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private Slider _xpBar;

    private void Start()
    {
        _xpBar.interactable = false;
    }
}
