using System.Collections;
using TMPro;
using UnityEngine;

public class ActionFeedbackUI : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private TMP_Text text;

    [Header("Timing")]
    [SerializeField] private float visibleTime = 0.6f;
    [SerializeField] private float fadeTime = 0.35f;

    Coroutine routine;

    void Reset()
    {
        text = GetComponent<TMP_Text>();
    }

    void Awake()
    {
        if (text == null) text = GetComponent<TMP_Text>();
        Clear();
    }

    public void Show(string message)
    {
        if (text == null) return;

        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(ShowRoutine(message));
    }

    public void Clear()
    {
        if (text == null) return;
        text.text = "";
        var c = text.color;
        c.a = 0f;
        text.color = c;
    }

    IEnumerator ShowRoutine(string message)
    {
        text.text = message;

        // alpha 1 direct
        var c = text.color;
        c.a = 1f;
        text.color = c;

        yield return new WaitForSecondsRealtime(visibleTime);

        float t = 0f;
        while (t < fadeTime)
        {
            t += Time.unscaledDeltaTime; // marche même en pause
            float a = Mathf.Lerp(1f, 0f, t / fadeTime);
            c = text.color;
            c.a = a;
            text.color = c;
            yield return null;
        }

        Clear();
        routine = null;
    }

public bool ContainsPoint(Vector3 worldPoint)
{
    // On vérifie si le point touché par le raycast est bien dans la "case"
    // Version simple basée sur le collider.
    Collider col = GetComponent<Collider>();
    if (col == null) return false;

    // Option 1 : bounds (simple et suffisant pour un carré bien aligné)
    return col.bounds.Contains(worldPoint);
}

}

