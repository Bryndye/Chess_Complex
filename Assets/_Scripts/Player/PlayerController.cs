using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerManager myPlayerManager;
    protected Animator myAnimator;

    [Header("Tile paramaters")]
    public Tile TileStart;
    public Tile currentTile;
    public int PorteeBoost = 0;

    protected virtual void Awake()
    {
        myAnimator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        if (TileStart != null)
        {
            SetItemOnTile(TileStart);
        }
    }

    public void SetItemOnTile(Tile _tile)
    {
        if (currentTile != null)
        {
            currentTile.ExitTile(); 
        }

        Vector3 tilePositon = _tile.transform.position;
        currentTile = _tile;
        transform.localPosition = new Vector3(tilePositon.x, tilePositon.y + 0.8f, tilePositon.z);
        _tile.EnterTile(myPlayerManager);
        myAnimator.SetTrigger("SetPosition");
    }
}
