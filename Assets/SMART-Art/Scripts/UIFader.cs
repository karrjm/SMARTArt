using System.Collections;
using UnityEngine;

namespace Scripts
{
    public class UIFader : MonoBehaviour
    {
        public void FadeIn(CanvasGroup canvasGroup)
        {
            StartCoroutine(FadeCanvasGroup(canvasGroup, canvasGroup.alpha, 1));
        }
        public void FadeOut(CanvasGroup canvasGroup)
        {
            StartCoroutine(FadeCanvasGroup(canvasGroup, canvasGroup.alpha, 0));
        }

        public void FadeToQuarter(CanvasGroup canvasGroup)
        {
            StartCoroutine(FadeCanvasGroup(canvasGroup, canvasGroup.alpha, 0.25f));
        }

        public void FadeToHalf(CanvasGroup canvasGroup)
        {
            StartCoroutine(FadeCanvasGroup(canvasGroup, canvasGroup.alpha, 0.5f));
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
