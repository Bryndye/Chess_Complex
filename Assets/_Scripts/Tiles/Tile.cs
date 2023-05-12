using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    NoEvent,
    RandomEvent,
    Shop
}

public enum TileTState
{
    Unused,
    Used
}
public class Tile : MonoBehaviour
{
    public TileType MyType;
    public TileTState MyState = TileTState.Unused;
    public Vector2 MyPosition;

    private MeshRenderer myMeshRenderer;
    private Color originalColor;

    private void Awake()
    {
        myMeshRenderer = GetComponent<MeshRenderer>();
        originalColor = myMeshRenderer.material.color;
    }

    public void TouchedByMouse()
    {
        Debug.Log("Has been touched " + MyPosition);

    }

    public void ChangerMyMaterial(Color color)
    {
        Debug.Log("changed material ");
        myMeshRenderer.material.color = color;
    }

    private void OnMouseEnter()
    {
        // Code ex�cut� lorsque la souris entre dans le collider de l'objet
        Debug.Log("Souris entr�e sur l'objet : " + gameObject.name);
        ChangerMyMaterial(Color.red);
    }

    private void OnMouseExit()
    {
        // Code ex�cut� lorsque la souris quitte le collider de l'objet
        Debug.Log("Souris sortie de l'objet : " + gameObject.name);
        ChangerMyMaterial(originalColor);
    }
}


/* REGLE
 * Si tile est utilis� pour LA PREMIERE FOIS, son effet est d�clanch� et effet = neutre apr�s utilisation
 * Si la tile couleur == joueur color, le joueur en question gagne une carte PION, CHEVALIER
 */
