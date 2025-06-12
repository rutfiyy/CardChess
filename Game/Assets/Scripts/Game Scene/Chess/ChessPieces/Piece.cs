using UnityEngine;
using System.Collections.Generic;

public abstract class Piece : MonoBehaviour
{
    public bool isWhite;
    public bool canAttack = true;
    public bool canMove = true;
    public bool canAttacked = true;
    public bool diagonalMove = false;
    public bool orthogonalMove = false;
    public Vector2Int boardPosition;

    public enum StatusEffect { None, Decay, Freeze, Weak, Stone, DelayedPromotion }
    public StatusEffect currentStatus = StatusEffect.None;
    public int statusEffectDuration = 0;

    // Multiplayer: Only allow local player to interact with their own pieces
    public bool IsOwnedByLocalPlayer()
    {
        // White = MasterClient, Black = not MasterClient
        return (isWhite && GameManager.Instance.LocalSide == GameManager.Side.White && GameManager.Instance.IsLocalPlayersTurn())
            || (!isWhite && GameManager.Instance.LocalSide == GameManager.Side.Black && GameManager.Instance.IsLocalPlayersTurn());
    }

    public virtual List<Vector2Int> GetLegalMoves(Tile[,] board)
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        // Only allow local player to get moves for their own pieces on their turn
        if (!GameManager.Instance.CanControlPiece(this)) return moves;

        if (diagonalMove)
        {
            // Add bishop-like moves
            Vector2Int[] diagonals = {
                new Vector2Int(1, 1), new Vector2Int(-1, 1),
                new Vector2Int(1, -1), new Vector2Int(-1, -1)
            };
            foreach (var dir in diagonals)
            {
                Vector2Int pos = boardPosition + dir;
                while (IsInBounds(pos) && !HasPiece(board, pos))
                {
                    moves.Add(pos);
                    pos += dir;
                }
                if (IsInBounds(pos) && HasEnemyPiece(board, pos) && IsCaptureMove(board, pos))
                    moves.Add(pos);
            }
            return moves;
        }
        if (orthogonalMove)
        {
            // Add rook-like moves
            Vector2Int[] orthogonals = {
                new Vector2Int(1, 0), new Vector2Int(-1, 0),
                new Vector2Int(0, 1), new Vector2Int(0, -1)
            };
            foreach (var dir in orthogonals)
            {
                Vector2Int pos = boardPosition + dir;
                while (IsInBounds(pos) && !HasPiece(board, pos))
                {
                    moves.Add(pos);
                    pos += dir;
                }
                if (IsInBounds(pos) && HasEnemyPiece(board, pos) && IsCaptureMove(board, pos))
                    moves.Add(pos);
            }
            return moves;
        }

        // Otherwise, let the derived class handle normal moves
        return moves;
    }

    public virtual bool CanCaptureAt(Vector2Int pos, Tile[,] board)
    {
        return GetLegalMoves(board).Contains(pos);
    }

    protected bool HasPiece(Tile[,] board, Vector2Int pos)
    {
        return board[pos.x, pos.y].currentPiece != null;
    }

    protected bool HasEnemyPiece(Tile[,] board, Vector2Int pos)
    {
        var piece = board[pos.x, pos.y].currentPiece;
        return piece != null && piece.isWhite != isWhite;
    }

    public static bool IsInBounds(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < 8 && pos.y >= 0 && pos.y < 8;
    }

    protected bool IsCaptureMove(Tile[,] board, Vector2Int targetPosition)
    {
        if (canAttack == false) return false;
        if (!IsInBounds(targetPosition)) return false;
        if (!HasPiece(board, targetPosition)) return false;
        if (board[targetPosition.x, targetPosition.y].currentPiece.canAttacked == false) return false;
        return HasEnemyPiece(board, targetPosition);
    }

    public virtual void MoveTo(Tile[,] board, Vector2Int targetPosition)
    {
        board[boardPosition.x, boardPosition.y].currentPiece = null;

        if (board[targetPosition.x, targetPosition.y].currentPiece != null)
        {
            Destroy(board[targetPosition.x, targetPosition.y].currentPiece.gameObject);
            CardManager.Instance.OnCapture();
        }

        boardPosition = targetPosition;
        board[targetPosition.x, targetPosition.y].currentPiece = this;
        this.transform.SetParent(board[targetPosition.x, targetPosition.y].transform);
        this.transform.localPosition = new Vector3(0, 0, -1);
    }

    public virtual void ReduceStatusDurations()
    {
        if (statusEffectDuration > 0)
        {
            statusEffectDuration--;
            if (statusEffectDuration == 0)
            {
                currentStatus = StatusEffect.None;
                canAttack = true;
                canMove = true;
                canAttacked = true;
            }
        }
    }

    public bool IsFrozen() => currentStatus == StatusEffect.Freeze || currentStatus == StatusEffect.Stone;
    public bool IsWeakened() => currentStatus == StatusEffect.Decay || currentStatus == StatusEffect.Weak;
}