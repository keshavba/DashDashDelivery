/* Author: Keshav Balaji
 * Contributor(s): 
 * Created: 04-12-2021
 * Last Modified: 06-18-2021
 */
using System.Collections;
using UnityEngine;

public class CycloneArcade : ArcadeGame
{
    [Header("Light Speed")]
    public float lightSpeed = 300f;

    [Header("Other Stuff")]
    public GameObject arcadeCamera;
    public GameObject cameraPos;

    private Material lightShader;
    private float rotDegree = 0f;
    private money_tracking_system moneyManager;
    private Interactor interactor;
    private Player player;

    private const string ISPLAYING = "isPlaying";
    private const string JACKPOT = "jackpot";
    private const string ROTAMOUNT = "rotationAmount";

    private void Start()
    {
        moneyManager = GameObject.FindObjectOfType<money_tracking_system>();
        interactor = GetComponent<Interactor>();
        interactor.promptText = "Would you like to play " + gameName + " for " + cost.ToPrice() + "?" + "\n\nRules:\n" + "Press Space when the light is in the jackpot zone to win the biggest prize!\nP.S. You will still win something if the light stops close to the jackpot so make sure to play!";
        player = GameObject.FindObjectOfType<Player>();
    }

    protected override IEnumerator StartArcadeGame()
    {
        player.Interacting = true;
        player.playerCamera.enabled = false;
        arcadeCamera.transform.position = cameraPos.transform.position;
        arcadeCamera.transform.rotation = cameraPos.transform.rotation;
        arcadeCamera.GetComponent<Camera>().fieldOfView = 70f;
        arcadeCamera.SetActive(true);

        lightShader = GetComponent<MeshRenderer>().materials[1];
        lightShader.SetFloat(ISPLAYING, 1);

        while(!Input.GetKeyDown(KeyCode.Space))
        {
            rotDegree += lightSpeed * Time.deltaTime;
            lightShader.SetFloat(ROTAMOUNT, rotDegree);
            yield return null;
        }

        if(lightShader.GetFloat(ROTAMOUNT) % 360 == 0)
        {
            lightShader.SetFloat(JACKPOT, 1);
            moneyManager.addFunds(prizeAmount);
            Notification.Instance.PushNotification($"Congratulations! You won {prizeAmount.ToPrice()}.");
            yield return new WaitForSeconds(1f);
        }
        else if((lightShader.GetFloat(ROTAMOUNT) % 360 >= 12 && lightShader.GetFloat(ROTAMOUNT) % 360 < 24) || lightShader.GetFloat(ROTAMOUNT) % 360 >= 348)
        {
            moneyManager.addFunds(9f);
            Notification.Instance.PushNotification($"Congratulations! You won {9f.ToPrice()}.");
            yield return new WaitForSeconds(1f);
        }
        else if((lightShader.GetFloat(ROTAMOUNT) % 360 >= 24 && lightShader.GetFloat(ROTAMOUNT) % 360 < 36) || (lightShader.GetFloat(ROTAMOUNT) % 360 >= 336 && lightShader.GetFloat(ROTAMOUNT) % 360 < 348))
        {
            moneyManager.addFunds(7f);
            Notification.Instance.PushNotification($"Congratulations! You won {7f.ToPrice()}.");
            yield return new WaitForSeconds(1f);
        }
        else if((lightShader.GetFloat(ROTAMOUNT) % 360 >= 36 && lightShader.GetFloat(ROTAMOUNT) % 360 < 48) || (lightShader.GetFloat(ROTAMOUNT) % 360 >= 324 && lightShader.GetFloat(ROTAMOUNT) % 360 < 336))
        {
            moneyManager.addFunds(5f);
            Notification.Instance.PushNotification($"Congratulations! You won {5f.ToPrice()}.");
            yield return new WaitForSeconds(1f);
        }
        else if((lightShader.GetFloat(ROTAMOUNT) % 360 >= 48 && lightShader.GetFloat(ROTAMOUNT) % 360 < 60) || (lightShader.GetFloat(ROTAMOUNT) % 360 >= 312 && lightShader.GetFloat(ROTAMOUNT) % 360 < 324))
        {
            moneyManager.addFunds(3f);
            Notification.Instance.PushNotification($"Congratulations! You won {3f.ToPrice()}.");
            yield return new WaitForSeconds(1f);
        }
        else if((lightShader.GetFloat(ROTAMOUNT) % 360 >= 60 && lightShader.GetFloat(ROTAMOUNT) % 360 < 72) || (lightShader.GetFloat(ROTAMOUNT) % 360 >= 300 && lightShader.GetFloat(ROTAMOUNT) % 360 < 312))
        {
            moneyManager.addFunds(1f);
            Notification.Instance.PushNotification($"Congratulations! You won {1f.ToPrice()}.");
            yield return new WaitForSeconds(1f);
        }
        else
        {
            Notification.Instance.PushNotification("Too bad! Try again next time.");
        }

        OnInteractionEnd();
    }

    protected override void OnInteractionEnd()
    {
        base.OnInteractionEnd();
        lightShader.SetInt(ISPLAYING, 0);
        lightShader.SetInt(JACKPOT, 0);
        lightShader.SetFloat(ROTAMOUNT, 0f);
        arcadeCamera.SetActive(false);
        arcadeCamera.transform.ResetTransform();
        player.playerCamera.enabled = true;
        player.Interacting = false;
    }
}
