using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventsManager : MonoBehaviour
{
    public static EventsManager Instance;
    private TurnController turnController;
    private PlayerInterface playerInterface;

    public Card[] StockCards;

    [Header("Shop")]
    public bool IsShoping = false;
    [SerializeField] private CardShopInstance cardShoPrefab;
    [SerializeField] private GameObject shopInterface;
    [SerializeField] private Transform shopContainerCards;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        turnController = TurnController.instance;
        playerInterface = PlayerInterface.instance;
        shopInterface.SetActive(false);
    }

    public Card RandomCard()
    {
        int index = Random.Range(0, StockCards.Length);
        return StockCards[index];
    }


    public void SetShop()
    {
        IsShoping = true;
        Card[] cards = new Card[] { RandomCard(), RandomCard(), RandomCard() };
        shopInterface.SetActive(true);
        foreach (var card in cards)
        {
            var cardShopInstance = Instantiate(cardShoPrefab, shopContainerCards);
            cardShopInstance.MyCard = card;
        }
    }
    public void SelectCardFromShop(Card _card)
    {
        shopInterface.SetActive(false);
        PlayerManager pm = turnController.Player1ManagerTurn();
        pm.AddCard(_card);
        playerInterface.AddCardContainer(_card, true);
        ClearContainer();
        IsShoping = false;
    }

    private void ClearContainer()
    {
        int childCount = shopContainerCards.childCount;

        for (int i = childCount - 1; i >= 0; i--)
        {
            var cardInstance = shopContainerCards.GetChild(i).GetComponent<CardShopInstance>();
            Destroy(cardInstance.gameObject);
        }
    }

    public void Event()
    {

    }
}
