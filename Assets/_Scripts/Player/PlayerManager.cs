using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Player
{
    Player1, Player2
}
public class PlayerManager : MonoBehaviour
{
    public Player Player;

    public PlayerController MyPlayer;
    public Card[] MyCards;
}
