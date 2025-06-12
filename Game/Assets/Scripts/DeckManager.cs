using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class DeckManager : MonoBehaviour
{
    string baseUrl = "http://localhost:8080/api/decks";

    [System.Serializable]
    public class DeckDto
    {
        public long id;
        public long userId;
        public string cardList;
        public string deckName;
    }

    public void LoadDecks(long userId)
    {
        StartCoroutine(LoadDecksRequest(userId));
    }

    IEnumerator LoadDecksRequest(long userId)
    {
        UnityWebRequest request = UnityWebRequest.Get($"{baseUrl}/user/{userId}");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = "{\"decks\":" + request.downloadHandler.text + "}";
            DeckListWrapper wrapper = JsonUtility.FromJson<DeckListWrapper>(json);
            Debug.Log("✅ Decks Loaded: " + request.downloadHandler.text);
            // Use wrapper.decks as your deck list
        }
        else
        {
            Debug.LogError("❌ Load Decks Failed: " + request.error);
        }
    }

    [System.Serializable]
    public class DeckListWrapper
    {
        public List<DeckDto> decks;
    }
}