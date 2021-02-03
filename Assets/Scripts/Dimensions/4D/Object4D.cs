using UnityEngine;

public class Object4D : MonoBehaviour
{
    public D4Transform t4;
    public RenderData[] rends;

    void Awake()
    {
        foreach(RenderData data in rends)
        {
            data.normalScale = data.face.transform.localScale;
        }
    }
    
    void FixedUpdate()
    {
        UpdateW();
    }

    protected void UpdateW()
    {
        if (t4.v4Pos.w < 0) t4.v4Pos.w += 32;

        gameObject.layer = (int)t4.v4Pos.w;

        foreach(RenderData data in rends)
        {
            if (!data.applyW) continue;

            data.face.layer = (int)data.w;

            for(int i = 0; i < data.face.transform.childCount; i++)
            {
                data.face.transform.GetChild(i).gameObject.layer = (int)data.w;
            }
        }
    }
}

[System.Serializable]
public class RenderData
{
    public float w;
    public GameObject face;
    public Vector3 normalScale;
    public bool applyW = true;
    [Header("Visible Range")]
    public float visibleRange;
    public bool needsVisibleRange;
}

[System.Serializable]
public class D4Transform
{
    public Vector4 v4Pos;
    public virtual VectorN Position { get { return new VectorN(new float[]{ v4Pos.x, v4Pos.y, v4Pos.z, v4Pos.w }); } }
}