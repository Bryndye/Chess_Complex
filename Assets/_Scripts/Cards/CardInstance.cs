using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardInstance : MonoBehaviour
{
    public Card MyCard;
    public PlayerManager MyPlayerManager;

    [SerializeField] private Image myImage;

    public void Initialize(PlayerManager playerManager,Card _card)
    {
        MyCard = _card;
        MyPlayerManager = playerManager;
        myImage.sprite = MyCard.FrontSprite[0];
    }

    public void UseCard()
    {
        Debug.Log("clicked");
        MyPlayerManager.UsingCard(MyCard);
    }

    public void PointerEnter()
    {
        Debug.Log("hover enter");
    }


    public void PointerExit()
    {
        Debug.Log("hover exit");
    }
}
