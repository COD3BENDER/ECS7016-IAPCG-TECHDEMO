using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// reference: Sunny Valley Studios Tutorial L Systems i used this tutorial to learn to create a generator using Generative Grammars
public static class PlacementHelper // used for roadhelper an buildinghleper classes
{
    public static List<Direction> FindNeighbour(Vector3Int position, ICollection<Vector3Int> collection)
    {
        List<Direction> neighbourDirections = new List<Direction>();

        if (collection.Contains(position + Vector3Int.right)) // check x axis
        {
            neighbourDirections.Add(Direction.Right);
        }

        if (collection.Contains(position - Vector3Int.right))
        {
            neighbourDirections.Add(Direction.Left);
        }
        if (collection.Contains(position + new Vector3Int(0, 0, 1)))
        {
            neighbourDirections.Add(Direction.Up);
        }
        if (collection.Contains(position - new Vector3Int(0, 0, 1)))
        {
            neighbourDirections.Add(Direction.Down); // check z axis
        }
        return neighbourDirections;
    }

    internal static Vector3Int GetOffsetFromDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return new Vector3Int(0, 0, 1);
            case Direction.Down:
                return new Vector3Int(0, 0, -1);
            case Direction.Left:
                return Vector3Int.left;
            case Direction.Right:
                return Vector3Int.right;
            default:
                break;
        }
        throw new System.Exception("There is no direction of " + direction);
    }

    public static Direction GetReverseDirection(Direction direction) // we want the buildings to face the road
    {
        switch (direction)
        {
            case Direction.Up:
                return Direction.Down;
            case Direction.Down:
                return Direction.Up;
            case Direction.Left:
                return Direction.Right;
            case Direction.Right:
                return Direction.Left;
            default:
                break;
        }
        throw new System.Exception("There is no direction of " + direction);
    }
}
