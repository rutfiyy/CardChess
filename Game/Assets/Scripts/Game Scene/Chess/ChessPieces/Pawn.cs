using UnityEngine;
using System.Collections.Generic;

public class Pawn : Piece
{
    public override List<Vector2Int> GetLegalMoves(Tile[,] board)
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        moves = base.GetLegalMoves(board);
        if (moves.Count > 0)
        {
            return moves; // Return early if there are already moves
        }
        Vector2Int forward = isWhite ? new Vector2Int(0, 1) : new Vector2Int(0, -1);
        if (canMove == false)
        {
            return moves; // No moves allowed if cannot move
        }
        // Move forward
        Vector2Int oneStepForward = boardPosition + forward;
        if (IsInBounds(oneStepForward) && !HasPiece(board, oneStepForward))
        {
            moves.Add(oneStepForward);

            // Move two steps forward from starting position
            if ((isWhite && boardPosition.y == 1) || (!isWhite && boardPosition.y == 6))
            {
                Vector2Int twoStepsForward = boardPosition + forward * 2;
                if (IsInBounds(twoStepsForward) && !HasPiece(board, twoStepsForward))
                {
                    moves.Add(twoStepsForward);
                }
            }
        }

        // Capture diagonally and en passant
        for (int i = -1; i <= 1; i += 2)
        {
            Vector2Int diagonalCapture = boardPosition + new Vector2Int(i, forward.y);
            if (IsInBounds(diagonalCapture) && IsCaptureMove(board, diagonalCapture))
            {
                // Normal capture
                if (HasEnemyPiece(board, diagonalCapture))
                {
                    moves.Add(diagonalCapture);
                }
                // En passant
                else if (BoardManager.Instance.enPassantTarget.HasValue &&
                         BoardManager.Instance.enPassantTarget.Value == diagonalCapture)
                {
                    moves.Add(diagonalCapture);
                }
            }
        }

        return moves;
    }

    public override void MoveTo(Tile[,] board, Vector2Int targetPosition)
    {
        // En passant capture
        if (BoardManager.Instance.enPassantTarget.HasValue &&
            targetPosition == BoardManager.Instance.enPassantTarget.Value)
        {
            int direction = isWhite ? -1 : 1;
            Vector2Int capturedPawnPos = new Vector2Int(targetPosition.x, targetPosition.y + direction);
            Piece capturedPawn = board[capturedPawnPos.x, capturedPawnPos.y].currentPiece;
            if (capturedPawn != null && capturedPawn is Pawn)
            {
                Destroy(capturedPawn.gameObject);
                board[capturedPawnPos.x, capturedPawnPos.y].currentPiece = null;
            }
        }

        // Set en passant target if moved two squares
        if (Mathf.Abs(targetPosition.y - boardPosition.y) == 2)
        {
            int direction = isWhite ? 1 : -1;
            BoardManager.Instance.enPassantTarget = boardPosition + new Vector2Int(0, direction);
        }
        else
        {
            BoardManager.Instance.enPassantTarget = null;
        }

        base.MoveTo(board, targetPosition);

        // Promotion
        if ((isWhite && boardPosition.y == 7) || (!isWhite && boardPosition.y == 0))
        {
            Promote(board);
        }
    }

    private void Promote(Tile[,] board)
    {
        // Replace this pawn with a Queen (or show a UI for choice)
        GameObject queenPrefab = isWhite ? BoardManager.Instance.whiteQueenPrefab : BoardManager.Instance.blackQueenPrefab;
        GameObject queenObj = Object.Instantiate(queenPrefab, this.transform.position, Quaternion.identity, board[boardPosition.x, boardPosition.y].transform);
        Piece queen = queenObj.GetComponent<Piece>();
        queen.isWhite = this.isWhite;
        queen.boardPosition = this.boardPosition;
        board[boardPosition.x, boardPosition.y].currentPiece = queen;
        Destroy(this.gameObject);
    }

    public override void ReduceStatusDurations()
    {
        if (currentStatus == StatusEffect.DelayedPromotion)
        {
            if (statusEffectDuration > 0)
            {
                statusEffectDuration--;
                if (statusEffectDuration == 0)
                {
                    Promote(BoardManager.Instance.GetTiles());
                    currentStatus = StatusEffect.None;
                }
            }
        }
        else
        {
            base.ReduceStatusDurations();
        }
    }
}