using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class EndTurnButton : MonoBehaviour
{
    [SerializeField] private Rigidbody2D bigGear;
    [SerializeField] private Rigidbody2D smallGear1;
    [SerializeField] private Rigidbody2D smallGear2;
    [SerializeField] private Collider2D col;
    [SerializeField] private CoroutineQueue animQueue;
    [SerializeField] private TMP_Text[] texts;
    [SerializeField] private GameState gameState;

    private float rotationSpeed;
    private float zRot;
    private float distanceFromZero;

    bool isEnabled = true;

    public void EndTurn()
    {
        col.enabled = false;
        isEnabled = false;
        foreach (TMP_Text text in texts)
        {
            text.outlineWidth = 0f;
        }

        StartCoroutine(EndTurnAnimation());
                
        CardGame.PlayerHand.DiscardHand();

        CardGame.PlayerDeckManager.DrawCard(5);              

        animQueue.EnqueueAction(EnableEndTurnButton());
    }

    public IEnumerator EnableEndTurnButton()
    {
        col.enabled = true;
        isEnabled = true;

        yield return null;
    }

    public IEnumerator EndTurnAnimation()
    {
        bigGear.AddTorque(-1850);

        yield return null;
    }

    private void Update()
    {
        rotationSpeed = Mathf.Abs(bigGear.angularVelocity);
        zRot = bigGear.transform.rotation.eulerAngles.z;
        distanceFromZero = zRot > 180 ? zRot - 360 : zRot;


        if (zRot < 350 && zRot > 3 && rotationSpeed < 100f)
        {
            var acc = -1f;
            bigGear.AddTorque(acc);
        }

        else if (rotationSpeed < 80f && rotationSpeed > 0.2f)
        {
            bigGear.AddTorque((distanceFromZero) * -1f);
        }

        smallGear1.angularVelocity = bigGear.angularVelocity * -1;
        smallGear2.angularVelocity = bigGear.angularVelocity;

       // TextHighlight();
    }

    private void TextHighlight()
    {
        if (isEnabled)
        {
            foreach (TMP_Text text in texts)
            {
                text.outlineWidth = Mathf.PingPong(Time.time, 0.5f);
            }
        }
    }

    private void OnMouseUp()
    {
        EndTurn();
    }

}
