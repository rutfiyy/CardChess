using UnityEngine;
using TMPro;

public class MainMenuUIManager : MonoBehaviour
{
    public TMP_InputField usernameField;
    public TMP_InputField passwordField;
    public AuthManager authManager;

    public void OnRegisterClicked()
    {
        string username = usernameField.text;
        string password = passwordField.text;
        authManager.Register(username, password);
    }
}
