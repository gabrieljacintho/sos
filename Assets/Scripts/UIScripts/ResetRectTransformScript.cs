using UnityEngine;

public class ResetRectTransformScript : MonoBehaviour
{
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        ResetRectTransform();
    }

    public void ResetRectTransform()
    {
        rectTransform.anchoredPosition = Vector2.zero;
    }
}
