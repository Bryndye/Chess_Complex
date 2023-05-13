using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    public static EventsManager Instance;

    public Card[] StockCards;
    [SerializeField] private GameObject shopInterface;

    private void Awake()
    {
        Instance = this;
    }

    public Card RandomCard()
    {
        int index = Random.Range(0, StockCards.Length);
        return StockCards[index];
    }


    public void SetShop()
    {
        Card[] card = new Card[] { RandomCard(), RandomCard(), RandomCard() };
        shopInterface.SetActive(true);
    }
}
