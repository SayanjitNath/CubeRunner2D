using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ObstacleDissolve : MonoBehaviour
{
    [SerializeField] private float dissolveTime;

    private SpriteRenderer spriteRenderer;
    private Material obstacleMat;
    private Collider2D obstacleCollider;

    private int dissolveAmount = Shader.PropertyToID("_DissolveAmount");

    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        obstacleCollider = GetComponent<Collider2D>();

        obstacleCollider.enabled = true;
        spriteRenderer.enabled = true;

        obstacleMat = spriteRenderer.material;
    }

    public void Dissolve()
    {
        StartCoroutine(Vanish());
    }

    private IEnumerator Vanish()
    {
        float elaspedTime = 0f;
        while(elaspedTime < dissolveTime)
        {
            elaspedTime += Time.deltaTime;
            float lerpedDissolve = Mathf.Lerp(0f, 1.1f, (elaspedTime / dissolveTime));
            obstacleMat.SetFloat(dissolveAmount, lerpedDissolve);
            yield return null;
        }

        obstacleCollider.enabled = false;
        obstacleMat.SetFloat(dissolveAmount, 1.1f);
        spriteRenderer.enabled = false;

        obstacleMat.SetFloat(dissolveAmount, 0f);
    }
}
