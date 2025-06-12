using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomInput;
    public TMP_Text statusText;
    public TMP_Text usernameDisplayText;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        statusText.text = "Connecting to server...";

        if (UserSessionManager.Instance != null && !string.IsNullOrEmpty(UserSessionManager.Instance.Username))
        {
            usernameDisplayText.text = $"Logged in as: {UserSessionManager.Instance.Username}";
        }
        else
        {
            usernameDisplayText.text = "Not logged in";
        }
    }

    public override void OnConnectedToMaster()
    {
        statusText.text = "Connected. Enter room name.";
        PhotonNetwork.AutomaticallySyncScene = true;

        if (UserSessionManager.Instance != null)
        {
            PhotonNetwork.NickName = UserSessionManager.Instance.Username;
        }
    }

    public void CreateRoom()
    {
        if (roomInput.text != "")
        {
            PhotonNetwork.CreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 2 });
            statusText.text = "Creating room...";
        }
    }

    public void JoinRoom()
    {
        if (roomInput.text != "")
        {
            PhotonNetwork.JoinRoom(roomInput.text);
            statusText.text = "Joining room...";
        }
    }

    public void CancelAndReturnToMain()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom(); // will trigger OnLeftRoom → go to main menu
        }
        else
        {
            SceneManager.LoadScene("MainMenu"); // replace with your main menu scene name
        }
    }

    public override void OnJoinedRoom()
    {
        statusText.text = "Joined room. Waiting for other player...";
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            PhotonNetwork.LoadLevel("Game");
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            PhotonNetwork.LoadLevel("Game");
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        statusText.text = $"Failed to join: {message}";
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("MainMenu"); // navigate back when leaving room
    }
}
