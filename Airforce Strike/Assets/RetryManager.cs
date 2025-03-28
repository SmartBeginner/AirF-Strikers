using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RetryManager : MonoBehaviour
{
    public static event Action OnGameOver;
    [SerializeField] public GameObject retryPopup;
    [SerializeField] public Button yesButton;
    [SerializeField] public Button noButton;

    private void Awake()
    {
        OnGameOver += ShowRetryPopup;
        retryPopup.SetActive(false);
    }

    private void OnDestroy()
    {
        OnGameOver -= ShowRetryPopup;
    }

    public static void TriggerGameOver()
    {
        OnGameOver?.Invoke();
    }

    private void ShowRetryPopup()
    {
        Time.timeScale = 0;
        retryPopup.SetActive(true);
        
        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();
        
        yesButton.onClick.AddListener(RestartGame);
        noButton.onClick.AddListener(QuitRetry);
    }

    private void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void QuitRetry()
    {
        Time.timeScale = 1;
        retryPopup.SetActive(false);
    }
}
