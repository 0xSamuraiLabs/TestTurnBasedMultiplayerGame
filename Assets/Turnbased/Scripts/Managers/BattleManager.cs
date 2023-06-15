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
   public int TurnIndex;
   
   [SerializeField] private BattleUIManager _battleUIManager;
   private PhotonView pView;

   private void Start()
   {
      if (instance == null)
      {
         instance = this;
      }

      pView = PhotonView.Get(this);
      TurnIndex = TurnHandler.GetInstance().GetMyTurn();
   }

   public static BattleManager GetInstance()
   {
      return instance;
   }
   
   public void DoAction(int moveType)
   {
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
     TurnHandler.GetInstance().EndTurn(TurnIndex);
   }
   

   private void Swap()
   {
      throw new System.NotImplementedException();
   }

   private void Heal()
   {
      throw new System.NotImplementedException();
   }

   private void Defend()
   {
      throw new System.NotImplementedException();
   }

   private void Attack()
   {
      GameObject opponent = GetOpponentPlayer();
      if (opponent != null)
      {
         Unit opponentUnit = opponent.GetComponent<Unit>();
         if (opponentUnit!=null)
         {
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
}

public enum EMoveType
{
   Attack=0 , Defend=1 , Heal=2 , Swap=3
}
