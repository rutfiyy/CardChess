using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/MoveOrthogonal")]
public class MoveOrthogonal : Card
{
    private void OnEnable()
    {
        requiredTargets = 1;
        targetsFriendly = true;
        targetsEnemy = false;
        isEndTurn = false; // This card does not end the turn
    }

    public override bool CanPlay(Tile tile, Piece piece)
    {
        // Only allow selection of friendly, movable, non-pawn piece
        // Multiplayer: Only allow local player to select their own piece
        return piece != null
            && piece.canMove
            && piece.IsOwnedByLocalPlayer()
            && !(piece is Pawn); // Exclude pawns
    }

    public override void Play(List<Piece> targets)
    {
        if (targets.Count == 0) return;
        Piece targetPiece = targets[0];
        if (targetPiece != null)
        {
            targetPiece.orthogonalMove = true;
            targetPiece.canAttack = false; // Disable attack during forced move
            GameController.Instance.forcedMovePiece = targetPiece;
            GameController.Instance.isForcedMove = true;
            GameController.Instance.UnhighlightAllTiles();
            GameController.Instance.SelectPiece(targetPiece);
        }
    }
}