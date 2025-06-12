using UnityEngine;
using System.Collections.Generic;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance;
    public List<Card> hand = new();
    public int energy = 3;
    public int maxHandSize = 5;

    public List<Card> allCards;
    private Queue<Card> deck = new();
    public Card selectedCard;
    public List<Piece> selectedTargets = new List<Piece>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        InitializeDeck();
    }

    private void InitializeDeck()
    {
        List<Card> shuffled = new List<Card>(allCards);
        for (int i = 0; i < shuffled.Count; i++)
        {
            int rand = Random.Range(i, shuffled.Count);
            (shuffled[i], shuffled[rand]) = (shuffled[rand], shuffled[i]);
        }
        foreach (Card card in shuffled)
        {
            deck.Enqueue(Instantiate(card));
        }
    }

    public void OnTurnChange()
    {
        foreach (Piece p in FindObjectsOfType<Piece>())
        {
            p.ReduceStatusDurations();
        }
        if (hand.Count < maxHandSize && deck.Count > 0)
        {
            Card drawn = deck.Dequeue();
            hand.Add(drawn);
        }
        UIManager.Instance.UpdateHand();
    }

    public bool TryPlayCard(Card card, List<Piece> targets)
    {
        if (!hand.Contains(card) || energy < card.energyCost || targets.Count != card.requiredTargets)
            return false;

        card.Play(targets);
        energy -= card.energyCost;
        hand.Remove(card);

        // Cleanup and end turn
        selectedCard = null;
        selectedTargets.Clear();
        UIManager.Instance.UpdateHand();
        if (card.isEndTurn)
        {
            GameController.Instance.UnhighlightAllTiles();
            GameManager.Instance.EndTurn();
        }
        return true;
    }

    public void DrawCard()
    {
        if (hand.Count >= maxHandSize || deck.Count == 0) return;
        Card drawn = deck.Dequeue();
        hand.Add(drawn);
        UIManager.Instance.UpdateHand();
    }

    public void GainEnergy(int amount)
    {
        energy += amount;
        UIManager.Instance.UpdateHand();
    }

    public void OnCapture()
    {
        energy += 1;
    }

    public void SelectCard(Card card)
    {
        if (!GameManager.Instance.isWhiteTurn)
        {
            Debug.Log("Cannot select card: Not your turn.");
            return;
        }
        if (card == null || !hand.Contains(card))
        {
            Debug.LogWarning("Card not in hand or is null");
            return;
        }
        selectedCard = card;
        selectedTargets.Clear();
        //Debug.Log("Selected card: " + card.cardName);

        // Use the card's requiredTargets
        if (card.requiredTargets > 1)
        {
            UIManager.Instance.ShowAnnouncement($"Select {card.requiredTargets} pieces.");
        }
        if (card.energyCost <= energy)
        {
            GameController.Instance.HighlightPlayableCardTiles(selectedCard);
        }
    }

    public void UnselectCard()
    {
        selectedCard = null;
        GameController.Instance.UnhighlightAllTiles();
        //Debug.Log("Unselected card");
    }
}