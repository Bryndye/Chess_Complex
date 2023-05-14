using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInterface : MonoBehaviour
{
    public static PlayerInterface instance;
    private TurnController turnController;

    [SerializeField] CardInstance cardPrefab;
    [SerializeField] Transform cardsContainer;

    [SerializeField] PlayerManager playerManager1;
    [SerializeField] PlayerManager playerManager2;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        turnController = GetComponent<TurnController>();
    }

    public void NextTurnSetInterface(TurnController _turnController)
    {
        if (turnController == null)
        {
            turnController = _turnController;
        }
        Debug.Log(turnController);
        if (turnController.Player1Turn())
        {
            ClearCardsContainer();
            SetCardsContainer(playerManager1);
        }
        else
        {
            ClearCardsContainer();
            SetCardsContainer(playerManager2);
        }
    }

    public void SetCardsContainer(PlayerManager playerManager)
    {
        List<Card> _cards = playerManager.MyCards;
        foreach (Card card in _cards) {
            var cardInstance = Instantiate(cardPrefab, cardsContainer);
            cardInstance.Initialize(playerManager, card, turnController);
        }
    }

    public void ClearCardsContainer()
    {
        int childCount = cardsContainer.childCount;

        for (int i = childCount - 1; i >= 0; i--)
        {
            Transform child = cardsContainer.GetChild(i);
            Destroy(child.gameObject);
        }
    }

    public void DeleteCardContainer(CardInstance _cardInstance)
    {
        int childCount = cardsContainer.childCount;

        for (int i = childCount - 1; i >= 0; i--)
        {
            var cardInstance = cardsContainer.GetChild(i).GetComponent<CardInstance>();

            if (_cardInstance == cardInstance)
            {
                Destroy(cardInstance.gameObject);
            }
        }
    }

    public void AddCardContainer(Card _card, bool _fromShop = false)
    {
        var _cardInstance = Instantiate(cardPrefab, cardsContainer);
        _cardInstance.Initialize(turnController.Player1ManagerTurn(), _card, turnController);
        if (_fromShop)
        {
            _cardInstance.DisableCard();
        }
    }

    public void SetCardDisable()
    {
        int childCount = cardsContainer.childCount;

        for (int i = childCount - 1; i >= 0; i--)
        {
            var child = cardsContainer.GetChild(i).GetComponent<CardInstance>();
            //if (child.MyCard.MyCardType == CardType.Movement)
            //{
            //    child.DisableCard();
            //}
            child.DisableCard();
        }
    }
}
