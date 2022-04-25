using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// reference: Sunny Valley Studios Tutorial L Systems i used this tutorial to learn to create a generator using Generative Grammars

[CreateAssetMenu(menuName = "ProceduralCity/Rule")] // this attribule will allow us to create scriptable objects
public class Rule : ScriptableObject // it is a form of data container to create scriptable objects in the assets to be able to reuse the rule in different towns/dungeons
{
    public string letter; // this letter will trigger rule

    [SerializeField]
    private string[] results = null;

    [SerializeField]
    private bool randomResult = false;


    public string GetResult()
    {
        if (randomResult)
        {
            int randomIndex = UnityEngine.Random.Range(0, results.Length);
            return results[randomIndex];
        }
        return results[0]; // get different results based on random values
    }

}
