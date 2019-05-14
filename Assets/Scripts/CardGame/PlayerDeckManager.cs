using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeckManager : MonoBehaviour
{
    [SerializeField] private CardGameobject cardPrefab;
    [SerializeField] private GameState gameState;
    [SerializeField] private CoroutineQueue animQueue;
    [SerializeField] private List<Vector2> deckPositions;
    [SerializeField] private int cardsInDeck;

    internal void DrawCard()
    {
        if (gameState.PlayerDeck.Count < 1)
        {
            Refill();
            Shuffle();            
        }
        var cardToDraw = gameState.PlayerDeck[gameState.PlayerDeck.Count - 1];
        gameState.PlayerDeck.Remove(cardToDraw);
        cardToDraw.transform.SetParent(CardGame.CardGameRef.transform);
        CardGame.PlayerHand.AddCard(cardToDraw);
        animQueue.EnqueueAction(CardLeavesHand());
    }

    private IEnumerator CardLeavesHand()
    {
        cardsInDeck--;

        yield return null;
    }

    private void Shuffle()
    {
        gameState.PlayerDeck.Shuffle(new System.Random());
        animQueue.EnqueueAction(ShuffleStarts());

        foreach (CardGameobject card in gameState.PlayerDeck)
        {
            animQueue.EnqueueAction(ShuffleAnimation(card));
        }
        animQueue.EnqueueWait(1f);

    }

    private IEnumerator ShuffleAnimation(CardGameobject card)
    {
        Vector2 pos = transform.position;
        card.SetCardPositionTarget(pos + deckPositions[cardsInDeck]);

        card.SetSortOrder((50 - cardsInDeck) * -1);
       // card.FX.ShakeCard(card, 0.5f);

        cardsInDeck++;

        yield return null;
    }


    private IEnumerator ShuffleStarts()
    {
        cardsInDeck = 0;

        yield return null;
    }

    private void Refill()
    {
        foreach (CardGameobject card in gameState.PlayerDiscard)
        {
            AddCard(card);
        }
        gameState.PlayerDiscard.Clear();
        animQueue.EnqueueWait(1f);
    }

    internal void AddCard(CardGameobject card)
    {
        gameState.PlayerDeck.Add(card);
        animQueue.EnqueueAction(MoveCardToDeckAnimation(card));
        animQueue.EnqueueWait(0.1f);
    }

    public IEnumerator MoveCardToDeckAnimation(CardGameobject card)
    {
        Vector2 pos = transform.position;
        card.SetCardPositionTarget(pos + deckPositions[cardsInDeck]);
        card.SetCardRotationTarget(Quaternion.identity);
        card.SetSortOrder((50 - cardsInDeck) * -1);
       // card.FX.FlipCard();
        cardsInDeck++;

        yield return null;
    }

    internal void DrawCard(int v)
    {
        for (int i = 0; i < v; i++)
        {
            DrawCard();
        }
    }

    internal void InitStartDeck(PlayerDeck deck)
    {
        InitDeckPositions();
        cardsInDeck = 0;
        foreach (Card card in deck.Cards)
        {
            var cardObject = Instantiate(cardPrefab, transform);
          //  cardObject.transform.position = deckPositions[cardsInDeck];
            cardObject.transform.position = transform.position;
            gameState.PlayerDeck.Add(cardObject);
            cardObject.InitCardObject(card);
            // cardObject.SetSortOrder(cardsInDeck * -1);
            cardObject.SetSortOrder((50 - cardsInDeck) * -1);

            //cardObject.InitCardObject(card, (deck.Cards.Count- cardsInDeck) *-1);
            cardsInDeck++;
        }
    }

    private void InitDeckPositions()
    {
        deckPositions = new List<Vector2>();

        for (int i = 0; i < 50; i++)
        {
            var pos = new Vector2(i * -0.03f, i * 0.03f);
            deckPositions.Add(pos);
        }
    }
}
