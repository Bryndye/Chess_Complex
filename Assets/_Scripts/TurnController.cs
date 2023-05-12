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
    public Turn CurrentTurn = Turn.Player2;
    public int TurnCount = 0;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI currentTurnText;
    [SerializeField] private TextMeshProUGUI turnCountText;

    private void Awake()
    {
        NextTurn();
    }

    public void NextTurn()
    {
        CurrentTurn = CurrentTurn == Turn.Player1 ? Turn.Player2 : Turn.Player1;

        if (CurrentTurn == Turn.Player1)
        {
            TurnCount++;
        }

        currentTurnText.text = CurrentTurn.ToString();
        turnCountText.text = TurnCount.ToString();
    }
}
