using System.Collections;
using UnityEngine;

namespace TallahasseePrototype.Scripts
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

        private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float lerpTime = 0.5f)
        {
            var timeStartedLerping = Time.time;

            while (true)
            {
                var percentageComplete = timeStartedLerping / lerpTime;
                var currentValue = Mathf.Lerp(start, end, percentageComplete);

                cg.alpha = currentValue;

                if (percentageComplete >= 1)
                {
                    break;
                }
                
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
