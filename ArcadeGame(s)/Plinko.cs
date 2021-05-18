/* Author: Keshav Balaji
 * Contributor(s): 
 * Created: 04-06-2021
 * Last Modified: 04-16-2021
 */
using System.Collections;
using UnityEngine;

public class Plinko : ArcadeGame
{
    public GameObject puck;
    public GameObject arcadeCanvas;
    public GameObject arcadeCamera;
    public GameObject arcadeCameraPos;
    public GameObject[] puckSpawnPoints;
    public bool puckReached {get; set;}
    public float prize {get; set;}
    private int currentSpawnPoint = 1;
    private Interactor interactor;
    private Player player;
    private GameObject dropDown1;

    private void Start()
    {
        interactor = GetComponent<Interactor>();
        interactor.promptText = "Would you like to play " + gameName + " for " + cost.ToPrice() + "?" + "\nRules: Use the left and right arrow keys or A and D to choose where to drop the puck from at the top of the Plinko. Based on where the puck lands, your prize will be awarded!";
        player = GameObject.FindObjectOfType<Player>();
        dropDown1 = arcadeCanvas.transform.GetChild(1).gameObject;
    }

    protected override IEnumerator StartArcadeGame()
    {
        player.Interacting = true;
        player.hideNotifs = true;
        player.playerCamera.enabled = false;
        arcadeCamera.SetActive(true);
        arcadeCamera.transform.position = arcadeCameraPos.transform.position;
        arcadeCamera.transform.rotation = arcadeCameraPos.transform.rotation;
        arcadeCamera.GetComponent<Camera>().fieldOfView = 70;
        arcadeCamera.SetActive(true);
        arcadeCanvas.SetActive(true);

        while(!Input.GetKeyDown(KeyCode.Return))
        {
            if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if(currentSpawnPoint == 1)
                {
                    arcadeCanvas.transform.GetChild(currentSpawnPoint).gameObject.SetActive(false);
                    currentSpawnPoint = 5;
                    arcadeCanvas.transform.GetChild(currentSpawnPoint).gameObject.SetActive(true);
                }
                else
                {
                    arcadeCanvas.transform.GetChild(currentSpawnPoint).gameObject.SetActive(false);
                    currentSpawnPoint--;
                    arcadeCanvas.transform.GetChild(currentSpawnPoint).gameObject.SetActive(true);
                }
            }
            else if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                if(currentSpawnPoint == 5)
                {
                    arcadeCanvas.transform.GetChild(currentSpawnPoint).gameObject.SetActive(false);
                    currentSpawnPoint = 1;
                    arcadeCanvas.transform.GetChild(currentSpawnPoint).gameObject.SetActive(true);
                }
                else
                {
                    arcadeCanvas.transform.GetChild(currentSpawnPoint).gameObject.SetActive(false);
                    currentSpawnPoint++;
                    arcadeCanvas.transform.GetChild(currentSpawnPoint).gameObject.SetActive(true);
                }
            }

            yield return null;
        }

        arcadeCanvas.SetActive(false);
        arcadeCanvas.transform.GetChild(currentSpawnPoint).gameObject.SetActive(false);
        arcadeCanvas.transform.GetChild(1).gameObject.SetActive(true);

        Instantiate(puck, puckSpawnPoints[currentSpawnPoint - 1].transform.position, puckSpawnPoints[currentSpawnPoint - 1].transform.rotation);
        yield return new WaitUntil(() => puckReached);
        player.hideNotifs = false;
        Notification.Instance.PushNotification("You won " + prize.ToPrice() + ". Congratulations!");

        OnInteractionEnd();
    }

    protected override void OnInteractionEnd()
    {
        base.OnInteractionEnd();
        currentSpawnPoint = 1;
        puckReached = false;
        prize = 0f;
        arcadeCamera.SetActive(false);
        arcadeCamera.transform.ResetTransform();
        player.playerCamera.enabled = true;
        player.Interacting = false;
    }
}
