using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    BoardSquare[,] squares;

    [SerializeField] GameObject squarePrefab;
    [SerializeField] int x = 8; // columnas
    [SerializeField] int y = 8; // filas
    [SerializeField] float spacing;
    [SerializeField] Material whiteMaterial;
    [SerializeField] Material blackMaterial;
    GameObject square;
    Vector3 position;
    Vector2Int intPosition;

    private void Awake()
    {
        squares = new BoardSquare[x, y];
        StartCoroutine(InstantiateCubeEdges());
    }

    IEnumerator InstantiateCubeEdges()
    {
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                Vector2Int intPosition = new Vector2Int(i, j);
                squares[i, j] = new BoardSquare(intPosition, squarePrefab, transform, whiteMaterial, blackMaterial);
                yield return null;
            }
        }

        yield return new WaitForEndOfFrame();
    }
}
