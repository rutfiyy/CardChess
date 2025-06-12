using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public Transform boardCenter;
    public GameObject tilePrefab;
    public GameObject whiteKingPrefab;
    public GameObject whiteQueenPrefab;
    public GameObject whiteRookPrefab;
    public GameObject whiteBishopPrefab;
    public GameObject whiteKnightPrefab;
    public GameObject whitePawnPrefab;

    public GameObject blackKingPrefab;
    public GameObject blackQueenPrefab;
    public GameObject blackRookPrefab;
    public GameObject blackBishopPrefab;
    public GameObject blackKnightPrefab;
    public GameObject blackPawnPrefab;

    public int boardSize = 8;
    public static BoardManager Instance;
    private Tile[,] tiles = new Tile[8, 8];
    public Tile[,] GetTiles() => tiles;
    public Vector2Int? enPassantTarget = null; // Set to the square behind a pawn that just moved two steps

    private void Awake() => Instance = this;

    void Start()
    {
        CreateBoard();
        SpawnPieces();
    }

    void CreateBoard()
    {
        // Calculate offset so the board is centered on boardCenter
        Vector3 offset = boardCenter != null
            ? boardCenter.position - new Vector3((boardSize - 1) / 2f, (boardSize - 1) / 2f, 0)
            : new Vector3(-3.5f, -3.5f, 0f); // fallback

        for (int x = 0; x < boardSize; x++)
        {
            for (int y = 0; y < boardSize; y++)
            {
                Vector3 tilePos = new Vector3(x, y, 0) + offset;
                GameObject tileObj = Instantiate(tilePrefab, tilePos, Quaternion.identity);

                // Name tile like "A1", "B3", etc.
                char file = (char)('A' + x);
                int rank = y + 1;
                tileObj.name = $"{file}{rank}";

                Tile tile = tileObj.GetComponent<Tile>();
                tile.Setup(new Vector2Int(x, y), (x + y) % 2 == 0);
                tiles[x, y] = tile;
            }
        }
    }

    void SpawnPieces()
    {
        string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        SetupBoardFromFEN(fen);
    }

    public void SetupBoardFromFEN(string fen)
    {
        // FEN string's first part contains only piece placement info
        string[] parts = fen.Split(' ');
        string[] rows = parts[0].Split('/');

        Vector3 offset = new Vector3(-3.5f, -3.5f, 0f);

        for (int y = 0; y < 8; y++)
        {
            string row = rows[y];
            int x = 0;

            foreach (char c in row)
            {
                if (char.IsDigit(c))
                {
                    // Empty squares to skip
                    x += c - '0';
                }
                else
                {
                    GameObject prefab = GetPrefabFromChar(c);

                    if (prefab != null)
                    {
                        Vector2Int boardPos = new Vector2Int(x, 7 - y);  // 7 - y flips the row index (FEN ranks run top-to-bottom)
                        Vector3 spawnPos = tiles[boardPos.x, boardPos.y].transform.position + Vector3.back;

                        GameObject pieceObj = Instantiate(prefab, spawnPos, Quaternion.identity);
                        pieceObj.transform.SetParent(tiles[boardPos.x, boardPos.y].transform);
                        pieceObj.transform.localPosition = new Vector3(0, 0, -1);

                        Piece piece = pieceObj.GetComponent<Piece>();
                        piece.boardPosition = boardPos;
                        tiles[boardPos.x, boardPos.y].currentPiece = piece;
                    }

                    x++;
                }
            }
        }
    }

    GameObject GetPrefabFromChar(char c)
    {
        switch (char.ToLower(c))
        {
            case 'k': return char.IsUpper(c) ? whiteKingPrefab : blackKingPrefab;
            case 'q': return char.IsUpper(c) ? whiteQueenPrefab : blackQueenPrefab;
            case 'r': return char.IsUpper(c) ? whiteRookPrefab : blackRookPrefab;
            case 'b': return char.IsUpper(c) ? whiteBishopPrefab : blackBishopPrefab;
            case 'n': return char.IsUpper(c) ? whiteKnightPrefab : blackKnightPrefab;
            case 'p': return char.IsUpper(c) ? whitePawnPrefab : blackPawnPrefab;
            default: return null;
        }
    }

    public Piece GetPieceAt(Vector2Int pos)
    {
        if (pos.x < 0 || pos.x >= 8 || pos.y < 0 || pos.y >= 8) return null;
        return tiles[pos.x, pos.y].currentPiece;
    }
}