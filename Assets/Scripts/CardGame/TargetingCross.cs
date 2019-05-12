using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TargetingCross : MonoBehaviour
{
    internal bool isTargeting;
    [SerializeField] private SpriteRenderer brightT;
    [SerializeField] private CardGameobject card;
    private Monster target =null;
    public Monster Target { get { return target; } }
    private float targetAlpha;

    private void Update()
    {
        if (Math.Abs(brightT.color.a - targetAlpha) > 0.1f)
        {
            brightT.color = new Color(brightT.color.r, brightT.color.g, brightT.color.b, Mathf.Lerp(brightT.color.a, targetAlpha, 0.1f));
        }
    }

    [ContextMenu("StartTargeting")]
    public void StartTargeting()
    {
        card.FX.Alpha(0f);
        isTargeting = true;
        gameObject.SetActive(true);

        CardGame.HighlightMonsters(true);
    }
     
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Monster")
        {        
            target = other.GetComponent<Monster>();
            targetAlpha = 1f;          
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {       
        target = null;
        targetAlpha = 0f;
    }
       
    internal void StopTargeting()
    {
        if (target == null)
        {
              card.FX.Alpha(1f);            
        }
        isTargeting = false;
        CardGame.HighlightMonsters(false);
        gameObject.SetActive(false);
    }
}
