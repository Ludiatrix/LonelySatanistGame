using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LSG
{
    /// <summary>
    /// Fade-to-black scene transition. Put this on a full-screen black overlay
    /// (Canvas + Image + CanvasGroup) and drop one copy in every scene you switch
    /// between.
    ///
    /// - On load, the scene starts black and fades IN to clear.
    /// - FadeToScene(name/index) fades OUT to black, then loads the next scene
    ///   (which fades in via its own SceneFader).
    ///
    /// Wire a Button's OnClick to FadeToScene(string) or FadeToScene(int).
    /// The target scene must be in the build list (File > Build Profiles > Scene List).
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class SceneFader : MonoBehaviour
    {
        [Tooltip("Seconds for each fade (in and out).")]
        [SerializeField] private float fadeDuration = 0.7f;

        private CanvasGroup group;
        private bool isTransitioning;

        private void Awake()
        {
            group = GetComponent<CanvasGroup>();
            // Force a black cover at load time, regardless of the editor value,
            // so the scene always fades in cleanly.
            group.alpha = 1f;
            group.blocksRaycasts = true;
        }

        private void Start()
        {
            StartCoroutine(Fade(0f, unblockAtEnd: true));
        }

        /// <summary>Fade to black, then load the scene by name.</summary>
        public void FadeToScene(string sceneName)
        {
            if (isTransitioning)
                return;

            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError($"{nameof(SceneFader)} on '{gameObject.name}': no scene name provided.", this);
                return;
            }

            StartCoroutine(FadeOutAndLoad(sceneName, -1));
        }

        /// <summary>Fade to black, then load the scene by build index.</summary>
        public void FadeToScene(int buildIndex)
        {
            if (isTransitioning)
                return;

            StartCoroutine(FadeOutAndLoad(null, buildIndex));
        }

        private IEnumerator FadeOutAndLoad(string sceneName, int buildIndex)
        {
            isTransitioning = true;
            group.blocksRaycasts = true;

            yield return Fade(1f, unblockAtEnd: false);

            if (sceneName != null)
                SceneManager.LoadScene(sceneName);
            else
                SceneManager.LoadScene(buildIndex);
        }

        private IEnumerator Fade(float target, bool unblockAtEnd)
        {
            float start = group.alpha;
            float elapsed = 0f;

            if (fadeDuration > 0f)
            {
                while (elapsed < fadeDuration)
                {
                    elapsed += Time.unscaledDeltaTime;
                    float t = Mathf.Clamp01(elapsed / fadeDuration);
                    t = t * t * (3f - 2f * t); // smoothstep
                    group.alpha = Mathf.LerpUnclamped(start, target, t);
                    yield return null;
                }
            }

            group.alpha = target;

            // Let clicks through once the screen is clear again.
            if (unblockAtEnd)
                group.blocksRaycasts = false;
        }
    }
}
