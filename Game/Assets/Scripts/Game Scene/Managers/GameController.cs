using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

public class GameController : MonoBehaviourPun
{
    public static GameController Instance;

    public Piece selectedPiece;
    public Piece forcedMovePiece = null;
    public bool isForcedMove = false;
    public List<Vector2Int> legalMoves = new List<Vector2Int>();

    public GameManager.Side CurrentTurn = GameManager.Side.White;

    private void Awake() => Instance = this;

    public Color lightTileHighlightColor = Color.red;
    public Color darkTileHighlightColor = Color.red;

    public void SelectPiece(Piece piece)
    {
        if (!CanControlPiece(piece)) return;
        UnhighlightLegalMoves();
        selectedPiece = piece;
        legalMoves = piece.GetLegalMoves(BoardManager.Instance.GetTiles());
        HighlightLegalMoves();
        Debug.Log($"Selected piece: {piece.name} status: {piece.currentStatus}, legal moves: {legalMoves.Count}");
    }

    public void Deselect()
    {
        UnhighlightLegalMoves();
        selectedPiece = null;
        legalMoves.Clear();
    }

    public bool CanControlPiece(Piece piece)
    {
        // Only allow local player to select their own pieces on their turn
        return GameManager.Instance.IsLocalPlayersTurn() &&
               ((CurrentTurn == GameManager.Side.White && piece.isWhite) ||
                (CurrentTurn == GameManager.Side.Black && !piece.isWhite));
    }

    public void TryMoveSelectedPiece(Vector2Int target)
    {
        if (selectedPiece == null) return;
        if (!IsLegalMove(target)) return;
        if (!GameManager.Instance.IsLocalPlayersTurn()) return;

        // Send the move by coordinates, not by object reference
        photonView.RPC("RPC_MovePiece", RpcTarget.All, selectedPiece.boardPosition.x, selectedPiece.boardPosition.y, target.x, target.y);
        Deselect();
    }

    [PunRPC]
    private void RPC_MovePiece(int fromX, int fromY, int toX, int toY)
    {
        Tile[,] board = BoardManager.Instance.GetTiles();
        Piece piece = board[fromX, fromY].currentPiece;
        if (piece != null)
        {
            piece.MoveTo(board, new Vector2Int(toX, toY));
            Deselect();
            if (PhotonNetwork.IsMasterClient)
            {
                EndTurn();
            }
        }
    }

    public void EndTurn()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            CurrentTurn = (CurrentTurn == GameManager.Side.White) ? GameManager.Side.Black : GameManager.Side.White;
            photonView.RPC("RPC_SyncTurn", RpcTarget.All, (int)CurrentTurn);
        }
    }

    [PunRPC]
    private void RPC_SyncTurn(int turn)
    {
        CurrentTurn = (GameManager.Side)turn;
        UIManager.Instance.ShowTurn();
    }

    public void HighlightPlayableCardTiles(Card card)
    {
        UnhighlightAllTiles();

        Tile[,] tiles = BoardManager.Instance.GetTiles();
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                Tile tile = tiles[x, y];
                Piece piece = tile.currentPiece;
                if (card.CanPlay(tile, piece))
                {
                    tile.Highlight(lightTileHighlightColor, darkTileHighlightColor);
                }
            }
        }
    }

    public void UnhighlightAllTiles()
    {
        Tile[,] tiles = BoardManager.Instance.GetTiles();
        for (int x = 0; x < 8; x++)
            for (int y = 0; y < 8; y++)
                tiles[x, y].Unhighlight();
    }

    private void HighlightLegalMoves()
    {
        foreach (var move in legalMoves)
        {
            BoardManager.Instance.GetTiles()[move.x, move.y]
                .Highlight(lightTileHighlightColor, darkTileHighlightColor);
        }
    }

    private void UnhighlightLegalMoves()
    {
        foreach (var move in legalMoves)
        {
            BoardManager.Instance.GetTiles()[move.x, move.y].Unhighlight();
        }
    }

    public bool IsLegalMove(Vector2Int pos)
    {
        return legalMoves.Contains(pos);
    }
}