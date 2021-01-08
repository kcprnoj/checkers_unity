using System.Collections.Generic;
using UnityEngine;

public class Game
{
    public CheckersBoard Board;
    public PieceColor Turn;
    public PieceColor Player;
    public Dictionary<KeyValuePair<int, int>, List<Piece>> ValidMoves;
    public SimpleAlgoirthm AI;
    public bool End;

    public Game(CheckersBoard board)
    {
        Board = board;
        Turn = PieceColor.Black;


        if (UIData.Color == "Black")
        {
            Player = PieceColor.Black;
        }
        else
        {
            Player = PieceColor.White;
        }

        if (UIData.GameMode == "single")
        {
            AI = new SimpleAlgoirthm((Player == PieceColor.White) ? PieceColor.Black : PieceColor.White) ;
        }
    }

    public bool Select(int row, int col)
    {
        if (End)
            return false;
        UpdateValidMoves();
        if (Board.SelectedPawn != null)
        {
            if(!Move(row, col))
            {
                Board.ChangeMaterial(Board.SelectedPawn, false, false);
                Board.DeleteValidMoves();
                Board.SelectedPawn = null;
                Select(row, col);
            }
        }

        Piece piece = Board.GetPiece(row, col);
        if (piece != null && piece.Color == Turn)
        {
            Board.SelectedPawn = piece;
            ValidMoves = piece.ValidMoves;
            Board.ChangeMaterial(piece, true, false);
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
            Board.RemovePieces(ValidMoves[new KeyValuePair<int, int>(row, col)]);
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
        Board.ShowChosenPieces(false);
        if (Turn == PieceColor.Black)
            Turn = PieceColor.White;
        else
            Turn = PieceColor.Black;

        Board.DeleteValidMoves();
        Board.ChangeMaterial(Board.SelectedPawn, false, false);
        Board.CheckWinner();
        UpdateValidMoves();
        Debug.Log("Turn : " + Turn);
    }

    public void UpdateValidMoves()
    {
        bool skip = false;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Piece piece = Board.GetPiece(i, j);
                if (piece != null && piece.Color == Turn)
                {
                    piece.ValidMoves = GetValidMoves(piece);
                    if (HasSkip(piece))
                        skip = true;
                }
            }
        }

        if (skip)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    DeleteMovesWithoutSkip(Board.GetPiece(i, j));
                }
            }
        }

        if (!HasValidMoves())
        {
            Board.LoseGame();
        }

        Board.ShowChosenPieces(true);
    }

    private bool HasSkip(Piece piece)
    {
        if (piece.ValidMoves == null || piece == null)
            return false;

        foreach (KeyValuePair<KeyValuePair<int, int>, List<Piece>> move in piece.ValidMoves)
        {
            if (move.Value == null)
                continue;
            else if (move.Value.Count != 0)
                return true;
        }

        return false;
    }

    private void DeleteMovesWithoutSkip(Piece piece)
    {
        List<KeyValuePair<int, int>> movesToDelete = new List<KeyValuePair<int, int>>();
        if (piece == null || piece.ValidMoves == null)
        {
            return;
        }

        foreach (KeyValuePair<KeyValuePair<int, int>, List<Piece>> move in piece.ValidMoves)
        {
            if (move.Value == null || move.Value.Count == 0)
                movesToDelete.Add(move.Key);
        }

        foreach (KeyValuePair<int, int> move in movesToDelete)
        {
            piece.ValidMoves.Remove(move);
        }
    }

    private bool IsValidMove(int row, int col)
    {
        if (ValidMoves == null)
            return false;
        return ValidMoves.ContainsKey(new KeyValuePair<int, int>(row, col));
    }

    private bool HasValidMoves()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Piece piece = Board.GetPiece(i, j);
                if (piece != null && piece.Color == Turn)
                {
                    if (piece.ValidMoves == null)
                        continue;
                    else if (piece.ValidMoves.Count != 0)
                        return true;
                }
            }
        }
        return false;
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

    public Dictionary<KeyValuePair<int, int>, List<Piece>>
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

    public Dictionary<KeyValuePair<int, int>, List<Piece>>
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

