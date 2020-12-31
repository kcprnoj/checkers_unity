using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckersBoard : MonoBehaviour
{
    public int BlackLeft;
    public int WhiteLeft;
    public int BlackKings;
    public int WhiteKings;

    public Piece[,] Board;
    public Piece SelectedPawn;

    private Vector2 mouseOver;
    private Vector3 boardOffset = new Vector3(-4f, 0, -4f);

    public GameObject WhitePiecePrefab;
    public GameObject BlackPiecePrefab;
    public GameObject WhiteKingPrefab;
    public GameObject BlackKingPrefab;
    public GameObject InvisiblePiecePrefab;

    public Material whitePieceMaterial;
    public Material blackPieceMaterial;
    public Material chosenPieceMaterial;

    Game CheckersGame;

    private List<Piece> possibleMoves;

    public void Awake()
    {
        BlackLeft = 12;
        WhiteLeft = 12;
        BlackKings = 0;
        WhiteKings = 0;
        Board = new Piece[8, 8];
        CreateBoard();
        CheckersGame = new Game(this);
        possibleMoves = new List<Piece>();
    }

    public void OnMouseDown()
    {
        CheckMousePostition();
        CheckersGame.Select((int)mouseOver.x, (int)mouseOver.y);
        Debug.Log("Turn : " + CheckersGame.Turn);
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
        Debug.Log(row + " " + col);
        return Board[row, col];
    }

    public void Move(Piece piece, int row, int col)
    {
        Piece temp = Board[piece.Row, piece.Col];
        Board[piece.Row, piece.Col] = Board[row, col];
        Board[row, col] = temp;

        piece.Move(row, col);

        if (row == 7 || row == 0)
        {
            MakeKing(row, col);
            if (piece.Color == PieceColor.Black)
                BlackKings++;
            else
                WhiteKings++;
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
        else
            return;

        Board[x, y].PieceGameObject.transform.SetParent(transform, true);
        Board[x, y].PieceGameObject.transform.localScale = new Vector3(1, 1, 1);
        Board[x, y].MovePiece();
    }

    public void ChangeMaterial(Piece piece, bool selected)
    {
        if (piece == null)
            return;

        if (!selected && piece.Color == PieceColor.White)
        {
            piece.PieceGameObject.GetComponent<Renderer>().material = whitePieceMaterial;
        }
        else if (!selected && piece.Color == PieceColor.Black)
        {
            piece.PieceGameObject.GetComponent<Renderer>().material = blackPieceMaterial;
        }
        else 
        {
            piece.PieceGameObject.GetComponent<Renderer>().material = chosenPieceMaterial;
        }
    }

    public void ResetBoard()
    {
        SelectedPawn = null;
        for (int i=0; i<8; i++)
        {
            for (int j=0; j<8; j++)
            {
                if (Board[i, j] != null)
                {
                    Destroy(Board[i, j].PieceGameObject);
                }
            }
        }
        CreateBoard();
    }

    public void DrawValidMoves()
    {
        DeleteValidMoves();
        if (CheckersGame.ValidMoves == null)
            return;
        foreach(KeyValuePair<KeyValuePair<int, int>, List<Piece>> move in CheckersGame.ValidMoves)
        {
            int row = move.Key.Key;
            int col = move.Key.Value;
            GenerateInvisiblePiece(row, col);
        }
    }

    private void GenerateInvisiblePiece(int row, int col)
    {
        GameObject gameObject = Instantiate(InvisiblePiecePrefab);
        gameObject.transform.SetParent(transform, true);
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        possibleMoves.Add(new Piece(row, col, gameObject));
    }

    public void DeleteValidMoves()
    {
        for(int i = possibleMoves.Count-1; i >= 0; i--)
        {
            Destroy(possibleMoves[i].PieceGameObject);
            possibleMoves.RemoveAt(i);
        }
    }

    public void RemovePieces(List<Piece> skipped)
    {
        if (skipped == null)
            return;

        foreach(Piece skip in skipped)
        {
            Destroy(skip.PieceGameObject);
            Board[skip.Row, skip.Col] = null;
            if(skip.Color == PieceColor.Black)
            {
                BlackLeft--;
                if (skip.King)
                    BlackKings--;
            }
            else
            {
                WhiteLeft--;
                if (skip.King)
                    WhiteKings--;
            }
        }
    }
}
