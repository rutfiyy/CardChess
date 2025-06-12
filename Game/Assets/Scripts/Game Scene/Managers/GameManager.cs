using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool isWhiteTurn = true;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void EndTurn()
    {
        isWhiteTurn = !isWhiteTurn;
        UIManager.Instance.ShowTurn();
        if (isWhiteTurn)
        {
            // Notify card manager (energy gain, card draw, duration decrease, etc.)
            CardManager.Instance.OnTurnChange();

            // Update UI at turn end
            UIManager.Instance.UpdateHand();
        }

        CheckForGameEnd();
        Debug.Log("Turn Ended. Now " + (isWhiteTurn ? "White" : "Black") + "'s turn.");
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
            UIManager.Instance.ShowWinner(false); // Black wins
            EndGame();
        }
        else if (!blackKingExists)
        {
            UIManager.Instance.ShowWinner(true); // White wins
            EndGame();
        }
    }

    private void EndGame()
    {
        // End the game
    }
}
