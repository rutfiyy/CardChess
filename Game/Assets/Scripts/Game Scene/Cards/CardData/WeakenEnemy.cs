using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/WeakenEnemy")]
public class WeakenEnemy : Card
{
    public override bool CanPlay(Tile tile, Piece piece)
    {
        return piece != null && piece.currentStatus == Piece.StatusEffect.None 
               && !piece.isWhite;
    }

    public override void Play(List<Piece> targets)
    {
        if (targets.Count == 0) return;

        Piece targetPiece = targets[0];
        ApplyWeakenEffect(targetPiece);
    }
    private void ApplyWeakenEffect(Piece piece)
    {
        if (piece != null)
        {
            piece.canAttack = false; // Weaken effect disables attack
            piece.currentStatus = Piece.StatusEffect.Weak;
            piece.statusEffectDuration = 2;
        }
    }
}