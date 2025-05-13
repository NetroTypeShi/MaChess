using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [SerializeField] public GameObject cursorVisual;
    public GameObject squarePrefab;
    public GameObject piecePrefab; 
    public Material whiteMaterial;
    public Material blackMaterial;
    public Cursor cursor;
    public Board board;
    public Image counterFill;
    public Pieces pieces; 
    Vector2Int direction;
    public int boardWidth = 8;
    public int boardHeight = 8;
    public float maxTime;
    public float actualTime;
    public bool whiteTurn;
    public bool blackTurn;
    float fillAmount;


    void Start()
    {
        whiteTurn = true;
        blackTurn = false;
        board = new Board();
        cursor = new Cursor(boardWidth, boardHeight);
        cursor.CursorVisual(cursorVisual);
        board.CreateBoard(boardWidth, boardHeight, squarePrefab, transform, whiteMaterial, blackMaterial);
        actualTime = maxTime;

        pieces = new Pieces(piecePrefab, transform, whiteMaterial, blackMaterial);
        pieces.InstantiatePieces(boardWidth, boardHeight);
    }

    void Update()
    {
        HandleCursorInput();
        TimeBar();
        cursor.cursorColor();
    }

    private void HandleCursorInput()
    {
        direction = Vector2Int.zero;

        if (whiteTurn == true)
        {
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
        }
        if (blackTurn == true)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                direction = Vector2Int.up;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                direction = Vector2Int.down;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                direction = Vector2Int.left;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                direction = Vector2Int.right;
            }
        }


        if (direction != Vector2Int.zero)
        {
            cursor.Move(direction, boardWidth, boardHeight);
            cursor.UpdateCursorVisual();
            print("Cursor moved to:" + cursor.Position);
        }
    }

    private void TimeBar()
    {
        actualTime -= Time.deltaTime;
        counterFill.fillAmount = actualTime / maxTime;
        if (actualTime <= 0)
        {
            if (whiteTurn == true)
            {
                whiteTurn = false;
                blackTurn = true;
                counterFill.color = Color.black;
                print("Black turn");
            }
            else if (blackTurn == true)
            {
                whiteTurn = true;
                blackTurn = false;
                counterFill.color = Color.white;
                print("White turn");
            }
            actualTime = maxTime;

        }
    }
}


