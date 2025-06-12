using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    public enum Side { White, Black }
    public Side LocalSide { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        AssignPlayerSides();
    }

    private void AssignPlayerSides()
    {
        // MasterClient is always white, second player is black
        if (PhotonNetwork.IsMasterClient)
            LocalSide = Side.White;
        else
            LocalSide = Side.Black;
    }

    public bool IsLocalPlayersTurn()
    {
        // Use GameController's turn info
        return GameController.Instance.CurrentTurn == LocalSide;
    }

    // Add this method for multiplayer piece control
    public bool CanControlPiece(Piece piece)
    {
        // Only allow the local player to control their own pieces on their turn
        return (LocalSide == Side.White && piece.isWhite && IsLocalPlayersTurn())
            || (LocalSide == Side.Black && !piece.isWhite && IsLocalPlayersTurn());
    }

    public void CheckForGameEnd()
    {
        bool whiteKingExists = false;
        bool blackKingExists = false;

        Tile[,] tiles = BoardManager.Instance.GetTiles();
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                Piece piece = tiles[x, y].currentPiece;
                if (piece is King)
                {
                    if (piece.isWhite) whiteKingExists = true;
                    else blackKingExists = true;
                }
            }
        }

        if (!whiteKingExists)
        {
            photonView.RPC("RPC_ShowWinner", RpcTarget.All, false); // Black wins
            EndGame();
        }
        else if (!blackKingExists)
        {
            photonView.RPC("RPC_ShowWinner", RpcTarget.All, true); // White wins
            EndGame();
        }
    }

    [PunRPC]
    private void RPC_ShowWinner(bool whiteWins)
    {
        UIManager.Instance.ShowWinner(whiteWins);
    }

    private void EndGame()
    {
        PhotonNetwork.LeaveRoom();
    }
}