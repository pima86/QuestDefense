using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour, IStoreListener
{
    public static IAPManager Inst;
    void Awake() => Inst = this;

    IStoreController storeController;
    [SerializeField] GPGSManager gpgs;
    [SerializeField] GameObject adButton;
    [SerializeField] GameObject adIcon;

    void Start()
    {
        InitIAP();
    }

    public void InitIAP()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct("ad", ProductType.NonConsumable);

        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        storeController = controller;
        adButton.SetActive(!HasReceipt("ad"));
        adIcon.SetActive(!HasReceipt("ad"));
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("초기화 실패 : " + error);

    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log("초기화 실패 : " + error + message);

    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log("구매 실패");

    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        var product = purchaseEvent.purchasedProduct;

        Debug.Log("구매 성공 : " + product.definition.id);

        switch (product.definition.id)
        {
            case "ad":
                BuyAdsBlocking();
                break;
        }

        return PurchaseProcessingResult.Complete;
    }

    public void Puchase(string productID)
    {
        storeController.InitiatePurchase(productID);
    }

    public bool HasReceipt(string id)
    {
        var product = storeController.products.WithID(id);
        if (product != null)
            return product.hasReceipt;
        return false;
    }

    public void BuyAdsBlocking()
    {
        GameManager.inst.BuyAdsBlocking = true;
        adButton.SetActive(false);
        adIcon.SetActive(false);

        gpgs.SaveData();
        gpgs.LoadData();
    }
}
