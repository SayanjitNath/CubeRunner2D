using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class PlayerInteractionController : MonoBehaviour
{
    [Header("Orbz Hit effect Params")]
    [SerializeField] private GameObject orbzCollectionParticle;

    [Header("Obstacle Hit Effect Params")]
    [SerializeField] private GameObject shockWavePrefab;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float duration = 0.5f; 
    [SerializeField] private float strength = 0.5f; 
    [SerializeField] private int vibrato = 10;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip orbCollectClip;
    [SerializeField] private AudioClip obstacleCollisionClip;

    private PlayerHealth playerHealth;


    private void OnEnable()
    {
        playerHealth = GetComponent<PlayerHealth>();    
    }

    private async void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Orbz"))
        {
            SoundManager.instance.PlaySfxSound(orbCollectClip, false, true);
            GameManager.Instance.UpdateScore(2);
            await DestroyOrbz(collision);
        }
    }

    async Task DestroyOrbz(Collider2D collision)
    {
        GameObject collectingParticle = ObjectPoolManager.SpawnObject(orbzCollectionParticle, 
            collision.transform.position, Quaternion.identity, ObjectPoolManager.PoolType.ParticleSystem);

        collision.gameObject.SetActive(false);

        await Task.Delay(700); 

        ObjectPoolManager.ReturnObjectToPool(collectingParticle);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Obstacle")
        {
            SoundManager.instance.PlaySfxSound(obstacleCollisionClip, false, true);
            collision.transform.GetComponent<ObstacleDissolve>().Dissolve();
            StartCoroutine(GamePauseCoroutine());
        }
    }

    IEnumerator GamePauseCoroutine()
    {
        GameObject shockWave = ObjectPoolManager.SpawnObject(shockWavePrefab, transform);
        GameManager.Instance.PauseGame();

        yield return new WaitForSeconds(0.75f);
        ObjectPoolManager.ReturnObjectToPool(shockWave);

        yield return new WaitForSeconds(0.45f);
        playerHealth.TakeDamage();

        yield return new WaitForSeconds(0.4f);

        if (!GameManager.Instance.IsGameOver())
        {
            GameManager.Instance.PlayGame();
        }
    }

    public void ShakeCamera()
    {
        if (cameraTransform != null)
        {
            cameraTransform.DOShakePosition(duration, new Vector3(strength, 0, 0), vibrato);
        }
    }
}
