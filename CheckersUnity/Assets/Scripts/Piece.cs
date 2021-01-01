using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PieceColor
{
    White,
    Black
}

public class Piece
{
    public int Row, Col;
    private float posX, posZ;

    public PieceColor Color;

    public bool King;

    private Vector3 boardOffset = new Vector3(-4f, 0, -4f);
    private Vector3 pieceOffset = new Vector3(0.5f, 0, 0.5f);

    public GameObject PieceGameObject;
    public Dictionary<KeyValuePair<int, int>, List<Piece>> ValidMoves;

    public Piece(int row, int col, GameObject gameObject)
    {
        Row = row;
        Col = col;
        PieceGameObject = gameObject;
        MovePiece();
    }

    public Piece(int row, int col, PieceColor color, GameObject gameObjecty)
    {
        Row = row;
        Col = col;
        Color = color;
        PieceGameObject = gameObjecty;
        King = false;
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
        posX = Row;
        posZ = Col;
    }

    public void MovePiece()
    {
        calcPosition();
        PieceGameObject.transform.position = ((Vector3.right * posX) + (Vector3.forward * posZ) + boardOffset + pieceOffset);
        float yRotation =  Camera.main.transform.eulerAngles.y;
        PieceGameObject.transform.eulerAngles = new Vector3(PieceGameObject.transform.eulerAngles.x, -yRotation, PieceGameObject.transform.eulerAngles.z);
    }
}