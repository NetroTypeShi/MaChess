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
                position = new Vector3(i , 0, j);
                intPosition = new Vector2Int(i, j);
                square = Instantiate(squarePrefab, position, Quaternion.identity, transform);
                squares[i, j] = new BoardSquare(intPosition,squarePrefab);
                MeshRenderer renderer = square.GetComponentInChildren<MeshRenderer>();
                if ((i + j) % 2 == 0)
                    renderer.material = whiteMaterial;
                else
                    renderer.material = blackMaterial;

                yield return null;
            }
        }

        yield return new WaitForEndOfFrame();
    }
}
