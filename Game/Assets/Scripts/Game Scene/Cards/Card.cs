using System.Collections.Generic;
using UnityEngine;

public abstract class Card : ScriptableObject
{
    public string cardName;
    public int energyCost;
    public Sprite icon;
    public int requiredTargets = 1;
    public bool targetsFriendly = false;
    public bool targetsEnemy = false;
    public bool isEndTurn = true;

    // Returns true if the card can be played on this tile/piece
    public abstract bool CanPlay(Tile tile, Piece piece);

    // Applies the card effect
    public abstract void Play(List<Piece> targets);
}