using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardInstance : MonoBehaviour
{
    public Card MyCard;
    public PlayerManager MyPlayerManager;
    private TileController tileController;
    private Image myImage;
    private Button myBtn;
    private bool isDisable = false;

    private void Awake()
    {
        myBtn = GetComponent<Button>();
        myImage = GetComponent<Image>();
    }

    private void Start()
    {
        tileController = TileController.instance;
    }

    public void Initialize(PlayerManager playerManager,Card _card, TurnController turnController)
    {
        MyCard = _card;
        MyPlayerManager = playerManager;
        myImage.sprite = MyCard.FrontSprite[turnController.Player1Turn() ? 0 : 1];
    }

    public void UseCard()
    {
        if (isDisable)
        {
            return;
        }
        MyPlayerManager.UsingCard(this);
    }

    public void DisableCard()
    {
        isDisable = true;
        myBtn.interactable = false;
        //myImage.color = Color.grey;
    }

    public void PointerEnter()
    {
        if (isDisable || tileController.IsMoving())
        {
            return;
        }
        MyPlayerManager.PrevisualisationMovement(this);
    }


    public void PointerExit()
    {
        if (isDisable || tileController.IsMoving())
        {
            return;
        }
        MyPlayerManager.ResetPrevisualisation();
    }
}
