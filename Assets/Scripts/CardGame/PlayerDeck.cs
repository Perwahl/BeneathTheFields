using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/PlayerDeck", order = 1)]
public class PlayerDeck : ScriptableObject
{
    [SerializeField] private List<Card> cards;
    [SerializeField] public List<Card> Cards { get { return cards; } }
}
