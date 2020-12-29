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

    public CheckersBoard()
    {
        BlackLeft = 12;
        WhiteLeft = 12;
        BlackKings = 0;
        WhiteKings = 0;
        Board = new Piece[8, 8];
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
                        Board[i, j] = new Piece(i, j, 1);
                    else if (i > 4)
                        Board[i, j] = new Piece(i, j, 2);
                    else
                        Board[i, j] = new Piece(i, j, 0);
                }
                else
                    Board[i, j] = new Piece(i, j, 0);
            }
        }
    }

    public void Draw()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Board[i, j].Draw();
            }
            Console.Write("\n");
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
}
