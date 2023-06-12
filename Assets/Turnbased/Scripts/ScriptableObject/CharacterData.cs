using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Pokemon/CharacterData", order = 1)]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public int defense;
    public DamageInfo DamageInfo;
}
