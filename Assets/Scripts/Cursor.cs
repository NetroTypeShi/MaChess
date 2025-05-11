using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor
{
    public Vector2Int Position;
    public GameObject cursorVisualInstance;
    Manager manager;
    public Cursor(int boardWidth, int boardHeight)
    {
        Position = new Vector2Int(boardWidth / 2, boardHeight / 2);
    }

    public void Move(Vector2Int direction, int boardWidth, int boardHeight)
    {
        Vector2Int newPosition = Position + direction;
        if (newPosition.x < 0) { newPosition.x = boardWidth - 1; }

        else if (newPosition.x >= boardWidth) { newPosition.x = 0;}

        if (newPosition.y < 0) { newPosition.y = boardHeight - 1; }             
        else if (newPosition.y >= boardHeight) { newPosition.y = 0; }
        //% = resto de la divisi�n
        Position = newPosition;
    }
    public void CursorVisual(GameObject prefab)
    {
        cursorVisualInstance = GameObject.Instantiate(prefab, new Vector3(Position.x, 0, Position.y), Quaternion.identity);
    }
    public void UpdateCursorVisual()
    {
        cursorVisualInstance.transform.position = new Vector3(Position.x, 0, Position.y);
    }

    public void cursorColor()
    {
        /*
        if(manager.whiteTurn == true)
        {
            cursorVisualInstance.GetComponent<MeshRenderer>().material.color = Color.red;
        }
        else if (manager.blackTurn == true)
        {
            cursorVisualInstance.GetComponent<MeshRenderer>().material.color = Color.blue;
        }
        */
    }
}


