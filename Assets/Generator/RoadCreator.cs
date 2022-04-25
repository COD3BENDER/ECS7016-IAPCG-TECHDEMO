using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// reference: Sunny Valley Studios Tutorial L Systems i used this tutorial to learn to create a generator using Generative Grammars
public class RoadCreator : MonoBehaviour
{
    public GameObject straightRoad, cornerRoad, threeWayJunction, fourWayJunction, deadendRoad;
    // dictioanary that will store poistions of gameobject so we can delete them when done 
    Dictionary<Vector3Int, GameObject> roadDictionary = new Dictionary<Vector3Int, GameObject>();
    HashSet<Vector3Int> fixRoadCandidates = new HashSet<Vector3Int>(); // prefab will need to change to match the location such as corner block or terrace block
    public List<Vector3Int> GetRoadPositions()
    {
        return roadDictionary.Keys.ToList();
    }
    public void PlaceStreetPosition(Vector3 startPos, Vector3Int direction, int length)
    {
        var rotation = Quaternion.identity;
        if(direction.x == 0) // should follow x direction
        {
            rotation = Quaternion.Euler(0, 90, 0); // rotate the prefab to follow the road if its going in the z direction
        }
        for (int i = 0; i< length; i++)
        {
            var position = Vector3Int.RoundToInt(startPos + direction * i);

            if (roadDictionary.ContainsKey(position)) // this is to stop the agent to not place a road twice when the agent is rotating to place the road it needs to check if the road dictionary has a key of that position
            {
                continue;
            }

            var road = Instantiate(straightRoad, position, rotation, transform);
            roadDictionary.Add(position, road);
            if (i == 0 || i == length - 1)
            {
                fixRoadCandidates.Add(position);
            }
        }
    }
    public void FixRoad()
    {
        foreach (var position in fixRoadCandidates) // hashsets doesnt take duplicates so this ensures we only go through the position once
        {
            List<Direction> neighbourDirections = PlacementHelper.FindNeighbour(position, roadDictionary.Keys); // we can call findneighbour as placementhelper is a static method

            Quaternion rotation = Quaternion.identity; // we need to identify the position of the neighbouring tiles to rotate correctly

            if(neighbourDirections.Count == 1)
            {
                Destroy(roadDictionary[position]);
                if (neighbourDirections.Contains(Direction.Down)) // if you  want to rotate the road downwards it has to be rotated by 90 degrees as we start in the right position
                {
                    rotation = Quaternion.Euler(0, 90, 0);
                }
                else if (neighbourDirections.Contains(Direction.Left))
                {
                    rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (neighbourDirections.Contains(Direction.Up))
                {
                    rotation = Quaternion.Euler(0, 270, 0);
                }
                roadDictionary[position] = Instantiate(deadendRoad, position,rotation, transform);
            }
            else if (neighbourDirections.Count == 2)
            {
                if (neighbourDirections.Contains(Direction.Up) && neighbourDirections.Contains(Direction.Down) ||
                    neighbourDirections.Contains(Direction.Right) && neighbourDirections.Contains(Direction.Left))
                {
                    continue;
                }
                Destroy(roadDictionary[position]);

                if (neighbourDirections.Contains(Direction.Up) && neighbourDirections.Contains(Direction.Right)) // if you  want to rotate the road downwards it has to be rotated by 90 degrees as we start in the right position
                {
                    rotation = Quaternion.Euler(0, 90, 0);
                }
                else if (neighbourDirections.Contains(Direction.Right) && neighbourDirections.Contains(Direction.Down))
                {
                    rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (neighbourDirections.Contains(Direction.Down) && neighbourDirections.Contains(Direction.Left))
                {
                    rotation = Quaternion.Euler(0, 270, 0);
                }
                roadDictionary[position] = Instantiate(cornerRoad, position, rotation, transform);



            }
            else if (neighbourDirections.Count == 3) // if a 3 way street is needed 
            {
                Destroy(roadDictionary[position]);

                if (neighbourDirections.Contains(Direction.Right) && neighbourDirections.Contains(Direction.Left) && neighbourDirections.Contains(Direction.Down)) // if you  want to rotate the road downwards it has to be rotated by 90 degrees as we start in the right position
                {
                    rotation = Quaternion.Euler(0, 90, 0);
                }
                else if (neighbourDirections.Contains(Direction.Down) && neighbourDirections.Contains(Direction.Up) && neighbourDirections.Contains(Direction.Left))
                {
                    rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (neighbourDirections.Contains(Direction.Left) && neighbourDirections.Contains(Direction.Up) && neighbourDirections.Contains(Direction.Right))
                {
                    rotation = Quaternion.Euler(0, 270, 0);
                }
                roadDictionary[position] = Instantiate(threeWayJunction, position, rotation, transform);

            }
            else // if a 4 way street is needed 
            {
                Destroy(roadDictionary[position]);
                roadDictionary[position] = Instantiate(fourWayJunction, position, rotation, transform);
            }

        }
    }
}
