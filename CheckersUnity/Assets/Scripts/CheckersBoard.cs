using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckersBoard : MonoBehaviour
{
    private Piece selectedPawn;
    public int BlackLeft;
    public int WhiteLeft;
    public int BlackKings;
    public int WhiteKings;
    public Piece[,] Board;
    private Vector2 mouseOver;
    private Vector3 boardOffset = new Vector3(-4f, 0, -4f);
    private Vector3 pieceOffset = new Vector3(0.5f, 0, 0.5f);

    public GameObject WhitePiecePrefab;
    public GameObject BlackPiecePrefab;
    public GameObject WhiteKingPrefab;
    public GameObject BlackKingPrefab;

    public Material whitePieceMaterial;
    public Material blackPieceMaterial;
    public Material chosenPieceMaterial;

    public void Awake()
    {
        BlackLeft = 12;
        WhiteLeft = 12;
        BlackKings = 0;
        WhiteKings = 0;
        Board = new Piece[8, 8];
    }

    public void Start()
    {
        CreateBoard();
    }

    public void Update()
    {
        Print();
    }

    public void CreateBoard()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Board[i, j] = null;

                if ((i + j) % 2 == 0)
                {
                    if (i < 3)
                        GeneratePiece(i, j, 1);
                    else if (i > 4)
                        GeneratePiece(i, j, 2);
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
        Piece temp = Board[piece.Row, piece.Col];
        Board[piece.Row, piece.Col] = Board[row, col];
        Board[row, col] = temp;

        piece.Move(row, col);

        if (row == 8 || row == 0)
        {
            MakeKing(row, col);
            if (piece.Color == 1)
                BlackKings++;
            else
                WhiteKings++;
        }
    }

    public void OnMouseDown()
    {
        CheckMousePostition();
        if (selectedPawn != null) 
        {
            //MakeKing(selectedPawn.Row, selectedPawn.Col);
            ChangeMaterial(selectedPawn);
        }

    }

    private void CheckMousePostition()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50.0f, LayerMask.GetMask("Board")))
        {
            if (selectedPawn != null)
                ChangeMaterial(selectedPawn);

            mouseOver.x = (int)(hit.point.x - boardOffset.x);
            mouseOver.y = (int)(hit.point.z - boardOffset.z);
            selectedPawn = Board[(int)mouseOver.y, (int)mouseOver.x];
            if (selectedPawn == null)
                Debug.Log("Co do kurwy : ");
        }
        else
        {
            mouseOver.x = -1;
            mouseOver.y = -1;
        }
        Debug.Log("Mouse location : " + mouseOver + "  " + selectedPawn);
    }

    private void GeneratePiece(int x, int y, int color)
    {
        GameObject go = null;
        if (color == 1)
            go = Instantiate(BlackPiecePrefab);
        else if (color == 2)
            go = Instantiate(WhitePiecePrefab);
        else
            return;

        go.transform.SetParent(transform, true);
        go.transform.localScale = new Vector3(1, 1, 1);
        Board[x, y] = new Piece(x, y, color, go);
    }

    private void MakeKing(int x, int y)
    {
        if(Board[x, y] == null)
        {
            return;
        }

        Board[x, y].King = true;
        Destroy(Board[x, y].Go);
        if (Board[x, y].Color == 1)
            Board[x, y].Go = Instantiate(BlackKingPrefab);
        else if (Board[x, y].Color == 2)
            Board[x, y].Go = Instantiate(WhiteKingPrefab);
        else
            return;

        Board[x, y].Go.transform.SetParent(transform, true);
        Board[x, y].Go.transform.localScale = new Vector3(1, 1, 1);
        Board[x, y].MovePiece();
    }

    private void Print()
    {
        for (int i=0; i<8; i++)
        {
            for (int j=0; j<8; j++)
                if (Board[i, j] != null)
                    Board[i, j].MovePiece();
        }
    }

    private void ChangeMaterial(Piece selected)
    {
        if( selected.Active && selected.Color == 2)
        {
            selected.Go.GetComponent<Renderer>().material = whitePieceMaterial;
            selected.Active = false;
        }
        else if(selected.Active && selected.Color == 1)
        {
            selected.Go.GetComponent<Renderer>().material = blackPieceMaterial;
            selected.Active = false;
        }
        else 
        {
            selected.Go.GetComponent<Renderer>().material = chosenPieceMaterial;
            selected.Active = true;
        }
    }
}
