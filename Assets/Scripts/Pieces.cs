using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pieces
{
    private GameObject piecePrefab; 
    private Transform boardTransform; 
    private Material whiteMaterial; 
    private Material blackMaterial; 

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

        CreatePiece(new Vector3(middle, 0, 0), whiteMaterial);
        CreatePiece(new Vector3(middle + 1, 0, 0), whiteMaterial);
        CreatePiece(new Vector3(middle - 1, 0, 0), whiteMaterial);

        CreatePiece(new Vector3(middle, 0, 4), blackMaterial);
        CreatePiece(new Vector3(middle + 1, 0, 4), blackMaterial);
        CreatePiece(new Vector3(middle - 1, 0, 4), blackMaterial);
    }

    private void CreatePiece(Vector3 position, Material material)
    {
        GameObject piece = GameObject.Instantiate(piecePrefab, position, Quaternion.identity, boardTransform);
        piece.GetComponentInChildren<MeshRenderer>().material = material;
    }
}


