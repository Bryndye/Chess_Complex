using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    NoEvent,
    RandomEvent,
    Shop
}

public enum TileTState
{
    Unused,
    Used
}
public class Tile : MonoBehaviour
{
    public TileType MyType;
    public TileTState MyState = TileTState.Unused;
    public Vector2 MyPosition;
}
