using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Turnbased.Scripts.UI;
using Unity.VisualScripting;
using UnityEngine;
using Unit = Turnbased.Scripts.Player.Unit;

public class BattleManager : MonoBehaviour
{
   public static BattleManager instance;
   
   [SerializeField] private BattleUIManager _battleUIManager;
   private PhotonView pView;
   [SerializeField] private CharacterSwapManager _characterSwapManager;

   private void Start()
   {
      if (instance == null)
      {
         instance = this;
      }

      CharacterSwapUI.OnCharacterSwapped += OnCharacterSwapped;
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
      GameObject player = GetMyPlayer(); 
      if (player != null)
      {
         Unit playerUnit = player.GetComponent<Unit>();
         _characterSwapManager.StartSwapProcess(playerUnit.currentCharacter);
      }
   }

   private void OnCharacterSwapped(int obj)
   {
      GameObject player = GetMyPlayer(); 
      if (player != null)
      {
         Unit playerUnit = player.GetComponent<Unit>();
         playerUnit.SpawnCharacter(obj);
      }
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
      Unit playerUnit=null;
      GameObject player = GetMyPlayer();
      if (player != null)
      {
         playerUnit = player.GetComponent<Unit>();
         playerUnit.PlayAttackAnimation();
      }
      
      GameObject opponent = GetOpponentPlayer();
         if (opponent != null)
         {
            Unit opponentUnit = opponent.GetComponent<Unit>();
            if (opponentUnit!=null)
            {
               if (playerUnit != null) opponentUnit.Attack(playerUnit.charData.DamageInfo);
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

