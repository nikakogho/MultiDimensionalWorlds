using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public float startHealth = 100, startMoney = 0;
    public int startBullets = 0;
    public static float health;
    public static float money;
    public static int bullets;
    public Text moneyText, healthText, bulletText;
    public static PlayerStats instance;
    public Camera loseCamera;
    public GameObject loseUI;

    void Awake()
    {
        instance = this;
        money = startMoney;
        health = startHealth;
        bullets = startBullets;
    }

    void FixedUpdate()
    {
        moneyText.text = money.ToString();
        healthText.text = health.ToString();
        bulletText.text = bullets.ToString();

        if(health <= 0)
        {
            health = 0;
            loseUI.SetActive(true);
            loseCamera.enabled = true;
            Cursor.lockState = CursorLockMode.None;
            gameObject.SetActive(false);
        }
    }
}
