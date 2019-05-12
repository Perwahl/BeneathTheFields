using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/GameState", order = 1)]
internal class GameState : ScriptableObject
{
    [SerializeField] private List<CardGameobject> playerDeck;
    [SerializeField] public List<CardGameobject> PlayerDeck { get { return playerDeck; } }

    [SerializeField] private List<CardGameobject> playerDiscard;
    [SerializeField] public List<CardGameobject> PlayerDiscard { get { return playerDiscard; } }

    [SerializeField] private List<CardGameobject> playerHand;
    [SerializeField] public List<CardGameobject> PlayerHand { get { return playerHand; } }

    [SerializeField] private List<Monster> monsters;
    [SerializeField] public List<Monster> Monsters { get { return monsters; } }

    [SerializeField] private int playerHealth;
    public int PlayerHealth { get { return playerHealth; } set { playerHealth = value; } }
    [SerializeField] private int playerXP;
    public int PlayerXP { get { return playerXP; } set { playerXP = value; } }
    [SerializeField] private int[] effectValues;
    [SerializeField] public int[] EffectValues { get { return effectValues; } }

    public void ResetGameState()
    {
        playerDeck = new List<CardGameobject>();
        playerHand = new List<CardGameobject>();
        playerDiscard = new List<CardGameobject>();       
        monsters = new List<Monster>();
        effectValues = new int[10];

        PlayerHealth = 20;     
        PlayerXP = 0;
    }
}