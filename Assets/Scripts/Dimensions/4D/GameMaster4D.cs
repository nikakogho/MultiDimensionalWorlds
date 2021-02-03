using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameMaster4D : MonoBehaviour
{
    public static GameMaster4D instance;
    [HideInInspector]
    public List<Object4D> objects;
    public Player4D player;
    public GameObject instructionsUI;
    List<CompleteTimeData> data = new List<CompleteTimeData>();
    private bool began = false;

    int count = 0;

    new Camera camera;

    public int timeSide = 1;

    void Awake()
    {
        instance = this;
        camera = Camera.main;

        objects = FindObjectsOfType<Object4D>().ToList();
    }

    void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown("r")) timeSide *= -1;
        if (Input.GetKeyDown(KeyCode.Escape)) ToggleInstructions();
    }

    void FixedUpdate()
    {
        if (!began)
        {
            ShowWorld();
            began = true;
            return;
        }

        if(timeSide == 1)
        {
            count++;

            StoreData();
        }
        else if(count > 0)
        {
            count--;

            if(count > 0)
            {
                ReverseTimeData();
            }
        }

        if (!player.Moved && !Player4D.teleported) return;

        Player4D.teleported = false;

        ShowWorld();
    }

    #region Time Travel

    void ReverseTimeData()
    {
        int last = data.Count - 1;

        data.RemoveAt(last);
        last--;

        for (int i = 0; i < objects.Count; i++)
        {
            ObjectTimeData d = data[last].data[i];

            objects[i].t4.v4Pos = d.position.ToXYZW();
            objects[i].transform.rotation = d.rotation;

            for(int j = 0; j < objects[i].rends.Length; j++)
            {
                objects[i].rends[j].w = d.ws[j];
            }
        }

        PlayerTimeData playerTimeData = data[last].playerTimeData;

        Vector4 playerPos = playerTimeData.position.ToXYZW();

        player.transform.position = playerPos;
        player.transform.rotation = playerTimeData.rotation;
        player.obj.t4.v4Pos.w = playerPos.w;
        camera.transform.rotation = playerTimeData.cameraRotation;
        ShowWorld();
    }

    void StoreData()
    {
        ObjectTimeData[] objTimeData = new ObjectTimeData[objects.Count];

        for (int i = 0; i < objects.Count; i++)
        {
            Object4D obj = objects[i];
            float[] ws = new float[obj.rends.Length];

            for (int j = 0; j < obj.rends.Length; j++)
            {
                ws[j] = obj.rends[j].w;
            }

            objTimeData[i] = new ObjectTimeData(obj.t4.Position, obj.transform.rotation, ws);
        }

        PlayerTimeData playerTimeData = new PlayerTimeData(new VectorN(new float[] { player.transform.position.x, player.transform.position.y, player.transform.position.z, player.obj.t4.v4Pos.w }), player.transform.rotation, camera.transform.rotation);

        data.Add(new CompleteTimeData(objTimeData, playerTimeData, count));
    }

    #endregion

    public void ShowWorld()
    {
        foreach (Object4D obj in objects)
        {
            foreach (RenderData data in obj.rends)
            {
                bool active = data.face.activeSelf;
                float delta = data.w - player.obj.t4.v4Pos.w;
                
                bool shouldBeActive = delta >= player.closest && delta <= player.farthest;

                if (shouldBeActive && data.needsVisibleRange) shouldBeActive = delta <= data.visibleRange;

                if (active != shouldBeActive) data.face.SetActive(shouldBeActive);

                if (shouldBeActive)
                {
                    Vector3 desiredScale = data.normalScale / (delta + 1);

                    if (data.face.transform.localScale != desiredScale) data.face.transform.localScale = desiredScale;
                }
            }
        }
    }

    public void ToggleInstructions()
    {
        if (instructionsUI.activeSelf)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            instructionsUI.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            instructionsUI.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Exit()
    {
        Application.Quit();
    }
}

[System.Serializable]
public struct CompleteTimeData
{
    public ObjectTimeData[] data;
    public PlayerTimeData playerTimeData;
    
   // public int time;

    public CompleteTimeData(ObjectTimeData[] data, PlayerTimeData playerTimeData, int time)
    {
        this.data = data;

        this.playerTimeData = playerTimeData;

        //this.time = time;
    }
}

[System.Serializable]
public struct PlayerTimeData
{
    public VectorN position;
    public Quaternion rotation;
    public Quaternion cameraRotation;

    public PlayerTimeData(VectorN position, Quaternion rotation, Quaternion cameraRotation)
    {
        this.position = position;
        this.rotation = rotation;
        this.cameraRotation = cameraRotation;
    }
}

[System.Serializable]
public struct ObjectTimeData
{
    public VectorN position;
    public Quaternion rotation;
    public float[] ws;

    public ObjectTimeData(VectorN position, Quaternion rotation, float[] ws)
    {
        this.position = position;
        this.rotation = rotation;
        this.ws = (float[])ws.Clone();
    }
}
