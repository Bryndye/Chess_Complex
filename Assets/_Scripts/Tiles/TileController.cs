using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileController : MonoBehaviour
{
    public static TileController instance;
    private TurnController turnController;

    [Header("Tiles parameter")]
    public Tile[] tiles;
    public Tile[] tilesStayBlank;
    public Tile[] tilesOut;
    public const int tilesEventCount = 16;
    public const int tilesShopCount = 4;
    public const int tilesRandomCount = 16;
    public const int tilesKeyCount = 1;

    [Header("Movement tile")]
    [SerializeField] private bool canMovement = false;
    public List<Tile> TilesListMvtPossible;
    private PlayerManager currentPlayer;
    private Card currentCard;
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
        GenerateTilesEvent();
    }

    #region Init Tile Event
    private void GenerateTilesEvent()
    {
        // get all tiles which dont need to put event into
        List<Tile> tilesEvent = new List<Tile>();
        foreach (var tile in tiles)
        {
            if (!tilesStayBlank.Contains(tile) && !tilesOut.Contains(tile))
            {
                tilesEvent.Add(tile);
            }
        }

        foreach (var tile in tilesOut)
        {
            tile.MyType = TileType.Out;
            tile.SetTexture();
        }

        int randomIndex = UnityEngine.Random.Range(0, tilesEvent.Count() - 1);
        Tile tileKey = tilesEvent[randomIndex];
        Debug.Log(tileKey + " has key");
        tileKey.MyType = TileType.Key;


        int shopCount = 0;
        int randomCardCount = 0;
        int eventCount = 0;

        foreach (var tile in tilesEvent)
        {
            if (tile.MyType == TileType.NoEvent)
            {
                // Assigner un type de tuile en fonction des compteurs
                if (shopCount < tilesShopCount)
                {
                    tile.MyType = TileType.Shop;
                    shopCount++;
                }
                else if (randomCardCount < tilesRandomCount)
                {
                    tile.MyType = TileType.RandomCard;
                    randomCardCount++;
                }
                else if (eventCount < tilesEventCount)
                {
                    tile.MyType = TileType.Event;
                    eventCount++;
                }
                tile.SetTexture();
            }
        }
    }


    #endregion


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
            //Debug.Log("UI entre souris et 3D");
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
                        playerManager.HasPlayedMovement = true;
                        canMovement = false;
                        if (!currentCard.KnightMovement)
                        {
                            DiscoverTiles(_tile);
                        }
                        playerManager.MyPlayer.SetItemOnTile(_tile);
                        if (currentCard.KnightMovement)
                        {
                            DiscoverTiles(_tile);
                        }

                        ResetTile();
                        TilesListMvtPossible.Clear();
                        ResetMovementParameter();
                    }
                    else
                    {
                        Debug.Log("Wrong tile");
                    }
                }
                //Debug.Log("Objet cliqué : " + clickedObject.name);
            }
        }
    }

    private void DiscoverTiles(Tile _end)
    {
        if (currentCard.CircleMovement)
        {
            //PorteeSquare(currentPlayer.MyPlayer, currentCard, previsualisation);
        }
        if (currentCard.LigneMovement)
        {
            //PorteeLigne(currentPlayer.MyPlayer, currentCard, previsualisation);
            GetLine(currentPlayer.MyPlayer.currentTile, _end);
        }
        if (currentCard.DiagonaleMovement)
        {
            //PorteeDiagonale(currentPlayer.MyPlayer, currentCard, previsualisation);
            GetDiagonalLine(currentPlayer.MyPlayer.currentTile, _end);
        }
        if (currentCard.KnightMovement)
        {
            PorteeSquare(currentPlayer.MyPlayer, currentCard, false, true);
        }
    }

    #region CalculatePortee
    public void CalculatePortee(PlayerController playerController, Card _card, bool previsualisation = false)
    {
        ResetMovementParameter();
        if (_card.CircleMovement)
        {
            PorteeSquare(playerController, _card, previsualisation);
        }
        if (_card.LigneMovement)
        {
            PorteeLigne(playerController, _card, previsualisation);
        }
        if (_card.DiagonaleMovement)
        {
            PorteeDiagonale(playerController, _card, previsualisation);
        }
        if (_card.KnightMovement)
        {
            PorteeKnigth(playerController, _card, previsualisation);
        }
        // can movement player
        if (!previsualisation)
        {
            currentCard = _card;
            currentPlayer = turnController.Player1Turn() ? playerManager1 : playerManager2;
            canMovement = true;
        }
    }

    private void PorteeLigne(PlayerController playerController, Card _card, bool previsualisation = false)
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
                //Debug.Log("Case dans la portée : " + tile);
            }
        }
    }

    private void PorteeDiagonale(PlayerController playerController, Card _card, bool previsualisation = false)
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
                //Debug.Log("Case dans la portée : " + tile);
            }
        }
    }

    private void PorteeSquare(PlayerController playerController, Card _card, bool previsualisation = false, bool forDiscovering = false)
    {
        Vector2 posInitPlayer = playerController.currentTile.MyPosition;

        int normalRange = 0;
        if (forDiscovering)
        {
            normalRange = 1;
        }
        else
        {
            normalRange = _card.Portee + playerController.PorteeBoost;
        }

        // Convertir la position en coordonnées entières
        int startX = Mathf.RoundToInt(posInitPlayer.x);
        int startY = Mathf.RoundToInt(posInitPlayer.y);

        // Parcourir les cases dans la portée maximale
        for (int x = startX - normalRange; x <= startX + normalRange; x++)
        {
            for (int y = startY - normalRange; y <= startY + normalRange; y++)
            {
                // Vérifier si la case est différente de la position initiale du joueur
                if (x != startX || y != startY)
                {
                    // Calculer la distance entre la position de départ et la case actuelle
                    int distanceX = Mathf.Abs(x - startX);
                    int distanceY = Mathf.Abs(y - startY);

                    // Vérifier si la case est dans la portée normale ou diagonale
                    if (distanceX <= normalRange && distanceY <= normalRange)
                    {
                        Vector2 tilePosition = new Vector2(x, y);
                        Tile tile = GetTileAtPosition(tilePosition);
                        if (tile != null && !tile.PlaceTaken)
                        {
                            if (forDiscovering)
                            {
                                tile.SetValueToShader("_Show", 1);
                            }
                            else if (!LineOfSightBlocked(startX, startY, x, y))
                            {
                                SetTileIntoPortee(tile);
                            }
                        }
                    }
                }
            }
        }
    }

    private bool LineOfSightBlocked(int startX, int startY, int endX, int endY)
    {
        int dx = Mathf.Abs(endX - startX);
        int dy = Mathf.Abs(endY - startY);
        int n = 1 + dx + dy;
        int x = startX;
        int y = startY;
        int x_inc = (endX > startX) ? 1 : -1;
        int y_inc = (endY > startY) ? 1 : -1;
        int error = dx - dy;
        dx *= 2;
        dy *= 2;

        // Ignore the first tile (the player's tile)
        if (error > 0)
        {
            x += x_inc;
            error -= dy;
        }
        else
        {
            y += y_inc;
            error += dx;
        }

        for (; n > 1; --n) // We start at n > 1 to ignore the player's tile
        {
            Vector2 tilePosition = new Vector2(x, y);
            Tile tile = GetTileAtPosition(tilePosition);

            if (tile != null && tile.PlaceTaken)
            {
                return true;
            }

            if (error > 0)
            {
                x += x_inc;
                error -= dy;
            }
            else
            {
                y += y_inc;
                error += dx;
            }
        }

        return false;
    }

    private void PorteeKnigth(PlayerController playerController, Card _card, bool previsualisation = false)
    {
        Vector2 posInitPlayer = playerController.currentTile.MyPosition;
        int boost = playerController.PorteeBoost;

        // Convertir la position en coordonnées entières
        int startX = Mathf.RoundToInt(posInitPlayer.x);
        int startY = Mathf.RoundToInt(posInitPlayer.y);

        // Déplacements possibles du cavalier sous forme de décalages de coordonnées
        int[] offsetX = { -2 - boost, -1, 1 , 2 + boost, -2 - boost, -1, 1, 2 + boost };
        int[] offsetY = { 1, 2 + boost, 2 + boost, 1, -1, -2 - boost, -2 - boost, -1 };

        // Parcourir les déplacements possibles
        for (int i = 0; i < offsetX.Length; i++)
        {
            int x = startX + offsetX[i];
            int y = startY + offsetY[i];

            Vector2 tilePosition = new Vector2(x, y);
            Tile tile = GetTileAtPosition(tilePosition);
            if (tile != null)
            {
                if (!tile.PlaceTaken)
                {
                    SetTileIntoPortee(tile);
                }
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
                    //Debug.Log("Case dans la portée : " + tile);
                }
            }
        }
    }

    public void GetLine(Tile start, Tile end)
    {
        Vector2 currentPosition = start.MyPosition;

        int dx = Math.Abs((int)end.MyPosition.x - (int)start.MyPosition.x), sx = start.MyPosition.x < end.MyPosition.x ? 1 : -1;
        int dy = Math.Abs((int)end.MyPosition.y - (int)start.MyPosition.y), sy = start.MyPosition.y < end.MyPosition.y ? 1 : -1;
        int err = (dx > dy ? dx : -dy) / 2, e2;

        while (true)
        {
            Tile currentTile = Array.Find(tiles, tile => tile.MyPosition.x == currentPosition.x && tile.MyPosition.y == currentPosition.y);
            if (currentTile != null)
            {
                currentTile.SetValueToShader("_Show", 1);
            }
            if (currentPosition.x == end.MyPosition.x && currentPosition.y == end.MyPosition.y) break;
            e2 = err;
            if (e2 > -dx) { err -= dy; currentPosition.x += sx; }
            if (e2 < dy) { err += dx; currentPosition.y += sy; }
        }
    }


    public void GetDiagonalLine(Tile start, Tile end)
    {
        int dx = Math.Abs((int)end.MyPosition.x - (int)start.MyPosition.x);
        int dy = Math.Abs((int)end.MyPosition.y - (int)start.MyPosition.y);
        int sx = start.MyPosition.x < end.MyPosition.x ? 1 : -1;
        int sy = start.MyPosition.y < end.MyPosition.y ? 1 : -1;
        int err = dx - dy;
        int e2;

        Vector2 currentPosition = start.MyPosition;

        while (true)
        {
            Tile currentTile = Array.Find(tiles, tile => tile.MyPosition.x == currentPosition.x && tile.MyPosition.y == currentPosition.y);
            if (currentTile != null)
            {
                currentTile.SetValueToShader("_Show", 1);
            }

            if (currentPosition.x == end.MyPosition.x && currentPosition.y == end.MyPosition.y)
                break;

            e2 = 2 * err;
            if (e2 > -dy)
            {
                Debug.Log(currentPosition.x);
                err -= dy;
                currentPosition.x += sx;
            }
            if (e2 < dx)
            {
                Debug.Log(currentPosition.y);
                err += dx;
                currentPosition.y += sy;
            }
        }

        Debug.Log(start.MyPosition);

    }

    #endregion

    public bool IsMoving()
    {
        if (currentPlayer != null)
        {
            return true;
        }
        return false;
    }

    #region Tile Fct parameter
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

    public void ResetMovementParameter()
    {
        ResetTile();
        TilesListMvtPossible.Clear();
        currentPlayer = null;
        currentCard = null;
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
    #endregion
}
