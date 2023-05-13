using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public static TileController instance;
    public Tile[] tiles;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        tiles = FindObjectsOfType<Tile>();
    }

    public void CalculatePortee(PlayerController playerController, Card _card)
    {
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
    }

    private void PorteeLigne(PlayerController playerController, Card _card)
    {
        Vector2 posInitPlayer = playerController.currentTile.MyPosition;
        int maxRange = _card.Portee;

        // Convertir la position en coordonnées entières
        int startX = Mathf.RoundToInt(posInitPlayer.x);
        int startY = Mathf.RoundToInt(posInitPlayer.y);

        List<Tile> list = new List<Tile>();

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
                    tile.ChangerMyMaterial(Color.blue);
                    list.Add(tile);
                }
                Debug.Log("Case dans la portée : " + tile);
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
                    tile.ChangerMyMaterial(Color.blue);
                    list.Add(tile);
                }
                Debug.Log("Case dans la portée : " + tile);
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
                    tile.ChangerMyMaterial(Color.blue);
                    list.Add(tile);
                }
                Debug.Log("Case dans la portée : " + tile);
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
                    tile.ChangerMyMaterial(Color.blue);
                    list.Add(tile);
                }
                Debug.Log("Case dans la portée : " + tile);
            }
        }
    }

    private void PorteeDiagonale(PlayerController playerController, Card _card)
    {
        Vector2 posInitPlayer = playerController.currentTile.MyPosition;
        int maxRange = _card.Portee;

        // Convertir la position en coordonnées entières
        int startX = Mathf.RoundToInt(posInitPlayer.x);
        int startY = Mathf.RoundToInt(posInitPlayer.y);

        List<Tile> list = new List<Tile>();

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
                    tile.ChangerMyMaterial(Color.blue);
                    list.Add(tile);
                }
                Debug.Log("Case dans la portée : " + tile);
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
                    tile.ChangerMyMaterial(Color.blue);
                    list.Add(tile);
                }
                Debug.Log("Case dans la portée : " + tile);
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
                    tile.ChangerMyMaterial(Color.blue);
                    list.Add(tile);
                }
                Debug.Log("Case dans la portée : " + tile);
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
                    tile.ChangerMyMaterial(Color.blue);
                    list.Add(tile);
                }
                Debug.Log("Case dans la portée : " + tile);
            }
        }
    }



    private void PorteeStandard(PlayerController playerController, Card _card)
    {
        Vector2 posInitPlayer = playerController.currentTile.MyPosition;
        int maxRange = _card.Portee;

        // Convertir la position en coordonnées entières
        int startX = Mathf.RoundToInt(posInitPlayer.x);
        int startY = Mathf.RoundToInt(posInitPlayer.y);

        List<Tile> list = new List<Tile>();

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
                        tile.ChangerMyMaterial(Color.blue);
                        list.Add(tile);
                    }
                    Debug.Log("Case dans la portée : " + tile);
                }
            }
        }
    }

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
}
