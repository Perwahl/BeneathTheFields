using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour {

    [SerializeField] private ParticleSystem particles;
    [SerializeField] private SpriteRenderer hitMark;
    [SerializeField] private Vector2 force;
    [SerializeField] public Vector2 Force { get { return force; } }

    [ContextMenu("TestEffect")]
    private void TestEffect()
    {
        particles.Play();
        FadeInMark();
    }

    private void Start()
    {
        StartCoroutine(FadeInMark());
    }

    public void Clear()
    {
        StartCoroutine(FadeOutMark());
    }

    private  IEnumerator FadeInMark()
    {
        for (float i = 0.0f; i <= 1.0f; i += 0.05f)
        {

            hitMark.color = new Color(hitMark.color.r, hitMark.color.g, hitMark.color.b, i);

            yield return null;
        }

        yield return null;
    }

    private IEnumerator FadeOutMark()
    {
        yield return new WaitForEndOfFrame();
        for (float i = 0.0f; i <= 1.0f; i += 0.05f)
        {
            hitMark.color = new Color(hitMark.color.r, hitMark.color.g, hitMark.color.b, 0.9f-i);

            yield return null;
        }

        yield return null;
    }

}
