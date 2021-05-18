using System.Collections;
using UnityEngine;

public class RestaurantObjects : MonoBehaviour
{
    public bool productPurchased {get; set;}
    private Interactor interactor;
    private float enableWait = 300f;
    
    void Start()
    {
        productPurchased = false;
        interactor = transform.GetComponentInChildren<Interactor>();
    }

    void Update()
    {
        if(productPurchased)
        {
            StartCoroutine(EnableWait());
            productPurchased = false;
        }
    }

    private IEnumerator EnableWait()
    {
        interactor.Interactable = false;

        if(transform.childCount > 1)
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(enableWait);

        if(transform.childCount > 1)
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }

        interactor.Interactable = true;
    }
}
