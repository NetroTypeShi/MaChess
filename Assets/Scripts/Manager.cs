using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] public GameObject cursorVisual;
    public GameObject squarePrefab;
    public Material whiteMaterial;
    public Material blackMaterial;
    public int boardWidth = 8;
    public int boardHeight = 8;
    public Cursor cursor;
    public Board board;


    void Start()
    {
        board = new Board();
        cursor = new Cursor(boardWidth, boardHeight);
        cursor.CursorVisual(cursorVisual);
        board.CreateBoard(boardWidth, boardHeight, squarePrefab, transform, whiteMaterial, blackMaterial);

    }

    void Update()
    {
        HandleCursorInput();
        
    }
    private void HandleCursorInput()
    {
        Vector2Int direction = Vector2Int.zero;
        

        if (Input.GetKeyDown(KeyCode.W))
        {
            direction = Vector2Int.up;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            direction = Vector2Int.down;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            direction = Vector2Int.left;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            direction = Vector2Int.right;
        }

        if (direction != Vector2Int.zero)
        {
            cursor.Move(direction, boardWidth, boardHeight);
            cursor.UpdateCursorVisual();
            print("Cursor moved to:" + cursor.Position);
        }
    }
    
}

