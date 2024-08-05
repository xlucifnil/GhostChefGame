using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MatchingManagement : MonoBehaviour
{
    struct match
    {
        public List<GameObject> matchedTiles;
    }

    public int score = 0;
    public int scoreingMultiplier = 2;
    public TMP_Text scoreDisplay;
    public GameObject[] tileTypes;
    public int rows, cols;
    public float gridSpacing;
    public GameObject[,] board;
    public GameObject firstClicked = null, secondClicked = null;
    public GameObject[] cookingSteps;
    int currentTurn = 1;
    List<match> matches;
    bool moveMade = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        GenerateBoard();
        DisplayScore();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit != false)
            {
                if (firstClicked == null && hit != false && hit.collider.gameObject.tag == "Tile")
                {
                    firstClicked = hit.collider.gameObject;
                }
                else if (firstClicked == hit.collider.gameObject && hit != false && hit.collider.gameObject.tag == "Tile")
                {
                    firstClicked = null;
                }
                else if (secondClicked == null && hit != false && hit.collider.gameObject.tag == "Tile")
                {
                    secondClicked = hit.collider.gameObject;
                    moveMade = true;
                }
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
                    if (firstClicked == board[i, j])
                    {
                        firstCol = i;
                        firstRow = j;
                    }
                    if (secondClicked == board[i, j])
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
        }
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
                else if (aMatch.matchedTiles[0].GetComponent<Tile>().type == board[currentCol, currentRow].GetComponent<Tile>().type)
                {
                    aMatch.matchedTiles.Add(board[currentCol, currentRow]);
                }
                else
                {
                    if(aMatch.matchedTiles.Count >= 3)
                    {
                        for (int k = 0; k < aMatch.matchedTiles.Count; k++)
                        {
                            board[currentCol, currentRow - 1 - k].GetComponent<Tile>().matched = true;
                        }
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
            aMatch.matchedTiles = new List<GameObject>();
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
                        for (int k = 0; k < aMatch.matchedTiles.Count; k++)
                        {
                            board[currentCol - 1 - k, currentRow].GetComponent<Tile>().matched = true;
                        }
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
                bool bonusMatch = false;
                bool penaltyMatch = false;
                matches[i].matchedTiles[j].gameObject.GetComponent<Tile>().matched = true;
                matches[i].matchedTiles[j].GetComponent<Tile>().MatchEffect();
                for (int k = 0; k < cookingSteps[currentTurn].GetComponent<CookingStep>().bonusTiles.Length; k++)
                {
                    if (matches[i].matchedTiles[j].GetComponent<Tile>().type == cookingSteps[currentTurn].GetComponent<CookingStep>().bonusTiles[k].GetComponent<Tile>().type)
                    {
                        bonusMatch = true;
                    }
                    else if (matches[i].matchedTiles[j].GetComponent<Tile>().type == cookingSteps[currentTurn].GetComponent<CookingStep>().penaltyTiles[k].GetComponent<Tile>().type)
                    {
                        penaltyMatch = true;
                    }
                }
                if(bonusMatch)
                {
                    score += 1 * scoreingMultiplier;
                }
                else if(penaltyMatch)
                {
                    score += 0;
                }
                else
                {
                    score += 1;
                }
                DisplayScore();
                Destroy(matches[i].matchedTiles[j]);
                matches[i].matchedTiles[j] = null;
            }
        }

        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                if (board[i, j].GetComponent<Tile>().matched == true)
                {
                    board[i, j] = null;
                }
            }
        }

        for (int i = 0; i < cols; i++)
        {
            for (int j = rows-1; j >= 0; j--)
            {
                if (board[i, j] != null)
                {
                    int newRow = j;
                    for (int k = j; k < rows; k++)
                    {
                        if (board[i, k] == null)
                        {
                            newRow = k;
                        }
                    }
                    if (newRow != j)
                    {
                        board[i, newRow] = board[i, j];
                        board[i, j] = null;
                    }
                }
            }
        }

        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                if (board[i, j] != null)
                {
                    board[i, j].transform.position = new Vector2(gameObject.transform.position.x + (i * gridSpacing), gameObject.transform.position.y + (-j * gridSpacing));
                }
            }
        }

        matches.Clear();

        RefillBoard();

        if(moveMade)
        {
            currentTurn++;
            moveMade = false;
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
       
        do
        {
            RefillBoard();
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
                    else if (aMatch.matchedTiles[0].GetComponent<Tile>().type == board[currentCol, currentRow].GetComponent<Tile>().type)
                    {
                        aMatch.matchedTiles.Add(board[currentCol, currentRow]);
                    }
                    else
                    {
                        if (aMatch.matchedTiles.Count >= 3)
                        {
                            for (int k = 0; k < aMatch.matchedTiles.Count; k++)
                            {
                                board[currentCol, currentRow - 1 - k].GetComponent<Tile>().matched = true;
                            }
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
                aMatch.matchedTiles = new List<GameObject>();
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
                            for (int k = 0; k < aMatch.matchedTiles.Count; k++)
                            {
                                board[currentCol - 1 - k, currentRow].GetComponent<Tile>().matched = true;
                            }
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
                    matches[i].matchedTiles[j].gameObject.GetComponent<Tile>().matched = true;
                    Destroy(matches[i].matchedTiles[j]);
                    matches[i].matchedTiles[j] = null;
                }
            }

            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    if (board[i, j].GetComponent<Tile>().matched == true)
                    {
                        board[i, j] = null;
                    }
                }
            }

            for (int i = 0; i < cols; i++)
            {
                for (int j = rows - 1; j >= 0; j--)
                {
                    if (board[i, j] != null)
                    {
                        int newRow = j;
                        for (int k = j; k < rows; k++)
                        {
                            if (board[i, k] == null)
                            {
                                newRow = k;
                            }
                        }
                        if (newRow != j)
                        {
                            board[i, newRow] = board[i, j];
                            board[i, j] = null;
                        }
                    }
                }
            }

            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    if (board[i, j] != null)
                    {
                        board[i, j].transform.position = new Vector2(gameObject.transform.position.x + (i * gridSpacing), gameObject.transform.position.y + (-j * gridSpacing));
                    }
                }
            }
            
        } while (CheckMissingTiles());

        
    }

    public void RefillBoard()
    {
        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                if (board[i, j] == null)
                {
                    board[i, j] = Instantiate(GenerateTile());
                    board[i, j].transform.position = new Vector2(gameObject.transform.position.x + (i * gridSpacing), gameObject.transform.position.y + (-j * gridSpacing));
                }
            }
        }
    }

    public GameObject GenerateTile()
    {
        GameObject tile;

        tile = tileTypes[Random.Range(0,tileTypes.Length)];

        return tile;
    }

    public bool CheckMissingTiles()
    {
        bool missing = false;
        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                if(board[i, j] == null)
                {
                    missing = true;
                }
            }
        }

        return missing;
    }

    public void DisplayScore()
    {
        scoreDisplay.text = score.ToString();
    }
}
