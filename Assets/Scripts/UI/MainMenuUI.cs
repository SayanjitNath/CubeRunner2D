using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button startButton, exitButton;
    [SerializeField] private AudioClip clickSound;

    private void OnEnable()
    {
        startButton.onClick.AddListener(StartGame);
        exitButton.onClick.AddListener(ExitGame);
    }

    private void StartGame()
    {
        SoundManager.instance.PlaySfxSound(clickSound, false, true);
        Hide();
        GameManager.Instance.StartGame();
    }

    private void ExitGame()
    {
        SoundManager.instance.PlaySfxSound(clickSound, false, true);
        GameManager.Instance.ExitGame();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
