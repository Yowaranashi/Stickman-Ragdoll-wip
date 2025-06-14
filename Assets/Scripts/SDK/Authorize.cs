using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SDK
{
    public class Authorize : MonoBehaviour
    {
        private static bool Authorized;
        [Header("All about Authorization")]
        [SerializeField] private GameObject _authorizationPanel;
        [SerializeField] private TextMeshProUGUI _playerNameText, _playerIdText;
        [SerializeField] private Image _playerIcon;
        [SerializeField] private Button _authorizationButton;

        private void Start()
        {
            if (Authorized)
                SetUIOnAuthorized();
        }

        public void Authorization()
        {
            SetUIOnAuthorized();
        }

        private void SetUIOnAuthorized()
        {
            Authorized = true;
            if (_authorizationPanel != null)
                _authorizationPanel.SetActive(true);
            if (_playerNameText != null)
            {
                if (DatabaseManager.Instance != null && !string.IsNullOrEmpty(DatabaseManager.Instance.CurrentUsername))
                    _playerNameText.text = DatabaseManager.Instance.CurrentUsername;
                else
                    _playerNameText.text = "Player";
            }
            if (_playerIdText != null)
            {
                if (DatabaseManager.Instance != null)
                    _playerIdText.text = DatabaseManager.Instance.CurrentUserId.ToString();
                else
                    _playerIdText.text = "0";
            }
            if (_authorizationButton != null)
                _authorizationButton.interactable = false;
        }
    }
}
