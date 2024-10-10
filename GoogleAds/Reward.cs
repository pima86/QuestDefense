using GoogleMobileAds;
using GoogleMobileAds.Api;
using System;
using System.Collections;
using UnityEngine;

public class GoogleMobileAdsScript : MonoBehaviour
{
    public void Awake()
    {
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            LoadRewardedAd();
        });
    }

    //"ca-app-pub-4291604535745947/9268088802";
    string _adUnitIdReward = "ca-app-pub-3940256099942544/5224354917";
    RewardedAd _rewardedAd;

    public void LoadRewardedAd()
    {
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        var adRequest = new AdRequest();
        RewardedAd.Load(_adUnitIdReward, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                _rewardedAd = ad;
            });
    }

    float timeScale = 0;
    public void ShowRewardedAd()
    {
        timeScale = Time.timeScale;

        //튜토리얼
        if (!Json.inst.playerData.tutorialClear)
        {
            GameManager.inst.state = GameManager.State.Wave;
            Json.inst.playerData.tutorialClear = true;
            Json.inst.Save();
        }
        else if (IAPManager.Inst.HasReceipt("ad"))
            GameManager.inst.state = GameManager.State.Wave;

        //메인게임 시작
        else
        {
            if (_rewardedAd != null && _rewardedAd.CanShowAd())
            {
                _rewardedAd.Show((Reward reward) =>
                {
                    StartCoroutine(GetReward());
                });
            }
        }
    }

    IEnumerator GetReward()
    {
        yield return new WaitForSeconds(0.1f);

        // 이후 보상처리 (게임 코드)
        Time.timeScale = timeScale;
        GameManager.inst.state = GameManager.State.Wave;
    }
}
