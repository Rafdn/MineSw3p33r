using System;
using TMPro;
using UnityEngine;


public class UpgradeSystem : MonoBehaviour
{
    private int health;
    private int money;
    private int shield;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI shieldText;
    

    private void Start()
    {
        if (PlayerPrefs.GetInt("Game") == 0)
        {
            health = 100;
            money = 150;
            shield = 0;
            PlayerPrefs.SetInt("Health",100);
            PlayerPrefs.SetInt("Money",150);
            PlayerPrefs.SetInt("Shield",0);
            PlayerPrefs.SetInt("Game",1);
            PlayerPrefs.Save();
        }
        else
        {
            money = PlayerPrefs.GetInt("Money");
            health = PlayerPrefs.GetInt("Health");
            shield = PlayerPrefs.GetInt("Shield");
        }
        updateCanvas();
    }

    private void OnApplicationQuit()
    {
        setPlayerPrefs();
    }

    void setPlayerPrefs()
    {
        PlayerPrefs.SetInt("Health",health);
        PlayerPrefs.SetInt("Money",money);
        PlayerPrefs.SetInt("Shield",shield);
        PlayerPrefs.Save();
    }

    void updateCanvas()
    {
        healthText.text = health.ToString();
        moneyText.text = money.ToString();
        shieldText.text = shield.ToString();
    }
    public void Upgrade(int upgradeNumber)
    {
        switch (upgradeNumber)
        {
            case 0:
                Debug.Log("health");
                if (money >= 250)
                {
                    health += 50;
                    money -= 250;
                }
                break;

            case 1:
                Debug.Log("mineupgrade");
                if (money >= 100)
                {
                    shield++;
                    money -= 100;
                }
                break;
        }
        updateCanvas();
        setPlayerPrefs();
        
        
    }
}