using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileController : MonoBehaviour
{
    public static TileController instance;
    private TurnController turnController;
    public Tile[] tiles;

    [Header("Movement tile")]
    [SerializeField] private bool canMovement = false;
    public List<Tile> TilesListMvtPossible;
    private PlayerManager currentPlayer;
    [SerializeField] PlayerManager playerManager1;
    [SerializeField] PlayerManager playerManager2;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        turnController = TurnController.instance;
        tiles = FindObjectsOfType<Tile>();
    }


    void Update()
    {
        if (canMovement && !currentPlayer.HasPlayedMovement)
        {
            mouseIntoWorld(currentPlayer);
        }
    }

    public void mouseIntoWorld(PlayerManager playerManager)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            // La souris est sur un objet UI, ignorer le raycast pour les objets 3D
            Debug.Log("UI entre souris et 3D");
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedObject = hit.collider.gameObject;
                if (clickedObject.TryGetComponent(out Tile _tile))
                {
                    if (CheckTileIntoPortee(_tile))
                    {
                        playerManager.MyPlayer.SetItemOnTile(_tile);
                        playerManager.HasPlayedMovement = true;
                        canMovement = false;
                    }
                    else
                    {
                        Debug.Log("Wrong tile");
                    }
                }

                // Utilisez 'clickedObject' comme vous le souhaitez, par exemple :
                //Debug.Log("Objet cliqué : " + clickedObject.name);
            }
        }
    }

    #region CalculatePortee
    public void CalculatePortee(PlayerController playerController, Card _card)
    {
        TilesListMvtPossible.Clear();

        if (_card.CircleMovement)
        {

        }
        if (_card.LigneMovement)
        {
            PorteeLigne(playerController, _card);
        }
        if (_card.DiagonaleMovement)
        {
            PorteeDiagonale(playerController, _card);
        }
        if (_card.KnightMovement)
        {

        }
        // can movement player
        currentPlayer = turnController.Player1Turn() ? playerManager1 : playerManager2;
        canMovement = true;
    }

    private void PorteeLigne(PlayerController playerController, Card _card)
    {
        Vector2 posInitPlayer = playerController.currentTile.MyPosition;
        int maxRange = _card.Portee + playerController.PorteeBoost;

        // Convertir la position en coordonnées entières
        int startX = Mathf.RoundToInt(posInitPlayer.x);
        int startY = Mathf.RoundToInt(posInitPlayer.y);

        

        // Parcourir les cases dans la portée maximale
        for (int x = startX + 1; x <= startX + maxRange; x++)
        {
            // Calculer la distance horizontale par rapport à la position de départ
            int distanceX = x - startX;

            // Vérifier si la distance horizontale est dans la portée maximale
            if (distanceX <= maxRange)
            {
                SetTileIntoPortee(x, startY);
                //Debug.Log("Case dans la portée : " + tile);
            }
        }

        for (int x = startX - 1; x >= startX - maxRange; x--)
        {
            // Calculer la distance horizontale par rapport à la position de départ
            int distanceX = startX - x;

            // Vérifier si la distance horizontale est dans la portée maximale
            if (distanceX <= maxRange)
            {
                SetTileIntoPortee(x, startY);
                //Debug.Log("Case dans la portée : " + tile);
            }
        }

        for (int y = startY + 1; y <= startY + maxRange; y++)
        {
            // Calculer la distance verticale par rapport à la position de départ
            int distanceY = y - startY;

            // Vérifier si la distance verticale est dans la portée maximale
            if (distanceY <= maxRange)
            {
                SetTileIntoPortee(startX, y);
                //Debug.Log("Case dans la portée : " + tile);
            }
        }

        for (int y = startY - 1; y >= startY - maxRange; y--)
        {
            // Calculer la distance verticale par rapport à la position de départ
            int distanceY = startY - y;

            // Vérifier si la distance verticale est dans la portée maximale
            if (distanceY <= maxRange)
            {
                SetTileIntoPortee(startX, y);
                //Debug.Log("Case dans la portée : " + tile);
            }
        }
    }

    private void PorteeDiagonale(PlayerController playerController, Card _card)
    {
        Vector2 posInitPlayer = playerController.currentTile.MyPosition;
        int maxRange = _card.Portee + playerController.PorteeBoost;

        // Convertir la position en coordonnées entières
        int startX = Mathf.RoundToInt(posInitPlayer.x);
        int startY = Mathf.RoundToInt(posInitPlayer.y);


        // Parcourir les cases dans la portée maximale
        for (int i = 1; i <= maxRange; i++)
        {
            int x = startX + i;
            int y = startY + i;

            // Vérifier si la case est dans la portée maximale
            if (x <= startX + maxRange && y <= startY + maxRange)
            {
                SetTileIntoPortee(x, y);
                //Debug.Log("Case dans la portée : " + tile);
            }
        }

        for (int i = 1; i <= maxRange; i++)
        {
            int x = startX - i;
            int y = startY + i;

            // Vérifier si la case est dans la portée maximale
            if (x >= startX - maxRange && y <= startY + maxRange)
            {
                SetTileIntoPortee(x, y);
                //Debug.Log("Case dans la portée : " + tile);
            }
        }

        for (int i = 1; i <= maxRange; i++)
        {
            int x = startX + i;
            int y = startY - i;

            // Vérifier si la case est dans la portée maximale
            if (x <= startX + maxRange && y >= startY - maxRange)
            {
                SetTileIntoPortee(x, y);
                //Debug.Log("Case dans la portée : " + tile);
            }
        }

        for (int i = 1; i <= maxRange; i++)
        {
            int x = startX - i;
            int y = startY - i;

            // Vérifier si la case est dans la portée maximale
            if (x >= startX - maxRange && y >= startY - maxRange)
            {
                SetTileIntoPortee(x, y);
                //Debug.Log("Case dans la portée : " + tile);
            }
        }
    }

    private void PorteeStandard(PlayerController playerController, Card _card)
    {
        Vector2 posInitPlayer = playerController.currentTile.MyPosition;
        int maxRange = _card.Portee + playerController.PorteeBoost;

        // Convertir la position en coordonnées entières
        int startX = Mathf.RoundToInt(posInitPlayer.x);
        int startY = Mathf.RoundToInt(posInitPlayer.y);

        

        // Parcourir les cases dans la portée maximale
        for (int x = startX - maxRange; x <= startX + maxRange; x++)
        {
            for (int y = startY - maxRange; y <= startY + maxRange; y++)
            {
                // Calculer la distance entre la position de départ et la case actuelle
                int distance = Mathf.Abs(x - startX) + Mathf.Abs(y - startY);

                // Vérifier si la case est dans la portée maximale
                if (distance <= maxRange)
                {
                    // La case est dans la portée, vous pouvez faire quelque chose avec cette case
                    SetTileIntoPortee(x,y);
                    //Debug.Log("Case dans la portée : " + tile);
                }
            }
        }
    }
    #endregion

    private Tile GetTileAtPosition(Vector2 position)
    {
        foreach (Tile tile in tiles)
        {
            if (tile.MyPosition == position)
            {
                return tile;
            }
        }

        return null;
    }

    private void SetTileIntoPortee(int x, int y)
    {
        Vector2 tilePosition = new Vector2(x, y);
        Tile tile = GetTileAtPosition(tilePosition);
        if (tile != null)
        {
            tile.ChangerMyMaterial(Color.blue);
            TilesListMvtPossible.Add(tile);
        }
    }

    private bool CheckTileIntoPortee(Tile _tile)
    {
        foreach(Tile tile in TilesListMvtPossible)
        {
            if (_tile.MyPosition == tile.MyPosition)
            {
                return true;
            }
        }
        return false;
    }
}
