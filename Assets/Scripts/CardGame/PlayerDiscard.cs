using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDiscard : MonoBehaviour
{
    [SerializeField] private CoroutineQueue animQueue;
    [SerializeField] private GameState gameState;
    private List<CardGameobject> cardsInDiscard = new List<CardGameobject>();

    internal void AddCard(CardGameobject card)
    {
        gameState.PlayerDiscard.Add(card);
        card.Interactable(false);
        animQueue.EnqueueAction(MoveCardToDiscardAnimation(card));
        animQueue.EnqueueWait(0.1f);
    }

    public IEnumerator MoveCardToDiscardAnimation(CardGameobject card)
    {
        cardsInDiscard.Add(card);
        card.SetCardPositionTarget(new Vector2(transform.position.x+UnityEngine.Random.Range(-0.1f,0.1f), transform.position.y+ UnityEngine.Random.Range(-0.1f, 0.1f)));
        card.SetCardRotationTarget(Quaternion.AngleAxis(UnityEngine.Random.Range(-5f, 5f),Vector3.forward)* Quaternion.identity);
        card.HighlightPlayable(false);
        StartCoroutine(Fade(card));

        yield return null;
    }

    public IEnumerator Fade(CardGameobject card)
    {
        for (float t = 0.0f; t < 1f; t += Time.deltaTime * (1f))
        {
            card.FX.Alpha(t);

            yield return null;
        }
    }
}
