using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Collections;

public class AuthManager : MonoBehaviour
{
    string baseUrl = "http://localhost:8080/api/auth";

    public void Register(string username, string password)
    {
        StartCoroutine(RegisterRequest(username, password));
    }
    public void Login(string username, string password)
    {
        StartCoroutine(LoginRequest(username, password));
    }

    IEnumerator RegisterRequest(string username, string password)
    {
        string json = JsonUtility.ToJson(new RegisterData { username = username, password = password });
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest($"{baseUrl}/register", "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("✅ Register Success: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("❌ Register Failed: " + request.error);
            Debug.LogError("Status Code: " + request.responseCode);
        }
    }

    IEnumerator LoginRequest(string username, string password)
    {
        string json = JsonUtility.ToJson(new RegisterData { username = username, password = password });
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest($"{baseUrl}/login", "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            UserSessionManager.Instance.SetUsername(username);
            Debug.Log("✅ Login Success: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("❌ Login Failed: " + request.error);
            Debug.LogError("Status Code: " + request.responseCode);
        }
    }

    [System.Serializable]
    public class RegisterData
    {
        public string username;
        public string password;
    }
}
