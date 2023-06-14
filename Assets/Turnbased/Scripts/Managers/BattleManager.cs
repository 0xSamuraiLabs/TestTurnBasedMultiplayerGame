using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Turnbased.Scripts.Player;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
   public static BattleManager instance;
   public int TurnIndex;
   
   [SerializeField] private BattleUIManager _battleUIManager;
   [SerializeField] private Unit myPlayer;
   private PhotonView pView;

   private void Start()
   {
      if (instance == null)
      {
         instance = this;
      }

      pView = PhotonView.Get(this);
   }

   public static BattleManager GetInstance()
   {
      return instance;
   }

   public void Init(Unit player)
   {
      myPlayer = player;
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
     TurnHandler.GetInstance().EndTurn(myPlayer.get);
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
      myPlayer.Attack();
      // _battleUIManager.ShowIncomingBattleText(attacker.charData);
   }
}

public enum EMoveType
{
   Attack=0 , Defend=1 , Heal=2 , Swap=3
}
