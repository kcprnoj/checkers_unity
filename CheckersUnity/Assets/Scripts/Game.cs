using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Game
{
    public CheckersBoard Board;
    public PieceColor Turn;
    public Dictionary<KeyValuePair<int, int>, List<Piece>> ValidMoves;

    public Game(CheckersBoard board)
    {
        Board = board;
        Turn = PieceColor.Black;
    }

    public void ResetGame()
    {
        Board.ResetBoard();
        Turn = PieceColor.Black;
        ValidMoves = null;
    }

    public bool Select(int row, int col)
    {
        if(Board.SelectedPawn != null)
        {
            if(!Move(row, col))
            {
                Board.SelectedPawn = null;
                Select(row, col);
            }
        }

        Piece piece = Board.GetPiece(row, col);
        if (piece != null && piece.Color == Turn)
        {
            Board.SelectedPawn = piece;
            ValidMoves = GetValidMoves(piece);
            Board.ChangeMaterial(piece, true);
            Board.DrawValidMoves();
            return true;
        }

        return false;
    }

    private bool Move(int row, int col)
    {
        Piece piece = Board.GetPiece(row, col);
        if (Board.SelectedPawn != null && piece == null && IsValidMove(row, col))
        {
            Board.Move(Board.SelectedPawn, row, col);
            ChangeTurn();
        }
        else
        {
            return false;
        }

        return true;
    }

    private void ChangeTurn()
    {
        if (Turn == PieceColor.Black)
            Turn = PieceColor.White;
        else
            Turn = PieceColor.Black;

        Board.DeleteValidMoves();
        Board.ChangeMaterial(Board.SelectedPawn, false);
    }

    private bool IsValidMove(int row, int col)
    {
        if (ValidMoves == null)
            return false;
        return ValidMoves.ContainsKey(new KeyValuePair<int, int>(row, col));
    }

    private Dictionary<KeyValuePair<int, int>, List<Piece>> GetValidMoves(Piece piece)
    {
        Dictionary<KeyValuePair<int, int>, List<Piece>> moves = new Dictionary<KeyValuePair<int, int>, List<Piece>>();
        int left = piece.Col - 1;
        int right = piece.Col + 1;
        int row = piece.Row;

        if (piece.Color == PieceColor.Black || piece.King)
        {
            AddRange(moves, TraverseLeft(row - 1, System.Math.Max(row - 3, -1), -1, piece.Color, left));
            AddRange(moves, TraverseRight(row - 1, System.Math.Max(row - 3, -1), -1, piece.Color, right));
        }

        if (piece.Color == PieceColor.White || piece.King)
        {
            AddRange(moves, TraverseLeft(row + 1, System.Math.Min(row + 3, 8), 1, piece.Color, left));
            AddRange(moves, TraverseRight(row + 1, System.Math.Min(row + 3, 8), 1, piece.Color, right));
        }

        return moves;
    }

    private Dictionary<KeyValuePair<int, int>, List<Piece>>
        TraverseLeft(int start, int stop, int step, PieceColor color, int left, List<Piece> skipped = null)
    {
        Dictionary<KeyValuePair<int, int>, List<Piece>> moves = new Dictionary<KeyValuePair<int, int>, List<Piece>>();
        List<Piece> last = new List<Piece>();

        if(skipped == null)
        {
            skipped = new List<Piece>();
        }

        for (int i = start; (step == -1 && i > stop) || (step == 1 && i < stop); i += step)
        {
            if (left < 0)
                break;

            Piece current = Board.GetPiece(i, left);
            if (current == null)
            {
                if (skipped.Count != 0 && last.Count == 0)
                    break;
                else if (skipped.Count != 0)
                {
                    List<Piece> temp = new List<Piece>();
                    temp.AddRange(last);
                    temp.AddRange(skipped);
                    moves.Add(new KeyValuePair<int, int>(i, left), temp);
                }
                else
                    moves.Add(new KeyValuePair<int, int>(i, left), last);

                if (last.Count != 0)
                {
                    int row;
                    if (step == -1)
                        row = System.Math.Max(i - 3, 0);
                    else
                        row = System.Math.Min(i + 3, 8);
                    AddRange(moves, TraverseLeft(i + step, row, step, color, left - 1, last));
                    AddRange(moves, TraverseRight(i + step, row, step, color, left + 1, last));
                }
                break;
            }
            else if (current.Color == color)
                break;
            else
                last = new List<Piece>() { current };
            left--;
        }

        return moves;
    }

    private Dictionary<KeyValuePair<int, int>, List<Piece>>
        TraverseRight(int start, int stop, int step, PieceColor color, int right, List<Piece> skipped = null)
    {
        Dictionary<KeyValuePair<int, int>, List<Piece>> moves = new Dictionary<KeyValuePair<int, int>, List<Piece>>();
        List<Piece> last = new List<Piece>();

        if (skipped == null)
        {
            skipped = new List<Piece>();
        }

        for (int i = start; (step == -1 && i > stop) || (step == 1 && i < stop) ; i += step)
        {
            if (right >= 8)
                break;

            Piece current = Board.GetPiece(i, right);
            if (current == null)
            {
                if (skipped.Count != 0 && last.Count == 0)
                    break;
                else if (skipped.Count != 0)
                {
                    List<Piece> temp = new List<Piece>();
                    temp.AddRange(last);
                    temp.AddRange(skipped);
                    moves.Add(new KeyValuePair<int, int>(i, right), temp);
                }
                else
                    moves.Add(new KeyValuePair<int, int>(i, right), last);

                if (last.Count != 0)
                {
                    int row;
                    if (step == -1)
                        row = System.Math.Max(i - 3, 0);
                    else
                        row = System.Math.Min(i + 3, 8);
                    AddRange(moves, TraverseLeft(i + step, row, step, color, right - 1, last));
                    AddRange(moves, TraverseRight(i + step, row, step, color, right + 1, last));
                }
                break;
            }
            else if (current.Color == color)
                break;
            else
                last = new List<Piece>() { current };
            right++;
        }

        return moves;
    }

    public static void AddRange<T, S>(Dictionary<T, S> source, Dictionary<T, S> collection)
    {
        if (collection == null)
        {
            return;
        }

        foreach (var item in collection)
        {
            if (!source.ContainsKey(item.Key))
            {
                source.Add(item.Key, item.Value);
            }
        }
    }
}

