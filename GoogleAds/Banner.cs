using UnityEngine;
using GoogleMobileAds.Api;

public class GoogleMobileAdsBanner : MonoBehaviour
{
    string _adUnitId = "ca-app-pub-3940256099942544/6300978111";//"ca-app-pub-4291604535745947/1827151867";
    BannerView _bannerView;

    private void Start()
    {
        LoadAd();
    }

    public void LoadAd()
    {
        if (_bannerView == null)
            CreateBannerView();

        var adRequest = new AdRequest();
        _bannerView.LoadAd(adRequest);
    }
    
    public void CreateBannerView()
    {
        if (_bannerView != null)
            DestroyAd();

        _bannerView = new BannerView(_adUnitId, AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth), AdPosition.Bottom);
    }

    public void DestroyAd()
    {
        if (_bannerView != null)
        {
            _bannerView.Destroy();
            _bannerView = null;
        }
    }
}
