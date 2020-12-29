using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece
{
    public int Row, Col;
    public int Color;
    public bool King;
    public int Direction;
    private Vector3 boardOffset = new Vector3(-4f, 0, -4f);
    private Vector3 pieceOffset = new Vector3(0.5f, 0, 0.5f);
    public GameObject Go;
    private float posX, posZ;

    public Piece(int row, int col, int color, GameObject go)
    {
        Row = row;
        Col = col;
        Color = color;
        Go = go;
        King = false;
        if (color == 1)
            Direction = 1;
        else
            Direction = -1;
        MovePiece();
    }

    public void Move(int row, int col)
    {
        Row = row;
        Col = col;
        MovePiece();
    }

    public void calcPosition()
    {
        posX = Col;
        posZ = Row;
    }

    public void MovePiece()
    {
        calcPosition();
        Go.transform.position = ((Vector3.right * posX) + (Vector3.forward * posZ) + boardOffset + pieceOffset);
    }
}