using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum Turn
{
    Player1,
    Player2
}
public class TurnController : MonoBehaviour
{
    public static TurnController instance;
    private PlayerInterface playerInterface;
    [SerializeField] PlayerManager playerManager1;
    [SerializeField] PlayerManager playerManager2;

    public Turn CurrentTurn = Turn.Player2;
    public int TurnCount = 0;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI currentTurnText;
    [SerializeField] private TextMeshProUGUI turnCountText;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerInterface = PlayerInterface.instance;
        NextTurn();
    }

    public bool Player1Turn()
    {
        return CurrentTurn == Turn.Player1;
    }

    public void NextTurn()
    {
        // DOIT RESET LES MOVEMENTS EN COURS!
        // BTN DOIT ETRE DISABLE SI MVT EN COURS
        // DOIT REST BOOST STAT/ MALUS

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
