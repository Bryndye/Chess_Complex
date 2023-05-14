using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmadaRouge : PlayerController
{
    private TurnController turnController;

    public int TurnCreation = 1;

    protected override void Awake()
    {
        base.Awake();
        turnController = TurnController.instance;
    }

    public void Initialize()
    {
        TurnCreation = turnController.TurnCount;
    }

    private void Update()
    {
        if (TurnCreation - turnController.TurnCount > 0)
        {
            Destroy(gameObject);
        }
    }
}
