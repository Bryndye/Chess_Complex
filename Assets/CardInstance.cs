using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardInstance : MonoBehaviour
{
    public Card MyCard;

    [SerializeField] private Image myImage;

    public void Initialize(Card _card)
    {
        MyCard = _card;
        myImage.sprite = MyCard.FrontSprite[0];
    }

    public void UseCard()
    {
        Debug.Log("clicked");
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
