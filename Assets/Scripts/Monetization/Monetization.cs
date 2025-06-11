using System.Collections;
using System.Collections.Generic;
using InstantGamesBridge;
using InstantGamesBridge.Modules.Advertisement;
using UnityEngine;

public class Monetization : MonoBehaviour
{
    public static System.Action<bool> RewardGet;
    public static System.Action RewardClose;
    private void Awake()
    {
        InitializeBridge();
    }

    private void InitializeBridge()
    {
        Debug.Log("In InitializeBridge()");    
        ShowAd();
    }

    public static bool ShowRewardedAd()
    {
        return Reward();
    }

    public static bool ShowAd()
    {
        return Ad();
    }
    private static bool Ad()
    {
        Debug.Log("In Ad()");
        bool isSuccess = false;

        /* -- -- -- Interstitial -- -- -- */
        // Необязательный параметр, игнорировать ли минимальную задержку
        bool ignoreDelay = false; // По умолчанию = false

        // Одинаково для всех платформ
        Bridge.advertisement.ShowInterstitial(
            ignoreDelay,
            success =>
            {
                if (success)
                {
                    // Success
                    Debug.Log("Bridge.advertisement.ShowInterstitial success: " + success);
                    isSuccess = true;
                }
                else
                {
                    // Error
                    Debug.Log("Bridge.advertisement.ShowInterstitial success: " + success);
                }
            });
        return isSuccess;
    }

    private static bool Reward()
    {
        Debug.Log("In Reward()");

        bool isSuccess = false;
        Bridge.advertisement.ShowRewarded(success =>
        {
            if (success)
            {
                // Success
                Debug.Log("Bridge.advertisement.ShowRewarded success: " + success);
                isSuccess = true;
            }
            else
            {
                // Error
                Debug.Log("Bridge.advertisement.ShowRewarded success: " + success);
            }
        });

        Bridge.advertisement.interstitialStateChanged += state => { Debug.Log($"Interstitial state: {state}"); };
        Bridge.advertisement.rewardedStateChanged += state => {
            if (state == RewardedState.Rewarded)
                RewardGet?.Invoke(true);
            else if (state == RewardedState.Closed)
                RewardClose?.Invoke();
            Debug.Log($"Rewarded state: {state}"); 
        };

        return isSuccess;
    }
}