using UnityEngine;
using System.Collections.Generic;

public class Knight : Piece
{
    public override List<Vector2Int> GetLegalMoves(Tile[,] board)
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        moves = base.GetLegalMoves(board);
        if (moves.Count > 0)
        {
            return moves; // Return early if there are already moves
        }
        Vector2Int[] knightMoves = {
            new Vector2Int(2, 1), new Vector2Int(2, -1),
            new Vector2Int(-2, 1), new Vector2Int(-2, -1),
            new Vector2Int(1, 2), new Vector2Int(1, -2),
            new Vector2Int(-1, 2), new Vector2Int(-1, -2)
        };
        if (canMove == false)
        {
            return moves; // No moves allowed if cannot move
        }

        foreach (var move in knightMoves)
        {
            Vector2Int pos = boardPosition + move;
            if (IsInBounds(pos))
            {
                if (!HasPiece(board, pos))
                {
                    moves.Add(pos);
                }
                if (HasEnemyPiece(board, pos) && IsCaptureMove(board, pos))
                {
                    moves.Add(pos); // Capture move
                }
            }
        }

        return moves;
    }
}