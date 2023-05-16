using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    public static VictoryManager instance;
    [SerializeField] private TileController tileController;
    [SerializeField] private PlayerInterface playerInterface;

    [SerializeField] private PlayerManager playerManager1;
    [SerializeField] private PlayerManager playerManager2;

    [Header("Score parameters")]
    [SerializeField] private int scoreMax = 100;
    [SerializeField] private int Score1 = 0;
    [SerializeField] private int Score2 = 0;
    [SerializeField] private int Round1 = 0;
    [SerializeField] private int Round2 = 0;

    public bool eventKeyStarted = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void SetReferences()
    {
        if (playerManager1 == null || playerManager2 == null)
        {
            var pm = FindObjectsOfType<PlayerManager>();
            foreach (var p in pm)
            {
                if (p.Player == Player.Player1 )
                {
                    playerManager1 = p;
                }
                else if (p.Player == Player.Player2)
                {
                    playerManager2 = p;
                }
            }
        }
        if (tileController == null)
        {
            tileController = TileController.instance;
        }
        if (playerInterface == null)
        {
            playerInterface = PlayerInterface.instance;
        }
    }

    private void Update()
    {
        SetReferences();
        if (!eventKeyStarted && (playerManager1.HasKey || playerManager2.HasKey))
        {
            Debug.Log("GET OUT!");
            eventKeyStarted = true;
            EventKeyStarted();
        }
    }

    public void PlayerOnTileOut(PlayerManager _pm, Tile _tile)
    {
        // Possibilité animation sur _tile de la sortie
        if (eventKeyStarted)
        {
            foreach (var tile in tileController.tilesOut)
            {
                if (playerManager1.MyPlayer.currentTile == tile)
                {
                    Debug.Log(playerManager1.Player + " win this round!");
                    EndRound();
                }
                else if (playerManager2.MyPlayer.currentTile == tile)
                {
                    Debug.Log(playerManager2.Player + " win this round!");
                }
            }
            EndRound();
        }
    }

    private void EventKeyStarted()
    {
        // Animation CANVAS + 3D
        foreach (var tile in tileController.tilesOut)
        {
            tile.SetValueToShader("_OutReady", 1);
        }
    }

    private void EndRound()
    {
        eventKeyStarted = false;
        Score1 += playerManager1.Score;
        Score2 += playerManager2.Score;
        if (Score1 > Score2)
        {
            Round1++;
        }
        else
        {
            Round2++;
        }

        SetInterfaceEndScreen();

        if (Round1 >= scoreMax)
        {
            playerInterface.buttonNextRound.gameObject.SetActive(true);
            EndGame("Player 1");
        }
        else if(Round2 >= scoreMax)
        {
            playerInterface.buttonNextRound.gameObject.SetActive(true);
            EndGame("Player 2");
        }
    }

    private void SetInterfaceEndScreen()
    {
        playerInterface.endScreen.SetActive(true);
        playerInterface.scoreP1.text = Score1.ToString();
        playerInterface.scoreP2.text = Score2.ToString();
        playerInterface.RoundP1.text = Round1.ToString();
        playerInterface.RoundP2.text = Round2.ToString();
        playerInterface.buttonNextRound.onClick.AddListener(GoToNextRound);
    }

    public void GoToNextRound()
    {
        //Debug.LogError("LOAD SCENE NEXT FRAME");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void EndGame(string _who)
    {
        Debug.Log("END GAME " + _who);
        playerInterface.buttonEndGame.gameObject.SetActive(true);
        playerInterface.buttonEndGame.GetComponentInChildren<TextMeshProUGUI>().text = _who;
    }
}
