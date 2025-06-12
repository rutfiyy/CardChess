using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/StoneEnemy")]
public class StoneEnemy : Card
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
        ApplyStoneEffect(targetPiece);
    }
    private void ApplyStoneEffect(Piece piece)
    {
        if (piece != null)
        {
            piece.canAttack = false;
            piece.canMove = false; // Stone effect disables movement
            piece.canAttacked = false; // Stone effect disables being attacked
            piece.currentStatus = Piece.StatusEffect.Stone;
            piece.statusEffectDuration = 2;
        }
    }
}