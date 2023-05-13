using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardInstance : MonoBehaviour
{
    public Card MyCard;
    public PlayerManager MyPlayerManager;

    [SerializeField] private Image myImage;
    private bool isDisable = false;

    public void Initialize(PlayerManager playerManager,Card _card)
    {
        MyCard = _card;
        MyPlayerManager = playerManager;
        myImage.sprite = MyCard.FrontSprite[0];
    }

    public void UseCard()
    {
        Debug.Log("clicked");
        if (isDisable)
        {
            return;
        }
        MyPlayerManager.UsingCard(MyCard);
    }

    public void DisableCard()
    {
        isDisable = true;
        myImage.color = Color.grey;
    }

    public void PointerEnter()
    {
        Debug.Log("hover enter");
        if (isDisable)
        {
            return;
        }
    }


    public void PointerExit()
    {
        Debug.Log("hover exit");
    }
}
