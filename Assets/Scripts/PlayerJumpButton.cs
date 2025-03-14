using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerJumpButton : MonoBehaviour
{
    [SerializeField] private AudioClip tapSound;
    private Button jumpButton;

    public static Action OnJumpButtonPressed = delegate { };

    private void Awake()
    {
        jumpButton = GetComponent<Button>();
    }

    private void Start()
    {
        jumpButton.onClick.AddListener(OnJumpButtonClicked);
    }

    private void OnJumpButtonClicked() 
    {
        SoundManager.instance.PlaySfxSound(tapSound, false, true);
        OnJumpButtonPressed?.Invoke();
    }
}
