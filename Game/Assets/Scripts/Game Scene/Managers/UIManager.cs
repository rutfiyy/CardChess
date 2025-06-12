using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Transform handPanel;
    public GameObject cardUIPrefab;
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI announcementText; 
    public TextMeshProUGUI turnText;
    public TextMeshProUGUI winnerText;
    public float spread = 1.6f; // Distance in pixels between card

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowTurn()
    {
        if (turnText != null)
            turnText.text = GameManager.Instance.isWhiteTurn ? "White's Turn" : "Black's Turn";
    }

    public void ShowWinner(bool whiteWon)
    {
        if (winnerText != null)
            winnerText.text = whiteWon ? "White Wins!" : "Black Wins!";
    }

    public void ShowAnnouncement(string message)
    {
        if (announcementText != null)
        {
            announcementText.text = message;
            announcementText.gameObject.SetActive(true);
        }
    }

    public void UpdateHand()
    {
        if (CardManager.Instance == null) return;

        foreach (Transform child in handPanel)
            Destroy(child.gameObject);

        float startX = -((CardManager.Instance.hand.Count - 1) * spread) / 2f;

        for (int i = 0; i < CardManager.Instance.hand.Count; i++)
        {
            Card card = CardManager.Instance.hand[i];
            GameObject ui = Instantiate(cardUIPrefab, handPanel);

            // Set local position for spread
            ui.transform.localPosition = new Vector3(startX + i * spread, 0, 0);

            // Set event camera for the card's canvas
            Canvas cardCanvasComponent = ui.GetComponentInChildren<Canvas>();
            if (cardCanvasComponent != null)
                cardCanvasComponent.worldCamera = Camera.main;

            Transform cardCanvas = ui.transform.Find("CardCanvas");
            TextMeshProUGUI cardNameText = cardCanvas.Find("CardName")?.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI cardCostText = cardCanvas.Find("CardCost")?.GetComponent<TextMeshProUGUI>();
            Image icon = cardCanvas.Find("CardIcon")?.GetComponent<Image>();

            if (card != null)
            {
                CardUI cardUI = ui.GetComponent<CardUI>();
                if (cardUI != null)
                {
                    cardUI.Setup(card);
                }
            }
        }

        if (energyText != null)
            energyText.text = "Energy: " + CardManager.Instance.energy;
    }
}
