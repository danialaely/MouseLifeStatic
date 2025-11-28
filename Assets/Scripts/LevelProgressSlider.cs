using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LevelProgressSlider : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TMP_Text progressText;

    [Header("Settings")]
    [SerializeField] private float fillSpeed = 1f; // Higher = faster animation
    [SerializeField] private float progressPerLevel = 25f;

    private float currentProgress;

    private void Start()
    {
        // Load saved progress (if any)
        currentProgress = PlayerPrefs.GetFloat("ProgressValue", 0);
        progressSlider.value = currentProgress / 100f;
        UpdateProgressText();
    }

    public void OnLevelComplete()
    {
        float targetProgress = currentProgress + progressPerLevel;

        if (targetProgress >= 100f)
        {
            targetProgress = 100f;
            StartCoroutine(AnimateSlider(targetProgress, true)); // Reset after full
        }
        else
        {
            StartCoroutine(AnimateSlider(targetProgress, false));
        }
    }

    private IEnumerator AnimateSlider(float target, bool resetAfter)
    {
        float startValue = currentProgress;
        float elapsed = 0f;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * fillSpeed;
            float newProgress = Mathf.Lerp(startValue, target, elapsed);
            progressSlider.value = newProgress / 100f;
            progressText.text = $"{Mathf.RoundToInt(newProgress)}%";
            yield return null;
        }

        currentProgress = target;
        if (resetAfter)
        {
            yield return new WaitForSeconds(0.4f);
            currentProgress = 0f;
            progressSlider.value = 0f;
            progressText.text = "0%";
            // TODO: Unlock Reward Here
            Debug.Log("🎁 Reward Unlocked!");
        }

        PlayerPrefs.SetFloat("ProgressValue", currentProgress);
        PlayerPrefs.Save();
    }

    private void UpdateProgressText()
    {
        progressText.text = $"{Mathf.RoundToInt(currentProgress)}%";
    }
}
