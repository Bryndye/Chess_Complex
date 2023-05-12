using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Tile TileStart;
    Animator myAnimator;

    private void Awake()
    {
        myAnimator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        SetItemOnTile(TileStart);
    }

    public void SetItemOnTile(Tile _tile)
    {
        Vector3 tilePositon = _tile.transform.position;
        transform.localPosition = new Vector3(tilePositon.x, tilePositon.y + 0.8f, tilePositon.z);
        myAnimator.SetTrigger("SetPosition");
    }
}
