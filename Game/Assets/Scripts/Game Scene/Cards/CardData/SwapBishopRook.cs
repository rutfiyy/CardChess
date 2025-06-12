using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/SwapBishopRook")]
public class SwapBishopRook : Card
{
    private void OnEnable()
    {
        requiredTargets = 2;
        targetsFriendly = true;
        targetsEnemy = false;
    }
    public override bool CanPlay(Tile tile, Piece piece)
    {
        if (piece == null || piece.currentStatus != Piece.StatusEffect.None || !piece.isWhite)
            return false;

        var selected = CardManager.Instance.selectedTargets;

        if (selected.Count == 0)
        {
            // First selection: allow Bishop or Rook
            return piece is Bishop || piece is Rook;
        }
        else if (selected.Count == 1)
        {
            // Second selection: must be different type and not the same piece
            Piece first = selected[0];
            if (first == piece) return false;
            if (first is Bishop && piece is Rook) return true;
            if (first is Rook && piece is Bishop) return true;
            return false;
        }
        return false;
    }

    public override void Play(List<Piece> targets)
    {
        if (targets == null || targets.Count != 2) return;

        Piece piece1 = targets[0];
        Piece piece2 = targets[1];

        Vector2Int pos1 = piece1.boardPosition;
        Vector2Int pos2 = piece2.boardPosition;
        Tile[,] board = BoardManager.Instance.GetTiles();

        // Swap in board and GameObject
        board[pos1.x, pos1.y].currentPiece = piece2;
        board[pos2.x, pos2.y].currentPiece = piece1;
        piece1.boardPosition = pos2;
        piece2.boardPosition = pos1;
        piece1.transform.position = board[pos2.x, pos2.y].transform.position;
        piece2.transform.position = board[pos1.x, pos1.y].transform.position;
    }
}