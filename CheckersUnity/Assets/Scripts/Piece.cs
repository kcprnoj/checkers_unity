using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public int Row, Col;
    public int Color;
    public bool King;
    public int Direction;
    public GameObject Go;
    public Vector3 boardOffset = new Vector3(-4.0f, 0, -4.0f);
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
    }

    public void Move(int row, int col)
    {
        Row = row;
        Col = col;
    }

    public void calcPosition()
    {
        posX = Col;
        posZ = Row;
    }

    public void MovePiece()
    {
        calcPosition();
        Go.transform.position = (Vector3.right * posX) + (Vector3.forward * posZ);
    }
}