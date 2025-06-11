using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FinishPanel : MonoBehaviour
{
    [SerializeField] private SkinsDatabase _database;
    [SerializeField] private Button _rewardButton;
    [SerializeField] private RewardAd _reward;
    [SerializeField] private TextMeshProUGUI _rewardText;
    [SerializeField] private SaveManager _saveManager;
    private void OnEnable()
    {
        _reward.RewardGet += GetReward;
    }
    private void OnDisable()
    {
        _reward.RewardGet -= GetReward; 
    }
    private void GetReward(bool reward)
    {
        print("REWARD GET");
        _rewardButton.enabled = false;
        _rewardButton.GetComponent<Image>().color = new Color(1,1,1,.5f);
        _database.Money += 500;
        _rewardText.text = "600";
        _saveManager.SaveMoney();
    }
}
