using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using Unity.Collections;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InteractionScript : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    GameObject[,,] cubesReference;
    private int[,,] minesReference;
    private GameObject controller;
    private int money;
    private int health;
    private int shield;
    private int level;
    public GameObject textHolder;
    List<int> minesX;
    List<int> minesY;
    List<int> minesZ;
    private int mineCount;
    private int opened;
    public TextMeshProUGUI gameOverText;
    private int cubeCount;
    private bool levelUp = false;

    private void Start()
    {
        money = PlayerPrefs.GetInt("Money");
        health = PlayerPrefs.GetInt("Health");
        shield = PlayerPrefs.GetInt("Shield");
        level = PlayerPrefs.GetInt("Level");
        opened = 0;
        if (SceneManager.GetActiveScene().name == "MineScene")
        {
            controller = GameObject.Find("GridController");
            cubesReference = controller.GetComponent<GridGenerator>().Cubes;
            minesReference = controller.GetComponent<MineGenerator>().mines;
            mineCount = controller.GetComponent<MineGenerator>().mineCount;
            minesX = controller.GetComponent<MineGenerator>().mineX;
            minesY = controller.GetComponent<MineGenerator>().mineY;
            minesZ = controller.GetComponent<MineGenerator>().mineZ;
            cubeCount = cubesReference.GetLength(0) * cubesReference.GetLength(1) * cubesReference.GetLength(2);
        }
    }

    
    void updateCanvas()
    {
        textHolder.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = money.ToString();
        textHolder.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = health.ToString();
        textHolder.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = shield.ToString();
    }

    void setPlayerPrefs()
    {
        PlayerPrefs.SetInt("Money", money);
        PlayerPrefs.SetInt("Health", health);
        PlayerPrefs.SetInt("Shield", shield);
        PlayerPrefs.SetInt("Level", level);
        PlayerPrefs.Save();
    }

    public void exit()
    {
        if (shield >= 2)
        {
            shield -= 2;
            setPlayerPrefs();
            GetComponent<SceneScript>().switchScene();

        }
        else
        {
            gameOver();
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            health = 1000;
            money = 1000;
            level = 1;
            setPlayerPrefs();
            updateCanvas();
        }
        if ((cubeCount - opened- mineCount) == 0 && !levelUp && SceneManager.GetActiveScene().name == "MineScene" )
        {
            levelUp = true;
            win();
        }
        
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            GameObject h = hit.collider.gameObject;
            if (Input.GetMouseButtonDown(0))
            {
                if (hit.collider.name.Contains("MineCube"))
                {
                    if (h.GetComponent<CubeController>().isMine && !h.GetComponent<CubeController>().flagged)
                    {
                        if (shield >= 1)
                        {
                            shield--;
                            mineCount--;
                            h.GetComponent<MeshRenderer>().enabled = false;
                            h.GetComponent<CubeController>().opened = true;
                            opened++;
                            h.transform.GetChild(0).gameObject.SetActive(true);
                        }
                        else if ((mineCount * 25)+25 < health)
                        {
                            failEnd();
                        }
                        else
                            gameOver();
                    }

                    cubeOpener(h);

                    setPlayerPrefs();
                    updateCanvas();
                }
                else if (hit.collider.name.Contains("Upgrade"))
                {
                    GetComponent<UpgradeSystem>()
                        .Upgrade(Convert.ToInt32(hit.collider.name[hit.collider.name.Length - 1].ToString()));
                }

                // int x = h.GetComponent<CubeController>().X;
                // int y = h.GetComponent<CubeController>().Y;
                // int z= h.GetComponent<CubeController>().Z;

                //string log = hit.collider.name +$" X:{x}, Y:{y}, Z:{z}";
                //Debug.Log(log);
                //Debug.Log(h.GetComponent<CubeController>().mineCount);
            }
            else if (Input.GetMouseButtonDown(1) && hit.collider.name.Contains("MineCube") &&
                     !h.GetComponent<CubeController>().opened)
            {
                h.GetComponent<CubeController>().flagged = !h.GetComponent<CubeController>().flagged;
                if (h.GetComponent<CubeController>().flagged)
                {
                    foreach (var rawImage in h.GetComponentsInChildren<RawImage>())
                    {
                        rawImage.enabled = true;
                    }
                }
                else
                {
                    foreach (var rawImage in h.GetComponentsInChildren<RawImage>())
                    {
                        rawImage.enabled = false;
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                if (hit.collider.name == "Entrance")
                {
                    GetComponent<SceneScript>().switchScene();
                }
            }
        }
    }

    private void failEnd()
    {
        for (int i = 0; i < minesX.Count; i++)
        {
            cubesReference[minesZ[i], minesY[i], minesX[i]].GetComponent<MeshRenderer>().enabled = false;
            cubesReference[minesZ[i], minesY[i], minesX[i]].GetComponent<CubeController>().opened = true;
            cubesReference[minesZ[i], minesY[i], minesX[i]].transform.GetChild(0).gameObject.SetActive(true);
        }
        health -= 50;
        setPlayerPrefs();
        gameOverText.text = "YOU FAILED";
        GetComponent<SceneScript>().disableScripts(2);
        
    }

    void win()
    {
        shield += 2;
        money += 150;
        level++;
        
        setPlayerPrefs();
        
        GetComponent<SceneScript>().disableScripts(3);
    }

    void gameOver()
    {
        gameOverText.text = "YOU DIED";
        
        for (int i = 0; i < minesX.Count; i++)
        {
            cubesReference[minesZ[i], minesY[i], minesX[i]].GetComponent<MeshRenderer>().enabled = false;
            cubesReference[minesZ[i], minesY[i], minesX[i]].GetComponent<CubeController>().opened = true;
            cubesReference[minesZ[i], minesY[i], minesX[i]].transform.GetChild(0).gameObject.SetActive(true);
        }
        health = 100;
        money = 0;
        shield = 0;
        level = 1;
        setPlayerPrefs();
        GetComponent<SceneScript>().disableScripts(2);
    }

    void cubeOpener(GameObject mineCube)
    {
        if (mineCube.GetComponent<CubeController>().opened || mineCube.GetComponent<CubeController>().flagged ||
            mineCube.GetComponent<CubeController>().isMine)
            return;
        money += 10;
        opened++;
        if (mineCube.GetComponent<CubeController>().mineCount != 0)
        {
            foreach (TextMeshProUGUI component in mineCube.GetComponentsInChildren<TextMeshProUGUI>())
            {
                component.text = mineCube.GetComponent<CubeController>().mineCount.ToString();
                switch (mineCube.GetComponent<CubeController>().mineCount)
                {
                    case 1:
                        component.color = new Color(0f, 0f, 253f / 255f);
                        break;
                    case 2:
                        component.color = new Color(1f / 255f, 126f / 255f, 0f);
                        break;
                    case 3:
                        component.color = new Color(254f / 255f, 0f, 1f / 255f);
                        break;
                    case 4:
                        component.color = new Color(1f / 255f, 1f / 255f, 128f / 255f);
                        break;
                    case 5:
                        component.color = new Color(131f / 255f, 0f, 3f / 255f);
                        break;
                    case 6:
                        component.color = new Color(0f, 128f / 255f, 128f / 255f);
                        break;
                    case 7:
                        component.color = new Color(0f, 0f, 0f);
                        break;
                    case 8:
                        component.color = new Color(128f / 255f, 128 / 255f, 128f / 255f);
                        break;
                    default:
                        component.color = new Color(40f / 255f, 40f / 255f, 40f / 255f);
                        break;
                }
            }

            mineCube.GetComponent<CubeController>().opened = true;
        }
        else
        {
            int X = mineCube.GetComponent<CubeController>().X;
            int Y = mineCube.GetComponent<CubeController>().Y;
            int Z = mineCube.GetComponent<CubeController>().Z;

            mineCube.GetComponent<CubeController>().opened = true;
            mineCube.SetActive(false);
            for (int j = -1; j < 2; j++)
            {
                for (int k = -1; k < 2; k++)
                {
                    for (int m = -1; m < 2; m++)
                    {
                        if (j == 0 && k == 0 && m == 0)
                            continue;
                        if (Z + j >= 0 && Z + j < minesReference.GetLength(0) && Y + k >= 0 &&
                            Y + k < minesReference.GetLength(1) && X + m >= 0 &&
                            X + m < minesReference.GetLength(2))
                        {
                            cubeOpener(cubesReference[Z + j, Y + k, X + m]);
                        }
                    }
                }
            }
        }
    }
}