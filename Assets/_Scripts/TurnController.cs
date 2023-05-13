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


    [SerializeField] PlayerManager playerManager1;
    [SerializeField] PlayerManager playerManager2;

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
        NextTurn();
    }

    private void Update()
    {
        nextTurnBtn.interactable = !tileController.IsMoving();
    }

    public bool Player1Turn()
    {
        return CurrentTurn == Turn.Player1;
    }

    public void NextTurn()
    {
        // BTN DOIT ETRE DISABLE SI MVT EN COURS
        // DOIT REST BOOST STAT/ MALUS
        tileController.ResetMovementParameter();

        CurrentTurn = CurrentTurn == Turn.Player1 ? Turn.Player2 : Turn.Player1;
        PlayerManager currentPM = CurrentTurn == Turn.Player1 ? playerManager1 : playerManager2;

        if (CurrentTurn == Turn.Player1)
        {
            TurnCount++;
        }
        currentPM.MyCards.Insert(0, EventsManager.Instance.StockCards[1]);
        currentPM.NextTurnSet();

        currentTurnText.text = CurrentTurn.ToString();
        turnCountText.text = TurnCount.ToString();

        playerInterface.NextTurnSetInterface();
    }
}
