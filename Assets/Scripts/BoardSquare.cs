using UnityEngine;

public class BoardSquare
{
    public Vector2Int Position { get; set; }
    public GameObject Piece { get; set; }
    public bool HasChest { get; set; }
    public string ChestType { get; set; } // "Health" o "Damage"
    public GameObject SquareObject { get; set; } // <-- Añade esto

    public BoardSquare(Vector2Int position, GameObject sqPrefab, Transform parent, Material whiteMaterial, Material blackMaterial)
    {
        Position = position;
        Piece = null;
        HasChest = false;
        ChestType = "";
        SquareObject = sqPrefab; // <-- Guarda la referencia al objeto de la casilla
    }
}

