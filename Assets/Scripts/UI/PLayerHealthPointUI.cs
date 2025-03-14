using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PLayerHealthPointUI : MonoBehaviour
{
    [SerializeField] private Image[] playerHealthPoint;
    [SerializeField] private float healthPointDisappearTime = 0.2f;

    private float elapsedTime = 0f;
    private int currentHealthPointIndex;

    private void Start()
    {
        InitialSetUp();
    }

    public void InitialSetUp()
    {
        elapsedTime = 100f;
        currentHealthPointIndex = playerHealthPoint.Length;
        for (int i=0;i<playerHealthPoint.Length;i++)
        {
            playerHealthPoint[i].color = new Color(1f, 1f, 1f, 1f);
        }
    }

    public int GetHealthPoint() => playerHealthPoint.Length;

    private void Update()
    {
        if (elapsedTime < healthPointDisappearTime)
        {
            elapsedTime += Time.deltaTime;
            FadeOutHealthPoint();
        }
    }

    private void FadeOutHealthPoint()
    {
        float alpha = 1f;
        float interpolatedAlpha = Mathf.Lerp(alpha, 0f, (elapsedTime / healthPointDisappearTime));
        playerHealthPoint[currentHealthPointIndex].color = new Color(1f, 1f, 1f, interpolatedAlpha);
    }

    public void DecreaseHealthPoint()
    {
        currentHealthPointIndex--;
        elapsedTime = 0f;
    }
}
