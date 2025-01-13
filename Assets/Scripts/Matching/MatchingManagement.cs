using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MatchingManagement : MonoBehaviour
{
    public enum boardState
    {
        Move,
        Matching,
        Falling,
        Filling,
        Finish
    }

    public enum tileDirection
    {
        Null,
        Right,
        Left,
        Up,
        Down,
    }


    struct match
    {
        public List<GameObject> matchedTiles;
    }

    public boardState currentState = boardState.Move;
    public int score = 0;
    public int scoreingMultiplier = 2;
    public float fallSpeed = 1;
    public float tileSpawnSize = 0.1f;
    public float tileGrowSpeed = 0.1f;
    public TMP_Text scoreDisplay, stepDisplay, turnDisplay;
    public GameObject finishText;
    public GameObject[] tileTypes;
    public int rows, cols;
    public float gridSpacing;
    public GameObject[,] board;
    public GameObject selectedTile = null, secondClicked = null;
    Vector3 selectedStartPosition;
    public GameObject[] cookingSteps;
    public Vector3 selectedScale;
    int turnsPassed = 0;
    List<match> matches;
    public tileDirection direction;


    // Start is called before the first frame update
    void Start()
    {
        GenerateBoard();
        DisplayScore();
        DisplayTurn();
        DisplayStep();
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case boardState.Move:
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
                    if (hit != false)
                    {
                        if (selectedTile == null && hit != false && hit.collider.gameObject.tag == "Tile")
                        {
                            selectedTile = hit.collider.gameObject;
                            selectedStartPosition = selectedTile.transform.position;
                            selectedTile.gameObject.transform.localScale += selectedScale;
                        }
                        else if (selectedTile == hit.collider.gameObject && hit != false && hit.collider.gameObject.tag == "Tile")
                        {
                            selectedTile.gameObject.transform.localScale -= selectedScale;
                            selectedTile = null;
                        }
                        else if (secondClicked == null && hit != false && hit.collider.gameObject.tag == "Tile")
                        {
                            secondClicked = hit.collider.gameObject;
                        }
                    }
                }


                if (selectedTile != null)
                {

                    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    selectedTile.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);

                    //Making sure the selected tile doesn't move outside the board space.
                    if (selectedTile.transform.position.y > gameObject.transform.position.y + (.4f * gridSpacing))
                    {
                        selectedTile.transform.position = new Vector2(selectedTile.transform.position.x, gameObject.transform.position.y + (.4f * gridSpacing));
                    }
                    if (selectedTile.transform.position.y < gameObject.transform.position.y - gridSpacing * rows + (.4f * gridSpacing))
                    {
                        selectedTile.transform.position = new Vector2(selectedTile.transform.position.x, gameObject.transform.position.y - gridSpacing * rows + (.4f * gridSpacing));
                    }
                    if (selectedTile.transform.position.x < gameObject.transform.position.x - (.4f * gridSpacing))
                    {
                        selectedTile.transform.position = new Vector2(gameObject.transform.position.x - (.4f * gridSpacing), selectedTile.transform.position.y);
                    }
                    if (selectedTile.transform.position.x > gameObject.transform.position.x + gridSpacing * cols - (.4f * gridSpacing))
                    {
                        selectedTile.transform.position = new Vector2(gameObject.transform.position.x + gridSpacing * cols - (.4f * gridSpacing), selectedTile.transform.position.y);
                    }

                    if (selectedTile.transform.position.x > selectedStartPosition.x && selectedTile.transform.position.y < selectedStartPosition.y + gridSpacing &&
                        selectedTile.transform.position.y > selectedStartPosition.y - gridSpacing)
                    {
                        if (direction != tileDirection.Right)
                        {
                            ResetTilePositions();
                            direction = tileDirection.Right;
                        }
                        for (int i = selectedTile.GetComponent<Tile>().x + 1; i < cols; i++)
                        {
                            if (selectedTile.transform.position.x > board[i, selectedTile.GetComponent<Tile>().y].GetComponent<Tile>().position.x)
                            {
                                board[i, selectedTile.GetComponent<Tile>().y].transform.position = new Vector3(board[i, selectedTile.GetComponent<Tile>().y].GetComponent<Tile>().position.x - gridSpacing,
                                    board[i, selectedTile.GetComponent<Tile>().y].transform.position.y, board[i, selectedTile.GetComponent<Tile>().y].transform.position.z);
                            }
                            else
                            {
                                board[i, selectedTile.GetComponent<Tile>().y].transform.position = board[i, selectedTile.GetComponent<Tile>().y].GetComponent<Tile>().position;
                            }
                        }
                    }
                    else if (selectedTile.transform.position.x < selectedStartPosition.x && selectedTile.transform.position.y < selectedStartPosition.y + gridSpacing
                        && selectedTile.transform.position.y > selectedStartPosition.y - gridSpacing)
                    {
                        if (direction != tileDirection.Left)
                        {
                            ResetTilePositions();
                            direction = tileDirection.Left;
                        }
                        for (int i = selectedTile.GetComponent<Tile>().x - 1; i >= 0; i--)
                        {
                            if (selectedTile.transform.position.x < board[i, selectedTile.GetComponent<Tile>().y].GetComponent<Tile>().position.x)
                            {
                                board[i, selectedTile.GetComponent<Tile>().y].transform.position = new Vector3(board[i, selectedTile.GetComponent<Tile>().y].GetComponent<Tile>().position.x + gridSpacing,
                                    board[i, selectedTile.GetComponent<Tile>().y].transform.position.y, board[i, selectedTile.GetComponent<Tile>().y].transform.position.z);
                            }
                            else
                            {
                                board[i, selectedTile.GetComponent<Tile>().y].transform.position = board[i, selectedTile.GetComponent<Tile>().y].GetComponent<Tile>().position;
                            }
                        }
                    }
                    else if (selectedTile.transform.position.y < selectedStartPosition.y && selectedTile.transform.position.x < selectedStartPosition.x + gridSpacing
                        && selectedTile.transform.position.x > selectedStartPosition.x - gridSpacing)
                    {
                        if (direction != tileDirection.Down)
                        {
                            ResetTilePositions();
                            direction = tileDirection.Down;
                        }
                        for (int i = selectedTile.GetComponent<Tile>().y + 1; i < rows; i++)
                        {
                            if (selectedTile.transform.position.y < board[selectedTile.GetComponent<Tile>().x, i].GetComponent<Tile>().position.y)
                            {
                                board[selectedTile.GetComponent<Tile>().x, i].transform.position = new Vector3(board[selectedTile.GetComponent<Tile>().x, i].transform.position.x,
                                    board[selectedTile.GetComponent<Tile>().x, i].GetComponent<Tile>().position.y + gridSpacing, board[selectedTile.GetComponent<Tile>().x, i].transform.position.z);
                            }
                            else
                            {
                                board[selectedTile.GetComponent<Tile>().x, i].transform.position = board[selectedTile.GetComponent<Tile>().x, i].GetComponent<Tile>().position;
                            }
                        }
                    }
                    else if (selectedTile.transform.position.y > selectedStartPosition.y && selectedTile.transform.position.x < selectedStartPosition.x + gridSpacing
                        && selectedTile.transform.position.x > selectedStartPosition.x - gridSpacing)
                    {
                        if (direction != tileDirection.Up)
                        {
                            ResetTilePositions();
                            direction = tileDirection.Up;
                        }
                        for (int i = selectedTile.GetComponent<Tile>().y - 1; i >= 0; i--)
                        {
                            if (selectedTile.transform.position.y > board[selectedTile.GetComponent<Tile>().x, i].GetComponent<Tile>().position.y)
                            {
                                board[selectedTile.GetComponent<Tile>().x, i].transform.position = new Vector3(board[selectedTile.GetComponent<Tile>().x, i].transform.position.x,
                                    board[selectedTile.GetComponent<Tile>().x, i].GetComponent<Tile>().position.y - gridSpacing, board[selectedTile.GetComponent<Tile>().x, i].transform.position.z);
                            }
                            else
                            {
                                board[selectedTile.GetComponent<Tile>().x, i].transform.position = board[selectedTile.GetComponent<Tile>().x, i].GetComponent<Tile>().position;
                            }
                        }
                    }
                    else
                    {
                        ResetTilePositions();
                        direction = tileDirection.Null;
                    }

                }

                if (Input.GetMouseButtonUp(0) && selectedTile != null)
                {
                    if (direction != tileDirection.Null)
                    {
                        if (selectedTile.transform.position.x > selectedTile.GetComponent<Tile>().position.x + gridSpacing || selectedTile.transform.position.x < selectedTile.GetComponent<Tile>().position.x - gridSpacing
                            || selectedTile.transform.position.y > selectedTile.GetComponent<Tile>().position.y + gridSpacing || selectedTile.transform.position.y < selectedTile.GetComponent<Tile>().position.y - gridSpacing)
                        {
                            SetTiles();
                            currentState = boardState.Matching;
                        }
                    }
                    ResetTilePositions();
                    UpdateTilePositions();
                    selectedTile.gameObject.transform.localScale -= selectedScale;
                    selectedTile = null;
                }

                break;

            case boardState.Matching:

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
                        bool bonusMatch = false;
                        bool penaltyMatch = false;
                        matches[i].matchedTiles[j].gameObject.GetComponent<Tile>().matched = true;
                        matches[i].matchedTiles[j].GetComponent<Tile>().MatchEffect();
                        for (int k = 0; k < cookingSteps[turnsPassed].GetComponent<CookingStep>().bonusTiles.Length; k++)
                        {
                            if (matches[i].matchedTiles[j].GetComponent<Tile>().type == cookingSteps[turnsPassed].GetComponent<CookingStep>().bonusTiles[k].GetComponent<Tile>().type)
                            {
                                bonusMatch = true;
                            }
                            else if (matches[i].matchedTiles[j].GetComponent<Tile>().type == cookingSteps[turnsPassed].GetComponent<CookingStep>().penaltyTiles[k].GetComponent<Tile>().type)
                            {
                                penaltyMatch = true;
                            }
                        }
                        if (bonusMatch)
                        {
                            score += 1 * scoreingMultiplier;
                        }
                        else if (penaltyMatch)
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

                matches.Clear();

                bool full = true;

                for (int i = 0; i < cols; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        if (board[i, j] == null)
                        {
                            full = false;
                        }
                    }
                }

                if(full)
                {
                    currentState = boardState.Move;
                    turnsPassed++;
                    DisplayTurn();
                    if (turnsPassed >= cookingSteps.Length)
                    {
                        currentState = boardState.Finish;
                    }
                    else
                    {
                        DisplayStep();
                    }
                }
                else
                {
                    currentState = boardState.Falling;
                }
                
                break;

            case boardState.Falling:

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
                            Vector2 newPosition = Vector2.MoveTowards(board[i, j].transform.position, new Vector2(gameObject.transform.position.x + (i * gridSpacing), gameObject.transform.position.y + (-j * gridSpacing)), fallSpeed * Time.deltaTime);

                            board[i, j].transform.position = newPosition;
                        }
                    }
                }

                bool inPlace = true;

                for (int i = 0; i < cols; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        if (board[i, j] != null)
                        {
                            if (board[i, j].transform.position != new Vector3(gameObject.transform.position.x + (i * gridSpacing), gameObject.transform.position.y + (-j * gridSpacing), board[i, j].transform.position.z))
                            {
                                inPlace = false;
                            }
                        }
                    }
                }

                if (inPlace)
                {
                    currentState = boardState.Filling;
                    RefillBoard();
                }

                break;

            case boardState.Filling:

                bool filled = true;

                for (int i = 0; i < cols; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        if (board[i, j].transform.localScale != tileTypes[1].transform.localScale)
                        {
                            board[i, j].transform.localScale = new Vector3(board[i, j].transform.localScale.x + tileGrowSpeed * Time.deltaTime, board[i, j].transform.localScale.y + tileGrowSpeed * Time.deltaTime, 1);
                            if (board[i, j].transform.localScale.x > tileTypes[0].transform.localScale.x)
                            {
                                board[i, j].transform.localScale = tileTypes[0].transform.localScale;
                            }
                            else
                            {
                                filled = false;
                            }
                        }
                    }
                }

                if (filled)
                {
                    currentState = boardState.Matching;
                }

                break;

            case boardState.Finish:
                finishText.SetActive(true);
                break;
        }
        
    }

    //Initially creates a board filled with tiles.
    public void GenerateBoard()
    {
        board = new GameObject[cols, rows];

        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                board[i, j] = Instantiate(GenerateTile(), gameObject.transform);
                board[i, j].transform.position = new Vector2(i * gridSpacing, -j * gridSpacing);
            }
        }

        do
        {
            FillBoard();
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

    //Fills an empty board.
    public void FillBoard()
    {
        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                if (board[i, j] == null)
                {
                    board[i, j] = Instantiate(GenerateTile(), gameObject.transform);
                    board[i, j].transform.position = new Vector3(gameObject.transform.position.x + (i * gridSpacing), gameObject.transform.position.y + (-j * gridSpacing), 1);
                }
            }
        }
        UpdateTilePositions();
    }

    //Places tiles in all spots on the grid that do not have one.
    public void RefillBoard()
    {
        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                if (board[i, j] == null)
                {
                    GameObject newTile = Instantiate(GenerateTile(), gameObject.transform);
                    newTile.transform.localScale = new Vector3(tileSpawnSize, tileSpawnSize, 1);
                    board[i, j] = newTile;
                    board[i, j].transform.position = new Vector3(gameObject.transform.position.x + (i * gridSpacing), gameObject.transform.position.y + (-j * gridSpacing), 1);
                }
            }
        }
        UpdateTilePositions();
    }

    //Makes a new tile.
    public GameObject GenerateTile()
    {
        GameObject tile;

        tile = tileTypes[Random.Range(0, tileTypes.Length)];

        return tile;
    }

    //Sees if any grid spots do not have a tile.
    public bool CheckMissingTiles()
    {
        bool missing = false;
        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                if (board[i, j] == null)
                {
                    missing = true;
                }
            }
        }

        return missing;
    }

    //Makes it so the tiles know their new grid position and world position.
    void UpdateTilePositions()
    {
        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                if (board[i, j] != null)
                {
                    board[i, j].GetComponent<Tile>().SetPosition(i, j, board[i, j].transform.position);
                }
            }
        }
    }

    //Puts all tiles back into there previous positions.
    void ResetTilePositions()
    {
        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                if (board[i, j] != null)
                {
                    board[i, j].transform.position = new Vector3(gameObject.transform.position.x + (i * gridSpacing), gameObject.transform.position.y + (-j * gridSpacing), 1);
                }
            }
        }
    }

    //Locks tiles to a grid spot after a tile is moved.
    void SetTiles()
    {
        GameObject[] Tiles = GameObject.FindGameObjectsWithTag("Tile");
        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                for (int k = 0; k < Tiles.Length; k++)
                {
                    if (Vector2.Distance(board[i, j].transform.position, new Vector2(gameObject.transform.position.x + i * gridSpacing, gameObject.transform.position.y - j * gridSpacing))
                        > Vector2.Distance(Tiles[k].transform.position, new Vector2(gameObject.transform.position.x + i * gridSpacing, gameObject.transform.position.y - j * gridSpacing)))
                    {
                        board[i, j] = Tiles[k];
                    }
                }
            }
        }
    }

    public void DisplayScore()
    {
        scoreDisplay.text = score.ToString();
    }

    public void DisplayStep()
    {
        stepDisplay.text = cookingSteps[turnsPassed].GetComponent<CookingStep>().stepName;
    }

    public void DisplayTurn()
    {
        turnDisplay.text = (cookingSteps.Length - turnsPassed).ToString();
    }
}
