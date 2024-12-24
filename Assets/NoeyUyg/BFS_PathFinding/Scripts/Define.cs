using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Define {}

public class RectSize
{
    public float sizeX;
    public float sizeY;

    public RectSize(float sizeX, float sizeY)
    {
        this.sizeX = sizeX;
        this.sizeY = sizeY;
    }
}

public class Pos
{
    public int X;
    public int Y;
    public RoadType roadType;

    public Pos(int X, int Y)
    {
        this.X = X;
        this.Y = Y;
        roadType = RoadType.None;
    }

    public override bool Equals(object obj)
    {
        if (obj is Pos other)
        {
            return X == other.X && Y == other.Y;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return (X, Y).GetHashCode();
    }
}