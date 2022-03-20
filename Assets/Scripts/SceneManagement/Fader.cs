using UnityEngine;
using System.Collections;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {


        CanvasGroup canvasGroup;
        
        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public IEnumerator FadeOut(float fadeTime)
        {
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime / fadeTime;
                yield return null;
            }
        }
        public IEnumerator FadeIn(float fadeTime)
        {
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime / fadeTime;
               yield return null;
            }
        }
        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1;
        }

    }

}