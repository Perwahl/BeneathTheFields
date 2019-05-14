using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGame : MonoBehaviour
{
    //[SerializeField] private DungeonDeckManager dungeonDeckManager;
    //public static DungeonDeckManager DungeonDeckManager;

    [SerializeField] private PlayerDeckManager playerDeckManager;
    public static PlayerDeckManager PlayerDeckManager;

    [SerializeField] private PlayerDiscard playerDiscardManager;
    public static PlayerDiscard PlayerDiscardManager;

    [SerializeField] private PlayerHand playerHand;
    public static PlayerHand PlayerHand;

    [SerializeField] private PlayerHealthGlobe playerHealthGlobe;
    public static PlayerHealthGlobe PlayerHealthGlobe;
   
    [SerializeField] private PlayerDeck testPlayerDeck;

    [SerializeField] private GameState gameState;
    [SerializeField] private CoroutineQueue animationQueue;

    [SerializeField] private Camera playerCam;
    public static Camera PlayerCam;        
    public static CardGame CardGameRef;


    internal static void HighlightMonsters(bool v)
    {
        throw new NotImplementedException();
    }

    void Start()
    {
        SetupBoard();
        SetupNewGame();
        animationQueue.m_Owner = this;
        animationQueue.StartLoop();       
        playerDeckManager.DrawCard(5);
    }

    private void SetupBoard()
    {
        PlayerDeckManager = playerDeckManager;
        PlayerHand = playerHand;
        PlayerDiscardManager = playerDiscardManager;
        PlayerCam = playerCam;
        CardGameRef = this;
       // PlayerHealthGlobe = playerHealthGlobe;
    }

    private void SetupNewGame()
    {
        gameState.ResetGameState();

        PlayerDeck playerDeck = testPlayerDeck;
        
        playerDeckManager.InitStartDeck(playerDeck);

    }  
}
