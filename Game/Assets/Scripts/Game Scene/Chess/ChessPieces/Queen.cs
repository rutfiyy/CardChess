using UnityEngine;
using System.Collections.Generic;

public class Queen : Piece
{
    public override List<Vector2Int> GetLegalMoves(Tile[,] board)
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        moves = base.GetLegalMoves(board);
        if (moves.Count > 0)
        {
            return moves; // Return early if there are already moves
        }
        Vector2Int[] directions = {
            new Vector2Int(1, 0), new Vector2Int(-1, 0), // Horizontal
            new Vector2Int(0, 1), new Vector2Int(0, -1), // Vertical
            new Vector2Int(1, 1), new Vector2Int(1, -1), // Diagonal
            new Vector2Int(-1, 1), new Vector2Int(-1, -1) // Diagonal
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
}