using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardUI : MonoBehaviour
{
    public Image icon;
    public Image background;
    public Image Highlight;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;
    public Card card;
    private bool isSelected = false;

    public void Setup(Card card)
    {
        //Debug.Log($"Setting up CardUI for: {card.cardName}");
        if (card == null)
        {
            Debug.LogError("Card is null in CardUI.Setup");
            return;
        }
        this.card = card;
        icon.sprite = card.icon;
        nameText.text = card.cardName;
        costText.text = card.energyCost.ToString();
        SetHighlight(false); // Ensure highlight is off by default
    }

    public void OnClickCard()
    {
        // Turn off highlight for all other cards
        foreach (Transform child in UIManager.Instance.handPanel)
        {
            CardUI cardUI = child.GetComponent<CardUI>();
            if (cardUI != null)
                cardUI.SetHighlight(false);
        }

        if (isSelected)
        {
            //unselect the card
            //Debug.Log($"Unselecting card: {card.cardName}");
            CardManager.Instance.UnselectCard();
            SetHighlight(false);
            isSelected = false;
        }
        else
        {
            // Set this card as selected in CardManager
            //Debug.Log($"Selecting card: {card.cardName}");
            CardManager.Instance.SelectCard(card);
            // Highlight this card
            SetHighlight(true);
            isSelected = true;
        }
    }

    public void SetHighlight(bool active)
    {
        if (Highlight != null)
            Highlight.gameObject.SetActive(active);
    }

    public void OnMouseEnter()
    {
        SetHighlight(true);
    }

    public void OnMouseExit()
    {
        if (CardManager.Instance.selectedCard != card) // Only unhighlight if this isn't the selected card
            SetHighlight(false);
    }

    public void OnMouseDown()
    {
        OnClickCard();
    }
}