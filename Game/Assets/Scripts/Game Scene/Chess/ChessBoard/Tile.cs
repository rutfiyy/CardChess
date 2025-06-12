using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color whiteColor;
    [SerializeField] private Color blackColor;
    public Vector2Int position;
    public SpriteRenderer renderer;
    public Piece currentPiece;
    private Color originalColor;
    public bool isLight;

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        originalColor = renderer.color;
    }

    public void Setup(Vector2Int pos, bool isLight)
    {
        position = pos;
        renderer = GetComponent<SpriteRenderer>();
        renderer.color = isLight ? whiteColor : blackColor;
        originalColor = renderer.color;
        currentPiece = GetComponentInChildren<Piece>();
        this.isLight = isLight;
    }

    public void Highlight(Color lightHighlight, Color darkHighlight)
    {
        renderer.color = isLight ? lightHighlight : darkHighlight;
    }

    public void Unhighlight()
    {
        renderer.color = originalColor;
    }

    private void OnMouseDown()
    {
        // Force move logic
        if (GameController.Instance.isForcedMove)
        {
            Piece forced = GameController.Instance.forcedMovePiece;
            
            if (GameController.Instance.selectedPiece == forced && GameController.Instance.IsLegalMove(position))
            {
                forced.MoveTo(BoardManager.Instance.GetTiles(), position);
                forced.diagonalMove = false;
                forced.orthogonalMove = false;
                forced.canAttack = true; // Re-enable attack after forced move
                GameController.Instance.forcedMovePiece = null;
                GameController.Instance.isForcedMove = false;
                GameController.Instance.Deselect();
                GameManager.Instance.EndTurn(); // <-- End turn after forced move
                return;
            }
            Debug.Log("Forced move piece is not selected.");
            GameController.Instance.SelectPiece(forced);
            UIManager.Instance.ShowAnnouncement("You must move the selected piece.");
            return;
        }

        // If a card is selected, try to play it on this tile
        if (CardManager.Instance.selectedCard != null)
        {
            Piece clickedPiece = currentPiece;
            if (clickedPiece == null) return;
            if (!CardManager.Instance.selectedCard.CanPlay(this, clickedPiece))
                return;

            CardManager.Instance.selectedTargets.Add(clickedPiece);

            if (CardManager.Instance.selectedTargets.Count < CardManager.Instance.selectedCard.requiredTargets)
            {
                // Check if there are any more selectable pieces left
                bool anySelectable = false;
                Tile[,] board = BoardManager.Instance.GetTiles();
                for (int x = 0; x < board.GetLength(0); x++)
                {
                    for (int y = 0; y < board.GetLength(1); y++)
                    {
                        Piece p = board[x, y].currentPiece;
                        if (p != null && CardManager.Instance.selectedCard.CanPlay(board[x, y], p) && !CardManager.Instance.selectedTargets.Contains(p))
                        {
                            anySelectable = true;
                            break;
                        }
                    }
                    if (anySelectable) break;
                }

                if (!anySelectable)
                {
                    // No more selectable pieces, try to play the card now
                    if (CardManager.Instance.TryPlayCard(CardManager.Instance.selectedCard, CardManager.Instance.selectedTargets))
                    {
                        UIManager.Instance.ShowAnnouncement("Card played!");
                    }
                    else
                    {
                        UIManager.Instance.ShowAnnouncement("Card could not be played.");
                    }
                    return;
                }

                int remaining = CardManager.Instance.selectedCard.requiredTargets - CardManager.Instance.selectedTargets.Count;
                UIManager.Instance.ShowAnnouncement($"Select {remaining} more.");
                GameController.Instance.HighlightPlayableCardTiles(CardManager.Instance.selectedCard);
                return;
            }

            if (CardManager.Instance.TryPlayCard(CardManager.Instance.selectedCard, CardManager.Instance.selectedTargets))
            {
                UIManager.Instance.ShowAnnouncement("Card played!");
            }
            else
            {
                UIManager.Instance.ShowAnnouncement("Card could not be played.");
            }
            return;
        }

        // --- No piece selected yet ---
        if (GameController.Instance.selectedPiece == null)
        {
            if (currentPiece != null && currentPiece.isWhite == GameManager.Instance.isWhiteTurn)
            {
                GameController.Instance.SelectPiece(currentPiece);
            }
        }
        else // A piece is already selected
        {
            if (GameController.Instance.IsLegalMove(position))
            {
                // Capture
                if (currentPiece != null && currentPiece.isWhite != GameController.Instance.selectedPiece.isWhite)
                {
                    Destroy(currentPiece.gameObject);
                    CardManager.Instance.OnCapture();
                }

                // Move selected piece to this tile
                GameController.Instance.selectedPiece.MoveTo(BoardManager.Instance.GetTiles(), position);
                GameController.Instance.Deselect();
                GameManager.Instance.EndTurn();
            }
            else if (currentPiece != null && currentPiece.isWhite == GameController.Instance.selectedPiece.isWhite)
            {
                // Select different friendly piece
                GameController.Instance.SelectPiece(currentPiece);
            }
            else
            {
                Debug.Log("Invalid move");
            }
        }
    }

    // Called externally when a piece is moved to or removed from this tile
    public void SetPiece(Piece piece)
    {
        currentPiece = piece;
    }
}
