using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/PushEnemyUp")]
public class PushEnemyUp : Card
{
    private void OnEnable()
    {
        requiredTargets = 3;
        targetsFriendly = false;
        targetsEnemy = true;
    }
    public override bool CanPlay(Tile tile, Piece piece)
    {
        // Only allow if the tile above is empty
        if (piece == null || piece.isWhite || piece.currentStatus != Piece.StatusEffect.None)
            return false;

        Tile[,] board = BoardManager.Instance.GetTiles();
        int boardHeight = board.GetLength(1);
        Vector2Int above = piece.boardPosition + new Vector2Int(0, 1);

        // Check if above is in bounds and empty
        bool canPushUp = above.y < boardHeight && board[above.x, above.y].currentPiece == null;

        return canPushUp && !CardManager.Instance.selectedTargets.Contains(piece);
    }

    public override void Play(List<Piece> targets)
    {
        if (targets.Count == 0) return;

        Piece piece1 = targets[0];
        Piece piece2 = targets[1];
        Piece piece3 = targets[2];
        // Ensure all pieces are valid targets
        if (piece1 == null || piece2 == null || piece3 == null) return;
        // Apply push effect to each target piece
        ApplyPushEffect(piece1);
        ApplyPushEffect(piece2);
        ApplyPushEffect(piece3);
    }
    private void ApplyPushEffect(Piece piece)
    {
        if (piece != null)
        {
            Vector2Int newPos = piece.boardPosition + new Vector2Int(0, 1);
            Tile[,] board = BoardManager.Instance.GetTiles();
            if (Piece.IsInBounds(newPos) && !board[newPos.x, newPos.y].currentPiece)
            {
                piece.MoveTo(board, newPos);
            }
        }
    }
}