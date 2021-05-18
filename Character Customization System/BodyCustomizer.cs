using UnityEngine;
using UnityEngine.UI;

public enum Type
{
    Skin_Color,
    Eye_Color,
    Eyebrow_Color
};

public class BodyCustomizer : MonoBehaviour
{
    public Type scriptType;
    public GameObject femalePlayer;
    public GameObject malePlayer;
    public Image currentColor;

    private Player player;

    private Material femaleSkinColor;
    private Material femaleFaceColor;
    private Material femaleEyeColor;
    private Material femaleEyebrowColor;

    private Material maleSkinColor;
    private Material maleFaceColor;
    private Material maleEyeColor;
    private Material maleEyebrowColor;

    private Material material;
    private Material additionalMat;

    public Material Material { get => material; set => material = value; }
    public Material AdditionalMaterial { get => additionalMat; set => additionalMat = value; }

    void Start()
    {
        SkinnedMeshRenderer femaleRend = femalePlayer.GetComponent<SkinnedMeshRenderer>();
        femaleFaceColor = femaleRend.materials[0];
        femaleSkinColor = femaleRend.materials[1];
        femaleEyebrowColor = femaleRend.materials[2];
        femaleEyeColor = femaleRend.materials[3];

        SkinnedMeshRenderer maleRend = malePlayer.GetComponent<SkinnedMeshRenderer>();
        maleFaceColor = maleRend.materials[0];
        maleSkinColor = maleRend.materials[1];
        maleEyebrowColor = maleRend.materials[2];
        maleEyeColor = maleRend.materials[3];

        SetMaterial();
    }
    public void Initialize()
    {
        SkinnedMeshRenderer femaleRend = femalePlayer.GetComponent<SkinnedMeshRenderer>();
        femaleFaceColor = femaleRend.materials[0];
        femaleSkinColor = femaleRend.materials[1];
        femaleEyebrowColor = femaleRend.materials[2];
        femaleEyeColor = femaleRend.materials[3];

        SkinnedMeshRenderer maleRend = malePlayer.GetComponent<SkinnedMeshRenderer>();
        maleFaceColor = maleRend.materials[0];
        maleSkinColor = maleRend.materials[1];
        maleEyebrowColor = maleRend.materials[2];
        maleEyeColor = maleRend.materials[3];

        SetMaterial();
    }

    public void SetSkinColor(Color pickedColor)
    {
        material.SetColor("baseMapColor", pickedColor);
        additionalMat.SetColor("baseMapColor", pickedColor);
    }
    public void SetEyeColor(Color pickedColor)
    {
        material.SetColor("additionalColorMapColor", pickedColor);
    }
    public void SetEyebrowColor(Color pickedColor)
    {
        material.SetColor("baseMapColor", pickedColor);
    }
    
    public void SetMaterial()
    {
        player = GameObject.FindObjectOfType<Player>();

        if(scriptType == Type.Skin_Color)
        {
            if (player.sex == Sex.Male)
            {
                material = maleSkinColor;
                additionalMat = maleFaceColor;
            }
            else
            {
                material = femaleSkinColor;
                additionalMat = femaleFaceColor;
            }
        }
        else if(scriptType == Type.Eye_Color)
        {
            if (player.sex == Sex.Male)
            {
                material = maleEyeColor;
            }
            else
            {
                material = femaleEyeColor;
            }
        }
        else
        {
            if (player.sex == Sex.Male)
            {
                material = maleEyebrowColor;
            }
            else
            {
                material = femaleEyebrowColor;
            }
        }
        if (transform.GetSiblingIndex() == 1 || transform.GetSiblingIndex() == 2)
        {
            currentColor.color = material.GetColor("baseMapColor");
        }
        else
        {
            currentColor.color = material.GetColor("additionalColorMapColor");
        }
    }
}
