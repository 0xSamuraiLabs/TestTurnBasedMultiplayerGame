using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Turnbased.Scripts.Player;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
   public static BattleManager instance;
   public Unit attacker;
   public Unit defender;

   [SerializeField] private BattleUIManager _battleUIManager;
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

   public void Init(Unit attacker , Unit defender)
   {
      this.attacker = attacker;
      this.defender = defender;
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
      // Swap the attacker and defender for the next turn
      (attacker, defender) = (defender, attacker);
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
      defender.TakeDamage(attacker.GetDamage());
      _battleUIManager.ShowIncomingBattleText(attacker.charData);
   }
}

public enum EMoveType
{
   Attack=0 , Defend=1 , Heal=2 , Swap=3
}
