using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectSkinButton : MonoBehaviour
{
    [SerializeField] private Button _adButton, _buyButton;
    [SerializeField] private RewardAd _rewardAd;
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private Image _characterIcon, _closeIcon, _selectIcon;
    [SerializeField] private Sprite _chosenLayout, _notChosenLayout;
    private Image _backgroundImage;
    private Shop _shop;
    private CharacterSkin _skin;
    private void OnDisable()
    {
        _rewardAd.RewardGet -= OpenSkinForAd;
    }
    public void Init(CharacterSkin skin, Shop shopPanel)
    {
        _shop = shopPanel;
        _skin = skin;
        _rewardAd.RewardGet += OpenSkinForAd;
        _backgroundImage = GetComponent<Image>();
        _characterIcon.sprite = skin.Head;
        if (!_skin.Opened)
        {
            _closeIcon.gameObject.SetActive(true);
            if (_skin.OpenForAd)
            {
                _adButton.gameObject.SetActive(true);
            }
            else
            {
                _buyButton.gameObject.SetActive(true);
                _costText.text = _skin.Cost.ToString();
            }
            return;
        }
        if(_skin.Opened && _skin.Chosen)
        {
            _backgroundImage.sprite = _chosenLayout;
            _selectIcon.gameObject.SetActive(true);
        }
        else
        {
            _backgroundImage.sprite = _notChosenLayout;
        }
    }
    public void OpenSkin()
    {
        if (!_shop.CanBuySkin(_skin.Cost)) return;
        _skin.Opened = true;
        _closeIcon.gameObject.SetActive(false);
        _adButton.gameObject.SetActive(false);  
        _buyButton.gameObject.SetActive(false);
        _shop.Save();
    }
    public void SelectSkin()
    {
        if(_skin.Chosen || !_skin.Opened)
        {
            return;
        }
        _shop.ChangeSkin(_skin);
        _skin.Chosen = true;
        _backgroundImage.sprite = _chosenLayout;
        _selectIcon.gameObject.SetActive(true);
        _shop.Save();
    }
    public void DisableSkin()
    {
        _skin.Chosen = false;
        _backgroundImage.sprite = _notChosenLayout;
        _selectIcon.gameObject.SetActive(false);
    }
    private void OpenSkinForAd(bool opened)
    {
        if(opened)
        {
            print("SKIN OPENED");
        }
        _skin.Opened = true;
        _closeIcon.gameObject.SetActive(false);
        _adButton.gameObject.SetActive(false);
        _buyButton.gameObject.SetActive(false);
        _shop.Save();
    }
}
