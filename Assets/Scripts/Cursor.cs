using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor
{
    public Vector2Int Position;

    public Cursor(int boardWidth, int boardHeight)
    {
        Position = new Vector2Int(boardWidth / 2, boardHeight / 2);
    }

    public void Move(Vector2Int direction, int boardWidth, int boardHeight)
    {
        Vector2Int newPosition = Position + direction;
        newPosition.x = (newPosition.x + boardWidth) % boardWidth;
        newPosition.y = (newPosition.y + boardHeight) % boardHeight;
        Position = newPosition;
    }
}


