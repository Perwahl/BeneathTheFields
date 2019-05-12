using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/CardEffects/GainBlock", order = 1)]
public class GainBlock : CardEffect
{
    [SerializeField] private int blocks;  

    public override void ApplyEffect(CardGameobject card)
    {
       
    }

    public override IEnumerator EffectAnimation(CardGameobject card)
    {       
        yield return new WaitForSeconds(0.1f);

        yield return null;
    }
}
