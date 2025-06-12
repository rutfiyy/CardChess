using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class MatchManager : MonoBehaviour
{
    string baseUrl = "http://localhost:8080/api/match-history";

    [System.Serializable]
    public class MatchHistoryDto
    {
        public long player1Id;
        public long player2Id;
        public string result; // e.g. "player1_win", "player2_win", "draw"
        public string playedAt; // ISO 8601 string
    }

    public void SendMatchResult(long player1Id, long player2Id, string result)
    {
        MatchHistoryDto match = new MatchHistoryDto
        {
            player1Id = player1Id,
            player2Id = player2Id,
            result = result,
            playedAt = System.DateTime.UtcNow.ToString("o")
        };
        StartCoroutine(SendMatchResultRequest(match));
    }

    IEnumerator SendMatchResultRequest(MatchHistoryDto match)
    {
        string json = JsonUtility.ToJson(match);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest($"{baseUrl}", "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("✅ Match Result Sent: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("❌ Send Match Result Failed: " + request.error);
        }
    }
}