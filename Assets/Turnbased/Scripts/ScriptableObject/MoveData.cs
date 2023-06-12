using UnityEngine;

[CreateAssetMenu(fileName = "MoveData", menuName = "Pokemon/Move", order = 1)]
public class MoveData : ScriptableObject
{
    public string moveName;
    public int power;
    public float accuracy;
}