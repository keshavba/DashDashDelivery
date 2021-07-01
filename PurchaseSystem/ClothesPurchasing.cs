using UnityEngine;

public enum ObjectType
{
    Model,
    Pattern,
    Logo
};


public enum ModelType
{
    None,
    Short_Sleeved_Shirt,
    Long_Sleeved_Shirt,
    Shorts,
    Pants,
    Shoe,
    Basic_Backpack,
    Sling_Backpack
};


public class ClothesPurchasing : Interaction
{
    public ObjectType objectType;
    public ModelType modelType;
    public string productName = "";
    public float price;

    [HideInInspector] public bool isPurchased = false;

    private money_tracking_system moneyTracker;
    private CharacterLists lists;
    private Player player;
    private Interactor interactor;
    private CustomizationSearchPurchased search;

    private Texture texture;
    private GameObject model;
    private Mesh mesh;

    void Start()
    {
        moneyTracker = GameObject.FindObjectOfType<money_tracking_system>();
        lists = GameObject.FindObjectOfType<CharacterLists>();
        player = GameObject.FindObjectOfType<Player>();
        interactor = GetComponent<Interactor>();
        search = GameObject.FindObjectOfType<CustomizationSearchPurchased>();
        
        if(transform.childCount > 0)
            model = transform.GetChild(0).gameObject;
        else
            model = gameObject;
        
        if(transform.childCount > 0)
            mesh = gameObject.transform.GetChild(0).gameObject.GetComponent<MeshFilter>().sharedMesh;
        else
            mesh = GetComponent<MeshFilter>().sharedMesh;
        
        if(productName != string.Empty)
        {
            interactor.promptText = "Would you like to purchase " + productName + " for " + price.ToPrice() + " plus tax?";
        }
        else
            Debug.Log("No product name on " + this.name);

        if(objectType == ObjectType.Model)
        {
            texture = model.GetComponent<Renderer>().material.GetTexture("baseMap");
        }
        else if(objectType == ObjectType.Pattern)
        {
            texture = model.GetComponent<Renderer>().material.mainTexture;
        }
        else
        {
            texture = model.GetComponent<Renderer>().material.mainTexture;
        }
    }

    public override void OnInteractionStart(Interactor interactor)
    {
        base.OnInteractionStart(interactor);
        PurchaseItem();
    }

    private void PurchaseItem()
    {
        if(moneyTracker.currentAmount < price)
        {
            Notification.Instance.PushNotification("Unable to purchase clothing item due to lack of funds!");
            OnInteractionEnd();
        }
        else
        {
            Notification.Instance.PushNotification("Successfully purchased " + productName + " for " + (price + (price * money_tracking_system.salesTax)).ToPrice() + " including tax!");

            moneyTracker.subtractFunds(price);
            
            search.SearchPurchased(objectType, modelType, productName, texture, mesh, true);
            search.SearchPurchased(objectType, modelType, productName, texture, mesh, false);

            isPurchased = true;

            OnInteractionEnd();
        }
    }

    protected override void OnInteractionEnd()
    {
        base.OnInteractionEnd();
        Interactor.Interactable = false;
    }
}
