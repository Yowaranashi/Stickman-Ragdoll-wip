using UnityEngine;
using UnityEngine.UI;

public class AuthUI : MonoBehaviour
{
    [SerializeField] private AuthManager _authManager;
    [SerializeField] private InputField _loginField;
    [SerializeField] private InputField _passwordField;
    [SerializeField] private GameObject _registerPanel;

    public void OnLoginButton()
    {
        if (!_authManager.Login(_loginField.text, _passwordField.text))
        {
            Debug.Log("Invalid login or password");
        }
    }

    public void ToggleRegisterPanel()
    {
        if (_registerPanel != null)
            _registerPanel.SetActive(!_registerPanel.activeSelf);
    }

    public void OnRegister(InputField username, InputField password)
    {
        _authManager.Register(username.text, password.text);
        ToggleRegisterPanel();
    }
}
