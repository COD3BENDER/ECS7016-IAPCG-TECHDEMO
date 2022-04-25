using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
// reference: Sunny Valley Studios Tutorial L Systems i used this tutorial to learn to create a generator using Generative Grammars
public class BuildingType
{
    [SerializeField]
    private GameObject[] prefabs; // this array list holds the prefabs to be used in the techdemo game 
    public int sizeRequired;
    public int quantity;
    public int quanityAlreadyPlaced;

    public GameObject GetPrefab()
    {
        quanityAlreadyPlaced++;
        if(prefabs.Length > 1)
        {
            var random = UnityEngine.Random.Range(0, prefabs.Length);
            return prefabs[random];
        }
        return prefabs[0];
    }

    public bool IsBuildingAvailable() // if the amount of buildings placed is enough then dont place any more but if enough wasnt placed then place more
    {
        return quanityAlreadyPlaced < quantity;
    }

    public void Reset() // useful when implementing generate button
    {
        quanityAlreadyPlaced = 0;
    }
    
}
