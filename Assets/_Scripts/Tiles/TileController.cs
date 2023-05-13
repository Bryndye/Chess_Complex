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
                        ResetTile();
                        TilesListMvtPossible.Clear();
                    }
                    else
                    {
                        Debug.Log("Wrong tile");
                    }
                }
                //Debug.Log("Objet cliqu� : " + clickedObject.name);
            }
        }
    }

    #region CalculatePortee
    public void CalculatePortee(PlayerController playerController, Card _card)
    {
        if (_card.CircleMovement)
        {
            PorteeSquare(playerController, _card);
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
            PorteeKnigth(playerController, _card);
        }
        // can movement player
        currentPlayer = turnController.Player1Turn() ? playerManager1 : playerManager2;
        canMovement = true;
    }

    private void PorteeLigne(PlayerController playerController, Card _card)
    {
        Vector2 posInitPlayer = playerController.currentTile.MyPosition;
        int maxRange = _card.Portee + playerController.PorteeBoost;

        // Convertir la position en coordonn�es enti�res
        int startX = Mathf.RoundToInt(posInitPlayer.x);
        int startY = Mathf.RoundToInt(posInitPlayer.y);

        

        // Parcourir les cases dans la port�e maximale
        for (int x = startX + 1; x <= startX + maxRange; x++)
        {
            // Calculer la distance horizontale par rapport � la position de d�part
            int distanceX = x - startX;

            // V�rifier si la distance horizontale est dans la port�e maximale
            if (distanceX <= maxRange)
            {
                Vector2 tilePosition = new Vector2(x, startY);
                Tile tile = GetTileAtPosition(tilePosition);
                if (tile != null)
                {
                    if (!tile.PlaceTaken)
                    {
                        SetTileIntoPortee(tile);
                    }
                    else
                    {
                        break;
                    }
                }
                //Debug.Log("Case dans la port�e : " + tile);
            }
        }

        for (int x = startX - 1; x >= startX - maxRange; x--)
        {
            // Calculer la distance horizontale par rapport � la position de d�part
            int distanceX = startX - x;

            // V�rifier si la distance horizontale est dans la port�e maximale
            if (distanceX <= maxRange)
            {
                Vector2 tilePosition = new Vector2(x, startY);
                Tile tile = GetTileAtPosition(tilePosition);
                if (tile != null)
                {
                    if (!tile.PlaceTaken)
                    {
                        SetTileIntoPortee(tile);
                    }
                    else
                    {
                        break;
                    }
                }
                //Debug.Log("Case dans la port�e : " + tile);
            }
        }

        for (int y = startY + 1; y <= startY + maxRange; y++)
        {
            // Calculer la distance verticale par rapport � la position de d�part
            int distanceY = y - startY;

            // V�rifier si la distance verticale est dans la port�e maximale
            if (distanceY <= maxRange)
            {
                Vector2 tilePosition = new Vector2(startX, y);
                Tile tile = GetTileAtPosition(tilePosition);
                if (tile != null)
                {
                    if (!tile.PlaceTaken)
                    {
                        SetTileIntoPortee(tile);
                    }
                    else
                    {
                        break;
                    }
                }
                //Debug.Log("Case dans la port�e : " + tile);
            }
        }

        for (int y = startY - 1; y >= startY - maxRange; y--)
        {
            // Calculer la distance verticale par rapport � la position de d�part
            int distanceY = startY - y;

            // V�rifier si la distance verticale est dans la port�e maximale
            if (distanceY <= maxRange)
            {
                Vector2 tilePosition = new Vector2(startX, y);
                Tile tile = GetTileAtPosition(tilePosition);
                if (tile != null)
                {
                    if (!tile.PlaceTaken)
                    {
                        SetTileIntoPortee(tile);
                    }
                    else
                    {
                        break;
                    }
                }
                //Debug.Log("Case dans la port�e : " + tile);
            }
        }
    }

    private void PorteeDiagonale(PlayerController playerController, Card _card)
    {
        Vector2 posInitPlayer = playerController.currentTile.MyPosition;
        int maxRange = _card.Portee + playerController.PorteeBoost;

        // Convertir la position en coordonn�es enti�res
        int startX = Mathf.RoundToInt(posInitPlayer.x);
        int startY = Mathf.RoundToInt(posInitPlayer.y);


        // Parcourir les cases dans la port�e maximale
        for (int i = 1; i <= maxRange; i++)
        {
            int x = startX + i;
            int y = startY + i;

            // V�rifier si la case est dans la port�e maximale
            if (x <= startX + maxRange && y <= startY + maxRange)
            {
                Vector2 tilePosition = new Vector2(x, y);
                Tile tile = GetTileAtPosition(tilePosition);
                if (tile != null)
                {
                    if (!tile.PlaceTaken)
                    {
                        SetTileIntoPortee(tile);
                    }
                    else
                    {
                        break;
                    }
                }
                //Debug.Log("Case dans la port�e : " + tile);
            }
        }

        for (int i = 1; i <= maxRange; i++)
        {
            int x = startX - i;
            int y = startY + i;

            // V�rifier si la case est dans la port�e maximale
            if (x >= startX - maxRange && y <= startY + maxRange)
            {
                Vector2 tilePosition = new Vector2(x, y);
                Tile tile = GetTileAtPosition(tilePosition);
                if (tile != null)
                {
                    if (!tile.PlaceTaken)
                    {
                        SetTileIntoPortee(tile);
                    }
                    else
                    {
                        break;
                    }
                }
                //Debug.Log("Case dans la port�e : " + tile);
            }
        }

        for (int i = 1; i <= maxRange; i++)
        {
            int x = startX + i;
            int y = startY - i;

            // V�rifier si la case est dans la port�e maximale
            if (x <= startX + maxRange && y >= startY - maxRange)
            {
                Vector2 tilePosition = new Vector2(x, y);
                Tile tile = GetTileAtPosition(tilePosition);
                if (tile != null)
                {
                    if (!tile.PlaceTaken)
                    {
                        SetTileIntoPortee(tile);
                    }
                    else
                    {
                        break;
                    }
                }
                //Debug.Log("Case dans la port�e : " + tile);
            }
        }

        for (int i = 1; i <= maxRange; i++)
        {
            int x = startX - i;
            int y = startY - i;

            // V�rifier si la case est dans la port�e maximale
            if (x >= startX - maxRange && y >= startY - maxRange)
            {
                Vector2 tilePosition = new Vector2(x, y);
                Tile tile = GetTileAtPosition(tilePosition);
                if (tile != null)
                {
                    if (!tile.PlaceTaken)
                    {
                        SetTileIntoPortee(tile);
                    }
                    else
                    {
                        break;
                    }
                }
                //Debug.Log("Case dans la port�e : " + tile);
            }
        }
    }

    private void PorteeSquare(PlayerController playerController, Card _card)
    {
        Vector2 posInitPlayer = playerController.currentTile.MyPosition;
        int normalRange = _card.Portee + playerController.PorteeBoost;
        int diagonalRange = normalRange + 1; // Port�e diagonale d'une case suppl�mentaire

        // Convertir la position en coordonn�es enti�res
        int startX = Mathf.RoundToInt(posInitPlayer.x);
        int startY = Mathf.RoundToInt(posInitPlayer.y);

        // Parcourir les cases dans la port�e maximale
        for (int x = startX - normalRange; x <= startX + normalRange; x++)
        {
            for (int y = startY - normalRange; y <= startY + normalRange; y++)
            {
                // V�rifier si la case est diff�rente de la position initiale du joueur
                if (x != startX || y != startY)
                {
                    // Calculer la distance entre la position de d�part et la case actuelle
                    int distanceX = Mathf.Abs(x - startX);
                    int distanceY = Mathf.Abs(y - startY);

                    // V�rifier si la case est dans la port�e normale ou diagonale
                    if (distanceX + distanceY <= normalRange || (distanceX <= diagonalRange && distanceY <= diagonalRange))
                    {
                        Vector2 tilePosition = new Vector2(x, y);
                        Tile tile = GetTileAtPosition(tilePosition);
                        if (tile != null)
                        {
                            if (!tile.PlaceTaken)
                            {
                                SetTileIntoPortee(tile);
                            }
                            else
                            {
                                break;
                            }
                        }
                        // La case est dans la port�e, vous pouvez faire quelque chose avec cette case
                        //Debug.Log("Case dans la port�e : " + tile);
                    }
                }
            }
        }
    }

    private void PorteeKnigth(PlayerController playerController, Card _card)
    {
        Vector2 posInitPlayer = playerController.currentTile.MyPosition;
        int maxRange = _card.Portee + playerController.PorteeBoost;

        // Convertir la position en coordonn�es enti�res
        int startX = Mathf.RoundToInt(posInitPlayer.x);
        int startY = Mathf.RoundToInt(posInitPlayer.y);

        List<Tile> list = new List<Tile>();

        // D�placements possibles du cavalier sous forme de d�calages de coordonn�es
        int[] offsetX = { -2, -1, 1, 2, -2, -1, 1, 2 };
        int[] offsetY = { 1, 2, 2, 1, -1, -2, -2, -1 };

        // Parcourir les d�placements possibles
        for (int i = 0; i < offsetX.Length; i++)
        {
            int x = startX + offsetX[i];
            int y = startY + offsetY[i];

            // V�rifier si la case est dans la port�e maximale
            if (Mathf.Abs(x - startX) + Mathf.Abs(y - startY) <= maxRange)
            {
                Vector2 tilePosition = new Vector2(x, y);
                Tile tile = GetTileAtPosition(tilePosition);
                if (tile != null)
                {
                    if (!tile.PlaceTaken)
                    {
                        SetTileIntoPortee(tile);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }


    private void PorteeStandard(PlayerController playerController, Card _card)
    {
        Vector2 posInitPlayer = playerController.currentTile.MyPosition;
        int maxRange = _card.Portee + playerController.PorteeBoost;

        // Convertir la position en coordonn�es enti�res
        int startX = Mathf.RoundToInt(posInitPlayer.x);
        int startY = Mathf.RoundToInt(posInitPlayer.y);

        

        // Parcourir les cases dans la port�e maximale
        for (int x = startX - maxRange; x <= startX + maxRange; x++)
        {
            for (int y = startY - maxRange; y <= startY + maxRange; y++)
            {
                // Calculer la distance entre la position de d�part et la case actuelle
                int distance = Mathf.Abs(x - startX) + Mathf.Abs(y - startY);

                // V�rifier si la case est dans la port�e maximale
                if (distance <= maxRange)
                {
                    // La case est dans la port�e, vous pouvez faire quelque chose avec cette case
                    Vector2 tilePosition = new Vector2(x, y);
                    Tile tile = GetTileAtPosition(tilePosition);
                    if (tile != null)
                    {
                        if (!tile.PlaceTaken)
                        {
                            SetTileIntoPortee(tile);
                        }
                        else
                        {
                            break;
                        }
                    }
                    //Debug.Log("Case dans la port�e : " + tile);
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

    private void SetTileIntoPortee(Tile tile)
    {
        tile.SetValueToShader("_InPortee", 1);
        TilesListMvtPossible.Add(tile);
    }

    private void ResetTile()
    {
        foreach (var tile in TilesListMvtPossible)
        {
            tile.SetValueToShader("_InPortee", 0);
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
