using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimations : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private float hideAlpha;
    [SerializeField] private Vector3 SweepOffset;

    private float[] defaultAlphas;
    private Vector3 defaultPosition;
    private RectTransform rectTransform;
    private Image[] images;

    private void Awake()
    {
        if (!rectTransform) Setup();
    }

    private void Setup()
    {
        rectTransform = this.GetComponent<RectTransform>();
        List<Image> tempImages = new List<Image>();
        tempImages.AddRange(this.GetComponents<Image>());
        tempImages.AddRange(this.GetComponentsInChildren<Image>());
        images = tempImages.ToArray();

        defaultPosition = rectTransform.localPosition;
        defaultAlphas = new float[images.Length];

        for (int i = 0; i < images.Length; i++)
        {
            defaultAlphas[i] = images[i].color.a;
        }
    }

    public void ResetElement()
    {
        if (!rectTransform) Setup();
        rectTransform.localPosition = defaultPosition;

        for (int i = 0; i < images.Length; i++)
        {
            images[i].color = new Color(images[i].color.r, images[i].color.g, images[i].color.b, defaultAlphas[i]);
        }
    }

    public void TriggerMoveFromOffset()
    {
        if (!rectTransform) Setup();
        StopAllCoroutines();
        StartCoroutine(MoveFromOffset());
    }

    private IEnumerator MoveFromOffset() 
    {
        for (float i = 0; i < duration; i += Time.deltaTime)
        {
            rectTransform.localPosition = Vector3.Lerp(defaultPosition + SweepOffset, defaultPosition, i/duration);
            yield return null;
        }
        rectTransform.localPosition = defaultPosition;
    }

    public void TriggerMoveToOffset()
    {
        if (!rectTransform) Setup();
        StopAllCoroutines();
        StartCoroutine(MoveToOffset());
    }

    private IEnumerator MoveToOffset()
    {
        for (float i = 0; i < duration; i += Time.deltaTime)
        {
            rectTransform.localPosition = Vector3.Lerp(defaultPosition, defaultPosition + SweepOffset, i / duration);
            yield return null;
        } 
        rectTransform.localPosition = defaultPosition + SweepOffset;
    }

    public void TriggerBlendToHidden()
    {
        if (!rectTransform) Setup();
        StopAllCoroutines();
        StartCoroutine(BlendToHidden());
    }

    private IEnumerator BlendToHidden()
    {
        for (float i = 0; i < duration; i += Time.deltaTime)
        {
            for (int x = 0; x < images.Length; x++)
            {
                images[x].color = new Color(images[x].color.r, images[x].color.g, images[x].color.b, Mathf.Lerp(defaultAlphas[x], hideAlpha, i / duration));
            }
            yield return null;
        }
        rectTransform.localPosition = defaultPosition + SweepOffset;
    }

    public void TriggerBlendFromHidden()
    {
        if (!rectTransform) Setup();
        StopAllCoroutines();
        StartCoroutine(BlendFromHidden());
    }

    private IEnumerator BlendFromHidden()
    {
        for (float i = 0; i < duration; i += Time.deltaTime)
        {
            for (int x = 0; x < images.Length; x++)
            {
                images[x].color = new Color(images[x].color.r, images[x].color.g, images[x].color.b, Mathf.Lerp(hideAlpha, defaultAlphas[x], i / duration));
            }
            yield return null;
        }
        rectTransform.localPosition = defaultPosition + SweepOffset;
    }
}
