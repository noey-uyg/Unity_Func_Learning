using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Define {}


#region BFS_PathFinding
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

public enum RoadType
{
    None = 0,
    Start,
    End,
    Wall,

    LastInstance
}
#endregion

#region Move_Learning
public enum TargetObject
{
    target1,
    target2,

    LastInstance
}

public enum MoveMode
{
    Mouse,
    Keyboard,

    LastInstance
}
#endregion

#region Inventory_Learning
public enum ItemType
{
    Head,
    Body,
    Hand,
    Weapon,
    RFoot,
    LFoot,

    LastInstance
}

public class ItemAttribute
{
    public ItemType itemType;
    public int ability;
    public bool use;
}

#endregion

#region DesignPattern

#region Factory

public enum FactoryUnitType
{
    SmallRed,
    SmallBlue,
    SmallGreen,
    BigRed,
    BigBlue,
    BigGreen,

    LastInstance
}


#endregion

#endregion