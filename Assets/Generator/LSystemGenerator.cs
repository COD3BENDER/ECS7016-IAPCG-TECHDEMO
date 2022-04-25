using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// This Script is the generation of The recursive L system Generative Grammars
// reference: Sunny Valley Studios Tutorial L Systems i used this tutorial to learn to create a generator using Generative Grammars
public class LSystemGenerator : MonoBehaviour
{
    public Rule[] rules; // this parameter will hold all the rules we want to use in the l system
    public string rootSentence; // this is the axioms
    [Range(0, 10)] // this gives the iteration limit a range between 0 and 10 
    public int iterationLimit = 1;

    public bool randomIgnoreRuleModifier = true;

    [Range(0, 1)]
    public float chanceToIgnoreRule = 0.3f; // 30% chance to not create a branch where it should be to randomize

    private void Start()
    {
        Debug.Log(GenerateSentence());
    }

    public string GenerateSentence(string word = null)
    {
        if(word == null)
        {
            word = rootSentence;
        }
        return GrowRecursive(word);
    }

    private string GrowRecursive(string word, int currentIteration = 0)
    {
        if(currentIteration >= iterationLimit)
        {
            return word;
        }
        StringBuilder stringBuilder = new StringBuilder(); // represents stringlike object where values are mutable helps performance and parameter stringBuilder is a new word

        foreach (var c in word)
        {
            stringBuilder.Append(c); // append each character and add recursive part
            ProcessRulesRecursively(stringBuilder, c, currentIteration);
        }

        return stringBuilder.ToString();
    }

    private void ProcessRulesRecursively(StringBuilder stringBuilder, char c, int currentIteration)
    {
        foreach (var rule in rules)
        {
           if(rule.letter == c.ToString())
            {
                if (randomIgnoreRuleModifier && currentIteration > 1)
                {
                    if (UnityEngine.Random.value < chanceToIgnoreRule)
                    {
                        return; // ignore a rule while creating L system output to give randomness
                    }
                }
                stringBuilder.Append(GrowRecursive(rule.GetResult(), currentIteration + 1)); // process new symbols as long as we are below limitiation of iteration index
            }
        }
    }
}
