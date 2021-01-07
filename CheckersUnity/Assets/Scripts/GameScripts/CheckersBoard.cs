using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CheckersBoard : MonoBehaviour
{
    public int BlackLeft;
    public int WhiteLeft;

    public Piece[,] Board;
    public Piece SelectedPawn;

    private Vector2 mouseOver;
    private Vector3 boardOffset = new Vector3(-4f, 0, -4f);
    private Vector3 pieceOffset = new Vector3(0.5f, 0, 0.5f);

    public GameObject WhitePiecePrefab;
    public GameObject BlackPiecePrefab;
    public GameObject WhiteKingPrefab;
    public GameObject BlackKingPrefab;
    public GameObject InvisiblePiecePrefab;

    public Camera WhiteCamera;
    public Camera BlackCamera;

    public Material whitePieceMaterial;
    public Material blackPieceMaterial;
    public Material chosenPieceMaterial;
    public Material sideBlackPieceMaterial;
    public Material sideWhitePieceMaterial;

    public AudioSource skipPieceSound;
    public AudioSource movePieceSound;

    public Transform ChatMessageContainer;
    public GameObject MessagePrefab;

    Game CheckersGame;

    private List<GameObject> possibleMoves;

    public void Awake()
    {
        BlackLeft = 12;
        WhiteLeft = 12;
        Board = new Piece[8, 8];
        CheckersGame = new Game(this);
        possibleMoves = new List<GameObject>();
        CreateBoard();
        if (UIData.Color != null)
            SetCamera(UIData.Color);
        else
            SetCamera("black");

        if (UIData.GameMode != "multi")
            FindObjectOfType<Canvas>().gameObject.SetActive(false);
    }

    public void OnMouseDown()
    {
        if (CheckersGame.Turn == CheckersGame.Player)
        {
            CheckMousePostition();
            CheckersGame.Select((int)mouseOver.x, (int)mouseOver.y);
            if (UIData.GameMode == "multi")
            {
                string move = "CMOVE:" + (int)mouseOver.x + ":" + (int)mouseOver.y + ":" + ((UIData.Color == "black") ? "0" : "1");
                Client client = FindObjectOfType<Client>();
                client.Send(move);
            }
        }
    }

    public void Update()
    {
        if (CheckersGame.Player != CheckersGame.Turn)
        {
            if (UIData.GameMode == "single")
            {
                CheckersGame.UpdateValidMoves();
                CheckersGame.AI.FindBestMove(Board);
                KeyValuePair<KeyValuePair<int, int>, List<Piece>> move = CheckersGame.AI.BestMove;
                Piece piece = CheckersGame.AI.BestPiece;
                CheckersGame.Select(piece.Row, piece.Col);
                CheckersGame.Select(move.Key.Key, move.Key.Value);
            }
            else if (UIData.GameMode == "multi")
            {
                Client client = FindObjectOfType<Client>();
                if (client.EnemyMove.x >= 0)
                {
                    Debug.Log("Enemy Move " + client.EnemyMove);
                    CheckersGame.Select((int)client.EnemyMove.x, (int)client.EnemyMove.y);
                    client.EnemyMove = new Vector2(-1.0f, -1.0f);
                }
            }
        } 
    }

    public void CreateBoard()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Board[i, j] = null;

                if ((i + j) % 2 == 1)
                {
                    if (i < 3)
                        GeneratePiece(i, j, PieceColor.White);
                    else if (i > 4)
                        GeneratePiece(i, j, PieceColor.Black);
                }
            }
        }
    }

    public Piece GetPiece(int row, int col)
    {
        return Board[row, col];
    }

    public void Move(Piece piece, int row, int col)
    {
        movePieceSound.Play();
        Piece temp = Board[piece.Row, piece.Col];
        Board[piece.Row, piece.Col] = Board[row, col];
        Board[row, col] = temp;

        piece.Move(row, col);

        if (row == 7 || row == 0)
        {
            MakeKing(row, col);
        }
    }

    private void CheckMousePostition()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50.0f, LayerMask.GetMask("Board")))
        {
            mouseOver.x = (int)(hit.point.x - boardOffset.x);
            mouseOver.y = (int)(hit.point.z - boardOffset.z);
        }
        else
        {
            mouseOver.x = -1;
            mouseOver.y = -1;
        }
        Debug.Log("Mouse location : " + mouseOver);
    }

    private void GeneratePiece(int x, int y, PieceColor color)
    {
        GameObject gameObject;
        if (color == PieceColor.Black)
            gameObject = Instantiate(BlackPiecePrefab);
        else if (color == PieceColor.White)
            gameObject = Instantiate(WhitePiecePrefab);
        else
            return;

        gameObject.transform.SetParent(transform, true);
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        Board[x, y] = new Piece(x, y, color, gameObject);
    }

    private void MakeKing(int x, int y)
    {
        if(Board[x, y] == null)
        {
            return;
        }

        Board[x, y].King = true;
        Destroy(Board[x, y].PieceGameObject);
        if (Board[x, y].Color == PieceColor.Black)
            Board[x, y].PieceGameObject = Instantiate(BlackKingPrefab);
        else if (Board[x, y].Color == PieceColor.White)
            Board[x, y].PieceGameObject = Instantiate(WhiteKingPrefab);

        Board[x, y].PieceGameObject.transform.SetParent(transform, true);
        Board[x, y].PieceGameObject.transform.localScale = new Vector3(1, 1, 1);
        Board[x, y].MovePiece();
    }

    public void ChangeMaterial(Piece piece, bool selected, bool possibleMove)
    {
        if (piece == null)
            return;

        if (CheckersGame.Turn != CheckersGame.Player)
        {
            selected = false;
            possibleMove = false;
        }

        if (!selected && piece.Color == PieceColor.White)
        {
            Material[] currentMaterial = piece.PieceGameObject.GetComponent<Renderer>().materials;
            currentMaterial[0] = whitePieceMaterial;
            currentMaterial[1] = sideWhitePieceMaterial;
            piece.PieceGameObject.GetComponent<Renderer>().materials = currentMaterial;
        }
        else if (!selected && piece.Color == PieceColor.Black)
        {
            Material[] currentMaterial = piece.PieceGameObject.GetComponent<Renderer>().materials;
            currentMaterial[0] = blackPieceMaterial;
            currentMaterial[1] = sideBlackPieceMaterial;
            piece.PieceGameObject.GetComponent<Renderer>().materials = currentMaterial;
        }
        else 
        {
            piece.PieceGameObject.GetComponent<Renderer>().material = chosenPieceMaterial;
        }

        if(possibleMove)
        {
            Material[] currentMaterial = piece.PieceGameObject.GetComponent<Renderer>().materials;
            currentMaterial[1] = chosenPieceMaterial;
            piece.PieceGameObject.GetComponent<Renderer>().materials = currentMaterial;
        }
    }

    public void DrawValidMoves()
    {
        if (CheckersGame.Turn != CheckersGame.Player)
            return;
        DeleteValidMoves();
        if (CheckersGame.ValidMoves == null)
            return;
        foreach (KeyValuePair<KeyValuePair<int, int>, List<Piece>> move in CheckersGame.ValidMoves)
        {
            int row = move.Key.Key;
            int col = move.Key.Value;
            GeneratePosibleMove(row, col);
        }
    }

    private void GeneratePosibleMove(int row, int col)
    {
        GameObject gameObject = Instantiate(InvisiblePiecePrefab);
        gameObject.transform.SetParent(transform, true);
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        MovePiece(row, col, gameObject);
        possibleMoves.Add(gameObject);
    }

    public void DeleteValidMoves()
    {
        for (int i = possibleMoves.Count-1; i >= 0; i--)
        {
            Destroy(possibleMoves[i]);
            possibleMoves.RemoveAt(i);
        }
    }

    public void RemovePieces(List<Piece> skipped)
    {
        if (skipped == null)
            return;

        foreach(Piece skip in skipped)
        {
            skipPieceSound.Play();
            Destroy(skip.PieceGameObject);
            Board[skip.Row, skip.Col] = null;
            if(skip.Color == PieceColor.Black)
            {
                BlackLeft--;
            }
            else
            {
                WhiteLeft--;
            }
        }
    }

    public void CheckWinner()
    {
        if(WhiteLeft <= 0)
        {
            UIData.Winner = "black";
        }
        else if(BlackLeft <= 0)
        {
            UIData.Winner = "white";
        }
        else
            return;
        CheckersGame.End = true;
        SceneManager.LoadScene(2);
    }


    private void SetCamera(string color)
    {
        if(color == "black")
        {
            WhiteCamera.enabled = false;
            WhiteCamera.GetComponent<AudioListener>().enabled = false;
            BlackCamera.enabled = true;
            BlackCamera.GetComponent<AudioListener>().enabled = true;
        }
        else
        {
            WhiteCamera.enabled = true;
            WhiteCamera.GetComponent<AudioListener>().enabled = true;
            BlackCamera.enabled = false;
            BlackCamera.GetComponent<AudioListener>().enabled = false;
        }
    }

    public void ShowChosenPieces(bool show)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (Board[i, j] != null && Board[i, j].Color == CheckersGame.Turn)
                {
                    if (show && (Board[i, j].ValidMoves != null && Board[i, j].ValidMoves.Count != 0))
                    {
                        ChangeMaterial(Board[i, j], false, true);
                    }
                    else
                    {
                        ChangeMaterial(Board[i, j], false, false);
                    }
                }
            }
        }
    }

    public void MovePiece(int row, int col, GameObject gameObject)
    {
        gameObject.transform.position = ((Vector3.right * row) + (Vector3.forward * col) + boardOffset + pieceOffset);
        float yRotation = Camera.main.transform.eulerAngles.y;
        gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, -yRotation, gameObject.transform.eulerAngles.z);
    }

    public void SendChatMessage()
    {
        InputField input = GameObject.Find("InputMessage").GetComponent<InputField>();

        if (input.text == "")
            return;

        FindObjectOfType<Client>().Send("CMSG:" + UIData.Name + ":" + input.text);
        input.text = "";
    }

    public void ChatMessage(string message)
    {
        GameObject gameObject = Instantiate(MessagePrefab);
        gameObject.transform.SetParent(ChatMessageContainer);

        gameObject.GetComponentInChildren<Text>().text = message;
    }
}
