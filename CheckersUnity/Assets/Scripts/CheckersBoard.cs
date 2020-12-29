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

    public void CreateBoard()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if ((i + j) % 2 == 1)
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

    public void GeneratePiece(int x, int y, int color)
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
        p.MovePiece();
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
}
