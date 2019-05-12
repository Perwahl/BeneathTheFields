using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerHealthGlobe : MonoBehaviour
{
    [SerializeField] private Material mat;
    [SerializeField] private GameState gameState;
    [SerializeField] private CoroutineQueue animQueue;

    [SerializeField] private TMP_Text text;
    private int displayedHealth=20;

    private float currentDisplayedHealth=0;

    // Update is called once per frame
    void Update()
    {
        if(displayedHealth > currentDisplayedHealth)
        {
            currentDisplayedHealth = Mathf.Lerp(currentDisplayedHealth, displayedHealth, 0.05f);
            UpdateHealth();
        }
        if (displayedHealth < currentDisplayedHealth)
        {
            currentDisplayedHealth = Mathf.Lerp(currentDisplayedHealth, displayedHealth, 0.05f);
            UpdateHealth();
        }
    }

    private void UpdateHealth()
    {
        mat.SetFloat("_Health", currentDisplayedHealth * 5);
        text.text = Mathf.RoundToInt(currentDisplayedHealth) + "/20";
    }

    internal void ChangePlayerHealth(int v)
    {
        gameState.PlayerHealth = gameState.PlayerHealth + v;
        animQueue.EnqueueAction(Health(v));

    }

    private IEnumerator Health(int change)
    {
        displayedHealth = displayedHealth + change;
        yield return null;
    }
}
