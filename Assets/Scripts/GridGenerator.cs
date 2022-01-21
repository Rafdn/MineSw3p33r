using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    private int rows = 1;
    private int cols;
    private float tileSize = 1.05f;
    private int depth;
    private int level;
    public GameObject[,,] Cubes;

    void Start()
    {
        
        if (PlayerPrefs.GetInt("Level") == 0)
        {
            PlayerPrefs.SetInt("Level", 1);
            level = 1;
            PlayerPrefs.Save();
        }
        else
        {
            level = PlayerPrefs.GetInt("Level");
        }
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        if (level >= 10)
        {
            rows = 2;
        }

        cols = level + 5;
        depth = level + 5;
        Cubes = new GameObject[rows, depth, cols];
        GameObject referenceTile = (GameObject) Instantiate(Resources.Load("MineCube"));
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                for (int d = 0; d < depth; d++)
                {
                    GameObject tile = (GameObject) Instantiate(referenceTile, transform);
                    tile.GetComponent<CubeController>().X = col;
                    tile.GetComponent<CubeController>().Y = d;
                    tile.GetComponent<CubeController>().Z = row;

                    Cubes[row, d, col] = tile;

                    float posX = col * tileSize;
                    float posY = d * tileSize;
                    float posZ = row * tileSize;
                    //tile.name = $"X:{posX}, Y:{posY}, Z:{posZ}";

                    tile.transform.position = new Vector3(posX, posY, posZ);
                }
            }
        }

        Destroy(referenceTile);
    }
}