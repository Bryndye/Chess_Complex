using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType
{
    Movement,
    Boost
}

[CreateAssetMenu]
public class Card : ScriptableObject
{
    public CardType MyCardType;
    public Sprite[] FrontSprite;
    public Sprite BackSprite;

    [Header("Card Movement")]
    public int Portee = 1;

    public bool LigneMovement = false;
    public bool DiagonaleMovement = false;
    public bool KnightMovement = false;
    public bool CircleMovement = false;

    [Header("Card Boost")]
    public int BoostMovement = 0;

    [Space]
    public string Description = "C pa bien";
}
