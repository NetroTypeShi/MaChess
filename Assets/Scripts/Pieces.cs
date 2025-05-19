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

        piece.GetComponentInChildren<MeshRenderer>().material = material; // Asignar material
        piece.tag = tag; // Asignar tag

        board.squares[position.x, position.y].Piece = piece; // Asignar pieza a la casilla
    }
}





