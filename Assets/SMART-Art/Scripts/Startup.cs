using System.Collections;
using UnityEngine;

namespace Scripts
{
    public class Startup : MonoBehaviour
    {
        private CanvasRenderer _cRenderer;
        private void Awake()
        {
            // _cRenderer=FindObjectOfType<>()
        }

        private void Start()
        {
            TutorialFade(_cRenderer,_cRenderer.GetAlpha(),0);
        }

        private IEnumerator TutorialFade(CanvasRenderer cRenderer, float start, float end, float duration = 1f)
        {
            var elapsedTime = 0f;
            while (elapsedTime <= duration)
            {
                elapsedTime += Time.deltaTime;
                var lerpingAlpha=Mathf.Lerp(start, end, elapsedTime / duration);
                cRenderer.SetAlpha(lerpingAlpha);
                yield return null;
            }

            cRenderer.SetAlpha(end);
        }
    }
}