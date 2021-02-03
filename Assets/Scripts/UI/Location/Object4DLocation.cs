public class Object4DLocation : Location
{
    public Object4D obj;

    protected override float GetW()
    {
        return obj.t4.v4Pos.w;
    }
}
