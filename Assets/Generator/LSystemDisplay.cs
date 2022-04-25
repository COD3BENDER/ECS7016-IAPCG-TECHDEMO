using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LSystemParameters;

// reference: Sunny Valley Studios Tutorial L Systems i used this tutorial to learn to create a generator using Generative Grammars
public class LSystemDisplay : MonoBehaviour
{
    public LSystemGenerator lsystem;
    List<Vector3> positions = new List<Vector3>(); // this list will save the positions where the agent has already travelled
    private int length = 12;
    private float angle = 90; // this will be the angle the agent will use to turn left or right
    public RoadCreator roadCreator;
    public BuildingCreator buildingCreator;

    public int Length
    {
        get
        {
            if (length > 0)
            {
                return length;
            }
            else
            {
                return 1; // need to initialize with a value for the map to be visable
            }

        }
        set => length = value;
    }
    private void Start()
    {
        var sequence = lsystem.GenerateSentence();
        VisualizeSequence(sequence);
    }

    private void VisualizeSequence(string sequence) // sequence is the output of the L system
    {
        Stack<LSystemParameters> savePoints = new Stack<LSystemParameters>();   //last in point is the first out point LIFO
        var currentPosition = Vector3.zero;

        Vector3 direction = Vector3.forward; // starting direction
        Vector3 tempPosition = Vector3.zero; // this will be used when drawing road and the temp position to the next position will draw a line which is the line between temp pos and current pos

        positions.Add(currentPosition); // adds current position to the array and will be used to draw the prefabs where the agent has stopped

        foreach (var letter in sequence) // look for each letter again to draw 
        {
            ActionLetters encoding = (ActionLetters)letter; // ensure that each letter in the sequence is mapped into the enum
            switch (encoding)
            {
                case ActionLetters.save:
                    savePoints.Push(new LSystemParameters
                    {
                        position = currentPosition,
                        direction = direction,
                        length = length
                    });
                    break;
                case ActionLetters.load:
                    if (savePoints.Count > 0)
                    {
                        var agentParameter = savePoints.Pop(); // get last saved position
                        currentPosition = agentParameter.position;
                        direction = agentParameter.direction;
                        Length = agentParameter.length;
                    }
                    else
                    {
                        throw new System.Exception("no savepoint saved into stack");
                    }
                    break;
                case ActionLetters.draw:
                    tempPosition = currentPosition; // we need to save it to draw a line
                    currentPosition += direction * length; // give a new point that is moved by the length in the direction that is set
                    roadCreator.PlaceStreetPosition(tempPosition, Vector3Int.RoundToInt(direction), length);
                    Length -= 2; // this will cause the next line to be shorter 
                    positions.Add(currentPosition);
                    break;
                case ActionLetters.turnRight:
                    direction = Quaternion.AngleAxis(angle, Vector3.up) * direction; // angle turned to the right
                    break;
                case ActionLetters.turnLeft:
                    direction = Quaternion.AngleAxis(-angle, Vector3.up) * direction; // angle turned to the left by adding a - to the angle
                    break;
                default:
                    break;
            }

        }
        roadCreator.FixRoad();
        buildingCreator.PlaceBuildingAroundRoad(roadCreator.GetRoadPositions());


    }

    public enum ActionLetters // command letters for generation
    {
        unknown = '1',
        save = '[',
        load = ']',
        draw = 'A',
        turnRight = '+',
        turnLeft = '-'
    }
}
