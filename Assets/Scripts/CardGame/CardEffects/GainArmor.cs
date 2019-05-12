using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/CardEffects/GainArmor", order = 1)]
public class GainArmor : CardEffect
{
    [SerializeField] private int armorValue;   
    
    public override void ApplyEffect(CardGameobject card)
    {
       // CardGame.PlayerEffectZone.UpdateEffect(PlayerEffectZone.PlayerEffect.Armor, armorValue);
    }

    public override IEnumerator EffectAnimation(CardGameobject card)
    {
        

        yield return new WaitForSeconds(0.1f);

        yield return null;
    }
}
