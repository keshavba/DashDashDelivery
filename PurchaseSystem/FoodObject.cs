using System.Collections;
using UnityEngine;

public class FoodObject : Interaction
{
    public string productName = "";
    public float price, foodGain, waterGain;

    [Header("Turn on if this object is a vending machine")]
    public bool isVendingMachine;

    [HideInInspector] public bool vm_Purchased = false;

    //Cooldown period for vending machines in real-time seconds
    [HideInInspector] public float notRestaurantObject = 1.5f.RealToGameTime();

    private RestaurantObjects restaurantObjectScript;
    private money_tracking_system moneyTracker;
    private energy_system energyTracker;
    private Player player;
    
    public Interactor interactor {get; set;}
    public string vmStatus { get; private set; } = string.Empty;
    private void Start()
    {
        moneyTracker = GameObject.FindObjectOfType<money_tracking_system>();
        energyTracker = GameObject.FindObjectOfType<energy_system>();
        player = GameObject.FindObjectOfType<Player>();
        interactor = gameObject.GetComponent<Interactor>();

        if (!isVendingMachine)
        {
            if (!transform.GetComponentInParent<RestaurantObjects>())
                transform.parent.gameObject.AddComponent<RestaurantObjects>();

            restaurantObjectScript = transform.GetComponentInParent<RestaurantObjects>();
        }
        else
        {
            vmStatus = this.Info(player);
        }

        if(productName != null)
        {
            interactor.promptText = "Would you like to purchase " + productName + " for " + price.ToPrice() + " plus tax?";
        }
    }

    public override void OnInteractionStart(Interactor interactor)
    {
        base.OnInteractionStart(interactor);
        BuyFood();
    }

    private void BuyFood()
    {
        if(moneyTracker.currentAmount < price)
        {
            Notification.Instance.PushNotification("Unable to purchase item for sustenance due to lack of funds!");
            OnInteractionEnd();
        }
        else
        {
            Notification.Instance.PushNotification("Successfully purchased " + productName + " for " + (price + (price * money_tracking_system.salesTax)).ToPrice() + " including tax!");

            if(restaurantObjectScript != null)
            {
                restaurantObjectScript.productPurchased = true;
                OnInteractionEnd();
            }
            else
            {
                OnInteractionEnd();
                vm_Purchased = true;
                StartCoroutine(NotRestaurantObject());
            }

            moneyTracker.subtractFunds(price);
            EatFood();
        }
    }

    private IEnumerator NotRestaurantObject()
    {
        interactor.Interactable = false;
        StartCoroutine(VM_Countdown());
        yield return new WaitUntil(() => !vm_Purchased);
        interactor.Interactable = true;
    }

    private void EatFood()
    {
        energyTracker.addFood(foodGain);
        energyTracker.addLiquid(waterGain);
    }

    protected override void OnInteractionEnd()
    {
        base.OnInteractionEnd();
    }

    private IEnumerator VM_Countdown()
    {
        vmStatus = "Available again after " + notRestaurantObject + " minutes!";
        float timeLeft = notRestaurantObject;
        
        while(timeLeft != 0f)
        {
            yield return new WaitForSeconds(3.75f);
            timeLeft -= 1f;
            vmStatus = "Available again after " + timeLeft + " minutes!";
        }

        vm_Purchased = false;
        vmStatus = this.Info(player);
    }
}
