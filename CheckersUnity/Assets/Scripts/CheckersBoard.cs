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

    public GameObject WhitePiecePrefab;
    public GameObject BlackPiecePrefab;

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

    }

    public void CreateBoard()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if ((i + j) % 2 == 0)
                {
                    if (i < 3)
                        GeneratePiece(i, j, 1);
                    else if (i > 4)
                        GeneratePiece(i, j, 2);
                    else
                        Board[i, j] = null;
                }
                else
                    Board[i, j] = null;
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
            piece.King = true;
            if (piece.Color == 1)
                BlackKings++;
            else
                WhiteKings++;
        }
    }

    public void OnMouseDown()
    {
        CheckMousePostition();
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
        Debug.Log("Mouse position : " + mouseOver);
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
        Piece p = new Piece(x, y, color, go);
        Board[x, y] = p;
    }
}
