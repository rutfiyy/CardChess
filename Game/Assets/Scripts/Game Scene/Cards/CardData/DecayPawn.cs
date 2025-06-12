using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/DecayPawn")]
public class DecayPawn : Card
{
    public override bool CanPlay(Tile tile, Piece piece)
    {
        return piece != null && piece.currentStatus == Piece.StatusEffect.None && piece is Pawn && !piece.isWhite;
    }

    public override void Play(List<Piece> targets)
    {
        if (targets.Count == 0) return;

        Piece targetPiece = targets[0];
        ApplyDecayEffect(targetPiece);
    }
    private void ApplyDecayEffect(Piece piece)
    {
        if (piece != null)
        {
            piece.canAttack = false; // Decay effect disables attack
            piece.currentStatus = Piece.StatusEffect.Decay;

            piece.statusEffectDuration = 2;
        }
    }
}