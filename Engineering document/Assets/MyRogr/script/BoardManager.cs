using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    [SerializeField]
    public class Count
    {
        public int minimum;
        public int maximum;
        public Count(int _min ,int _max )
        {
            this.minimum = _min;
            this.maximum = _max;
        }

    }

    public int columns = 8;
    public int rows = 8;

    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;
    public GameObject[] wallTiles;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    void InitialiseList()
    {
        gridPositions.Clear();

        for(int x = 1;x < columns-1;x++)
        {
            for(int y =1;y< rows - 1;y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));

            }
        }
    }

    private void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;

        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if (x == -1 || y == -1 || x == columns || y == rows)
                {
                    
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] _tiles ,int _min , int _max)
    {
        int objectCount = Random.Range(_min, _max+1);
        for(int i =0;i<objectCount;i++)
        {
            Vector3 vector3 = RandomPosition();
            GameObject gameObject = _tiles[Random.Range(0, _tiles.Length)];
            Instantiate(gameObject, vector3, Quaternion.identity);
        }

    }

    public void SetupScene(int _level)
    {
        BoardSetup();
        InitialiseList();
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
        int enemyCount = (int)Mathf.Log(_level);
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
    }

    
}


