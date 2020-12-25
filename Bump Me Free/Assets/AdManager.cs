using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsListener
{
    string GooglePlay_id = "3894647";
    bool TestMode = true;

    string placementId = "rewardedVideo";

    GameManager gameManager;
    CrossSceneManager crossSceneManager;

    private void Start()
    {
        Advertisement.Initialize(GooglePlay_id, TestMode);
        Advertisement.AddListener(this);

        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        crossSceneManager = GameObject.FindGameObjectWithTag("PanelManager").GetComponent<CrossSceneManager>();
    }

    public void DisplayInterstitialAd()
    {
        Advertisement.Show();
    }

    public void DisplayVideoAd()
    {
        Advertisement.Show(placementId);
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            // Reward the user for watching the ad to completion.
            Debug.Log("Finished ad");
            PlayerPrefs.SetInt("Lives", 4);
            Destroy(GameObject.FindGameObjectWithTag("DeathPanel"));
            gameManager.AdFinish(0.5f);

        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
            Instantiate(crossSceneManager.adSkipped);
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");
            Instantiate(crossSceneManager.adFailed);
        }
    }

    public void OnUnityAdsReady(string placementID)
    {
        // If the ready Placement is rewarded, show the ad:
        if (placementID == placementId)
        {
            // Optional actions to take when the placement becomes ready(For example, enable the rewarded ads button)
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }

    // When the object that subscribes to ad events is destroyed, remove the listener:
    public void OnDestroy()
    {
        Advertisement.RemoveListener(this);
    }
}
