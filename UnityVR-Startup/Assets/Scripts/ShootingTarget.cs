using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using VRStandardAssets.Utils;
using VRStandardAssets.Common;

public class ShootingTarget : MonoBehaviour
{
    public int score = 1;
    public float destroyTimeOutDuration = 2f;
    public event Action<ShootingTarget> OnRemove;
    private Transform cameraTransfrom;
    private AudioSource audioSource;
    private VRInteractiveItem vrInteractiveItem;
    private Renderer mRenderer;
    private Collider mCollider;
    public AudioClip destroyClip;
    public GameObject destroyPrefab;
    private bool isEnding;

    private void Awake()
    {
        cameraTransfrom = Camera.main.transform;
        audioSource = GetComponent<AudioSource>();
        vrInteractiveItem = GetComponent<VRInteractiveItem>();
        mRenderer = GetComponent<Renderer>();
        mCollider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        vrInteractiveItem.OnDown += HandleDown;
    }
    private void OnDisable()
    {
        vrInteractiveItem.OnDown -= HandleDown;
    }
    private void OnDestroy()
    {
        OnRemove = null;
    }
    private void HandleDown()
    {
        StartCoroutine(OnHit());
    }
    private IEnumerator OnHit()
    {
        mRenderer.enabled = false;
        mCollider.enabled = false;
        audioSource.clip = destroyClip;
        audioSource.Play();
        SessionData.AddScore(score);
        GameObject destroyedTarget = Instantiate<GameObject>(destroyPrefab, transform.position, transform.rotation);
        Destroy(destroyedTarget, destroyTimeOutDuration);
        yield return new WaitForSeconds(destroyClip.length);
        if (OnRemove != null)
            OnRemove(this);
    }
}
