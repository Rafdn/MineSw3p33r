using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneScript : MonoBehaviour
{
    private GameObject canvas;

    private void Start()
    {
        canvas = GameObject.Find("Canvas");
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "MineScene")
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                disableScripts(1);
            }
        }
    }

    public void disableScripts(int panel)
    {
        canvas.transform.GetChild(0).gameObject.SetActive(false);
        switch (panel)
        {
            case 1:
                canvas.transform.GetChild(1).gameObject.SetActive(true);
                canvas.transform.GetChild(2).gameObject.SetActive(false);
                canvas.transform.GetChild(3).gameObject.SetActive(false);
                break;
            case 2:
                canvas.transform.GetChild(2).gameObject.SetActive(true);
                canvas.transform.GetChild(1).gameObject.SetActive(false);
                canvas.transform.GetChild(3).gameObject.SetActive(false);
                break;
            case 3:
                canvas.transform.GetChild(3).gameObject.SetActive(true);
                canvas.transform.GetChild(2).gameObject.SetActive(false);
                canvas.transform.GetChild(1).gameObject.SetActive(false);
                break;
        }
        cursorLock(true);
        GetComponent<MineSceneMovement>().enabled = false;
        GetComponent<InteractionScript>().enabled = false;
        Camera.main.GetComponent<MouseLook>().enabled = false;
    }

    public void switchScene()
    {
        if (SceneManager.GetActiveScene().name == "MineScene")
        {
            StartCoroutine(LoadScene("GameScene"));
        }
        else if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            StartCoroutine(LoadScene("GameScene"));
        }
        else
        {
            StartCoroutine(LoadScene("MineScene"));
        }
    }

    IEnumerator LoadScene(string scene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void cursorLock(bool isLocked)
    {
        if (isLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}