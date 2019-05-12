using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/CardEffects/DamageTarget", order = 1)]
public class DamageTarget : CardEffect
{
    [SerializeField] private int damageValue;
    [SerializeField] private HitEffect hitEffect;


    public override void ApplyEffect(CardGameobject card)
    {
        card.GetTarget().TakeDamage(damageValue, hitEffect);
    }

    public override IEnumerator EffectAnimation(CardGameobject card)
    {       
        yield return new WaitForSeconds(0.1f);

        yield return null;
    }
}
