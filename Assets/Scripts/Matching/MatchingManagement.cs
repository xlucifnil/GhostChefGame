using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchingManagement : MonoBehaviour
{
    public GameObject[] tileTypes;
    public int rows, cols;
    public float gridSpacing;
    public GameObject[,] board;
    
    // Start is called before the first frame update
    void Start()
    {
        GenerateBoard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateBoard()
    {
        board = new GameObject[cols, rows];
        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                board[i, j] = Instantiate(GenerateTile());
                board[i, j].transform.position = new Vector2(gameObject.transform.position.x + (i * gridSpacing), gameObject.transform.position.y + (-j * gridSpacing));
            }
        }
    }

    public GameObject GenerateTile()
    {
        GameObject tile;

        tile = tileTypes[Random.Range(0,tileTypes.Length)];

        return tile;
    }
}
