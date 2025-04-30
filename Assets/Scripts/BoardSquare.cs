using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSquare
{
    GameObject square;
    public Vector2Int position;
    //public BoardEntity linkedEntity;
    public BoardSquare(Vector2Int position, GameObject sqPrefab)
    {
        square = Instantiate(position, sqPrefab);
    }
}
