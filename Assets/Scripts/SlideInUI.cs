using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SlideInUI : MonoBehaviour
{
    public Vector2 startPosition = new Vector2(-1920f, 0);
    public Vector2 endPosition = new Vector2(0f, 0);
    public float slideDuration = 1f;
    public float delayBeforeStart = 0.5f;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private float elapsedTime = 0f;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        // Si no tiene CanvasGroup, se lo agregamos
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    void Start()
    {
        rectTransform.anchoredPosition = startPosition;
        canvasGroup.alpha = 0f;
        StartCoroutine(SlideAndFadeIn());
    }

    IEnumerator SlideAndFadeIn()
    {
        yield return new WaitForSeconds(delayBeforeStart);

        while (elapsedTime < slideDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / slideDuration);
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            // Posición
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, smoothT);

            // Opacidad
            canvasGroup.alpha = smoothT;

            yield return null;
        }

        // Asegura posición final y opacidad completa
        rectTransform.anchoredPosition = endPosition;
        canvasGroup.alpha = 1f;
    }
}
