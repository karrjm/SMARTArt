using System.Collections;
using UnityEngine;

namespace Scripts.Utilities
{
    public class UIFader : MonoBehaviour
    {
        [SerializeField] [Range(0.0f, 1f)] private float alpha;

        public void FadeIn(CanvasGroup canvasGroup)
        {
            StartCoroutine(FadeCanvasGroup(canvasGroup, canvasGroup.alpha, 1));
        }

        public void FadeOut(CanvasGroup canvasGroup)
        {
            StartCoroutine(FadeCanvasGroup(canvasGroup, canvasGroup.alpha, alpha));
        }

        private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration = 0.1f)
        {
            var elapsedTime = 0f;
            while (elapsedTime <= duration)
            {
                elapsedTime += Time.deltaTime;
                cg.alpha = Mathf.Lerp(start, end, elapsedTime / duration);
                yield return null;
            }

            cg.alpha = end;
        }
    }
}