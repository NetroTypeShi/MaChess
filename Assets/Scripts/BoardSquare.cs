using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSquare
{
    public Vector2Int Position { get; private set; }
    public GameObject Piece { get; set; } // Referencia a la pieza en esta casilla

    public BoardSquare(Vector2Int position, GameObject sqPrefab, Transform parent, Material whiteMaterial, Material blackMaterial)
    {
        Position = position;

        GameObject square = GameObject.Instantiate(sqPrefab, new Vector3(position.x, 0, position.y), Quaternion.identity, parent);

        Material material = (position.x + position.y) % 2 == 0 ? whiteMaterial : blackMaterial;
        square.GetComponentInChildren<MeshRenderer>().material = material;
    }
}

