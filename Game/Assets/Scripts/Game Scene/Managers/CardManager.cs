using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;

public class CardManager : MonoBehaviourPun
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
        // Only allow local player to play cards on their turn
        if (!GameManager.Instance.IsLocalPlayersTurn())
            return false;

        if (!hand.Contains(card) || energy < card.energyCost || targets.Count != card.requiredTargets)
            return false;

        // Network the card play
        photonView.RPC("RPC_PlayCard", RpcTarget.All, card.cardName, SerializeTargets(targets));

        // Local cleanup
        selectedCard = null;
        selectedTargets.Clear();
        return true;
    }

    [PunRPC]
    private void RPC_PlayCard(string cardName, string serializedTargets)
    {
        Card card = FindCardByName(cardName);
        List<Piece> targets = DeserializeTargets(serializedTargets);

        if (card == null)
        {
            Debug.LogWarning("Card not found: " + cardName);
            return;
        }

        if (hand.Contains(card) && energy >= card.energyCost && targets.Count == card.requiredTargets)
        {
            card.Play(targets);
            energy -= card.energyCost;
            hand.Remove(card);

            UIManager.Instance.UpdateHand();
            if (card.isEndTurn)
            {
                GameController.Instance.UnhighlightAllTiles();
                GameController.Instance.EndTurn(); // FIXED: Use GameController, not GameManager
            }
        }
    }

    private string SerializeTargets(List<Piece> targets)
    {
        // Serialize as "x1,y1;x2,y2"
        List<string> parts = new List<string>();
        foreach (var piece in targets)
        {
            parts.Add($"{piece.boardPosition.x},{piece.boardPosition.y}");
        }
        return string.Join(";", parts);
    }

    private List<Piece> DeserializeTargets(string data)
    {
        List<Piece> result = new List<Piece>();
        if (string.IsNullOrEmpty(data)) return result;
        Tile[,] tiles = BoardManager.Instance.GetTiles();
        string[] parts = data.Split(';');
        foreach (var part in parts)
        {
            if (string.IsNullOrEmpty(part)) continue;
            string[] xy = part.Split(',');
            int x = int.Parse(xy[0]);
            int y = int.Parse(xy[1]);
            Piece p = tiles[x, y].currentPiece;
            if (p != null) result.Add(p);
        }
        return result;
    }

    private Card FindCardByName(string name)
    {
        foreach (var c in hand)
        {
            if (c.cardName == name) return c;
        }
        foreach (var c in allCards)
        {
            if (c.cardName == name) return c;
        }
        return null;
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
        // Only allow local player to select cards on their turn
        if (!GameManager.Instance.IsLocalPlayersTurn())
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
    }
}