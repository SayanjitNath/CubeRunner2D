using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveController : MonoBehaviour
{
    [SerializeField] private float shockWaveTime = 0.75f;

    private Material shockWavematerial;
    private int waveDistFromCenter = Shader.PropertyToID("_WaveDistanceFromCenter");
    private int xSizeRatio = Shader.PropertyToID("_XSizeRatio");

    private void OnEnable()
    {
        shockWavematerial = GetComponent<SpriteRenderer>().material;
        shockWavematerial.SetFloat(xSizeRatio, Camera.main.aspect);
        StartShockWave();
    }

    public void StartShockWave()
    {
        StartCoroutine(ShockWaveAction(-0.1f, 1f));
    }

    IEnumerator ShockWaveAction(float startPos, float endPos)
    {
        shockWavematerial.SetFloat(waveDistFromCenter, startPos);

        float lerpedPos = 0f;
        float elapsedTime = 0f;

        while(elapsedTime < shockWaveTime) 
        { 
            elapsedTime += Time.deltaTime;

            lerpedPos = Mathf.Lerp(startPos, endPos, (elapsedTime / shockWaveTime));
            shockWavematerial.SetFloat(waveDistFromCenter, lerpedPos);
            yield return null;
        }
    }

}

