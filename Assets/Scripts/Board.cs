using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoardSquare
{
    public Vector2Int position;
    //public BoardEntity linkedEntity;
    public BoardSquare(Vector2Int position, GameObject sqPrefab)
    {
        //linkedEntity = null;
    }
}

public class Manager : MonoBehaviour
{
    [SerializeField] GameObject squarePrefab;
    
    private void Awake()
    {
        {
            StartCoroutine(InstantiateCubeEdges());
        }        
    }
    IEnumerator InstantiateCubeEdges()
    {
        for (int i < 0, i++, )
        yield return new WaitForEndOfFrame();
    }
}