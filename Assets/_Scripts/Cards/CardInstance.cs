using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardInstance : MonoBehaviour
{
    public Card MyCard;
    public PlayerManager MyPlayerManager;

    private Image myImage;
    private Button myBtn;
    private bool isDisable = false;

    private void Awake()
    {
        myBtn = GetComponent<Button>();
        myImage = GetComponent<Image>();
    }

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
        myBtn.interactable = false;
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
