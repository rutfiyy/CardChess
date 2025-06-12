using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/FreezeEnemy")]
public class FreezeEnemy : Card
{
    public override bool CanPlay(Tile tile, Piece piece)
    {
        return piece != null && piece.canAttack && piece.currentStatus == Piece.StatusEffect.None && !piece.isWhite;
    }

    public override void Play(List<Piece> targets)
    {
        if (targets.Count == 0) return;

        Piece targetPiece = targets[0];
        ApplyFreezeEffect(targetPiece);
    }
    private void ApplyFreezeEffect(Piece piece)
    {
        if (piece != null)
        {
            piece.canAttack = false;
            piece.canMove = false; // Freeze effect disables movement
            piece.currentStatus = Piece.StatusEffect.Freeze;
            piece.statusEffectDuration = 3;
        }
    }
}