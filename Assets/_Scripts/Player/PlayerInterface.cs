using System.Collections.Generic;
using TMPro;
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
        foreach (Card card in _cards)
        {
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

    #region Event UI
    [Header("UI")]
    [SerializeField] private GameObject gameObjectEventUI;
    [SerializeField] private Animator anim;
    [SerializeField] private TextMeshProUGUI[] messages;
    [SerializeField] private Image imageCard;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image backgroundImage2;
    [SerializeField] private Sprite keySprite;

    [Header("Interface")]
    public GameObject endScreen;
    public Button buttonNextRound;
    public Button buttonEndGame;
    public TextMeshProUGUI scoreP1;
    public TextMeshProUGUI scoreP2;
    public TextMeshProUGUI RoundP1;
    public TextMeshProUGUI RoundP2;

    public void TriggerEventUI(TileType _tile, Card _card = null)
    {
        gameObjectEventUI.SetActive(true);
        string messageForText = "";
        switch (_tile)
        {
            case TileType.NoEvent:
                break;
            case TileType.RandomCard:
                anim.SetTrigger("RandomCard");
                messageForText = "You receive : \n" + _card.name + " card!";
                imageCard.sprite = _card.FrontSprite[turnController.Player1Turn() ? 0 : 1];
                break;
            case TileType.Shop:
                break;
            case TileType.Event:
                anim.SetTrigger("EventRedCard");
                messageForText = "You trigger an event!\n" + _card.name;
                backgroundImage2.gameObject.SetActive(true);
                backgroundImage2.color = new Color32(180, 55, 55, 255);
                imageCard.sprite = _card.FrontSprite[0];
                break;
            case TileType.Key:
                anim.SetTrigger("Key");
                messageForText = "You obtained the key!";
                imageCard.sprite = keySprite;
                break;
            case TileType.Out:
                break;
            default:
                break;
        }
        foreach (var message in messages)
        {
            message.text = messageForText;
        }
    }

    public void NextTurnUI()
    {
        string messageForText = turnController.Player1Turn() ? "Player 1 Turn" : "Player 2 Turn";
        gameObjectEventUI.SetActive(true);
        backgroundImage2.gameObject.SetActive(true);
        backgroundImage2.color = turnController.Player1Turn() ? Color.white : Color.black;

        foreach (var message in messages)
        {
            message.text = messageForText;
            if (turnController.Player1Turn())
            {
                message.color = Color.black;
            }
            else
            {
                message.color = Color.white;
            }
        }

        anim.SetTrigger("NewTurn");
        backgroundImage2.color = turnController.Player1Turn() ? Color.white : Color.black;
    }
    #endregion
}
