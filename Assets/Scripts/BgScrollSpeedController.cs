using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgScrollSpeedController : MonoBehaviour
{
    [SerializeField] private float scrollSpeed;
    private float scrolledSpeedMultiplier = 1f;
    private bool isScrolling = true;

    private Material scrollingMaterial;
    private float lastOffset = 0;
    private float startTime;

    private void Awake()
    {
        scrollingMaterial = GetComponent<SpriteRenderer>().material;
    }

    private void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        ScrollBackGround();
    }

    public void ScrollBackGround()
    {
        if (isScrolling)
        {
            scrolledSpeedMultiplier += Time.deltaTime * 0.01f;
            // Calculate the offset using time to ensure constant speed
            lastOffset = (Time.time - startTime) * scrollSpeed * scrolledSpeedMultiplier % 1f;
        }


        scrollingMaterial.SetVector("_ScrollSpeed", new Vector2(lastOffset , 0));
    }

    public void StopScrolling()
    {
        isScrolling = false;
    }

    public void ResumeScrolling()
    {
        isScrolling = true;
        startTime = Time.time - (lastOffset / (scrollSpeed * scrolledSpeedMultiplier));
    }


    public void SetScrollSpeed(float scrollSpeed, bool resetScrollSpeedMultiplier)
    {
        this.scrollSpeed = scrollSpeed;
        if (resetScrollSpeedMultiplier) scrolledSpeedMultiplier = 1f;
    }


}
