using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class NumberTile : MonoBehaviour
{
    public int x, y;
    [HideInInspector] public HelperManager helperManager;
    [HideInInspector] public RowGridGenerator gridGenerator;
    public enum TileType { baseTile, playableTile };

    public TileType tileType = TileType.baseTile;

    public int number {get;  private set; }  = 1;

    [SerializeField] private TextMeshProUGUI numberText, debugText, inputtedText;
    [SerializeField] private GameObject input, placeholder;
    [SerializeField] private Image image;
    private TMP_InputField inputField;

    private NumberTile[] tilesOfMyNumber = new NumberTile[9];

    private void Start()
    {
        inputField = input.GetComponent<TMP_InputField>();
    }
    public void UpdateNumber(int newNumber)
    {
        number = newNumber;
        numberText.SetText(number.ToString());
    }

    public void UpdateDebugText(string newText)
    {
        debugText.SetText(newText);
    }

    public void ConvertToPlayable()
    {
        tileType = TileType.playableTile;
        numberText.gameObject.SetActive(false);
        input.SetActive(true);
    }

    public void CheckInput(string inputText)
    {
        
        if (!string.IsNullOrEmpty(inputText))
        {
            print(inputText);
            if (int.Parse(inputText) == number)
            {
                inputtedText.color = SudokuColors.correct;
                helperManager.DecreaseHelper(number - 1);
                inputField.interactable = false;
                inputField.targetGraphic.raycastTarget = false;
                inputField.placeholder.raycastTarget = false;
                inputtedText.raycastTarget = false;
                placeholder.SetActive(false);

               SelectTile();
            }
            else
            {
                inputtedText.color = SudokuColors.wrong;
            }
        }
    }

    public void SelectTile()
    {
        //Cross highlight
        if (gridGenerator.selectedTile != null && gridGenerator.selectedTile != this) gridGenerator.selectedTile.Deselect();
        SelectAnim();

        gridGenerator.selectedTile = this;

        for (int i = 0; i < 9; i++)
        {
            if (i != x) gridGenerator.grid[i, y].HighlightAnim();
            if (i != y) gridGenerator.grid[x, i].HighlightAnim();
        }

        //Highlighting numbers

        if(tilesOfMyNumber[0] == null)
        {
            int count = 0;
            foreach (var cell in gridGenerator.grid)
            {
                if (cell.number == number)
                {
                    tilesOfMyNumber[count] = cell;
                    count++;
                }
            }
        }
        if((tileType == TileType.playableTile && input.GetComponent<TMP_InputField>().text == number.ToString()) || tileType == TileType.baseTile)
        {
            foreach (var tile in tilesOfMyNumber)
            {
                if(tile.tileType == TileType.playableTile) tile.inputtedText.color = SudokuColors.playableSelectedText;
                else tile.numberText.color = SudokuColors.selectedText;
            }
        }        
    }

    public void SelectAnim()
    {
        image.color = SudokuColors.selectedBg;
    }

    public void HighlightAnim()
    {
        image.color = SudokuColors.highlightedBg;
    }

    public void Deselect()
    {
        DeselectAnim();
       for (int i = 0; i < 9; i++)
        {
            if (i != x) gridGenerator.grid[i, y].DeselectAnim();
            if (i != y) gridGenerator.grid[x, i].DeselectAnim();
        }

        foreach (var tile in tilesOfMyNumber)
        {
            tile.numberText.color = SudokuColors.standard;
            if(tile.tileType == TileType.playableTile && tile != this) tile.CheckInput(tile.input.GetComponent<TMP_InputField>().text);
        }
    }

    public void DeselectAnim()
    {
        image.color = SudokuColors.transparent;
    }
}
