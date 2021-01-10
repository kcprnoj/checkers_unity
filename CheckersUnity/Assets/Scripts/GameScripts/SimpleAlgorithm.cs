using System.Collections.Generic;
using UnityEngine;

public class SimpleAlgoirthm
{
    public PieceColor Color;
    public KeyValuePair<KeyValuePair<int, int>, List<Piece>> BestMove;
    public Piece BestPiece;

    public SimpleAlgoirthm(PieceColor color)
    {
        Color = color;
    }

    public KeyValuePair<KeyValuePair<int, int>, List<Piece>> 
        FindBestMove(Piece[,] board)
    {
        BestMove = new KeyValuePair<KeyValuePair<int, int>, List<Piece>>(new KeyValuePair<int, int>(-1, -1), null);
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (board[i, j] != null && board[i, j].Color == Color && board[i, j].ValidMoves != null)
                {
                    foreach(KeyValuePair<KeyValuePair<int, int>, List<Piece>> move in board[i, j].ValidMoves)
                    {
                        if (ScoreMove(move, board[i, j]) > ScoreMove(BestMove, BestPiece))
                        {
                            BestMove = move;
                            BestPiece = board[i, j];
                            Debug.Log(ScoreMove(BestMove, BestPiece));
                        }
                    }
                }
            }
        }

        return BestMove;
    }

    private double ScoreMove(KeyValuePair<KeyValuePair<int, int>, List<Piece>> move, Piece piece)
    {
        if (move.Key.Key == -1)
        {
            return -1;
        }
        double score = move.Value.Count;

        if (Color == PieceColor.Black)
            score += piece.Row / 16.0;
        else
            score -= piece.Row / 16.0;

        return score;
    }
}
