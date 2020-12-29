using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public class Piece
    {
        public int Row;
        public int Col;
        public int Color;
        public bool King;
        public int Direction;

        public Piece(int row, int col, int color)
        {
            Row = row;
            Col = col;
            Color = color;
            King = false;
            if (color == 1)
                Direction = 1;
            else
                Direction = -1;
        }

        public void Draw()
        {
            
        }

        public void Move(int row, int col)
        {
            Row = row;
            Col = col;
        }
    }
}