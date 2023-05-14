using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardShopInstance : MonoBehaviour
{
    private EventsManager eventsManager;
    private TurnController turnController;
    public Card MyCard;
    private Image myImage;

    private void Awake()
    {
        eventsManager = EventsManager.Instance;
        turnController = TurnController.instance;
    }

    private void Start()
    {
        myImage = GetComponentInChildren<Image>();
        myImage.sprite = MyCard.FrontSprite[turnController.Player1Turn() ? 0 : 1];
    }

    public void SelectThisCard()
    {
        eventsManager.SelectCardFromShop(MyCard);
    }
}
