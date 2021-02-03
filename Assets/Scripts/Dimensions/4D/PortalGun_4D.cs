using UnityEngine;
using UnityEngine.UI;

public class PortalGun_4D : MonoBehaviour
{
    public Text text;
    public float chosenW = 0;
    private bool isPoint = false;
    int numbersAfterPoint = 0;
    public string inputText;
    public float offset;
    public float moveRightBy = 0.2f;
    public GameObject portal;

    void OnEnable()
    {
        Clear();
    }

    void Clear()
    {
        chosenW = 0;
        inputText = "";
        isPoint = false;
    }

    void Update()
    {
        bool changed = true;

        bool keyWasDown = false;

        for(int i = 0; i < 10; i++)
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                TakeInput(i);
                keyWasDown = true;
                break;
            }
        }

        if (!keyWasDown)
        {
            if (Input.GetKeyDown("."))
            {
                if (!isPoint)
                {
                    isPoint = true;
                    numbersAfterPoint = 0;
                    inputText += ".";
                }
            }
            else if (Input.GetKeyDown(KeyCode.Backspace))
            {
                Clear();
            }
            else changed = false;
        }

        if (changed) text.text = inputText;

        if (Input.GetMouseButtonDown(1))
        {
            CreatePortal();
        }
    }

    void CreatePortal()
    {
        Vector3 position = transform.position + transform.forward * offset + transform.right * moveRightBy;
        position.y = 0;
        Instantiate(portal, position, Quaternion.identity).GetComponent<Portal>().w2 = chosenW;
    }

    void TakeInput(int input)
    {
        if (isPoint)
        {
            numbersAfterPoint++;

            chosenW += input / Mathf.Pow(10, numbersAfterPoint);
        }
        else
        {
            chosenW *= 10;
            chosenW += input;
        }

        inputText += input.ToString();
    }
}
