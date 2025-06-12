using UnityEngine;

public class UserSessionManager : MonoBehaviour
{
    public static UserSessionManager Instance { get; private set; }

    public string Username { get; private set; }

    private void Awake()
    {
        // Make this GameObject persist across scenes
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetUsername(string username)
    {
        Username = username;
    }
}
