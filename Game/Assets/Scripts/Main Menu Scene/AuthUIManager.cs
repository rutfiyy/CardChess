using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AuthUIManager : MonoBehaviour
{
    public GameObject authPanel;
    public TextMeshProUGUI titleText;
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public Button submitButton;
    public Button closeButton;

    public AuthManager authManager;

    private bool isLoginMode = true;

    void Start()
    {
        authPanel.SetActive(false);
        closeButton.onClick.AddListener(() => authPanel.SetActive(false));
    }

    public void ShowLogin()
    {
        isLoginMode = true;
        authPanel.SetActive(true);
        titleText.text = "Login";
        submitButton.onClick.RemoveAllListeners();
        submitButton.onClick.AddListener(() =>
        {
            authManager.Login(usernameInput.text, passwordInput.text);
            authPanel.SetActive(false);
        });
    }

    public void ShowRegister()
    {
        isLoginMode = false;
        authPanel.SetActive(true);
        titleText.text = "Register";
        submitButton.onClick.RemoveAllListeners();
        submitButton.onClick.AddListener(() =>
        {
            authManager.Register(usernameInput.text, passwordInput.text);
            authPanel.SetActive(false);
        });
    }
}
