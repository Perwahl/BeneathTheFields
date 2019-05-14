using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private CoroutineQueue animQueue;
    [SerializeField] private GameState gameState;
    [SerializeField] private List<CardGameobject> cardsInHand;  
    [SerializeField] private HandPositions handPosManager;
    private int draggedCardPos;

    internal void AddCard(CardGameobject card)
    {
        gameState.PlayerHand.Add(card);        
        animQueue.EnqueueAction(MoveCardToHandAnimation(card));
        animQueue.EnqueueWait(0.3f);        
    }

    void Update()
    {
        var positions = handPosManager.GetPositions(cardsInHand.Count);
        
        for (int i = 0; i < cardsInHand.Count; i++)
        {         
            cardsInHand[i].SetCardPositionTarget(positions[i].position);
            cardsInHand[i].SetCardRotationTarget(positions[i].rotation);
            cardsInHand[i].SetSortOrder(i);
        }
    }

    public IEnumerator MoveCardToHandAnimation(CardGameobject card)
    {
        cardsInHand.Add(card);
        card.FX.FlipCard();
        card.Interactable(true);
        card.SetSortOrder(20);
        UpdatePlayables();
        yield return null;
    }

    internal void DiscardHand()
    {        
        foreach(CardGameobject card in gameState.PlayerHand)
        {            
            CardGame.PlayerDiscardManager.AddCard(card);
        }
        gameState.PlayerHand.Clear();
        cardsInHand.Clear();
    }

    internal void RemoveCard(CardGameobject cardGameObject)
    {
        gameState.PlayerHand.Remove(cardGameObject);
        cardsInHand.Remove(cardGameObject);
    }

    internal void DragCard(CardGameobject cardGameObject)
    {        
        draggedCardPos = cardsInHand.IndexOf(cardGameObject);
        RemoveCard(cardGameObject);        
    }

    internal void ReleaseCard(CardGameobject cardGameObject)
    {
        gameState.PlayerHand.Add(cardGameObject);
        cardsInHand.Insert(draggedCardPos, cardGameObject);        
    }

    internal void UpdatePlayables()
    {
        foreach (var card in cardsInHand)
        {
            card.IsPlayable();
        }
    }
}
