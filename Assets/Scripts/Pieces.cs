using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pieces
{
    private GameObject piecePrefab;
    private Transform boardTransform;
    private Material whiteMaterial;
    private Material blackMaterial;
    private Board board;

    private List<GameObject> allPieces = new List<GameObject>();
    private Dictionary<GameObject, Piece> pieceLogicMap = new Dictionary<GameObject, Piece>();

    public Pieces(GameObject piecePrefab, Transform boardTransform, Material whiteMaterial, Material blackMaterial, Board board)
    {
        this.piecePrefab = piecePrefab;
        this.boardTransform = boardTransform;
        this.whiteMaterial = whiteMaterial;
        this.blackMaterial = blackMaterial;
        this.board = board;
    }

    public void InstantiatePieces(int boardWidth, int boardHeight)
    {
        int center = boardWidth / 2;
        int[] positions = { center - 1, center, center + 1 };

        // Blancas en y=0
        foreach (int x in positions)
        {
            if (x >= 0 && x < boardWidth)
                CreatePiece(new Vector2Int(x, 0), whiteMaterial, "WhitePiece");
        }

        // Negras en y=4
        foreach (int x in positions)
        {
            if (x >= 0 && x < boardWidth && boardHeight > 4)
                CreatePiece(new Vector2Int(x, 4), blackMaterial, "BlackPiece");
        }
    }

    private void CreatePiece(Vector2Int position, Material material, string tag)
    {
        // Obtener la posición local de la casilla en el tablero
        GameObject squareObj = board.squares[position.x, position.y].SquareObject;
        Vector3 localPosition = squareObj.transform.localPosition;

        GameObject piece = GameObject.Instantiate(piecePrefab, localPosition, Quaternion.identity, boardTransform);
        piece.GetComponentInChildren<Renderer>().material = material;
        piece.tag = tag;

        // Asignar lógica de la pieza
        Piece pieceLogic = new Piece(10f); // Salud inicial de 10
        pieceLogicMap[piece] = pieceLogic;

        // Agregar la pieza al tablero
        board.squares[position.x, position.y].Piece = piece;

        // Guardar la pieza en la lista
        allPieces.Add(piece);
    }

    public void DamagePiece(GameObject piece, float damage)
    {
        if (pieceLogicMap.TryGetValue(piece, out Piece pieceLogic))
        {
            pieceLogic.TakeDamage(damage);
            UpdatePieceHeight(piece, pieceLogic);
        }
    }

    public void HealPiece(GameObject piece, float healAmount)
    {
        if (pieceLogicMap.TryGetValue(piece, out Piece pieceLogic))
        {
            pieceLogic.Heal(healAmount);
            UpdatePieceHeight(piece, pieceLogic);
        }
    }

    private void UpdatePieceHeight(GameObject piece, Piece pieceLogic)
    {
        float healthFactor = pieceLogic.GetHealthFactor();
        Vector3 scale = piece.transform.localScale;
        piece.transform.localScale = new Vector3(scale.x, healthFactor, scale.z);
    }

    public bool IsPieceDestroyed(GameObject piece)
    {
        if (pieceLogicMap.TryGetValue(piece, out Piece pieceLogic))
        {
            return pieceLogic.IsDestroyed();
        }
        return false;
    }

    public void BoostPieceHealth(GameObject piece, float boostAmount)
    {
        if (pieceLogicMap.TryGetValue(piece, out Piece pieceLogic))
        {
            pieceLogic.Heal(boostAmount);
            UpdatePieceHeight(piece, pieceLogic);
        }
    }

    public void BoostPieceDamage(GameObject piece, float boostAmount)
    {
        if (pieceLogicMap.TryGetValue(piece, out Piece pieceLogic))
        {
            pieceLogic.IncreaseDamage(boostAmount);
        }
    }

    // Aplica boost según el tipo de cofre
    public void ApplyChestBoost(GameObject piece, string chestType)
    {
        if (chestType == "Health")
        {
            BoostPieceHealth(piece, 5f);
        }
        else if (chestType == "Damage")
        {
            BoostPieceDamage(piece, 2f); 
        }
    }

    public float GetPieceDamage(GameObject piece)
    {
        if (pieceLogicMap.TryGetValue(piece, out Piece pieceLogic))
        {
            return pieceLogic.GetDamage();
        }
        return 0f;
    }

    public void ExampleUsage(GameObject targetPiece, GameObject selectedPiece)
    {
        // Obtener el daño real de la pieza atacante
        float attackerDamage = GetPieceDamage(selectedPiece);
        DamagePiece(targetPiece, attackerDamage);
    }
}









