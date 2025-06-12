using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/DelayedPromotion")]
public class DelayedPromotion : Card
{
    public override bool CanPlay(Tile tile, Piece piece)
    {
        return piece != null && piece is Pawn && piece.currentStatus == Piece.StatusEffect.None;
    }

    public override void Play(List<Piece> targets)
    {
        if (targets.Count == 0) return;

        Piece targetPiece = targets[0];
        ApplyDelayedPromotionEffect(targetPiece);
    }
    private void ApplyDelayedPromotionEffect(Piece piece)
    {
        if (piece != null)
        {
            piece.currentStatus = Piece.StatusEffect.DelayedPromotion;
            piece.statusEffectDuration = 8;
        }
    }
}