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
    GameObject selectedPiece = null;
    Vector2Int direction;
    Vector2Int selectedPiecePosition;
    Vector2Int cursorPosition;
    Vector2Int destinationPosition;
    public int boardWidth = 8;
    public int boardHeight = 8;
    public float maxTime;
    public float actualTime;
    public bool whiteTurn;
    public bool blackTurn;
    float fillAmount;
    bool isWhitePiece;
    bool isBlackPiece;
    int deltaX;
    int deltaY;

    void Start()
    {
        whiteTurn = true;
        blackTurn = false;
        board = new Board();
        cursor = new Cursor(boardWidth, boardHeight);
        cursor.CursorVisual(cursorVisual);
        board.CreateBoard(boardWidth, boardHeight, squarePrefab, transform, whiteMaterial, blackMaterial);
        actualTime = maxTime;

        pieces = new Pieces(piecePrefab, transform, whiteMaterial, blackMaterial, board);
        pieces.InstantiatePieces(boardWidth, boardHeight);
    }

    void Update()
    {
        HandleCursorInput();
        TimeBar();
        cursor.cursorColor();

        cursorPosition = cursor.Position;
        BoardSquare currentSquare = board.squares[cursorPosition.x, cursorPosition.y];

        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandlePieceSelectionOrMovement(currentSquare);
        }
    }

    void HandleCursorInput()
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

    void HandlePieceSelectionOrMovement(BoardSquare currentSquare)
    {
        if (selectedPiece == null)
        {
            // Seleccionar una pieza si está presente en la casilla actual
            if (currentSquare.Piece != null)
            {
                // Verificar si la pieza pertenece al turno actual
                isWhitePiece = currentSquare.Piece.CompareTag("WhitePiece");
                isBlackPiece = currentSquare.Piece.CompareTag("BlackPiece");

                if ((whiteTurn && isWhitePiece) || (blackTurn && isBlackPiece))
                {
                    selectedPiece = currentSquare.Piece;
                    selectedPiecePosition = currentSquare.Position;
                    print("Pieza seleccionada: " + selectedPiece.name + " en la posición " + selectedPiecePosition);
                }
                else
                {
                    print("No puedes seleccionar esta pieza. Es el turno de las " + (whiteTurn ? "blancas" : "negras") + ".");
                }
            }
            else
            {
                print("No hay ninguna pieza en esta casilla para seleccionar.");
            }
        }
        else
        {
            // Mover la pieza seleccionada a la nueva casilla
            if (currentSquare.Piece == null)
            {
                // Validar que el movimiento sea de un cuadro en una dirección válida
                destinationPosition = currentSquare.Position;
                deltaX = Mathf.Abs(destinationPosition.x - selectedPiecePosition.x);
                deltaY = Mathf.Abs(destinationPosition.y - selectedPiecePosition.y);

                if ((deltaX == 1 && deltaY == 0) || (deltaX == 0 && deltaY == 1))
                {
                    // Movimiento válido: actualizar la posición de la pieza seleccionada
                    selectedPiece.transform.position = new Vector3(destinationPosition.x, 0, destinationPosition.y);

                    // Actualizar las referencias en las casillas
                    board.squares[selectedPiecePosition.x, selectedPiecePosition.y].Piece = null;
                    currentSquare.Piece = selectedPiece;

                    print("Pieza movida a la posición " + destinationPosition);

                    // Deseleccionar la pieza
                    selectedPiece = null;

                    // Cambiar el turno
                    ChangeTurn();
                }
                else
                {
                    print("Movimiento inválido. Solo puedes mover un cuadro hacia adelante, atrás, izquierda o derecha.");
                }
            }
            else
            {
                print("La casilla de destino ya tiene una pieza. Movimiento no permitido.");
            }
        }
    }

    void ChangeTurn()
    {
        if (whiteTurn)
        {
            whiteTurn = false;
            blackTurn = true;
            counterFill.color = Color.black;
            print("Turno de las negras");
        }
        else
        {
            whiteTurn = true;
            blackTurn = false;
            counterFill.color = Color.white;
            print("Turno de las blancas");
        }

        actualTime = maxTime;
    }

    void TimeBar()
    {
        actualTime -= Time.deltaTime;
        counterFill.fillAmount = actualTime / maxTime;

        if (actualTime <= 0)
        {
            ChangeTurn();
        }
    }
}
