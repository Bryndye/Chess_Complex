using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Card : ScriptableObject
{
    public Sprite FrontSprite;
    public Sprite BackSprite;

    public int Portee = 1;
    public bool LigneMovement = false;
    public bool Diagonalemovement = false;
    public bool CavalierMovement = false;
    public bool CircleMovement = false;
}
