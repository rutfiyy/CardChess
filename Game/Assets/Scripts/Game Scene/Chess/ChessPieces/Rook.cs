using UnityEngine;
using System.Collections.Generic;

public class Rook : Piece
{
    public bool hasMoved = false;

    public override List<Vector2Int> GetLegalMoves(Tile[,] board)
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        Vector2Int[] directions = {
            new Vector2Int(1, 0), new Vector2Int(-1, 0),
            new Vector2Int(0, 1), new Vector2Int(0, -1)
        };
        if (canMove == false)
        {
            return moves; // No moves allowed if cannot move
        }

        foreach (var direction in directions)
        {
            Vector2Int pos = boardPosition;
            while (true)
            {
                pos += direction;
                if (!IsInBounds(pos)) break;

                if (HasPiece(board, pos))
                {
                    if (HasEnemyPiece(board, pos) && IsCaptureMove(board, pos))
                    {
                        moves.Add(pos); // Capture move
                    }
                    break; // Can't move past a piece
                }
                moves.Add(pos); // Empty square
            }
        }
        return moves;
    }

    public override void MoveTo(Tile[,] board, Vector2Int targetPosition)
    {
        hasMoved = true;
        base.MoveTo(board, targetPosition);
    }
}