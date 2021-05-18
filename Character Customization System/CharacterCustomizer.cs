using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterCustomizer : MonoBehaviour
{
    [Header("Character Customization Details")]
    public TextMeshProUGUI modelText;
	public TextMeshProUGUI baseTextureText;
    public TextMeshProUGUI patternText;
	public TextMeshProUGUI logoText;

	public Image baseCurrentColor;
	public Image patternCurrentColor;
	public Image logoCurrentColor;

	private int index;
	[SerializeField] private List<Customization> currCustomization;
	[SerializeField] private Customization shoeCustomization;

    private int currentModel = 0;
	private int currentBaseTex = 0;
    private int currentPattern = 0;
	private int currentLogo = 0;

    private Material material;
	private Texture baseTexture;
	private Texture pattern;
	private Texture logo;

	private GameObject modelCanvas;
	private GameObject baseCanvas;
	private GameObject patternCanvas;
	private GameObject logoCanvas;

	private CharacterLists lists;

	void OnEnable()
	{
		lists = GameObject.FindObjectOfType<CharacterLists>();
		index = transform.GetSiblingIndex();

		modelCanvas = transform.GetChild(0).GetChild(1).gameObject;
		baseCanvas = transform.GetChild(0).GetChild(2).gameObject;
		patternCanvas = transform.GetChild(0).GetChild(3).gameObject;
		logoCanvas = transform.GetChild(0).GetChild(4).gameObject;

		if (currCustomization is null)
			currCustomization = new List<Customization>();
		
		switch(index)
		{
			case 2:
				foreach (var shirt in lists.shirtCustomizer)
				{
					if (shirt.isPurchased)
					{
						Customization shirtCopy = new Customization();
						shirtCopy.modelName = shirt.modelName;
						shirtCopy.model = shirt.model;
						currCustomization.Add(shirtCopy);

						//
						foreach (var baseTexture in shirt.baseTextures)
							if (baseTexture.isPurchased)
								shirtCopy.baseTextures.Add(baseTexture);
						foreach (var pattern in shirt.patterns)
							if (pattern.isPurchased)
								shirtCopy.patterns.Add(pattern);
						foreach (var logo in shirt.logos)
							if (logo.isPurchased)
								shirtCopy.logos.Add(logo);
					}
				}
				break;
			case 3:
				foreach (var pants in lists.pantCustomizer)
				{
					if (pants.isPurchased)
					{
						Customization pantsCopy = new Customization();
						pantsCopy.modelName = pants.modelName;
						pantsCopy.model = pants.model;
						currCustomization.Add(pantsCopy);

						foreach (var baseTexture in pants.baseTextures)
							if (baseTexture.isPurchased)
								pantsCopy.baseTextures.Add(baseTexture);
						foreach (var pattern in pants.patterns)
							if (pattern.isPurchased)
								pantsCopy.patterns.Add(pattern);
						foreach (var logo in pants.logos)
							if (logo.isPurchased)
								pantsCopy.logos.Add(logo);
					}
				}
				break;
			case 4:
				shoeCustomization = new Customization();
				currCustomization.Add(shoeCustomization);
				
				if(lists.shoeCustomizer.model != null)
				{
					shoeCustomization.model = lists.shoeCustomizer.model;
					shoeCustomization.additionalModel = lists.shoeCustomizer.additionalModel;
					shoeCustomization.material = lists.shoeCustomizer.model.GetComponent<Renderer>().material;

					foreach (var baseTexture in lists.shoeCustomizer.baseTextures)
						if (baseTexture.isPurchased)
							shoeCustomization.baseTextures.Add(baseTexture);
				}
				break;
			case 5:
				foreach (var backpack in lists.bPCustomizer)
				{
					if (backpack.isPurchased)
					{
						Customization backpackCopy = new Customization();
						backpackCopy.modelName = backpack.modelName;
						backpackCopy.model = backpack.model;
						currCustomization.Add(backpackCopy);

						foreach (var baseTexture in backpack.baseTextures)
							if (baseTexture.isPurchased)
								backpackCopy.baseTextures.Add(baseTexture);
						foreach (var pattern in backpack.patterns)
							if (pattern.isPurchased)
								backpackCopy.patterns.Add(pattern);
						foreach (var logo in backpack.logos)
							if (logo.isPurchased)
								backpackCopy.logos.Add(logo);
					}
				}
				break;
			case 6:
				foreach (var hair in lists.hairCustomizer)
				{
					if (hair.isPurchased)
					{
						Customization hairCopy = new Customization();
						hairCopy.modelName = hair.modelName;
						hairCopy.model = hair.model;
						currCustomization.Add(hairCopy);
					}
				}
				break;
			default:
				return;
		}

		if(currCustomization.Count == 0)
		{
			if(modelText != null)
				modelText.text = "None";

			if(baseTextureText != null)
				baseTextureText.text = "None";

			if(patternText != null)
				patternText.text = "None";

			if(logoText != null)
				logoText.text = "None";
			
			return;
		}

		if(index == 6)
		{
			ApplyHairModel();
		}

		//
		if(shoeCustomization.model != null)
		{
			ApplyShoeModel();
		}
		else if(currCustomization.Count != 0)
		{
			for(int i = 0; i < currCustomization.Count; i++)
			{
				if(currCustomization[i].model.activeSelf == true)
				{
					currentModel = i;
					modelText.text = currCustomization[currentModel].modelName;
					break;
				}
			}

			applyModel();

			if(modelCanvas != null && currCustomization.Count == 0 && modelCanvas.activeSelf)
			{
				modelCanvas.SetActive(false);
			}
			if(baseCanvas != null && currCustomization[currentModel].baseTextures.Count == 0 && baseCanvas.activeSelf && baseCanvas.name != "EyebrowColor_Customizer")
			{
				baseCanvas.SetActive(false);
			}
			if(patternCanvas != null && currCustomization[currentModel].patterns.Count == 0 && patternCanvas.activeSelf)
			{
				patternCanvas.SetActive(false);
			}
			if(logoCanvas != null && currCustomization[currentModel].logos.Count == 0 && logoCanvas.activeSelf)
			{
				logoCanvas.SetActive(false);
			}
		}
	}

	void OnDisable()
	{
		if(currCustomization.Count != 0)
			currCustomization.Clear();
		
		if(shoeCustomization.model != null)
		{
			shoeCustomization.model = null;
			shoeCustomization.material = null;
			shoeCustomization.baseTextures.Clear();
		}
	}

    public void NextModel()
	{
		if (currCustomization.Count == 0)
			return;

		//
		currentModel++;

		if (currentModel > currCustomization.Count - 1)
		{
			currentModel = 0;
		}

		applyModel();
	}

	public void PrevModel()
	{
		if (currCustomization.Count == 0)
			return;

		//
		currentModel--;

		if (currentModel < 0)
		{
			currentModel = currCustomization.Count - 1;
		}

		applyModel();
			
	}

	public void NextBaseText()
	{
		if (currCustomization.Count == 0)
			return;

		//
		currentBaseTex++;

		if(currentBaseTex > currCustomization[currentModel].baseTextures.Count - 1)
		{
			currentBaseTex = 0;
		}

		applyBaseTexture();
	}

	public void PrevBaseText()
	{
		if (currCustomization.Count == 0)
			return;

		//
		currentBaseTex--;

		if (currentBaseTex < 0)
		{
			currentBaseTex = currCustomization[currentModel].baseTextures.Count - 1;
		}

		applyBaseTexture();
	}

    public void NextPattern()
	{
		if (currCustomization.Count == 0)
			return;

		//
		currentPattern++;

		if(currentPattern > currCustomization[currentModel].patterns.Count - 1)
		{
			currentPattern = 0;
		}

		applyPattern();
	}

	public void PrevPattern()
	{
		if (currCustomization.Count == 0)
			return;

		//
		currentPattern--;

		if (currentPattern < 0)
		{
			currentPattern = currCustomization[currentModel].patterns.Count - 1;
		}

		applyPattern();
	}

	public void NextLogo()
	{
		if (currCustomization.Count == 0)
			return;
		
		//
		currentLogo++;

		if(currentLogo > currCustomization[currentModel].logos.Count - 1)
		{
			currentLogo = 0;
		}

		applyLogo();
	}

	public void PrevLogo()
	{
		if (currCustomization.Count == 0)
			return;

		//
		currentLogo--;

		if (currentLogo < 0)
		{
			currentLogo = currCustomization[currentModel].logos.Count - 1;
		}

		applyLogo();
	}

    private void applyModel()
	{
		material = currCustomization[currentModel].model.GetComponent<Renderer>().material;
		
		if(currCustomization[currentModel].baseTextures.Count > 0)
		{
			baseTexture = material.GetTexture("baseMap");
			for(int i = 0; i < currCustomization[currentModel].baseTextures.Count; i++)
			{
				if(baseTexture == currCustomization[currentModel].baseTextures[i].texture)
				{
					currentBaseTex = i;
					if(baseTextureText != null)
					{
						baseTextureText.text = currCustomization[currentModel].baseTextures[currentBaseTex].textureName;
					}

					break;
				}
			}

			if(baseCurrentColor != null)
			{
				baseCurrentColor.color = material.GetColor("baseMapColor");
			}
		}

		if(currCustomization[currentModel].patterns.Count > 0)
		{
			pattern = material.GetTexture("additionalColorMap");

			for(int i = 0; i < currCustomization[currentModel].patterns.Count; i++)
			{
				if(pattern == currCustomization[currentModel].patterns[i].texture)
				{
					currentPattern = i;
					if(patternText != null)
					{
						patternText.text = currCustomization[currentModel].patterns[currentPattern].textureName;
					}
					
					break;
				}
			}

			if(patternCurrentColor != null)
			{
				patternCurrentColor.color = material.GetColor("additionalColorMapColor");
			}
		}
		
		if(currCustomization[currentModel].logos.Count > 0)
		{
			logo = material.GetTexture("optionalLogo");

			for(int i = 0; i < currCustomization[currentModel].logos.Count; i++)
			{
				if(logo == currCustomization[currentModel].logos[i].texture)
				{
					currentLogo = i;
					if(logoText != null)
					{
						logoText.text = currCustomization[currentModel].logos[currentLogo].textureName;
					}
					
					break;
				}
			}

			if(logoCurrentColor != null)
			{
				logoCurrentColor.color = material.GetColor("optionalLogoColor");
			}
		}

		for(int i = 0; i < currCustomization.Count; i++)
		{
			if(i == currentModel)
			{
				currCustomization[i].model.SetActive(true);

				if(modelText != null)
				{
					modelText.text = currCustomization[i].modelName;
				}
			}
			else
			{
				currCustomization[i].model.SetActive(false);
			}
		}
	}

	private void ApplyShoeModel()
	{
		material = shoeCustomization.material;

		if(shoeCustomization.baseTextures.Count > 0)
		{
			baseTexture = material.GetTexture("baseMap");
			for(int i = 0; i < shoeCustomization.baseTextures.Count; i++)
			{
				if(baseTexture == shoeCustomization.baseTextures[i].texture)
				{
					currentBaseTex = i;
					baseTextureText.text = shoeCustomization.baseTextures[currentBaseTex].textureName;
					break;
				}
			}

			baseCurrentColor.color = material.GetColor("baseMapColor");
		}
	}

	public void ShoePrevBase()
	{
		//
		currentBaseTex--;

		if (currentBaseTex < 0)
		{
			currentBaseTex = shoeCustomization.baseTextures.Count - 1;
		}

		ApplyShoeBase();
	}

	public void ShoeNextBase()
	{
		//
		currentBaseTex++;

		if(currentBaseTex > shoeCustomization.baseTextures.Count - 1)
		{
			currentBaseTex = 0;
		}

		ApplyShoeBase();
	}

	private void ApplyShoeBase()
	{
		baseTexture = shoeCustomization.baseTextures[currentBaseTex].texture;
		baseTextureText.text = shoeCustomization.baseTextures[currentBaseTex].textureName;
		shoeCustomization.model.GetComponent<Renderer>().material.SetTexture("baseMap", baseTexture);
		shoeCustomization.additionalModel.GetComponent<Renderer>().material.SetTexture("baseMap", baseTexture);
	}

	public void ApplyShoeBaseColor(Color pickedColor)
	{
		if(shoeCustomization.baseTextures.Count == 0)
			return;
		
		shoeCustomization.model.GetComponent<Renderer>().material.SetColor("baseMapColor", pickedColor);
		shoeCustomization.additionalModel.GetComponent<Renderer>().material.SetColor("baseMapColor", pickedColor);
	}
	
	private void applyBaseTexture()
	{
		baseTexture = currCustomization[currentModel].baseTextures[currentBaseTex].texture;
		baseTextureText.text = currCustomization[currentModel].baseTextures[currentBaseTex].textureName;
		material.SetTexture("baseMap", baseTexture);
	}

    private void applyPattern()
	{
		pattern = currCustomization[currentModel].patterns[currentPattern].texture;
		patternText.text = currCustomization[currentModel].patterns[currentPattern].textureName;
		material.SetTexture("additionalColorMap", pattern);
	}

	private void applyLogo()
	{
		logo = currCustomization[currentModel].logos[currentLogo].texture;
		logoText.text = currCustomization[currentModel].logos[currentLogo].textureName;
		material.SetTexture("optionalLogo", logo);
	}

	public void applyBaseTextColor(Color pickedColor)
	{
		if (baseTextureText.text == "None")
			return;
		
		material.SetColor("baseMapColor", pickedColor);
	}

	public void applyPatternColor(Color pickedColor)
	{
		if (patternText.text == "None")
			return;

		material.SetColor("additionalColorMapColor", pickedColor);
	}

	public void applyLogoColor(Color pickedColor)
	{
		if (logoText.text == "None")
			return;

		material.SetColor("optionalLogoColor", pickedColor);
	}

	private void ApplyHairModel()
	{
		material = currCustomization[currentModel].model.GetComponent<Renderer>().material;

		if(baseCurrentColor != null)
		{
			baseCurrentColor.color = material.GetColor("baseMapColor");
		}

		for(int i = 0; i < currCustomization.Count; i++)
		{
			if(i == currentModel)
			{
				currCustomization[i].model.SetActive(true);

				if(modelText != null)
				{
					modelText.text = currCustomization[i].modelName;
				}
			}
			else
			{
				currCustomization[i].model.SetActive(false);
			}
		}
	}

	public void HairNextModel()
	{
		if (currCustomization.Count == 0)
			return;

		//
		currentModel++;

		if (currentModel > currCustomization.Count - 1)
		{
			currentModel = 0;
		}

		ApplyHairModel();
	}

	public void HairPrevModel()
	{
		if (currCustomization.Count == 0)
			return;

		//
		currentModel--;

		if (currentModel < 0)
		{
			currentModel = currCustomization.Count - 1;
		}

		ApplyHairModel();
	}
}
