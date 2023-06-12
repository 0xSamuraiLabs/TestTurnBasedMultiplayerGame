using System.Collections;
using System.Collections.Generic;
using Turnbased.Scripts.Player;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
   public Unit attacker;
   public Unit defender;

   [SerializeField] private BattleUIManager _battleUIManager;

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
      NextTurn();
   }
   
   private void NextTurn()
   {
      // Swap the attacker and defender for the next turn
      Unit temp = attacker;
      attacker = defender;
      defender = temp;
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
