using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] GameObject cursorVisual;
    public GameObject squarePrefab;
    public Material whiteMaterial;
    public Material blackMaterial;
    public int boardWidth = 8;
    public int boardHeight = 8;
    private Cursor cursor;
    public Board board;


    void Start()
    {
        board = new Board();
        cursorVisual = Instantiate(cursorVisual, new Vector3(cursor.Position.x, cursor.Position.y, 0), Quaternion.identity);
        board.CreateBoard(boardWidth, boardHeight, squarePrefab, transform, whiteMaterial, blackMaterial);
        cursor = new Cursor(boardWidth, boardHeight);

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

            print("Cursor moved to:" + cursor.Position);
        }
    }
}

