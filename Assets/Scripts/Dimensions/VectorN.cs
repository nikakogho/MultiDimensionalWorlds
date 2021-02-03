using UnityEngine;
using System.Linq;

public class VectorN
{
    public float[] values; //private?

    public float x { get { return values[0]; } set { values[0] = value; } }
    public float y { get { return values[1]; } set { values[1] = value; } }
    public float z { get { return values[2]; } set { values[2] = value; } }
    public float w { get { return values[3]; } set { values[3] = value; } }

    public float? X { get { if (values.Length > 0) return x; return null; } set { x = (float)value; } }
    public float? Y { get { if (values.Length > 1) return y; return null; } set { y = (float)value; } }
    public float? Z { get { if (values.Length > 2) return z; return null; } set { z = (float)value; } }
    public float? W { get { if (values.Length > 3) return w; return null; } set { w = (float)value; } }

    public int Size => values.Length;

    public float this[int index]
    {
        get { return values[index]; }
        set { values[index] = value; }
    }

    #region Constructors

    public VectorN(int size)
    {
        values = new float[size];
        for (int i = 0; i < size; i++) values[i] = 0;
    }

    #region Args

    public VectorN(float x, float y)
    {
        values = new float[] { x, y };
    }

    public VectorN(float x, float y, float z)
    {
        values = new float[] { x, y, z };
    }

    public VectorN(float x, float y, float z, float w)
    {
        values = new float[] { x, y, z, w};
    }

    public VectorN(float x, float y, float z, float w, params float[] args)
    {
        values = new float[args.Length + 4];
        values[0] = x;
        values[1] = y;
        values[2] = z;
        values[3] = w;
        for (int i = 0; i < args.Length; i++) values[i + 4] = args[i];
    }

    public VectorN(float[] values)
    {
        this.values = new float[values.Length];

        for (int i = 0; i < values.Length; i++) this.values[i] = values[i];
    }

    #endregion

    #region Vector

    public VectorN(Vector2 vec)
    :this(vec.x, vec.y)
    {
    }

    public VectorN(Vector3 vec)
    : this(vec.x, vec.y, vec.z)
    {
    }

    public VectorN(Vector4 vec)
    : this(vec.x, vec.y, vec.z, vec.w)
    {
    }

    public VectorN(VectorN copy)
    : this(copy.values)
    {
    }

    #endregion

    #endregion

    #region To Vector

    public Vector2 ToXY()
    {
        int size = values.Length;
        Vector2 v2 = new Vector2();
        if (size > 0)
        {
            v2.x = x;
            if (size > 1) v2.y = y;
        }
        return v2;
    }

    public Vector3 ToXYZ()
    {
        int size = values.Length;
        Vector3 v3 = new Vector3();
        if (size > 0)
        {
            v3.x = x;
            if (size > 1)
            {
                v3.y = y;
                if (size > 2) v3.z = z;
            }
        }
        return v3;
    }

    public Vector4 ToXYZW()
    {
        int size = values.Length;
        Vector4 v4 = new Vector4();

        if (size > 0)
        {
            v4.x = x;
            if (size > 1)
            {
                v4.y = y;
                if (size > 2)
                {
                    v4.z = z;
                    if (size > 3)
                    {
                        v4.w = w;
                    }
                }
            }
        }

        return v4;
    }

    #endregion

    #region Identify

    public override bool Equals(object obj)
    {
        if (obj is Vector2) return (Vector2)obj == (Vector2)this;
        if (obj is Vector3) return (Vector3)obj == (Vector3)this;
        if (obj is Vector4) return (Vector4)obj == (Vector4)this;
        if (obj is VectorN) return ((VectorN)obj).values.SequenceEqual(values);
        return false;
    }

    public override int GetHashCode()
    {
        float ans = 0;
        for (int i = 0; i < Size; i++) ans += values[i] * (i + 1); //needs work?
        return ans.GetHashCode();
    }

    #endregion

    #region Convert

    #region From N

    public static explicit operator Vector2(VectorN vec)
    {
        return vec.ToXY();
    }

    public static explicit operator Vector3(VectorN vec)
    {
        return vec.ToXYZ();
    }

    public static explicit operator Vector4(VectorN vec)
    {
        return vec.ToXYZW();
    }

    #endregion

    #region To N

    public static implicit operator VectorN(Vector2 vec)
    {
        return new VectorN(vec);
    }
    public static implicit operator VectorN(Vector3 vec)
    {
        return new VectorN(vec);
    }
    public static implicit operator VectorN(Vector4 vec)
    {
        return new VectorN(vec);
    }

    #endregion

    #endregion

    public static bool operator ==(VectorN a, VectorN b)
    {
        if (a.values.Length != b.values.Length) return false;

        for (int i = 0; i < a.values.Length; i++)
        {
            if (a.values[i] != b.values[i]) return false;
        }

        return true;
    }

    public static bool operator !=(VectorN a, VectorN b)
    {
        return !(a == b);
    }
}
