using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// reference: Sunny Valley Studios Tutorial L Systems i used this tutorial to learn to create a generator using Generative Grammars
public class BuildingCreator : MonoBehaviour
{
    public BuildingType[] buildingTypes; // ----------------------------------------------------------------------------change to wall type
    public Dictionary<Vector3Int, GameObject> structuresDictionary = new Dictionary<Vector3Int, GameObject>();

    public void PlaceBuildingAroundRoad(List<Vector3Int> roadPositions)
    {
        Dictionary<Vector3Int, Direction> freeEstateSpots = FindFreeSpacesAroundRoad(roadPositions);
        List<Vector3Int> blockedPositions = new List<Vector3Int>(); // this is to add buildings of size bigger than one space 
        foreach (var freeSpot in freeEstateSpots) // this method is to make the houses face in the correct direction to the road using the reverse position of the road 
        {
            if (blockedPositions.Contains(freeSpot.Key)) // when a structure is placed that needs more space then block the areas around it from objects being placed on it 
            {
                continue;
            }
            var rotation = Quaternion.identity;

            switch (freeSpot.Value)
            {
                case Direction.Up:
                    rotation = Quaternion.Euler(0, 90, 0); // all prefabs pointing to the left so rotate by 90 to turn up
                    break;
                case Direction.Down:
                    rotation = Quaternion.Euler(0, 270, 0); // all prefabs pointing to the left so rotate by 270 to turn down
                    break;
                case Direction.Right:
                    rotation = Quaternion.Euler(0, 180, 0); // all prefabs pointing to the left 
                    break;
                default:
                    break;
            }
            for (int i = 0; i < buildingTypes.Length; i++)
            {
                // the first structure is the most important and will go to next one and place the biggest first then go smaller
                if(buildingTypes[i].quantity == -1)
                {
                    var building = SpawnBuilding(buildingTypes[i].GetPrefab(), freeSpot.Key, rotation); // can get the position of free area and the rotation 
                    structuresDictionary.Add(freeSpot.Key, building); // pass position and building 
                    break;
                }// looping through all structures 
                if (buildingTypes[i].IsBuildingAvailable()) 
                {
                    if(buildingTypes[i].sizeRequired > 1)// this is used to place objects of bigger size 
                    {
                        var halfSize = Mathf.FloorToInt(buildingTypes[i].sizeRequired / 2.0f); // this will give block 2 spaces in both direction if there is a 3 sized object
                        List<Vector3Int> tempPositionsBlocked = new List<Vector3Int>();

                        // pass blocked positions below

                        if(verifyBuildingFits(halfSize, freeEstateSpots, freeSpot, blockedPositions, ref tempPositionsBlocked)) // this will return true or false so temp position Blocked has to be saved to list temppositionBlocked
                        {
                            blockedPositions.AddRange(tempPositionsBlocked); // to not place two objects at the same position
                            var building = SpawnBuilding(buildingTypes[i].GetPrefab(), freeSpot.Key, rotation); // can get the position of free area and the rotation 
                            structuresDictionary.Add(freeSpot.Key, building); // pass position and building 
                            foreach(var pos in tempPositionsBlocked)
                            {
                                structuresDictionary.Add(pos, building); // can be used to access the structure if the dictionary contains the clicked structure
                            }
                        }
                    }
                    else
                    {
                        var building = SpawnBuilding(buildingTypes[i].GetPrefab(), freeSpot.Key, rotation); // can get the position of free area and the rotation 
                        structuresDictionary.Add(freeSpot.Key, building); // pass position and building 
                        //break;
                    }
                    break;
                }
            }
            
        }

    }

    private bool verifyBuildingFits(int halfSize, Dictionary<Vector3Int, Direction> freeEstateSpots, KeyValuePair<Vector3Int, Direction> freeSpot, List<Vector3Int> blockedPositions, ref List<Vector3Int> tempPositionsBlocked)
    {
        Vector3Int direction = Vector3Int.zero; // WE NEED TO CHECK THIS DIRECTION
        if(freeSpot.Value == Direction.Down || freeSpot.Value == Direction.Up)
        {
            direction = Vector3Int.right; // this will allow the movement in both direction right or left 
        }
        else
        {
            direction = new Vector3Int(0,0,1); // if the road is on the right or left then set it to be a new vector 3 on z axis
        }
        for (int i = 1; i <= halfSize; i++)
        {
            // check one position to the right and one to the left (+ == right) and (- == left)
            var rightPos = freeSpot.Key + direction * i; // position of current free space 
            var leftPos = freeSpot.Key - direction * i;

            //check if the rightPos and 2 is empty if it isnt return false the blockedPositions is added in as a bug fix as a size greater than 1 couldnt be placed more than once before as there was keyvalue duplicates 
            if(!freeEstateSpots.ContainsKey(rightPos) || !freeEstateSpots.ContainsKey(leftPos)|| blockedPositions.Contains(rightPos) ||blockedPositions.Contains(leftPos))
            {
                return false;
            }
            tempPositionsBlocked.Add(rightPos);
            tempPositionsBlocked.Add(leftPos);
        }
        return true;
    }

    private GameObject SpawnBuilding(GameObject prefab, Vector3Int position, Quaternion rotation)
    {
        var newStructure = Instantiate(prefab, position, rotation, transform); // instatiate under structure helper and get transform as its the parent
        return newStructure;
    }

    private Dictionary<Vector3Int, Direction> FindFreeSpacesAroundRoad(List<Vector3Int> roadPositions)
    {
        Dictionary<Vector3Int, Direction> freeSpaces = new Dictionary<Vector3Int, Direction>();
        foreach (var position in roadPositions)
        {
            var neighbourDirections = PlacementHelper.FindNeighbour(position, roadPositions);
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                if(neighbourDirections.Contains(direction)== false)
                {
                    var newPosition = position + PlacementHelper.GetOffsetFromDirection(direction);
                    if (freeSpaces.ContainsKey(newPosition))
                    {
                        continue; // check two roads an they can have the same free spaces around them 
                    }
                    freeSpaces.Add(newPosition, PlacementHelper.GetReverseDirection(direction)); // we need to find the position of the street so the building can face it so a method that gives the opposite direction is needed as the building has to face the road
                }
            }
        }
        return freeSpaces;
    }
}
