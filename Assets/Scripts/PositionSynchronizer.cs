using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionSynchronizer : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform ghostPlayerTransform;
    [SerializeField] private float delayTime = 0.2f;
    [SerializeField] private float lerpSpeed = 20f;
    
    Queue<float> positionBuffer = new Queue<float>();


    void FixedUpdate()
    {
        positionBuffer.Enqueue(playerTransform.position.y);

        if (positionBuffer.Count > delayTime / Time.deltaTime)
        {
            float yPosition = positionBuffer.Dequeue();
            Vector2 updatedGhostPosition = new Vector2(ghostPlayerTransform.position.x, yPosition);
            ghostPlayerTransform.position = Vector2.Lerp(ghostPlayerTransform.position, updatedGhostPosition,
                lerpSpeed * Time.deltaTime);
        }
    }
}
