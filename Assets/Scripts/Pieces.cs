using UnityEngine;
using System.Collections.Generic;

public class Pieces
{
    private GameObject piecePrefab;
    private Transform boardTransform;
    private Material whiteMaterial;
    private Material blackMaterial;
    private Board board;

    // Diccionario para asociar GameObjects con la lógica de las piezas
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
        int middle = boardHeight / 2;

        // Instanciar piezas blancas
        CreatePiece(new Vector2Int(middle, 0), whiteMaterial, "WhitePiece");
        CreatePiece(new Vector2Int(middle + 1, 0), whiteMaterial, "WhitePiece");
        CreatePiece(new Vector2Int(middle - 1, 0), whiteMaterial, "WhitePiece");

        // Instanciar piezas negras
        CreatePiece(new Vector2Int(middle, 4), blackMaterial, "BlackPiece");
        CreatePiece(new Vector2Int(middle + 1, 4), blackMaterial, "BlackPiece");
        CreatePiece(new Vector2Int(middle - 1, 4), blackMaterial, "BlackPiece");
    }

    private void CreatePiece(Vector2Int position, Material material, string tag)
    {
        Vector3 worldPosition = new Vector3(position.x, 0, position.y);

        GameObject piece = GameObject.Instantiate(piecePrefab, worldPosition, Quaternion.identity, boardTransform);

        piece.GetComponentInChildren<MeshRenderer>().material = material;
        piece.tag = tag;

        // Crear la lógica de la pieza y asociarla con el GameObject
        Piece pieceLogic = new Piece(10); // Vida inicial de 10
        pieceLogicMap[piece] = pieceLogic; // Asociar la lógica con el GameObject

        // Asociar la pieza con la casilla
        BoardSquare square = board.squares[position.x, position.y];
        square.Piece = piece;

        // Almacenar la escala inicial de la pieza
        piece.transform.localScale = new Vector3(0.9f, 1f, 0.9f); // Escala inicial predeterminada
    }


    public void DamagePiece(GameObject piece, float damage)
    {
        if (pieceLogicMap.TryGetValue(piece, out Piece pieceLogic))
        {
            pieceLogic.TakeDamage(damage);
            UpdatePieceHeight(piece, pieceLogic);

            if (pieceLogic.IsDestroyed())
            {
                Object.Destroy(piece);
                pieceLogicMap.Remove(piece); // Eliminar del diccionario
            }
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
        Vector3 initialScale = new Vector3(0.9f, 1f, 0.9f); // Escala inicial predeterminada
        piece.transform.localScale = new Vector3(initialScale.x, initialScale.y * healthFactor, initialScale.z);
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
            pieceLogic.Heal(boostAmount); // Aumenta la salud de la pieza
            UpdatePieceHeight(piece, pieceLogic); // Actualiza la altura de la pieza en función de su salud
            Debug.Log($"La salud de la pieza ha sido aumentada en {boostAmount} puntos.");
        }
        else
        {
            Debug.LogError("No se encontró la lógica de la pieza en el diccionario.");
        }
    }
}








