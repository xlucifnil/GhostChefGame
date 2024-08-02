using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MatchingManagement : MonoBehaviour
{
    struct match
    {
        public List<GameObject> matchedTiles;
    }

    public GameObject[] tileTypes;
    public int rows, cols;
    public float gridSpacing;
    public GameObject[,] board;
    public GameObject firstClicked, secondClicked;
    List<match> matches;
    
    
    // Start is called before the first frame update
    void Start()
    {
        GenerateBoard();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (firstClicked == null)
            {
                firstClicked = hit.collider.gameObject;
            }
            else if(firstClicked == hit.collider.gameObject)
            {
                firstClicked = null;
            }
            else
            {
                secondClicked = hit.collider.gameObject;
            }
        }

        if (firstClicked != null && secondClicked != null)
        {
            int firstRow = 0, firstCol = 0;
            int secondRow = 0, secondCol = 0;
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    if(firstClicked == board[i,j])
                    {
                        firstCol = i;
                        firstRow = j;
                    }
                    if(secondClicked == board[i,j])
                    {
                        secondCol = i;
                        secondRow = j;
                    }
                }
            }

            Vector2 secondPos = secondClicked.transform.position;
            secondClicked.transform.position = firstClicked.transform.position;
            firstClicked.transform.position = secondPos;

            board[firstCol, firstRow] = secondClicked;
            board[secondCol, secondRow] = firstClicked;

            firstClicked = null;
            secondClicked = null;

            int currentRow = 0;
            int currentCol = 0;


            matches = new List<match>();
            match aMatch = new match();
            aMatch.matchedTiles = new List<GameObject>();

            for (int i = 0; i < cols; i++)
            {
                currentCol = i;
                for (int j = 0; j < rows; j++)
                {
                    currentRow = j;

                    if (j == 0)
                    {
                        aMatch.matchedTiles.Add(board[currentCol, currentRow]);
                    }
                    else if(aMatch.matchedTiles[0].GetComponent<Tile>().type == board[currentCol, currentRow].GetComponent<Tile>().type)
                    {
                        aMatch.matchedTiles.Add(board[currentCol, currentRow]);
                    }
                    else
                    {
                        if(aMatch.matchedTiles.Count >= 3)
                        {
                            matches.Add(aMatch);
                            aMatch.matchedTiles = new List<GameObject>();
                        }
                        aMatch.matchedTiles.Clear();
                        aMatch.matchedTiles.Add(board[currentCol, currentRow]);
                    }
                }
                if(aMatch.matchedTiles.Count >= 3)
                {
                    matches.Add(aMatch);
                    aMatch.matchedTiles = new List<GameObject>();
                }
                aMatch.matchedTiles.Clear();
            }

            for (int i = 0; i < rows; i++)
            {
                currentRow = i;
                for (int j = 0; j < cols; j++)
                {
                    currentCol = j;

                    if (j == 0)
                    {
                        aMatch.matchedTiles.Add(board[currentCol, currentRow]);
                    }
                    else if (aMatch.matchedTiles[0].GetComponent<Tile>().type == board[currentCol, currentRow].GetComponent<Tile>().type)
                    {
                        aMatch.matchedTiles.Add(board[currentCol, currentRow]);
                    }
                    else
                    {
                        if (aMatch.matchedTiles.Count >= 3)
                        {
                            matches.Add(aMatch);
                            aMatch.matchedTiles = new List<GameObject>();
                        }
                        aMatch.matchedTiles.Clear();
                        aMatch.matchedTiles.Add(board[currentCol, currentRow]);
                    }
                }
                if (aMatch.matchedTiles.Count >= 3)
                {
                    matches.Add(aMatch);
                    aMatch.matchedTiles = new List<GameObject>();
                }
                aMatch.matchedTiles.Clear();
            }

            for (int i = 0; i < matches.Count; i++)
            {
                for (int j = 0; j < matches[i].matchedTiles.Count; j++)
                {
                    Destroy(matches[i].matchedTiles[j]);
                }
            }
            matches.Clear();
        }
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
