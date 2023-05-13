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
            SetCardsContainer(playerManager1.MyCards);
        }
        else
        {
            ClearCardsContainer();
            SetCardsContainer(playerManager2.MyCards);
        }
    }

    public void SetCardsContainer(Card[] _cards)
    {
        foreach (Card card in _cards) {
            var cardInstance = Instantiate(cardPrefab, cardsContainer);
            cardInstance.Initialize(card);
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
}
