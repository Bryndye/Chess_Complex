using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator myAnimator;

    private void Awake()
    {
        myAnimator = GetComponentInChildren<Animator>();
    }

    public void SetItemOnTile(Tile _tile)
    {
        Vector3 tilePositon = _tile.transform.position;
        transform.localPosition = new Vector3(tilePositon.x, tilePositon.y + 1f, tilePositon.z);
        myAnimator.SetTrigger("SetPosition");
    }
}
