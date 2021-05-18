using UnityEngine;

public class CharacterCustomizationInteractor : Interaction
{
    public GameObject charCustomizationCanvas;
    public Camera playerCamera;
    public Camera charCustomizationCamera;
    public float cameraRotation;
    public BoxCollider dressingRoom_Collider;
    public Interactor customizationInteractor;
    public GameObject faceCamPos;
    public GameObject cameraPos;
    public GameObject playerPos;
    public bool originalRoom;

    [Header("Back/Exit Button GameObjects")]
    public GameObject skinOriginalBack;
    public GameObject skinNewBack;
    public GameObject faceOriginalBack;
    public GameObject faceNewBack;
    public GameObject exitOriginal;
    public GameObject exitNew;

    private Player player;
    private ClockUI clock;

    private void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        clock = GameObject.FindObjectOfType<ClockUI>();

        player.rotationSpeed = 0.7f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(originalRoom)
        {
            skinNewBack.SetActive(false);
            faceNewBack.SetActive(false);
            exitNew.SetActive(false);
        }
        else
        {
            skinOriginalBack.SetActive(false);
            faceOriginalBack.SetActive(false);
            exitOriginal.SetActive(false);
        }

        customizationInteractor.interactionEvents[0].AddListener(OnClickYes);
        charCustomizationCamera.transform.position = cameraPos.transform.position;
        charCustomizationCamera.transform.eulerAngles = new Vector3(0f, cameraRotation, 0f);

        player.SetMovementType(MovementType.Idle);

        customizationInteractor.OnInteractorStart(player);
    }

    public override void OnInteractionStart(Interactor interactor)
    {
        player.IsCustomizing = true;
        clock.pauseTime = true;

        base.OnInteractionStart(interactor);

        charCustomizationCanvas.SetActive(true);
        charCustomizationCamera.enabled = true;
        playerCamera.enabled = false;
    }

    private void OnClickYes()
    {
        player.transform.position = playerPos.transform.position;
        player.transform.rotation = playerPos.transform.rotation;
    }

    public void OnExitClicked()
    {
        player.IsCustomizing = false;
        clock.pauseTime = false;
        skinNewBack.SetActive(true);
        skinOriginalBack.SetActive(true);
        faceNewBack.SetActive(true);
        faceOriginalBack.SetActive(true);
        exitNew.SetActive(true);
        exitOriginal.SetActive(true);
        
        OnInteractionEnd();
    }

    protected override void OnInteractionEnd()
    {
        base.OnInteractionEnd();

        charCustomizationCanvas.SetActive(false);
        charCustomizationCamera.enabled = false;
        playerCamera.enabled = true;
    }

    public void FaceCam()
    {
        charCustomizationCamera.transform.position = Vector3.Lerp(charCustomizationCamera.transform.position, faceCamPos.transform.position, 1);
    }

    public void OnExitFaceCam()
    {
        charCustomizationCamera.transform.position = Vector3.Lerp(charCustomizationCamera.transform.position, cameraPos.transform.position, 1);
        charCustomizationCamera.transform.eulerAngles = new Vector3(0f, cameraRotation, 0f);
    }
}
