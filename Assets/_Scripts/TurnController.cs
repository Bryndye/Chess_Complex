using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Turn
{
    Player1,
    Player2
}
public class TurnController : MonoBehaviour
{
    public static TurnController instance;
    private PlayerInterface playerInterface;
    private TileController tileController;
    private EventsManager eventsManager;

    public PlayerManager playerManager1;
    public PlayerManager playerManager2;

    public Turn CurrentTurn = Turn.Player2;
    public int TurnCount = 0;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI currentTurnText;
    [SerializeField] private TextMeshProUGUI turnCountText;
    [SerializeField] private Button nextTurnBtn;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerInterface = PlayerInterface.instance;
        tileController = TileController.instance;
        eventsManager = GetComponent<EventsManager>();
        NextTurn();
    }

    private void Update()
    {
        nextTurnBtn.interactable = !tileController.IsMoving() && !eventsManager.IsShoping;
    }

    public bool Player1Turn()
    {
        return CurrentTurn == Turn.Player1;
    }
    public PlayerManager Player1ManagerTurn()
    {
        return CurrentTurn == Turn.Player1 ? playerManager1 : playerManager2;
    }


    public void NextTurn()
    {
        tileController.ResetMovementParameter();

        CurrentTurn = CurrentTurn == Turn.Player1 ? Turn.Player2 : Turn.Player1;
        PlayerManager currentPM = Player1ManagerTurn();

        if (CurrentTurn == Turn.Player1)
        {
            TurnCount++;
        }
        if (!currentPM.MyCards.Contains(EventsManager.Instance.StockCards[1]))
        {
            currentPM.MyCards.Insert(0, EventsManager.Instance.StockCards[1]);
        }
        currentPM.NextTurnSet();

        currentTurnText.text = CurrentTurn.ToString();
        turnCountText.text = TurnCount.ToString();

        playerInterface.NextTurnSetInterface(this);
    }
}
