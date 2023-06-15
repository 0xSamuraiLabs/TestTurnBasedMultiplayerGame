using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class TurnHandler : MonoBehaviour
{
    public static TurnHandler instance;
    
    private bool isMyTurn;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        isMyTurn = PhotonNetwork.IsMasterClient;
    }
    
    public static TurnHandler GetInstance()
    {
        return instance;
    }

    public void EndTurn()
    {
        isMyTurn = !isMyTurn;
    }

    public bool IsMyCurrentTurn()
    {
        return isMyTurn;
    }
        
}
