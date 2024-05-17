using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperManager : MonoBehaviour
{
    [SerializeField] private Helpers[] helpers;
    [SerializeField] private GameObject helperPrefab;

    public void InstantiateHelpers(int amount)
    {
        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }

        helpers = new Helpers[amount];
        for (int i = 0; i < amount; i++)
        {
            helpers[i] = Instantiate(helperPrefab, transform).GetComponent<Helpers>();

            helpers[i].Setup(i + 1);
        }
    }

    public void UpdateHelper(int helperIndex)
    {
        helpers[helperIndex].IncreaseQuantity();
    }
    public void DecreaseHelper(int helperIndex)
    {
        helpers[helperIndex].DecreaseQuantity();
    }
}
