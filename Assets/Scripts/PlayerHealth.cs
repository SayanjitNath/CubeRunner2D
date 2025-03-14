using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private PLayerHealthPointUI playerHealthUI;
    private int healthPoint;

    private void Start()
    {
        InitialSetUp();
    }

    public void InitialSetUp()
    {
        healthPoint = playerHealthUI.GetHealthPoint();
    }

    public void TakeDamage()
    {
        if (healthPoint > 0)
        {
            healthPoint--;
            playerHealthUI.DecreaseHealthPoint();

            if(healthPoint == 0)
            {
                // Game Over
                GameManager.Instance.GameOver();
            }
        }
    }
}
