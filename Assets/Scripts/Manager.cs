using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    [SerializeField] public GameObject cursorVisual;
    [SerializeField] AudioSource moveSound;
    [SerializeField] AudioSource destroySound;
    [SerializeField] AudioSource upgradeSound;

    public GameObject squarePrefab;
    public GameObject piecePrefab;
    public GameObject chestPrefab;
    public Material whiteMaterial;
    public Material blackMaterial;
    public Cursor cursor;
    public Board board;
    public Image counterFill;
    public Pieces pieces;
    public TMP_Text gameOverText;
    GameObject selectedPiece = null;
    Vector2Int direction;
    Vector2Int selectedPiecePosition;
    Vector2Int cursorPosition;
    Vector2Int destinationPosition;
    Vector2Int chestPosition;
    public int boardWidth = 8;
    public int boardHeight = 8;
    public float maxTime;
    public bool isWhiteTurn; // true = blancas, false = negras
    bool gameOver = false;
    bool isWhitePiece;
    bool isBlackPiece;
    float actualTime;
    float fillAmount;
    float chestSpawnTime = 10f; // Tiempo por turno en segundos
    int deltaX;
    int deltaY;

    void Start()
    {
        isWhiteTurn = true;
        board = new Board();
        cursor = new Cursor(boardWidth, boardHeight);
        cursor.CursorVisual(cursorVisual);
        board.CreateBoard(boardWidth, boardHeight, squarePrefab, transform, whiteMaterial, blackMaterial);
        actualTime = maxTime;

        pieces = new Pieces(piecePrefab, transform, whiteMaterial, blackMaterial, board);
        pieces.InstantiatePieces(boardWidth, boardHeight);

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

        // Comprobar victoria siempre que no haya terminado el juego
        if (!gameOver)
        {
            CheckGameOver();
        }
    }

    IEnumerator SpawnChestCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(chestSpawnTime);

            bool chestPlaced = false;
            int attempts = 0;
            int maxAttempts = 100;

            while (!chestPlaced && attempts < maxAttempts)
            {
                int randomX = Random.Range(0, boardWidth);
                int randomY = Random.Range(0, boardHeight);
                chestPosition = new Vector2Int(randomX, randomY);

                BoardSquare chestSquare = board.squares[chestPosition.x, chestPosition.y];

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

        if (isWhiteTurn)
        {
            if (Input.GetKeyDown(KeyCode.W)) direction = Vector2Int.up;
            if (Input.GetKeyDown(KeyCode.S)) direction = Vector2Int.down;
            if (Input.GetKeyDown(KeyCode.A)) direction = Vector2Int.left;
            if (Input.GetKeyDown(KeyCode.D)) direction = Vector2Int.right;
        }
        else // negras
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) direction = Vector2Int.up;
            if (Input.GetKeyDown(KeyCode.DownArrow)) direction = Vector2Int.down;
            if (Input.GetKeyDown(KeyCode.LeftArrow)) direction = Vector2Int.left;
            if (Input.GetKeyDown(KeyCode.RightArrow)) direction = Vector2Int.right;
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
            if (currentSquare.Piece != null)
            {
                isWhitePiece = currentSquare.Piece.CompareTag("WhitePiece");
                isBlackPiece = currentSquare.Piece.CompareTag("BlackPiece");

                if ((isWhiteTurn && isWhitePiece) || (!isWhiteTurn && isBlackPiece))
                {
                    selectedPiece = currentSquare.Piece;
                    selectedPiecePosition = currentSquare.Position;
                    print("Pieza seleccionada: " + selectedPiece.name + " en la posición " + selectedPiecePosition);
                }
                else
                {
                    print("No puedes seleccionar esta pieza. Es el turno de las " + (isWhiteTurn ? "blancas" : "negras") + ".");
                }
            }
            else
            {
                print("No hay ninguna pieza en esta casilla para seleccionar.");
            }
        }
        else
        {
            if (currentSquare.Piece == null)
            {
                destinationPosition = currentSquare.Position;
                deltaX = Mathf.Abs(destinationPosition.x - selectedPiecePosition.x);
                deltaY = Mathf.Abs(destinationPosition.y - selectedPiecePosition.y);

                if ((deltaX == 1 && deltaY == 0) || (deltaX == 0 && deltaY == 1))
                {
                    selectedPiece.transform.position = new Vector3(destinationPosition.x, 0, destinationPosition.y);

                    board.squares[selectedPiecePosition.x, selectedPiecePosition.y].Piece = null;
                    currentSquare.Piece = selectedPiece;

                    print("Pieza movida a la posición " + destinationPosition);

                    if (currentSquare.HasChest)
                    {
                        pieces.ApplyChestBoost(selectedPiece, currentSquare.ChestType);
                        upgradeSound.Play();
                        DestroyChest(currentSquare);
                    }

                    selectedPiece = null;
                    moveSound.Play();
                }
                else
                {
                    print("Movimiento inválido. Solo puedes mover un cuadro hacia adelante, atrás, izquierda o derecha.");
                }
            }
            else
            {
                GameObject targetPiece = currentSquare.Piece;

                if (targetPiece.tag != selectedPiece.tag)
                {
                    float attackerDamage = pieces.GetPieceDamage(selectedPiece);
                    pieces.DamagePiece(targetPiece, attackerDamage);

                    if (pieces.IsPieceDestroyed(targetPiece))
                    {
                        currentSquare.Piece = null;
                        destroySound.Play();
                        Destroy(targetPiece);
                        print("La pieza enemiga ha sido destruida.");
                        CheckGameOver();
                    }

                    selectedPiece = null;
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
        chestSquare.HasChest = false;
        chestSquare.ChestType = "";

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
        isWhiteTurn = !isWhiteTurn;
        counterFill.color = isWhiteTurn ? Color.white : Color.black;
        print(isWhiteTurn ? "Turno de las blancas" : "Turno de las negras");
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



