using UnityEngine;
using System.Collections;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {


        CanvasGroup canvasGroup;
        Coroutine currentActiveFade = null;
        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public Coroutine FadeOut(float fadeTime)
        {
            return Fade(1, fadeTime);
        }
        public Coroutine FadeIn(float fadeTime)
        {
           return Fade(0, fadeTime);
        }
        public Coroutine Fade(float alphaTarget, float fadeTime)
        {
            if (currentActiveFade != null)
            {
                StopCoroutine(currentActiveFade);
            }
            currentActiveFade = StartCoroutine(FadeRoutine(alphaTarget, fadeTime));
            return currentActiveFade;
        }

        private IEnumerator FadeRoutine(float alphaTarget, float fadeTime)
        {
            while (!Mathf.Approximately(canvasGroup.alpha, alphaTarget))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, alphaTarget, Time.deltaTime / fadeTime);
                yield return null;
            }
        }

        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1;
        }

    }

}