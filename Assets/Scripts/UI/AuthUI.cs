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
        if (DatabaseManager.Instance == null)
            new GameObject("Database").AddComponent<DatabaseManager>();
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
        if (DatabaseManager.Instance.Login(username, password))
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            Debug.LogError("Login failed");
        }
    }

    public void OnRegisterClick()
    {
        string username = usernameReg.text;
        string password = passReg.text;
        if (DatabaseManager.Instance.Register(username, password))
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            Debug.LogError("Registration failed");
        }
    }
}