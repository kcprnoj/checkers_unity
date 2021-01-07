using System.Collections.Generic;

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
                        }
                    }
                }
            }
        }

        return BestMove;
    }

    private int ScoreMove(KeyValuePair<KeyValuePair<int, int>, List<Piece>> move, Piece piece)
    {
        if (move.Key.Key == -1)
        {
            return -1;
        }
        int score = move.Value.Count;

        return score;
    }
}
