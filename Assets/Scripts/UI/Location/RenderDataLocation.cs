public class RenderDataLocation : Location
{
    public Object4D obj;
    public int index;

    RenderData Data { get { return obj.rends[index]; } }

    protected override float GetW()
    {
        return Data.w;
    }
}
