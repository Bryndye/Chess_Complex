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

    public void TouchedByMouse()
    {
        Debug.Log("Has been touched " + MyPosition);

    }

    public void ChangerMyMaterial()
    {
        Debug.Log("changed material ");
    }
}


/* REGLE
 * Si tile est utilis� pour LA PREMIERE FOIS, son effet est d�clanch� et effet = neutre apr�s utilisation
 * Si la tile couleur == joueur color, le joueur en question gagne une carte PION, CHEVALIER
 */
