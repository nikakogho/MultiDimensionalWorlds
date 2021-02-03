using UnityEngine;
using UnityEngine.UI;

public abstract class Location : MonoBehaviour
{
    public Image image;
    public Button button;
    public Text priceText, locationText;
    public float price;
    private bool unlocked = false;
    protected abstract float GetW();

    void Awake()
    {
        priceText.text = price.ToString();
    }

    public void Unlock()
    {
        if (unlocked) return;

        if(price <= PlayerStats.money)
        {
            unlocked = true;
            Destroy(button);
            Destroy(image);
            Destroy(priceText);
            locationText.enabled = true;
            locationText.text = GetW().ToString();
            PlayerStats.money -= price;
        }
    }

    void OnEnable()
    {
        if (unlocked) locationText.text = GetW().ToString();
    }
}
