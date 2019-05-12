using UnityEngine;
using System.Collections;
using System;

public class Outline : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private ParticleSystem[] edges;

    internal void SetAlpha(float alpha)
    {
        var col = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
        SetColor(col);
    }

    internal void SetColor(Color color)
    {
        sprite.color = color;

        foreach (ParticleSystem edge in edges)
        {
            var part = edge.main;
            part.startColor = color;

            ParticleSystem.Particle[] pSparticles = new ParticleSystem.Particle[edge.particleCount];
            edge.GetParticles(pSparticles);
            if (pSparticles.Length > 0)
            {
                for (int a = 0; a < pSparticles.Length; a++)
                {
                    pSparticles[a].color = color;

                    //var col = pSparticles[a].GetCurrentColor(edge);
                    //col = color;

                }
                edge.SetParticles(pSparticles, pSparticles.Length);
            }
        }
    }

    internal void SetStartColor(Color color)
    {
        sprite.color = color;

        foreach (ParticleSystem edge in edges)
        {
            var part = edge.main;
            part.startColor = color;
        }
    }
}