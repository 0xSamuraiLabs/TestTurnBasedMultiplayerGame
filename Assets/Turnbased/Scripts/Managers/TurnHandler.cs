using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class TurnHandler : MonoBehaviour
{
    public static TurnHandler instance;
        
    private int currentTurn = 0;

    private int numberOfTurns = 0;
    public event Action<int> OnTurnCompleted;
    public event Action OnTurnEnded;

    private int myTurn;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            myTurn = 0;
        }
        else
        {
            myTurn = 1;
        }
    }
        
        

    public static TurnHandler GetInstance()
    {
        return instance;
    }

    public void ResetGame()
    {
        numberOfTurns = 0;
    }

    public int GetTurnIndex()
    {
        return currentTurn;
    }

    public int GetMyTurn()
    {
        return myTurn;
    }
    

    public void EndTurn(int turn)
    {
        currentTurn = turn;
        OnTurnCompleted?.Invoke(currentTurn);
        currentTurn++;
        numberOfTurns++;
        if (currentTurn > 1)
        {
            currentTurn = 0;
        }
        OnTurnEnded?.Invoke();
    }

        
}
