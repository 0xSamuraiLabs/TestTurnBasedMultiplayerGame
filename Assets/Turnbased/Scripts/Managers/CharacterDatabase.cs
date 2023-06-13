using System;
using System.Collections;
using System.Collections.Generic;
using Turnbased.Scripts.Player;
using UnityEngine;

public class CharacterDatabase : Singleton<CharacterDatabase>
{
    [SerializeField]private List<Unit> Characters;

    public Unit GetCharacterWithID(int id)
    {
        Unit character=null;
        foreach (var ch in Characters)
        {
            if (ch.charData.charId == id)
            {
                character = ch;
            }
        }
        return character;
    }


}
