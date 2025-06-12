using UnityEngine;

public class UserSessionManager : MonoBehaviour
{
    public static UserSessionManager Instance { get; private set; }

    public long UserId { get; private set; }
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

    public void SetUserId(long userId)
    {
        UserId = userId;
        Debug.Log($"User ID set to: {UserId}");
    }
    public void SetUsername(string username)
    {
        Username = username;
        Debug.Log($"Username set to: {Username}");
    }
}
