using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NumberBlock : MonoBehaviour
{
    public NumberTile[,] blockTiles;
    public int width = 3, height = 3;

    [SerializeField] private GameObject basePrefab, playablePrefab;

    public void GenerateBlock()
    {
        blockTiles = new NumberTile[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                NumberTile currentTile = Instantiate(basePrefab, transform).GetComponent<NumberTile>();
                currentTile.UpdateNumber(Random.Range(1, 9));

                blockTiles[i, j] = currentTile;
            }
        }
    }
}
