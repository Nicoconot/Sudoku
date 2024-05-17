using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RowGridGenerator : MonoBehaviour
{
    public NumberTile[,] grid;
    public NumberBlock[,] blocks;

    public NumberTile selectedTile = null;
    [SerializeField] private Transform gridParent;
    [SerializeField] private TextMeshProUGUI gridLog;
    [SerializeField] private GameObject givenPrefab;
    [SerializeField] private HelperManager helperManager;

    [SerializeField] private int height = 9, width = 9;
    private enum Difficulty { VeryEasy, Easy, Medium, Hard, VeryHard};
    [SerializeField] private Difficulty difficulty = Difficulty.VeryEasy;

    [SerializeField] private List<int> oneToNine = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

    private ListDebugger debugger;

    private void Start()
    {
        debugger = GetComponent<ListDebugger>();
    }

    public void GenerateGrid()
    {
        foreach (Transform t in gridParent)
        {
            Destroy(t.gameObject);
        }
        grid = new NumberTile[height, width];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                print("Generating cell " + x + "," + y);
                //create a list of used number in row, column and block(create method for finding current block)
                //assign valid number based on that list
                List<int> usedBlockInts = new List<int>();
                List<int> usedRowAndColumnInts = new List<int>();

                List<int> unavailableInts = new List<int>();

                //Find available numbers in row
                for (int r = 0; r < x; r++)
                {
                   // print("checking previous cell " + r + "," + y);
                    int thisNumber = grid[r, y].number;
                    if (!unavailableInts.Contains(thisNumber)) unavailableInts.Add(thisNumber);
                    //print("Adding " + thisNumber + " to used numbers in row");
                }

                //Find available numbers in column
                for (int c = 0; c < y; c++)
                {
                   // print("checking previous cell " + x + "," + c);
                    int thisNumber = grid[x, c].number;
                    if (!unavailableInts.Contains(thisNumber)) unavailableInts.Add(thisNumber);
                   // print("Adding " + thisNumber + " to used numbers in column");
                }

                //Find available numbers in block

                //Finding current block
                //First find block index
                int firstRowInBlock = (int)((float)x / 3);
                int firstColumnInBlock = (int)((float)y / 3);

                //Then check for numbers within that block
                for (int bX = firstRowInBlock; bX < x; bX++)
                {
                    for (int bY = firstColumnInBlock; bY < y; bY++)
                    {
                        int thisNumber = grid[bX, bY].number;
                        if (!unavailableInts.Contains(thisNumber)) unavailableInts.Add(thisNumber);
                    }
                }


                //Now we have a list of all taken numbers
                // We create a new list to find which numbers AREN'T taken
                List<int> availableNumbers = ReturnAvailableNumbers(unavailableInts);
                //Then we choose a random number in that list to assign to our tile
                NumberTile currentTile = Instantiate(givenPrefab, transform).GetComponent<NumberTile>();

                currentTile.gridGenerator = this;

                int newUniqueNumber = availableNumbers[UnityEngine.Random.Range(0, availableNumbers.Count - 1)];
                currentTile.UpdateNumber(newUniqueNumber);
                currentTile.UpdateDebugText(x + "," + y);
                //And move forward
                grid[x, y] = currentTile;
            }
        }  

    }

    public void GenerateGridWithRowShift()
    {
        helperManager.InstantiateHelpers(height);
        foreach (Transform t in gridParent) Destroy(t.gameObject);
        grid = new NumberTile[width, height];

        int[] firstRow = new int[width];
        List<int> availableNumbers = new List<int>(oneToNine);
        for (int i = 0; i < width; i++)
        {            
            int newNumber = availableNumbers[Random.Range(0, availableNumbers.Count - 1)];

            firstRow[i] = newNumber;

            availableNumbers.Remove(newNumber);

            NumberTile newTile = Instantiate(givenPrefab, gridParent).GetComponent<NumberTile>();

            newTile.helperManager = helperManager;
            newTile.gridGenerator = this;
            newTile.UpdateNumber(newNumber);
            newTile.UpdateDebugText(i + "," + 0);

            newTile.x = i;
            newTile.y = 0;

            grid[i, 0] = newTile;
        }

        //Finished first row, now shifting array and assigning other rows to it
        int[] secondRow = ShiftArray(firstRow, 3);
        int[] thirdRow = ShiftArray(secondRow, 3);
        int[] fourthRow = ShiftArray(thirdRow, 1);
        int[] fifthRow = ShiftArray(fourthRow, 3);
        int[] sixthRow = ShiftArray(fifthRow, 3);
        int[] seventhRow = ShiftArray(sixthRow, 1);
        int[] eigthRow = ShiftArray(seventhRow, 3);
        int[] ninthRow = ShiftArray(eigthRow, 3);

        int[][] remainingRows = new int[8][];
        //randomize 0-1
        List<int> currentSection = new List<int> { 0, 1 };
        int chosen = Random.Range(0, 1);
        remainingRows[currentSection[chosen]] = secondRow;
        currentSection.RemoveAt(chosen);
        remainingRows[currentSection[0]] = thirdRow;
        //randomize 2-4
        currentSection = new List<int> { 2, 3, 4 };
        chosen = Random.Range(0, 2);
        remainingRows[currentSection[chosen]] = fourthRow;
        currentSection.RemoveAt(chosen);
        chosen = Random.Range(0, 1);
        remainingRows[currentSection[chosen]] = fifthRow;
        currentSection.RemoveAt(chosen);
        remainingRows[currentSection[0]] = sixthRow;
        //randomize 5-7
        currentSection = new List<int> { 5, 6, 7 };
        chosen = Random.Range(0, 2);
        remainingRows[currentSection[chosen]] = seventhRow;
        currentSection.RemoveAt(chosen);
        chosen = Random.Range(0, 1);
        remainingRows[currentSection[chosen]] = eigthRow;
        currentSection.RemoveAt(chosen);        
        remainingRows[currentSection[0]] = ninthRow;


        for (int i = 0; i < remainingRows.Length; i++)
        {
            for (int j = 0; j < remainingRows[i].Length; j++)
            {
                NumberTile newTile = Instantiate(givenPrefab, gridParent).GetComponent<NumberTile>();

                newTile.helperManager = helperManager;
                newTile.gridGenerator = this;
                newTile.UpdateNumber(remainingRows[i][j]);
                newTile.UpdateDebugText(j + "," + (i + 1));

                newTile.x = j;
                newTile.y = i + 1;

                grid[j, i + 1] = newTile;
            }
        }
     }

    public void GeneratePuzzle()
    {
        int numbersToConvert = 0;
        switch (difficulty)
        {
            case Difficulty.VeryEasy:
                numbersToConvert = 20;
                break;
            case Difficulty.Easy:
                numbersToConvert = 35;
                break;
            case Difficulty.Medium:
                numbersToConvert = 40;
                break;
            case Difficulty.Hard:
                numbersToConvert = 45;
                break;
            case Difficulty.VeryHard:
                numbersToConvert = 55;
                break;
        }

        List<NumberTile> convertedTiles = new List<NumberTile>();
        for (int i = 0; i < numbersToConvert; i++)
        {
            int randomX = Random.Range(0, width);
            int randomY = Random.Range(0, height);

            if(!convertedTiles.Contains(grid[randomX, randomY]))
            {
                helperManager.UpdateHelper(grid[randomX, randomY].number - 1);
                grid[randomX, randomY].ConvertToPlayable();
                convertedTiles.Add(grid[randomX, randomY]);
            }
        }
    }

    private void CheckIfValidPuzzle()
    {

    }

    private List<int> ReturnAvailableNumbers(List<int> usedNumbers)
    {
        List<int> availableNumbers = new List<int>(oneToNine);
        //print("Generating new available number list, starting with " + availableNumbers.Count + " items");
        foreach (int usedNumber in usedNumbers)
        {
            //print("Removing " + usedNumber + " from available number pool");
            availableNumbers.Remove(usedNumber);
        }

        return availableNumbers;
    }

    int[] ShiftArray(int[] array, int amount)
    {
        var result = new int[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            result[(i + amount) % array.Length] = array[i];
        }
        return result;
    }
}
