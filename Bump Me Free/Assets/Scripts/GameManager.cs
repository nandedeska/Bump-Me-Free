using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text jumpText;
    public GameObject touchButton;
    Text livesText;
    public GameObject deathPanel;
    public GameObject livesPanel;
    public Transform canvas;
    [HideInInspector] public CrossSceneManager crossSceneManager;
    public bool disablePanelManager;
    Button adButton;
    Button rewindButton;
    [HideInInspector] public GameObject player;
    GameObject skin;
    AdManager adManager;

    GameObject levelTransition;
    GameObject transitionCover;

    EndlessManager endlessManager;

    [HideInInspector] public bool finished;

    #region Start

    private void Start()
    {
        #region References

        canvas = GameObject.FindObjectOfType<Canvas>().transform;
        if (disablePanelManager) return;
        crossSceneManager = GameObject.FindGameObjectWithTag("PanelManager").GetComponent<CrossSceneManager>();
        crossSceneManager.AddPostProcessing();
        deathPanel = crossSceneManager.deathPanel;
        livesPanel = crossSceneManager.livesPanel;
        livesText = livesPanel.transform.GetComponentInChildren<Text>();
        jumpText.font = crossSceneManager.jumpFont;
        endlessManager = FindObjectOfType<EndlessManager>();

        #endregion

        #region Death

        if (PlayerPrefs.GetInt("Lives", 4) < 0)
        {
            // Stop time
            Time.timeScale = 0;

            // Modify the death panel's transform values
            GameObject _deathPanel = Instantiate(deathPanel, canvas, true);
            _deathPanel.transform.localPosition = Vector3.zero;
            _deathPanel.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            // Get buttons from death panel
            adButton = _deathPanel.transform.GetChild(6).GetComponent<Button>();
            rewindButton = _deathPanel.transform.GetChild(7).GetComponent<Button>();

            // Assign function to buttons
            rewindButton.onClick.AddListener(Rewind);
            adButton.onClick.AddListener(WatchAd);
        }

        #endregion

        player = GameObject.FindGameObjectWithTag("Player");
        skin = Instantiate(crossSceneManager.skins[PlayerPrefs.GetInt("Skin", 0)]);
        adManager = FindObjectOfType<AdManager>().GetComponent<AdManager>();

        levelTransition = Instantiate(crossSceneManager.levelTransition, canvas);
        transitionCover = Instantiate(crossSceneManager.transitionCover, canvas);

        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            levelTransition.transform.localScale = new Vector3(3f, 3f, 1);
        }
        else
        {
            levelTransition.transform.localScale = new Vector3(1.5f, 1.5f, 1);
        }
        transitionCover.transform.localScale = new Vector3(3f, 3f, 1);
        transitionCover.SetActive(false);
        //levelTransition.SetActive(false); 
        levelTransition.transform.GetChild(1).GetComponent<Text>().text = SceneManager.GetActiveScene().buildIndex.ToString();

        if (PlayerPrefs.GetInt("FadeStart", 0) == 0)
        {
            StartCoroutine(FadeOut(levelTransition.transform.GetChild(0).GetComponent<RawImage>()));
            StartCoroutine(TextFadeOut(levelTransition.GetComponentInChildren<Text>()));
            PlayerPrefs.SetInt("FadeStart", 1);
        }
        else
        {
            levelTransition.gameObject.SetActive(false);
            transitionCover.gameObject.SetActive(false);
        }

        Color bgColor; 
        ColorUtility.TryParseHtmlString("#1F1829", out bgColor);
        Camera.main.backgroundColor = bgColor;
    }

    #endregion

    #region Update

    private void Update()
    {
        if (disablePanelManager) return;
        livesText.text = PlayerPrefs.GetInt("Lives", 4).ToString();
    }

    #endregion

    #region Die

    public void Die(float delay, int index)
    {
        switch (index)
        {
            case 0:
                StartCoroutine(DieRoutine(delay));
                break;

            case 1:
                StartCoroutine(Die_Endless(delay));
                break;
        }
    }

    public IEnumerator Die_Endless(float delay)
    {
        Destroy(player);
        Destroy(skin);
        crossSceneManager.Explosion();
        endlessManager.dead = true;
        if (PlayerPrefs.GetInt("Muted", 0) == 0)
        {
            crossSceneManager.PlayExplosionSound();
        }

        yield return new WaitForSeconds(delay);

        PlayerPrefs.SetInt("High Score", Mathf.FloorToInt(endlessManager.score));
        var panel = endlessManager.endlessLosePanel;
        panel.SetActive(true);
        panel.transform.GetChild(2).GetComponent<Text>().text = "Score: " + Mathf.FloorToInt(endlessManager.score);
        panel.transform.GetChild(3).GetComponent<Text>().text = "High Score: " + PlayerPrefs.GetInt("High Score", 0);
    }

    public void JumpDie(float delay)
    {
        StartCoroutine(Restart(delay));
    }

    IEnumerator DieRoutine(float delay)
    {
        Destroy(player);
        Destroy(skin);
        crossSceneManager.Explosion();
        if (PlayerPrefs.GetInt("Muted", 0) == 0)
        {
            crossSceneManager.PlayExplosionSound();
        }
        // Delay the coroutine
        //player.GetComponent<Player>().movement = false;  
        yield return new WaitForSeconds(delay);

        // Prevents losing lives before level 7
        if (SceneManager.GetActiveScene().buildIndex > 6 && SceneManager.GetActiveScene().name != "Endless")
        {
            // Decrement player's lives
            PlayerPrefs.SetInt("Lives", PlayerPrefs.GetInt("Lives", 4) - 1);

            // Load lives panel
            GameObject _livesPanel = Instantiate(livesPanel, canvas, true);
            _livesPanel.transform.localPosition = Vector3.zero;
            _livesPanel.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            livesText.text = PlayerPrefs.GetInt("Lives", 4).ToString();
        }

        // Reload the scene
        StartCoroutine(Restart(delay));
    }

    #endregion

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    #region Utilities

    public void Finish()
    {
        finished = true;    
        StartCoroutine(TransitionToNextScene(2f));
        PlayerPrefs.SetInt("FadeStart", 0);
    }

    /// <summary>
    /// Loads the next scene with a fade-in and fade out transition.
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    IEnumerator TransitionToNextScene(float delay)
    {
        yield return new WaitForSeconds(delay * 0.4f);
        transitionCover.SetActive(true);
        StartCoroutine(FadeIn(transitionCover.GetComponent<RawImage>()));
        yield return new WaitForSeconds(delay * 0.6f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void AdFinish(float delay)
    {
        StartCoroutine(Restart(delay));
    }

    IEnumerator Restart(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (finished) yield break;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Rewind()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 3);
        PlayerPrefs.SetInt("Lives", 4);
        PlayerPrefs.SetInt("Level", SceneManager.GetActiveScene().buildIndex - 3);
    }

    void WatchAd()
    {
        Time.timeScale = 1;
        adManager.DisplayVideoAd();
    }

    #endregion

    #region Transitions

    /// <summary>
    /// Decreases the alpha value of the image's RGBA.
    /// </summary>
    /// <param name="cover"></param>
    IEnumerator FadeOut(RawImage cover)
    {
        cover.color = new Color(cover.color.r / 255, cover.color.g / 255, cover.color.b / 255, 1);
        for (float i = 1.5f; i >= 0; i -= Time.deltaTime)
        {
            cover.color = new Color(cover.color.r / 255, cover.color.g / 255, cover.color.b / 255, i);  
            yield return null;
        }
    }

    /// <summary>
    /// Increases the alpha value of the image's RBGA.
    /// </summary>
    /// <param name="cover"></param>
    IEnumerator FadeIn(RawImage cover)
    {
        cover.color = new Color(cover.color.r / 255, cover.color.g / 255, cover.color.b / 255, 0);
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            cover.color = new Color(cover.color.r / 255, cover.color.g / 255, cover.color.b / 255, i);
            yield return null;
        }
    }

    IEnumerator TextFadeOut(Text text)
    {
        text.color = new Color(1, 1, 1, 1);
        for (float i = 1;  i >= 0; i -= Time.deltaTime)
        {
            text.color = new Color(1, 1, 1, i);
            yield return null;
        }
    }

    #endregion
}