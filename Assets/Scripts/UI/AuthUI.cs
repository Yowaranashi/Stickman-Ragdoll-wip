using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class AuthUI : MonoBehaviour
{
    [Header("Panels")]
    public GameObject loginPanel;
    public GameObject registerPanel;

    [Header("Login Inputs")]
    public TMP_InputField usernameLogin;
    public TMP_InputField passLogin;

    [Header("Register Inputs")]
    public TMP_InputField usernameReg;
    public TMP_InputField passReg;

    void Start()
    {
        ShowLogin();
    }

    public void ShowLogin()
    {
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
    }
    public void ShowRegister()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
    }

    public void OnLoginClick()
    {
        string username = usernameLogin.text;
        string password = passLogin.text;
        /*AuthManager.Instance.Login(username, password, success => {
            if (success) SceneManager.LoadScene("MainScene");
            else Debug.LogError("Login failed");
        });*/
    }

    public void OnRegisterClick()
    {
        string username = usernameReg.text;
        string password = passReg.text;
        //AuthManager.Instance.Register(username, password, success => {
        //    if (success) SceneManager.LoadScene("MainScene");
        //    else Debug.LogError("Registration failed");
        //});
    }
}