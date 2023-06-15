using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using Unit = Turnbased.Scripts.Player.Unit;

public class BattleManager : MonoBehaviour
{
   public static BattleManager instance;
   
   [SerializeField] private BattleUIManager _battleUIManager;
   private PhotonView pView;

   private void Start()
   {
      if (instance == null)
      {
         instance = this;
      }

      pView = PhotonView.Get(this);
      SetTurnMessage();
      
   }

   void SetTurnMessage()
   {
      if (TurnHandler.GetInstance().IsMyCurrentTurn())
      {
         _battleUIManager.ShowIncomingBattleText("Please Choose Your Move");
         _battleUIManager.SetBattleActions(true);
      }
      else
      {
         _battleUIManager.ShowIncomingBattleText("Waiting For Opponent To Play");
         _battleUIManager.SetBattleActions(false);
      }
   }

   public static BattleManager GetInstance()
   {
      return instance;
   }
   
   
   public void DoAction(int moveType)
   {
      if (!TurnHandler.GetInstance().IsMyCurrentTurn())
      {
         return;
      }

      switch ((EMoveType)moveType)
      {
         case EMoveType.Attack:
            Attack();
            break;
         case EMoveType.Defend:
            Defend();
            break;
         case EMoveType.Heal:
            Heal();
            break;
         case EMoveType.Swap:
            Swap();
            break;
      }
      pView.RPC(nameof(NextTurn),RpcTarget.AllBuffered);
   }
   
   [PunRPC]
   private void NextTurn()
   {
     TurnHandler.GetInstance().EndTurn();
     SetTurnMessage();
   }
   

   private void Swap()
   {
      throw new System.NotImplementedException();
   }

   private void Heal()
   {
      GameObject player = GetMyPlayer();
      if (player != null)
      {
         Unit playerUnit = player.GetComponent<Unit>();
         playerUnit.Heal(50);
      }
   }

   private void Defend()
   {
      GameObject player = GetMyPlayer();
      if (player != null)
      {
         Unit playerUnit = player.GetComponent<Unit>();
         playerUnit.Defend(true);
      }
   }

   private void Attack()
   {
         GameObject opponent = GetOpponentPlayer();
         if (opponent != null)
         {
            Unit opponentUnit = opponent.GetComponent<Unit>();
            if (opponentUnit!=null)
            {
               if (opponentUnit.isDefending)
               {
                  Debug.Log("Defended");
                  return;
               }
               opponentUnit.Attack();
            }
         }
   }
   
   GameObject GetOpponentPlayer()
   {
      GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
      foreach (var pla in players)
      {
         if (!pla.GetComponent<PhotonView>().IsMine)
         {
            return pla;
         }
      }

      return null;
   }

   GameObject GetMyPlayer()
   {
      GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
      foreach (var pla in players)
      {
         if (pla.GetComponent<PhotonView>().IsMine)
         {
            return pla;
         }
      }

      return null;
   }

}

public enum EMoveType
{
   Attack=0 , Defend=1 , Heal=2 , Swap=3
}

