using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Helpers : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI quantityText, numberText;

    public void Setup(int myNumber)
    {
        gameObject.name = "Helper " + myNumber;
        numberText.color = SudokuColors.standard;

        numberText.SetText(myNumber.ToString());

        UpdateQuantity(0);
    }

    public void IncreaseQuantity()
    {
        UpdateQuantity(int.Parse(quantityText.text) + 1);
    }
    public void DecreaseQuantity()
    {
        UpdateQuantity(int.Parse(quantityText.text) - 1);
    }

    public void UpdateQuantity(int newQuantity)
    {
        try
        {
            quantityText.SetText(newQuantity.ToString());

            if (newQuantity == 0) Complete();
            else DeComplete();
        }

        
        catch
        {
            print("Error!");
        }
        
    }

    void Complete()
    {
        numberText.color = SudokuColors.completed;
        quantityText.color = SudokuColors.completed;
    }

    void DeComplete()
    {
        numberText.color = SudokuColors.standard;
        quantityText.color = SudokuColors.standard;
    }
}
