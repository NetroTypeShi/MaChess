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

    private List<Piece> allPieces = new List<Piece>();

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
        GameObject squareObj = board.squares[position.x, position.y].SquareObject;
        Vector3 localPosition = squareObj.transform.localPosition;

        GameObject pieceGO = GameObject.Instantiate(piecePrefab, localPosition, Quaternion.identity, boardTransform);
        pieceGO.GetComponentInChildren<Renderer>().material = material;
        pieceGO.tag = tag;

        Piece pieceLogic = new Piece(10f); // Salud inicial de 10
        allPieces.Add(pieceLogic);

        board.squares[position.x, position.y].Piece = pieceGO;

        pieceGO.AddComponent<PieceComponent>().Init(pieceLogic);
    }

    public void DamagePiece(GameObject pieceGO, float damage)
    {
        Piece pieceLogic = GetPieceLogic(pieceGO);
        if (pieceLogic != null)
        {
            pieceLogic.TakeDamage(damage);
            UpdatePieceHeight(pieceGO, pieceLogic);
        }
    }

    public void HealPiece(GameObject pieceGO, float healAmount)
    {
        Piece pieceLogic = GetPieceLogic(pieceGO);
        if (pieceLogic != null)
        {
            pieceLogic.Heal(healAmount);
            UpdatePieceHeight(pieceGO, pieceLogic);
        }
    }

    private void UpdatePieceHeight(GameObject pieceGO, Piece pieceLogic)
    {
        float healthFactor = pieceLogic.GetHealthFactor();
        Vector3 scale = pieceGO.transform.localScale;
        pieceGO.transform.localScale = new Vector3(scale.x, healthFactor, scale.z);
    }

    public bool IsPieceDestroyed(GameObject pieceGO)
    {
        Piece pieceLogic = GetPieceLogic(pieceGO);
        return pieceLogic != null && pieceLogic.IsDestroyed();
    }

    public void BoostPieceHealth(GameObject pieceGO, float boostAmount)
    {
        Piece pieceLogic = GetPieceLogic(pieceGO);
        if (pieceLogic != null)
        {
            pieceLogic.Heal(boostAmount);
            UpdatePieceHeight(pieceGO, pieceLogic);
        }
    }

    public void BoostPieceDamage(GameObject pieceGO, float boostAmount)
    {
        Piece pieceLogic = GetPieceLogic(pieceGO);
        if (pieceLogic != null)
        {
            pieceLogic.IncreaseDamage(boostAmount);
        }
    }

    public void ApplyChestBoost(GameObject pieceGO, string chestType)
    {
        if (chestType == "Health")
        {
            BoostPieceHealth(pieceGO, 5f);
        }
        else if (chestType == "Damage")
        {
            BoostPieceDamage(pieceGO, 2f);
        }
    }

    public float GetPieceDamage(GameObject pieceGO)
    {
        Piece pieceLogic = GetPieceLogic(pieceGO);
        return pieceLogic != null ? pieceLogic.GetDamage() : 0f;
    }

    private Piece GetPieceLogic(GameObject pieceGO)
    {
        var comp = pieceGO.GetComponent<PieceComponent>();
        return comp != null ? comp.PieceLogic : null;
    }
}

// Componente auxiliar para asociar la lógica al GameObject visual
public class PieceComponent : MonoBehaviour
{
    public Piece PieceLogic { get; private set; }
    public void Init(Piece logic) => PieceLogic = logic;
}









