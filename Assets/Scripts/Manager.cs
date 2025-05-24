using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    [SerializeField] public GameObject cursorVisual;
    public GameObject squarePrefab;
    public GameObject piecePrefab;
    public GameObject chestPrefab;
    public Material whiteMaterial;
    public Material blackMaterial;
    public Cursor cursor;
    public Board board;
    public Image counterFill;
    public Pieces pieces;
    public TMP_Text gameOverText; // Added for displaying game over message
    GameObject selectedPiece = null;
    Vector2Int direction;
    Vector2Int selectedPiecePosition;
    Vector2Int cursorPosition;
    Vector2Int destinationPosition;
    Vector2Int chestPosition; // Posición actual del cofre
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
    bool gameOver = false; // Variable para controlar el estado del juego

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

        // Iniciar la corrutina para spawnear cofres
        StartCoroutine(SpawnChestCoroutine());
    }

    void Update()
    {
        if (gameOver)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            return;
        }

        HandleCursorInput();
        TimeBar();
        cursor.cursorColor();

        cursorPosition = cursor.Position;
        BoardSquare currentSquare = board.squares[cursorPosition.x, cursorPosition.y];

        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandlePieceSelectionOrMovement(currentSquare);
        }

        CheckGameOver();
    }

    IEnumerator SpawnChestCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f); // Esperar 10 segundos

            bool chestPlaced = false;
            int attempts = 0;
            int maxAttempts = 100; // Evita bucles infinitos

            while (!chestPlaced && attempts < maxAttempts)
            {
                int randomX = Random.Range(0, boardWidth);
                int randomY = Random.Range(0, boardHeight);
                chestPosition = new Vector2Int(randomX, randomY);

                BoardSquare chestSquare = board.squares[chestPosition.x, chestPosition.y];

                // Solo colocar cofre si la casilla NO tiene pieza ni cofre
                if (chestSquare.Piece == null && !chestSquare.HasChest)
                {
                    chestSquare.HasChest = true;
                    chestSquare.ChestType = (Random.value > 0.5f) ? "Health" : "Damage";

                    Vector3 chestWorldPosition = new Vector3(chestPosition.x, 0.5f, chestPosition.y);
                    GameObject chestObj = Instantiate(chestPrefab, chestWorldPosition, Quaternion.identity, transform);
                    chestObj.tag = "Chest";

                    print($"Cofre spawneado en la posición {chestPosition} de tipo {chestSquare.ChestType}");
                    chestPlaced = true;
                }
                attempts++;
            }
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
                // Verificar si la pieza es su turno
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

                    // Verificar si la casilla tiene un cofre
                    if (currentSquare.HasChest)
                    {
                        // Aplicar el boost correspondiente
                        pieces.ApplyChestBoost(selectedPiece, currentSquare.ChestType);
                        DestroyChest(currentSquare);
                    }

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
                // Si la casilla de destino tiene una pieza enemiga, infligir daño
                GameObject targetPiece = currentSquare.Piece;

                if (targetPiece.tag != selectedPiece.tag)
                {
                    // Obtener el daño real de la pieza atacante
                    float attackerDamage = pieces.GetPieceDamage(selectedPiece);
                    pieces.DamagePiece(targetPiece, attackerDamage);

                    // Verificar si la pieza enemiga ha sido destruida
                    if (pieces.IsPieceDestroyed(targetPiece))
                    {
                        // Elimina la referencia en el tablero
                        currentSquare.Piece = null;
                        // Destruye el GameObject de la pieza
                        Destroy(targetPiece);
                        print("La pieza enemiga ha sido destruida.");
                        // Comprueba si alguien ha ganado
                        CheckGameOver();
                    }

                    // Deseleccionar la pieza
                    selectedPiece = null;

                    // Cambiar el turno
                    ChangeTurn();
                }
                else
                {
                    print("No puedes atacar a una pieza de tu propio equipo.");
                }
            }
        }
    }

    void DestroyChest(BoardSquare chestSquare)
    {
        // Eliminar el cofre de la casilla
        chestSquare.HasChest = false;
        chestSquare.ChestType = "";

        // Buscar y destruir el cofre en la posición actual
        GameObject[] chests = GameObject.FindGameObjectsWithTag("Chest");
        foreach (GameObject chest in chests)
        {
            Vector3 chestPos = chest.transform.position;
            if (Mathf.RoundToInt(chestPos.x) == chestSquare.Position.x && Mathf.RoundToInt(chestPos.z) == chestSquare.Position.y)
            {
                Destroy(chest);
                print("El cofre ha sido destruido automáticamente.");
                break;
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

    private void CheckGameOver()
    {
        // Busca si quedan piezas blancas y negras
        bool whiteExists = GameObject.FindGameObjectWithTag("WhitePiece") != null;
        bool blackExists = GameObject.FindGameObjectWithTag("BlackPiece") != null;

        if (!whiteExists)
        {
            ShowGameOver("¡Negras ganaron!\nPresiona P para reiniciar");
        }
        else if (!blackExists)
        {
            ShowGameOver("¡Blancas ganaron!\nPresiona P para reiniciar");
        }
    }

    private void ShowGameOver(string message)
    {
        gameOver = true;
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
            gameOverText.text = message;
        }
    }
}



