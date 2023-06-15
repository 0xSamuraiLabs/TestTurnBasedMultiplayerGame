using System;
using System.Collections;
using System.Collections.Generic;
using Turnbased.Scripts.Player;
using UnityEngine;

public class CharacterDatabase : Singleton<CharacterDatabase>
{
    public Unit CharacterPrefab;
    [SerializeField] private List<CharacterDTO> _characterDtos;
    public CharacterDTO GetCharacterWithID(int id)
    {
        CharacterDTO character=null;
        foreach (var ch in _characterDtos)
        {
            if (ch.CharacterData.charId == id)
            {
                character = ch;
            }
        }
        return character;
    }


}

[Serializable]
public class CharacterDTO
{
    public CharacterData CharacterData;
    public GameObject characterPrefab;
}
