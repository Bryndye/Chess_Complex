using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        turnController = TurnController.instance;
    }

    public void NextTurnSetInterface()
    {
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
            cardInstance.Initialize(playerManager, card);
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

    public void DeleteCardContainer(Card _card)
    {
        int childCount = cardsContainer.childCount;

        for (int i = childCount - 1; i >= 0; i--)
        {
            var cardInstance = cardsContainer.GetChild(i).GetComponent<CardInstance>();
            var card = cardInstance.MyCard;
            if (_card == card)
            {
                Destroy(cardInstance.gameObject);
            }
        }
    }

    public void SetCardDisable()
    {
        int childCount = cardsContainer.childCount;

        for (int i = childCount - 1; i >= 0; i--)
        {
            var child = cardsContainer.GetChild(i).GetComponent<CardInstance>();
            if (child.MyCard.MyCardType == CardType.Movement)
            {
                child.DisableCard();
            }
        }
    }
}
