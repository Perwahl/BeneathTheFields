using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/Card", order = 1)]
public class Card : ScriptableObject
{
    [SerializeField] private string cardName;
    [SerializeField] public string CardName { get { return cardName; } }

    [SerializeField] private string description;
    [SerializeField] public string Description { get { return description; } }

    [SerializeField] private CardType type;
    [SerializeField] public CardType Type { get { return type; } }

    [SerializeField] private CardEffect[] effects;
    [SerializeField] public CardEffect[] Effects { get { return effects; } }

    [SerializeField] private Sprite cardImage;
    [SerializeField] public Sprite CardImage { get { return cardImage; } }
        
    public enum CardType { undefined, gear, action, dungeon, treasure };

    public bool NeedsTarget()
    {
        foreach (var effect in effects)
        {
            if (effect.NeedsTarget)
            {
                return true;
            }
        }
        return false;
    }
}