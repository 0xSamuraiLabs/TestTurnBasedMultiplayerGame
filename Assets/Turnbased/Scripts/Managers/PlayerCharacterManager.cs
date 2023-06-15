using System.Collections.Generic;
using Turnbased.Scripts.Player;
using UnityEngine;

namespace Turnbased.Scripts.Managers
{
    public class PlayerCharacterManager : Singleton<PlayerCharacterManager>
    {
        [SerializeField] private int[] playerId;
        
        public int[] GetUserCharacterID()
        {
            return playerId;
        }
    }
}
