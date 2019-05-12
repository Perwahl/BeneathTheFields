using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffect : ScriptableObject
{
    [SerializeField] private bool needsTarget;
    [SerializeField] public bool NeedsTarget { get { return needsTarget; } }

    public virtual void ApplyEffect(CardGameobject card)
    {

    }

    public virtual IEnumerator EffectAnimation(CardGameobject card)
    {
        yield return null;
    }

}
