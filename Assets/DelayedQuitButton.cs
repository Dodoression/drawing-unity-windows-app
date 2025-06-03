using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;

public class DelayedQuitButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float m_quitDelay = 3.0f;

    private Coroutine m_quitCoroutine;

    public void OnPointerDown(PointerEventData eventData)
    {
        m_quitCoroutine = StartCoroutine(QuitCoroutine());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopCoroutine(m_quitCoroutine);
    }

    private IEnumerator QuitCoroutine()
    {
        yield return new WaitForSeconds(m_quitDelay);

#if UNITY_EDITOR
        Debug.Log("Quitting the application!");
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Debug.Log("Quitting the application!");
        Application.Quit();
#endif
    }
}