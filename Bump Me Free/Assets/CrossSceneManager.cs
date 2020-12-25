using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class CrossSceneManager : MonoBehaviour
{
    public GameObject deathPanel;
    public GameObject livesPanel;
    public GameObject adSkipped;
    public GameObject adFailed;
    public GameObject levelTransition;
    public GameObject transitionCover;

    public GameObject[] explosion;

    public AudioClip[] explosionSFX;
    public AudioClip[] jumpSFX;
    public AudioClip[] bounceSFX;

    public GameObject[] skins;

    public PostProcessProfile postProcessing;
    public PostProcessResources postProcessResources;

    CameraScreenResolution camScreenRes;

    public SpriteRenderer targetSize;
    public SpriteRenderer targetSize16_9;

    public Font jumpFont;

    public GameObject endlessLosePanel;

    GameManager game;

    private void Start()
    {
        game = FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }

    /// <summary>
    /// Instantiates the explosion particles.
    /// </summary>
    public void Explosion()
    {
        for (int i = 0; i < explosion.Length; i++)
        {
            Instantiate(explosion[i], game.player.transform.position, Quaternion.identity, game.canvas);
        }
    }

    /// <summary>
    /// Plays the explosion sound.
    /// </summary>
    public void PlayExplosionSound()
    {
        int i = Random.Range(0, explosionSFX.Length);
        GetComponent<AudioSource>().PlayOneShot(explosionSFX[i]);
    }

    /// <summary>
    /// Plays the jump sound.
    /// </summary>
    public void PlayJumpSound()
    {
        int i = Random.Range(0, jumpSFX.Length);
        GetComponent<AudioSource>().PlayOneShot(jumpSFX[i]);
    }

    /// <summary>
    /// Plays the bounce sound.
    /// </summary>
    public void PlayBounceSound()
    {
        int i = Random.Range(0, bounceSFX.Length);
        GetComponent<AudioSource>().PlayOneShot(bounceSFX[i]);
    }

    /// <summary>
    /// Adds post processing to the main camera.
    /// </summary>
    public void AddPostProcessing()
    {
        PostProcessLayer layer = Camera.main.gameObject.AddComponent<PostProcessLayer>();
        layer.Init(postProcessResources);
        layer.volumeLayer = LayerMask.GetMask("Post-Processing");

        PostProcessVolume volume = FindObjectOfType<PostProcessVolume>();

        if (!volume)
            volume = new GameObject("PostProcessVolume").AddComponent<PostProcessVolume>();

        volume.profile = postProcessing;
        volume.isGlobal = true;
        volume.gameObject.layer = LayerMask.NameToLayer("Post-Processing");

    }

    /// <summary>
    /// Adjusts the camera.
    /// </summary>
    public void AdjustCamera()
    {
        camScreenRes = Camera.main.gameObject.AddComponent<CameraScreenResolution>();
        camScreenRes.maintainWidth = true;
        camScreenRes.adaptPosition = 1;
    }
}