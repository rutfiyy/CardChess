using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public Piece selectedPiece;
    public Piece forcedMovePiece = null;
    public bool isForcedMove = false;
    public List<Vector2Int> legalMoves = new List<Vector2Int>();

    private void Awake() => Instance = this;

    public Color lightTileHighlightColor = Color.red;
    public Color darkTileHighlightColor = Color.red;

    public void SelectPiece(Piece piece)
    {
        UnhighlightLegalMoves(); // Unhighlight previous moves
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