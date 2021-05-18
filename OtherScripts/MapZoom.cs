/* Author: Keshav Balaji
 * Contributor(s): 
 * Created: 04-10-2021
 * Last Modified: 04-15-2021
 */
using UnityEngine;
using System.Collections;

public class MapZoom : Interaction
{
    public GameObject zoomCamera;
    public Transform cameraPos;
    public GameObject interactionCanvas;
    private Player player;
    private ClockUI clock;

    private void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        clock = GameObject.FindObjectOfType<ClockUI>();
    }

    public override void OnInteractionStart(Interactor interactor)
    {
        base.OnInteractionStart(interactor);
        StartCoroutine(Zoom());
    }

    private IEnumerator Zoom()
    {
        player.hideNotifs = true;
        yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Mouse0));

        player.playerCamera.enabled = false;
        Camera cam = zoomCamera.gameObject.GetComponent<Camera>();
        
        zoomCamera.gameObject.SetActive(true);
        zoomCamera.transform.position = cameraPos.transform.position;
        zoomCamera.transform.rotation = cameraPos.transform.rotation;
        zoomCamera.SetActive(true);

        yield return new WaitUntil(() => Input.anyKey && !Input.GetKey(KeyCode.Mouse0));

        OnInteractionEnd();
    }

    protected override void OnInteractionEnd()
    {
        base.OnInteractionEnd();

        player.hideNotifs = false;
        zoomCamera.SetActive(false);
        zoomCamera.transform.ResetTransform();
        player.playerCamera.enabled = true;
    }
}
