using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI {

    public class AnimateText : MonoBehaviour {

        private string originalText;
        private Text textComponent;
        private int currentTextIndex;
        private float animateTime = 1.5f;

        private void Awake() {
            textComponent = GetComponent<Text>();
            originalText = textComponent.text;
        }

        private void OnEnable() {
            textComponent.text = "";
            StartCoroutine(AnimateTextEffect());
        }

        IEnumerator AnimateTextEffect() {
            float elapsedTime = 0;
            int chars = originalText.Length;
            int indexPrevFrame = -1;
            currentTextIndex = 0;

            while (elapsedTime < animateTime) {
                float normalized = (elapsedTime / animateTime);
                currentTextIndex = (int)(chars * normalized);
                if (currentTextIndex != indexPrevFrame) {
                    textComponent.text += originalText[currentTextIndex];
                    print(currentTextIndex);
                }
                elapsedTime += Time.deltaTime;
                indexPrevFrame = currentTextIndex;

                yield return null;
            }
            textComponent.text = originalText;
        }
    }
}