using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    enum gridSpace { empty, floor, wall };
    enum entity { empty, player, enemy };
    gridSpace[,] grid;
    entity[,] entityGrid;
    int roomHeight, roomWidth;
    Vector2 roomSizeWorldUnits = new Vector2(35, 35);
    float worldUnitsInOneGridCell = 1.2f;
    struct walker
    {
        public Vector2 dir;
        public Vector2 pos;
    }
    List<walker> walkers;
    float chanceWalkerChangeDir = 0.65f, chanceWalkerSpawn = 0.1f;
    float chanceWalkerDestroy = 0.05f;
    int maxWalkers = 10;
    float percentToFill = 0.2f;
    public GameObject wallObj;
    public GameObject floorObj;

    float chanceEnemySpawn = 0.15f;
    int maxNumberOfEnemies = 15;
    int curNumberOfEnemies;
    [SerializeField] List<GameObject> randomEnemy;

    void Start()
    {
        Setup();
        CreateFloors();
        CreateWalls();
        RemoveSingleWalls();
        SetEnemies();
        SpawnLevel();
    }

    void Setup()
    {
        // find grid size
        roomHeight = Mathf.RoundToInt(roomSizeWorldUnits.x / worldUnitsInOneGridCell);
        roomWidth = Mathf.RoundToInt(roomSizeWorldUnits.y / worldUnitsInOneGridCell);
        // create grid
        grid = new gridSpace[roomWidth, roomHeight];
        entityGrid = new entity[roomWidth, roomHeight];
        // set grid's default state
        for (int x = 0; x < roomWidth - 1; x++)
        {
            for (int y = 0; y < roomHeight - 1; y++)
            {
                // make every cell "empty"
                grid[x, y] = gridSpace.empty;
                entityGrid[x, y] = entity.empty;
            }
        }

        //set first walker
        // init list
        walkers = new List<walker>();
        //create a walker
        walker newWalker = new walker();
        newWalker.dir = RandomDirection();
        // find center of grid;
        float centerX = Mathf.RoundToInt(roomWidth / 2.0f);
        float centerY = Mathf.RoundToInt(roomHeight / 2.0f);
        Vector2 spawnPos = new Vector2(centerX, centerY);
        newWalker.pos = spawnPos;
        // add walker to list
        walkers.Add(newWalker);

    }

    void CreateFloors()
    {
        // find center of grid;
        float centerX = Mathf.RoundToInt(roomWidth / 2.0f);
        float centerY = Mathf.RoundToInt(roomHeight / 2.0f);

        // create a base around the center where the player spawns
        for (int checkX = -1; checkX <= 1; checkX++)
        {
            for (int checkY = -1; checkY <= 1; checkY++)
            {
                grid[(int)centerX + checkX, (int)centerY + checkY] = gridSpace.floor;
            }
        }
        entityGrid[(int)centerX, (int)centerY] = entity.player;

        int iterations = 0; // loop will not run forever;
        do
        {
            //create floor at positiion of every walker
            foreach (walker myWalker in walkers)
            {
                grid[(int)myWalker.pos.x, (int)myWalker.pos.y] = gridSpace.floor;
            }

            // chance: destroy walker
            int numberChecks = walkers.Count; // might modify count while in this loop
            for (int i = 0; i < numberChecks; i++)
            {
                // only if its not the only, and at a low chance
                if (Random.value < chanceWalkerDestroy && walkers.Count > 1)
                {
                    walkers.RemoveAt(i);
                    break; // only destroy one per iteration
                }
            }

            // chance: walker pick new direction
            for (int i = 0; i < walkers.Count; i++)
            {
                if (Random.value < chanceWalkerChangeDir)
                {
                    walker thisWalker = walkers[i];
                    thisWalker.dir = RandomDirection();
                    walkers[i] = thisWalker;
                }
            }

            // chance: spawn new walker
            numberChecks = walkers.Count;
            for (int i = 0; i < numberChecks; i++)
            {
                // only if # of walkers < max, and at a low chance
                if (Random.value < chanceWalkerSpawn && walkers.Count < maxWalkers)
                {
                    // create a walker
                    walker newWalker = new walker();
                    newWalker.dir = RandomDirection();
                    newWalker.pos = walkers[i].pos;
                    walkers.Add(newWalker);
                }
            }

            // move walkers
            for (int i = 0; i < walkers.Count; i++)
            {
                walker thisWalker = walkers[i];
                thisWalker.pos += thisWalker.dir;
                walkers[i] = thisWalker;
            }

            //avoid boarder of grid
            for (int i = 0; i < walkers.Count; i++)
            {
                walker thisWalker = walkers[i];

                // clamp x,y to leave a 1 space boarder: leave room for walls
                thisWalker.pos.x = Mathf.Clamp(thisWalker.pos.x, 1, roomWidth - 2);
                thisWalker.pos.y = Mathf.Clamp(thisWalker.pos.y, 1, roomHeight - 2);
                walkers[i] = thisWalker;
            }

            // check to exit loop
            if ((float)NumberOfFloors() / (float)grid.Length > percentToFill)
            {
                break;
            }
            iterations++;
        } while (iterations < 100000);
    }

    void CreateWalls()
    {
        // loop through every grid space
        for (int x = 0; x < roomWidth-1; x++)
        {
            for (int y= 0; y < roomHeight - 1; y++)
            {
                // if there is a floor, check the spaces around it
                if (grid[x,y] == gridSpace.floor)
                {
                    // if any surrounding spaces are empty, place a wall
                    if (grid[x,y+1] == gridSpace.empty)
                    {
                        grid[x, y + 1] = gridSpace.wall;
                    }
                    if (grid[x, y-1] == gridSpace.empty)
                    {
                        grid[x, y - 1] = gridSpace.wall;
                    }
                    if (grid[x+1,y] == gridSpace.empty)
                    {
                        grid[x + 1, y] = gridSpace.wall;
                    }
                    if (grid[x-1,y] == gridSpace.empty)
                    {
                        grid[x - 1, y] = gridSpace.wall;
                    }
                }
            }
        }
    }

    void RemoveSingleWalls()
    {
        //loop through every grid space
        for (int x = 0; x < roomWidth-1; x++)
        {
            for (int y = 0; y < roomHeight-1; y++)
            {
                // if there is a wall, check the spaces around it
                if (grid[x,y] == gridSpace.wall)
                {
                    //assume all spaces around wall are floors
                    bool allFloors = true;
                    //check each side to see if they are all floors
                    for (int checkX = -1; checkX <= 1; checkX++)
                    {
                        for (int checkY = -1; checkY <= 1; checkY++)
                        {
                            if (x + checkX < 0 || x + checkX > roomWidth - 1 ||
                                y + checkY < 0 || y + checkY > roomHeight - 1)
                            {
                                // skip checks that are out of range
                                continue;
                            }

                            if ((checkX != 0 && checkY != 0) || (checkX == 0 && checkY == 0))
                            {
                                // skip corners and center
                                continue;
                            }

                            if (grid[x + checkX, y+ checkY] != gridSpace.floor)
                            {
                                allFloors = false;
                            }
                                
                        }
                    }
                    if (allFloors)
                    {
                        grid[x, y] = gridSpace.floor;
                    }
                }
            }
        }
    }

    void SetEnemies()
    {
        for (int x = 0; x < roomWidth - 1; x++)
        {
            for (int y = 0; y < roomHeight - 1; y++)
            {
                if (grid[x, y] == gridSpace.floor)
                {
                    //assume all spaces around wall are clear of enemies
                    bool allEmpty = true;
                    //check each side to see if they are all floors
                    for (int checkX = -1; checkX <= 1; checkX++)
                    {
                        for (int checkY = -1; checkY <= 1; checkY++)
                        {
                            if (x + checkX < 0 || x + checkX > roomWidth - 1 ||
                                y + checkY < 0 || y + checkY > roomHeight - 1)
                            {
                                // skip checks that are out of range
                                continue;
                            }

                            if (entityGrid[x + checkX, y + checkY] == entity.enemy ||
                                entityGrid[x + checkX, y + checkY] == entity.player )
                            {
                                allEmpty = false;
                            } 
                        }
                    }

                    if (allEmpty)
                    {
                        if (Random.value < chanceEnemySpawn && curNumberOfEnemies < maxNumberOfEnemies)
                        {
                            curNumberOfEnemies++;
                            entityGrid[x, y] = entity.enemy;
                        }
                    }
                }
            }
        }

    }

    void SpawnLevel()
    {
        for( int x = 0; x <roomWidth; x++)
        {
            for (int y = 0; y < roomHeight; y++)
            {
                switch (grid[x, y])
                {
                    case gridSpace.empty:
                        break;
                    case gridSpace.floor:
                        Spawn(x, y, floorObj);
                        break;
                    case gridSpace.wall:
                        Spawn(x, y, wallObj);
                        break;
                }

                switch (entityGrid[x, y])
                {
                    case entity.empty:
                        break;
                    case entity.enemy:
                        if (Random.Range(0f, 1f) > 0.7f)
                        {
                            Spawn(x, y, -0.2f, randomEnemy[1]);
                        }
                        else
                        {
                            Spawn(x, y, -0.2f, randomEnemy[0]);
                        }
                        
                        break;
                }

            }
        }
    }

    void Spawn(float x, float y, GameObject toSpawn)
    {
        Spawn(x, y, 0, toSpawn);
    }

    void Spawn(float x, float y, float z, GameObject toSpawn)
    {
        // find the position to spawn
        Vector2 offset = roomSizeWorldUnits / 2.0f;
        Vector2 spawnPos = new Vector2(x, y) * worldUnitsInOneGridCell - offset;

        Vector3 fullPos = new Vector3(spawnPos.x, spawnPos.y, z);
        // spawn object
        Instantiate(toSpawn, fullPos, Quaternion.identity);
    }
    
    int NumberOfFloors()
    {
        int count = 0;
        foreach (gridSpace space in grid)
        {
            if (space == gridSpace.floor)
            {
                count++;
            }
        }
        return count;
    }

    Vector2 RandomDirection()
    {
        // pick random int between 0 and 3
        int choice = Mathf.FloorToInt(Random.value * 3.99f);
        // use that int to choose a direction
        switch (choice)
        {
            case 0:
                return Vector2.down;
            case 1:
                return Vector2.left;
            case 2:
                return Vector2.up;
            case 3:
                return Vector2.right;
        }
        return Vector2.down;
    }
}
