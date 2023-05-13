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
    private PlayerInterface playerInterface;

    public Player Player;

    public PlayerController MyPlayer;
    public List<Card> MyCards;

    public bool HasPlayedMovement = false;

    private void Start()
    {
        tileController = TileController.instance;
        playerInterface = PlayerInterface.instance;
    }

    public void NextTurnSet()
    {
        HasPlayedMovement = false;
    }

    public void AddCard(Card _card)
    {
        MyCards.Add(_card);
    }

    public void RemoveCard(Card _card) 
    {
        if (MyCards.Contains(_card))
        {
            MyCards.Remove(_card);
        }
    }

    public void UsingCard(Card _card)
    {
        switch (_card.MyCardType)
        {
            case CardType.Movement:
                tileController.CalculatePortee(MyPlayer, _card);
                playerInterface.SetCardDisable();
                break;
            case CardType.Boost:
                MyPlayer.PorteeBoost += _card.BoostMovement;
                break;
            default:
                break;
        }
        playerInterface.DeleteCardContainer(_card);
        RemoveCard(_card);
    }
}
