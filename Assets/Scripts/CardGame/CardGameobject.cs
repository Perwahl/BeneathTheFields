using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using TMPro;

public class CardGameobject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private CoroutineQueue animQueue;
    [SerializeField] private GameState gameState;

    [SerializeField] private Card card;
    [SerializeField] public Card Card { get { return card; } }

    private Quaternion startRotation;
    private Quaternion targetRotation;
    private Vector3 targetPosition;
    private float startSize;
    private float targetSize;

    private float dragDistance;

    [SerializeField] private float speed = 0.1f;
    [SerializeField] private float distance;

    [SerializeField] private bool draggable;
    [SerializeField] private Rigidbody rb;

    private Vector2 dragStartPoint;
    private static bool beingDragged;
    [SerializeField] private SortingGroup sortGroup;
    [SerializeField] private TMP_Text cardName;
    [SerializeField] private TMP_Text cardDescription;
    [SerializeField] private Outline outline;
    [SerializeField] private CardFX fx;
    [SerializeField] public CardFX FX { get { return fx; } }
    [SerializeField] private TargetingCross targetCross;
    [SerializeField] private SpriteRenderer cardImage;
    [SerializeField] private GameObject cardBack;
    [SerializeField] private BoxCollider col;

    private bool isPlayable = false;
    private bool interactable = false;
    private bool zoomedIn = false;
    private bool sleeping = true;
    private bool readyToPlay = false;

    private void Update()
    {
        if (sleeping)
        {
            return;
        }

        var sleepcounter = 0;
        Vector3 finalTargetPosition = targetPosition;
        Quaternion finalTargetRotation = targetRotation;

        if (zoomedIn && !beingDragged && card.Type != Card.CardType.dungeon)
        {
            finalTargetPosition = targetPosition + transform.up+(transform.forward*-1f);
            finalTargetRotation = CardGame.CardGameRef.transform.rotation;
        }

        distance = Vector3.Distance(transform.position, finalTargetPosition);

        if (!beingDragged)
        {
            var force = finalTargetPosition - new Vector3(transform.position.x, transform.position.y, transform.position.z);

            rb.AddForce(force * 5.0f);          
        }
        else if (beingDragged)
        {
            transform.position = Vector3.Lerp(transform.position, finalTargetPosition, 0.5f);
        }
        else if (rb.velocity.magnitude < 0.01f)
        {
            sleepcounter++;
        }

        if (Mathf.Abs(transform.localScale.x - targetSize) > 0.01f)
        {
            var size = Mathf.Lerp(transform.localScale.x, targetSize, 0.1f);
            transform.localScale = new Vector3(size, size, 1.0f);
        }
        else
        {
            sleepcounter++;
        }

        if (Quaternion.Angle(transform.rotation, finalTargetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, finalTargetRotation, 1.0f);
        }
        else
        {
            sleepcounter++;
        }

        if (sleepcounter == 3)
        {
            sleeping = true;
        }
    }

    internal void IsPlayable()
    {
        //var monsterous = CardGame.MonsterZone.GetMonsters();
        //isPlayable = false;

        if (card.Effects.Length > 0)
        {
            isPlayable = true;
        }

        //if (card.NeedsTarget() && monsterous.Count < 1)
        //{
        //    isPlayable = false;
        //}

        HighlightPlayable(isPlayable);
    }

    public void SetSortOrder(int sort)
    {
        if (!zoomedIn)
        {
            sortGroup.sortingOrder = sort;
        }
    }

    internal void Interactable(bool interacable)
    {
        col.enabled = interacable;
    }

    public void HighlightPlayable(bool playable)
    {
        outline.gameObject.SetActive(playable);
        outline.SetColor(new Color(0.1f, 1f, 0.05f));
    }

    public Monster GetTarget()
    {
        return targetCross.Target;
    }

    internal void PlayCard()
    {
        foreach (CardEffect effect in card.Effects)
        {
            animQueue.EnqueueAction(effect.EffectAnimation(this));
            effect.ApplyEffect(this);
        }
        AfterPlay();
    }

    private void AfterPlay()
    {
        if (card.Type == Card.CardType.dungeon)
        {
            animQueue.EnqueueAction(DestroyCard());
        }
        else
        {
            outline.gameObject.SetActive(false);
            CardGame.PlayerDiscardManager.AddCard(this);
            Interactable(false);
            BringToFront(false);
        }
    }

    private IEnumerator DestroyCard()
    {
        //fx.Dissolve(new Color(0.13f, 0.945f, 0.882f));
        Invoke("ClearGameObject", 3f);

        yield return null;
    }

    private void ClearGameObject()
    {
        Destroy(gameObject);
    }

    internal void WakeCard()
    {        
        sleeping = false;
    }

    internal void SetCardRotationTarget(Quaternion v)
    {
        targetRotation = v;        
        sleeping = false;
    }

    public void SetCardPositionTarget(Vector3 pos)
    {
        targetPosition = pos;
        sleeping = false;
    }

    public void SetCardSizeTarget(float pos)
    {
        targetSize = pos;
        sleeping = false;
    }

    internal void InitCardObject(Card cardToDraw)
    {
        rb = GetComponent<Rigidbody>();
        card = cardToDraw;
        targetPosition = transform.position;
        startSize = transform.localScale.x;
        SetCardSizeTarget(startSize);
        startRotation = CardGame.CardGameRef.transform.rotation;
        targetRotation = startRotation;
        sortGroup.sortingOrder = 1;
        cardName.text = card.CardName;
        cardDescription.text = card.Description;
        cardImage.sprite = card.CardImage;
        Interactable(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        beingDragged = true;
        CardGame.PlayerHand.DragCard(this);
        dragStartPoint = transform.position;
        SetCardRotationTarget(startRotation);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //SetCardPositionTarget(CardGame.PlayerCam.ScreenToWorldPoint(eventData.position));
        SetCardPositionTarget(GetWorldPositionOnPlane(eventData.position, 0f));
        dragDistance = Vector2.Distance(transform.position, dragStartPoint);

        if (dragDistance > 2.0f && !readyToPlay && isPlayable)
        {
            readyToPlay = true;
            if (card.NeedsTarget())
            {
                targetCross.StartTargeting();
            }
            else
            {
                outline.SetColor(new Color(0.768f, 0.952f, 0.949f));
            }
        }
        else if (dragDistance < 2.0f)
        {
            readyToPlay = false;

            if (card.NeedsTarget())
            {
                targetCross.StopTargeting();
            }
            else
            {
                outline.SetColor(new Color(0.1f, 1f, 0.05f));
            }
        }
    }

    public Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z)
    {
        Ray ray = CardGame.PlayerCam.ScreenPointToRay(screenPosition);
       // Plane xy = new Plane(CardGame.CardGameRef.transform.forward, new Vector3(0, 0, z));
        Plane xy = new Plane(CardGame.CardGameRef.transform.forward,transform.position);
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        beingDragged = false;
        if (dragDistance > 2.0f && isPlayable)
        {
            if (targetCross.isTargeting && targetCross.Target == null)
            {
                CardGame.PlayerHand.ReleaseCard(this);
                BringToFront(false);
                SetCardPositionTarget(dragStartPoint);
                targetCross.StopTargeting();
            }
            else
            {
                targetCross.StopTargeting();
                PlayCard();
            }
        }
        else
        {
            BringToFront(false);
            CardGame.PlayerHand.ReleaseCard(this);
            targetCross.StopTargeting();

            SetCardPositionTarget(dragStartPoint);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!beingDragged)
        {
            BringToFront(true);
        }
    }

    internal void BringToFront(bool front)
    {
        if (front)
        {
           // SetCardSizeTarget(startSize * 1.5f);
            sortGroup.sortingOrder = 20;
            zoomedIn = true;
        }
        else
        {
           // SetCardSizeTarget(targetSize = startSize);
            sortGroup.sortingOrder = 1;
            zoomedIn = false;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!beingDragged)
        {
            BringToFront(false);
        }
    }
}
