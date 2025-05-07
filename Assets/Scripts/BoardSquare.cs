using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSquare
{
    public GameObject square;
    public Vector2Int position;
    MeshRenderer renderer;
    public BoardSquare(Vector2Int position, GameObject sqPrefab, Transform parent, Material whiteMaterial, Material blackMaterial)
    {
        this.position = position; //hay que poner el this pa que sea el puesto arriba y no el de BoardSquare  

        square = GameObject.Instantiate(sqPrefab, new Vector3(position.x, 0, position.y), Quaternion.identity, parent);

        renderer = square.GetComponentInChildren<MeshRenderer>();

        if ((position.x + position.y) % 2 == 0) 
        { 
            renderer.material = whiteMaterial;
        }

        else
        {
            renderer.material = blackMaterial;
        }
            
    }
}
