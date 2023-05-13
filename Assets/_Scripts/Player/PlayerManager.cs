using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Player
{
    Player1, Player2
}
public class PlayerManager : MonoBehaviour
{
    private TileController tileController;

    public Player Player;

    public PlayerController MyPlayer;
    public List<Card> MyCards;

    private void Start()
    {
        tileController = TileController.instance;
    }

    public void AddCard(Card _card)
    {
        MyCards.Add(_card);
    }

    public void RemoveCard(Card _card) 
    { 
        MyCards.Remove(_card);
    }

    public void UsingCard(Card _card)
    {
        switch (_card.MyCardType)
        {
            case CardType.Movement:
                tileController.CalculatePortee(MyPlayer, _card);
                break;
            case CardType.Boost:
                break;
            default:
                break;
        }
    }
}
