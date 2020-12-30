using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece
{
    public int Row, Col;
    private float posX, posZ;

    public int Color;
    public int Direction;

    public bool Active;
    public bool King;

    private Vector3 boardOffset = new Vector3(-4f, 0, -4f);
    private Vector3 pieceOffset = new Vector3(0.5f, 0, 0.5f);

    public GameObject PieceGameObject;

    public Piece(int row, int col, int color, GameObject gameObjecty)
    {
        Row = row;
        Col = col;
        Color = color;
        PieceGameObject = gameObjecty;
        King = false;
        Active = false;
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
        PieceGameObject.transform.position = ((Vector3.right * posX) + (Vector3.forward * posZ) + boardOffset + pieceOffset);
    }
}