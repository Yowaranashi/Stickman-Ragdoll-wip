using InstantGamesBridge.Modules.Advertisement;
using InstantGamesBridge;
using UnityEngine;

public class IntersititalAd : MonoBehaviour
{
    [SerializeField] private bool _showInterstitial = true;
    [SerializeField] private int _delaysSecondsVK = 180;
    [SerializeField] private bool _ignoreDelayInVK = false;
    public void ShowInterstitial()
    {
        if (_showInterstitial)
        {
            if (Bridge.advertisement.minimumDelayBetweenInterstitial == 0)
            {
                Bridge.advertisement.SetMinimumDelayBetweenInterstitial(
                    new SetMinimumDelayBetweenInterstitialVkOptions(_delaysSecondsVK));
            }

                Bridge.advertisement.ShowInterstitial(
                new ShowInterstitialVkOptions(_ignoreDelayInVK));

            print("SHOW INTERSTITAL AD");
        }

    }
}
