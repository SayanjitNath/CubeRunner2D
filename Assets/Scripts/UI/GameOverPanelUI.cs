using UnityEngine;
using UnityEngine.UI;

public class GameOverPanelUI : MonoBehaviour
{
    [SerializeField] private Button replayButton, mainMenuButton;
    [SerializeField] private AudioClip clickSound;

    private void OnEnable()
    {
        replayButton.onClick.AddListener(OnReplayButtonCLicked);
        mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
    }

    private void Start()
    {
        HidePanel();
    }

    private void HidePanel()
    {
        gameObject.SetActive(false);
    }

    private void OnReplayButtonCLicked()
    {
        SoundManager.instance.PlaySfxSound(clickSound, false, true);
        HidePanel();
        GameManager.Instance.StartGame();
    }

    private void OnMainMenuButtonClicked()
    {
        SoundManager.instance.PlaySfxSound(clickSound, false, true);
        GameManager.Instance.MainMenu();
    }
}
