using System.Collections;
using TMPro;
using UnityEngine;

public class CardFX : MonoBehaviour
{

    [SerializeField] private Sprite[] flashAnimSprites;
    [SerializeField] private SpriteMask dissolveMask;
    [SerializeField] private ParticleSystem dissolveParticles;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform cardTransform;
    [SerializeField] private CardGameobject card;


    //[SerializeField] private SpriteRenderer flashRenderer;
    //[SerializeField] private SpriteRenderer tintRenderer;

    [SerializeField] private SpriteRenderer[] sprites;
    [SerializeField] private TMP_Text[] texts;
    [SerializeField] private Outline outline;
    [SerializeField] private SpriteRenderer cardBack;
    private bool faceUp = false;
    public Color startColor;


    private Vector2 startPos = new Vector2(-3f, -2.5f);
    private Vector2 endPos = new Vector2(3f, 2.5f);
    public float flipSpeed;

    [ContextMenu("DissolveTest")]
    public void Dissolve(Color color)
    {
        StartCoroutine(DissolveObject(color));
    }

    private IEnumerator DissolveObject(Color color)
    {
        dissolveParticles.Play();
        yield return new WaitForSeconds(0.1f);
        for (float i = 0.0f; i <= 1.0f; i += 0.02f)
        {
            dissolveMask.alphaCutoff = i;
            Alpha((0.7f - i));
            Tint(new Color(color.r, color.g, color.b, Mathf.PingPong(i, 0.5f)));

            yield return null;
        }
    }

    [ContextMenu("FlashOnce")]
    public void FlashOnce()
    {
        FlashOnce(new Color(0.278f, 1f, 0.921f, 0.5f));
    }

    public void FlashOnce(Color color)
    {
        StartCoroutine(Flash(15f, color));
    }

    public void Tint(Color color)
    {
        // tintRenderer.color = color;
    }

    private IEnumerator Flash(float speed, Color color)
    {
        //tintRenderer.color = color;
        //flashRenderer.color = new Color(1f,1f,1f,0.5f);

        //for (float t = 0.0f; t < flashAnimSprites.Length; t += Time.deltaTime * (speed))
        //{
        //    flashRenderer.sprite = flashAnimSprites[(int)Mathf.Floor(t)];
        //    tintRenderer.color = new Color(tintRenderer.color.r, tintRenderer.color.g, tintRenderer.color.b, 0.1f+Mathf.PingPong(t/ flashAnimSprites.Length, 0.5f));        

        yield return null;
        //}

        //tintRenderer.color = new Color(0, 0, 0, 0);
        //flashRenderer.color = new Color(0, 0, 0, 0);

    }

    internal void TextAlpha(float alpha)
    {
        foreach (TMP_Text text in texts)
        {
            text.alpha = alpha;
        }
    }

    internal void SpriteAlpha(float alpha)
    {
        foreach (SpriteRenderer sprite in sprites)
        {
            //sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);

            startColor = sprite.material.GetColor("_Color");

            sprite.material.SetColor("_Color", new Color(startColor.r, startColor.g, startColor.b, alpha));
        }
    }

    internal void Alpha(float alpha)
    {
        TextAlpha(alpha);
        SpriteAlpha(alpha);
        outline.SetAlpha(alpha);
    }

    internal void CardbackAlpha(float alpha)
    {
        cardBack.color = new Color(cardBack.color.r, cardBack.color.g, cardBack.color.b, alpha);
    }


    public void ShakeCard(float power)
    {
        StartCoroutine(ShakeC(power));
    }

    [ContextMenu("Shake")]
    public void ShakeTest()
    {
        StartCoroutine(ShakeC(1));
    }

    private IEnumerator ShakeC(float power)
    {
        var x = 30 + UnityEngine.Random.Range(0, 20);
        var y = 30 + UnityEngine.Random.Range(0, 20);

        if (UnityEngine.Random.value > 0.5f)
        {
            x = x * -1;
        }

        if (UnityEngine.Random.value > 0.5f)
        {
            y = y * -1;
        }

        var randForce = new Vector2(x, y);

        rb.AddForce((randForce * power) / 2);
        randForce = randForce * -1;
        yield return new WaitForSeconds(0.05f);

        for (int i = 0; i < 4f; i++)
        {
            rb.AddForce(randForce * power);

            randForce = randForce * -1;

            randForce = new Vector2(randForce.x + UnityEngine.Random.Range(-20, 20), randForce.y + UnityEngine.Random.Range(-20, 20));

            yield return new WaitForSeconds(0.05f);
        }
        rb.AddForce((randForce * power) / 2);
        card.WakeCard();
    }

    [ContextMenu("Flip Card")]
    public void FlipCard()
    {
        StartCoroutine(Flip());
    }

    private IEnumerator Flip()
    {
        var t = cardTransform.localRotation * Quaternion.AngleAxis(90, Vector3.up);
        var s = Quaternion.identity;

        for (float i = 0.0f; i <= 1f; i += Time.deltaTime * flipSpeed)
        {
            cardTransform.localRotation = Quaternion.Lerp(s, t, i);
            yield return null;

        }

        faceUp = !faceUp;
        cardBack.gameObject.SetActive(!faceUp);
        t = Quaternion.identity * Quaternion.AngleAxis(-90, Vector3.up);
        cardTransform.localRotation = t;


        for (float i = 0.0f; i <= 1f; i += Time.deltaTime * flipSpeed)
        {
            cardTransform.localRotation = Quaternion.Lerp(t, s, i);
            yield return null;
        }
        cardTransform.localRotation = Quaternion.identity;
    }

}
