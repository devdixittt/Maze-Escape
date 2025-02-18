using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class mazeGenerator : MonoBehaviour
{
    [SerializeField]
    private mazeCell _mazePrefab;

    [SerializeField]
    private int mazeWidth;

    [SerializeField]
    private int mazeDepth;

    [SerializeField]
    private mazeCell[,] mazeGrid;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mazeGrid = new mazeCell[mazeWidth, mazeDepth];

        for(int x = 0; x < mazeWidth; x++)
        {
            for(int z = 0; z < mazeDepth; z++)
            {
                mazeGrid[x,z] = Instantiate(_mazePrefab, new Vector3(x, -1.08f, z), Quaternion.identity);
            }
        }

        generateMaze(null, mazeGrid[0,0]);
    }

    private void generateMaze(mazeCell previousMaze, mazeCell currentMaze)
    {
        currentMaze.Visited();
        clearWalls(previousMaze, currentMaze);

        mazeCell nextCell;
        do
        {
            nextCell = getUnvisitedCell(currentMaze);
            if (nextCell != currentMaze)
            {
                generateMaze(currentMaze, nextCell);
            }
        } while (nextCell != null);

    }

    private mazeCell getUnvisitedCell(mazeCell currentMaze)
    {
        var unvisitedCell = GetUnvisitedCell(currentMaze);
        return unvisitedCell.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }
    

    private IEnumerable<mazeCell> GetUnvisitedCell(mazeCell currentMaze)
    {
        int x = (int)currentMaze.transform.position.x;
        int z = (int)currentMaze.transform.position.z;

        if (x + 1 < mazeWidth)
        {
            var cellToRight = mazeGrid[x + 1, z];
            if(cellToRight.isVisited == false)
            {
                yield return cellToRight;
            }

            
        }

        if (x - 1 >= 0)
        {
            var cellToLeft = mazeGrid[x - 1, z];
            if (cellToLeft.isVisited == false)
            {
                yield return cellToLeft;
            }
        }

        if(z + 1 < mazeDepth)
        {
            var cellToForward = mazeGrid[x, z + 1];
            if(cellToForward.isVisited == false)
            {
                yield return cellToForward;
            }
        }

        if(z - 1 >= 0)
        {
            var cellToBack = mazeGrid[x, z - 1];
            if(cellToBack.isVisited == false)
            {
                yield return cellToBack;
            }
        }
    }
    private void clearWalls(mazeCell previousMaze, mazeCell currentMaze)
    {
        if(previousMaze == null)
        {
            return;
        }

        if(previousMaze.transform.position.x < currentMaze.transform.position.x)
        {
            previousMaze.clearRightWalls();
            currentMaze.clearLeftWalls();
            return;
        }

        if(previousMaze.transform.position.x > currentMaze.transform.position.x)
        {
            previousMaze.clearLeftWalls();
            currentMaze.clearRightWalls();
            return;
        }

        if(previousMaze.transform.position.z < currentMaze.transform.position.z)
        {
            previousMaze.clearFrontWalls();
            currentMaze.clearBackWalls();
            return;
        }

        if(previousMaze.transform.position.z > currentMaze.transform.position.z)
        {
            previousMaze.clearBackWalls();
            currentMaze.clearFrontWalls();
            return;
        }
    }
}
