using UnityEngine;
using System.Collections.Generic;

public class King : Piece
{
    public bool hasMoved = false;

    public override List<Vector2Int> GetLegalMoves(Tile[,] board)
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        moves = base.GetLegalMoves(board);
        if (moves.Count > 0)
        {
            return moves; // Return early if there are already moves
        }
        Vector2Int[] directions = {
            new Vector2Int(1, 0), new Vector2Int(-1, 0),
            new Vector2Int(0, 1), new Vector2Int(0, -1),
            new Vector2Int(1, 1), new Vector2Int(-1, 1),
            new Vector2Int(1, -1), new Vector2Int(-1, -1)
        };
        if (canMove == false)
        {
            return moves; // No moves allowed if cannot move
        }

        foreach (var dir in directions)
        {
            Vector2Int pos = boardPosition + dir;
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

        // Castling
        if (!hasMoved)
        {
            int y = isWhite ? 0 : 7;

            // Kingside
            if (CanCastle(board, new Vector2Int(7, y), new Vector2Int(5, y), new Vector2Int(6, y)))
                moves.Add(new Vector2Int(6, y));

            // Queenside
            if (CanCastle(board, new Vector2Int(0, y), new Vector2Int(1, y), new Vector2Int(2, y), new Vector2Int(3, y)))
                moves.Add(new Vector2Int(2, y));
        }

        return moves;
    }

    private bool CanCastle(Tile[,] board, Vector2Int rookPos, params Vector2Int[] between)
    {
        Piece rook = board[rookPos.x, rookPos.y].currentPiece;
        if (rook == null || !(rook is Rook) || ((Rook)rook).hasMoved)
            return false;

        foreach (var pos in between)
        {
            if (HasPiece(board, pos))
                return false;
        }

        // You may want to add additional checks for check conditions here

        return true;
    }

    public override void MoveTo(Tile[,] board, Vector2Int targetPosition)
    {
        // Castling move
        if (!hasMoved && Mathf.Abs(targetPosition.x - boardPosition.x) == 2)
        {
            int y = boardPosition.y;
            if (targetPosition.x == 6)
            {
                // Kingside
                Piece rook = board[7, y].currentPiece;
                rook.MoveTo(board, new Vector2Int(5, y));
            }
            else if (targetPosition.x == 2)
            {
                // Queenside
                Piece rook = board[0, y].currentPiece;
                rook.MoveTo(board, new Vector2Int(3, y));
            }
        }

        hasMoved = true;
        base.MoveTo(board, targetPosition);
    }
}