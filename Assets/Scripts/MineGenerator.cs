using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MineGenerator : MonoBehaviour
{
    private GameObject[,,] cubesReference;
    [HideInInspector]public int mineCount = 5;
    public int[,,] mines;
    private int level;
    
    [HideInInspector]public List<int> mineX;
    [HideInInspector]public List<int> mineY;
    [HideInInspector]public List<int> mineZ;
    

    private void Start()
    {
        level = PlayerPrefs.GetInt("Level");
        mineX = new List<int>();
        mineY = new List<int>();
        mineZ = new List<int>();
        cubesReference = GetComponent<GridGenerator>().Cubes;
        mines = new int[cubesReference.GetLength(0), cubesReference.GetLength(1), cubesReference.GetLength(2)];
        generateMines();
    }

    void generateMines()
    {
        mineCount = level +2;
        if (level >= 10)
        {
            
            mineCount += 10;
        }
        
        for (int i = 0; i < mineCount;)
        {
            int minePosZ = Random.Range(0, mines.GetLength(0));
            int minePosY = Random.Range(0, mines.GetLength(1));
            int minePosX = Random.Range(0, mines.GetLength(2));

            if (mines[minePosZ, minePosY, minePosX] != 1)
            {
                mines[minePosZ, minePosY, minePosX] = 1;
                mineX.Add(minePosX);
                mineY.Add(minePosY);
                mineZ.Add(minePosZ);
                cubesReference[minePosZ, minePosY, minePosX].GetComponent<CubeController>().isMine = true;
                i++;
                //cubesReference[minePosZ, minePosY, minePosX].SetActive(false);
                for (int j = -1; j < 2; j++)
                {
                    for (int k = -1; k < 2; k++)
                    {
                        for (int m = -1; m < 2; m++)
                        {
                            if (j == 0 && k == 0 && m == 0)
                                continue;
                            if (minePosZ + j >= 0 && minePosZ + j < mines.GetLength(0) && minePosY + k >= 0 &&
                                minePosY + k < mines.GetLength(1) && minePosX + m >= 0 &&
                                minePosX + m < mines.GetLength(2))
                            {
                                cubesReference[minePosZ + j, minePosY + k, minePosX + m].GetComponent<CubeController>()
                                    .mineCount++;
                            }
                        }
                    }
                }
                //Debug.Log(minePosX + " "+minePosY+" "+minePosZ);
            }
        }

        while (true)
        {
            int minePosZ = Random.Range(0, mines.GetLength(0));
            int minePosY = Random.Range(0, mines.GetLength(1));
            int minePosX = Random.Range(0, mines.GetLength(2));

            if (mines[minePosZ, minePosY, minePosX] != 1)
            {
                foreach (TextMeshProUGUI component in cubesReference[minePosZ,minePosY,minePosX].GetComponentsInChildren<TextMeshProUGUI>())
                {
                    component.text = "X";
                    component.color = new Color(1f / 255f, 126f / 255f, 0f);
                }
                break;
            }
        }
    }
}