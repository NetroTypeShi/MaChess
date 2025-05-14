using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pieces
{
    GameObject piecePrefab;
    Transform boardTransform;
    Material whiteMaterial;
    Material blackMaterial;
    Vector3 worldPosition;
    public Pieces(GameObject piecePrefab, Transform boardTransform, Material whiteMaterial, Material blackMaterial)
    {
        this.piecePrefab = piecePrefab;
        this.boardTransform = boardTransform;
        this.whiteMaterial = whiteMaterial;
        this.blackMaterial = blackMaterial;
    }

    public void InstantiatePieces(int boardWidth, int boardHeight)
    {
        int middle = boardHeight / 2;

        CreatePiece(new Vector2Int(middle, 0), whiteMaterial);
        CreatePiece(new Vector2Int(middle + 1, 0), whiteMaterial);
        CreatePiece(new Vector2Int(middle - 1, 0), whiteMaterial);

        CreatePiece(new Vector2Int(middle, 4), blackMaterial);
        CreatePiece(new Vector2Int(middle + 1, 4), blackMaterial);
        CreatePiece(new Vector2Int(middle - 1, 4), blackMaterial);
    }

    public void CreatePiece(Vector2Int position, Material material)
    {
        worldPosition = new Vector3(position.x, 0, position.y);

        GameObject piece = GameObject.Instantiate(piecePrefab, worldPosition, Quaternion.identity, boardTransform);

        piece.GetComponentInChildren<MeshRenderer>().material = material;
    }
}



