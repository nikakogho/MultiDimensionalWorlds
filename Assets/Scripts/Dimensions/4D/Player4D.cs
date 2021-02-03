using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class Player4D : MonoBehaviour
{
    public static bool teleported = false;
    public bool Moved;
    public Object4D obj;
    public float x, z, w;
    public float farthest = 7, closest = 0;
    public GameObject portalGun;
    public static bool portalGunUnlocked = false;
    public static Player4D instance;
    public float transitionTime = 15;
    public FirstPersonController fps;

    public GameObject locations;

    public Text WText;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateW();
    }
    
    void UpdateW()
    {
        WText.text = obj.t4.v4Pos.w.ToString();
    }

    void Update()
    {
        if (teleported) UpdateW();

        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        if (Input.GetButton("w+")) w = Time.deltaTime / transitionTime;
        else if (Input.GetButton("w-")) w = -Time.deltaTime / transitionTime;
        else w = 0;

        //w = Time.deltaTime / transitionTime;

        if (portalGunUnlocked)
        {
            if (Input.GetKeyDown("p")) ActivatePortalGun();
        }

        if (Input.GetKeyDown("l"))
        {
            bool state = locations.activeSelf;

            fps.enabled = state;
            locations.SetActive(!state);

            Cursor.lockState = state ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !state;
        }
    }

    public void ActivatePortalGun()
    {
        portalGun.SetActive(!portalGun.activeSelf);
    }

    void FixedUpdate()
    {
        Moved = w != 0;

        if (x == 0 && z == 0 && !Moved) return;
        
        obj.t4.v4Pos += new Vector4(0, 0, 0, w);
        obj.t4.v4Pos.w %= 32;

        if (Moved) UpdateW();

        obj.t4.v4Pos.x = transform.position.x;
        obj.t4.v4Pos.y = transform.position.y;
        obj.t4.v4Pos.z = transform.position.z;
    }
}
