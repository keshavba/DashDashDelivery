using System.Globalization;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(CharacterLists))]
public class CharacterListsEditor : Editor
{
    private string autoNamingInfo = "Automatic naming works by simply ticking the autoNaming checkbox wherever needed. For this system to work properly, please ensure the following:\n\ni. The words of the file name should be separated by either the underscore character or spaces.\nii. If the file name of textures (base texture, pattern, and logo) have the name of the model (ex: female_long_sleeve_shirt_gears) in them, please make sure the name of the model in the file names of both the model and the texture are the same.\n\nThat should be it. Let me (Keshav) know if there are any problems with the autonaming system.";

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox(autoNamingInfo, MessageType.Info);

        base.OnInspectorGUI();
    }
}
#endif

public class CharacterLists : MonoBehaviour
{
    [Header("Shirt Customization")]
    public List<Customization> shirtCustomizer = new List<Customization>();

    [Header("Pants Customization")]
    public List<Customization> pantCustomizer = new List<Customization>();

    [Header("Shoe Customization")]
    public Customization shoeCustomizer = new Customization();

    [Header("Backpack Customization")]
    public List<Customization> bPCustomizer = new List<Customization>();

    [Header("Hair Customization")]
    public List<Customization> hairCustomizer = new List<Customization>();

    public static char[] delimeters = {'_', ' '};

    void Start()
    {
        if(shirtCustomizer.Count > 0)
        {
            for(int i = 0; i < shirtCustomizer.Count; i++)
            {
                if(i == 1)
                    shirtCustomizer[i].isPurchased = true;
                else
                    shirtCustomizer[i].isPurchased = false;
                
                if(shirtCustomizer[i].autoNaming)
                    shirtCustomizer[i].modelName = AutoNamingModel(shirtCustomizer[i].model);
                    
                if(shirtCustomizer[i].isPurchased)
                {
                    if(shirtCustomizer[i].baseTextures.Count > 0)
                    {
                        shirtCustomizer[i].baseTextures[0].isPurchased = true;
                        if(shirtCustomizer[i].baseTextures[0].autoNaming)
                            shirtCustomizer[i].baseTextures[0].textureName = AutoNamingTex(shirtCustomizer[i].baseTextures[0].texture, shirtCustomizer[i].modelName);

                        for(int j = 1; j < shirtCustomizer[i].baseTextures.Count; j++)
                        {
                            if(shirtCustomizer[i].baseTextures[j].isPurchased)
                            {
                                shirtCustomizer[i].baseTextures[j].isPurchased = false;
                            }

                            if(shirtCustomizer[i].baseTextures[j].autoNaming)
                                shirtCustomizer[i].baseTextures[j].textureName = AutoNamingTex(shirtCustomizer[i].baseTextures[j].texture, shirtCustomizer[i].modelName);
                        }
                    }
                }
                else
                {
                    for(int j = 0; j < shirtCustomizer[i].baseTextures.Count; j++)
                    {
                        if(shirtCustomizer[i].baseTextures[j].isPurchased)
                        {
                            shirtCustomizer[i].baseTextures[j].isPurchased = false;
                        }

                        if(shirtCustomizer[i].baseTextures[j].autoNaming)
                            shirtCustomizer[i].baseTextures[j].textureName = AutoNamingTex(shirtCustomizer[i].baseTextures[j].texture, shirtCustomizer[i].modelName);
                    }
                }

                for(int j = 0; j < shirtCustomizer[i].patterns.Count; j++)
                {
                    if(shirtCustomizer[i].patterns[j].textureName == "None")
                        shirtCustomizer[i].patterns[j].isPurchased = true;
                    else
                        shirtCustomizer[i].patterns[j].isPurchased = false;

                    if(shirtCustomizer[i].patterns[j].autoNaming)
                    {
                        shirtCustomizer[i].patterns[j].textureName = AutoNamingTex(shirtCustomizer[i].patterns[j].texture, shirtCustomizer[i].modelName);
                    }
                }

                for(int j = 0; j < shirtCustomizer[i].logos.Count; j++)
                {
                    if(shirtCustomizer[i].logos[j].autoNaming)
                    {
                        shirtCustomizer[i].logos[j].textureName = AutoNamingTex(shirtCustomizer[i].logos[j].texture, shirtCustomizer[i].modelName);
                    }

                    if(shirtCustomizer[i].logos[j].textureName == "None")
                        shirtCustomizer[i].logos[j].isPurchased = true;
                    else if(shirtCustomizer[i].logos[j].textureName == "D3")
                        shirtCustomizer[i].logos[j].isPurchased = true;
                    else
                        shirtCustomizer[i].logos[j].isPurchased = false;
                }
            }
        }                

        if(pantCustomizer.Count > 0)
        {
            for(int i = 0; i < pantCustomizer.Count; i++)
            {
                if(i == 0)
                    pantCustomizer[i].isPurchased = true;
                else
                    pantCustomizer[i].isPurchased = false;
                
                if(pantCustomizer[i].autoNaming)
                    pantCustomizer[i].modelName = AutoNamingModel(pantCustomizer[i].model);
                    
                if(pantCustomizer[i].isPurchased)
                {
                    if(pantCustomizer[i].baseTextures.Count > 0)
                    {
                        pantCustomizer[i].baseTextures[0].isPurchased = true;
                        if(pantCustomizer[i].baseTextures[0].autoNaming)
                            pantCustomizer[i].baseTextures[0].textureName = AutoNamingTex(pantCustomizer[i].baseTextures[0].texture, pantCustomizer[i].modelName);

                        for(int j = 1; j < pantCustomizer[i].baseTextures.Count; j++)
                        {
                            if(pantCustomizer[i].baseTextures[j].isPurchased)
                            {
                                pantCustomizer[i].baseTextures[j].isPurchased = false;
                            }

                            if(pantCustomizer[i].baseTextures[j].autoNaming)
                                pantCustomizer[i].baseTextures[j].textureName = AutoNamingTex(pantCustomizer[i].baseTextures[j].texture, pantCustomizer[i].modelName);
                        }
                    }
                }
                else
                {
                    for(int j = 0; j < pantCustomizer[i].baseTextures.Count; j++)
                    {
                        if(pantCustomizer[i].baseTextures[j].isPurchased)
                        {
                            pantCustomizer[i].baseTextures[j].isPurchased = false;
                        }

                        if(pantCustomizer[i].baseTextures[j].autoNaming)
                            pantCustomizer[i].baseTextures[j].textureName = AutoNamingTex(pantCustomizer[i].baseTextures[j].texture, pantCustomizer[i].modelName);
                    }
                }
                
                for(int j = 0; j < pantCustomizer[i].patterns.Count; j++)
                {
                    if(pantCustomizer[i].patterns[j].textureName == "None")
                        pantCustomizer[i].patterns[j].isPurchased = true;
                    else
                        pantCustomizer[i].patterns[j].isPurchased = false;

                    if(pantCustomizer[i].patterns[j].autoNaming)
                    {
                        pantCustomizer[i].patterns[j].textureName = AutoNamingTex(pantCustomizer[i].patterns[j].texture, pantCustomizer[i].modelName);
                    }
                }
            }
        }
                
        if(bPCustomizer.Count > 0)
        {
            for(int i = 0; i < bPCustomizer.Count; i++)
            {
                bPCustomizer[i].isPurchased = false;

                if(bPCustomizer[i].autoNaming)
                    bPCustomizer[i].modelName = AutoNamingModel(bPCustomizer[i].model);
                
                if(bPCustomizer[i].baseTextures.Count > 0)
                {
                    for(int j = 0; j < bPCustomizer[i].baseTextures.Count; j++)
                    {
                        if(bPCustomizer[i].baseTextures[j].isPurchased)
                        {
                            bPCustomizer[i].baseTextures[j].isPurchased = false;
                        }

                        if(bPCustomizer[i].baseTextures[j].autoNaming)
                            bPCustomizer[i].baseTextures[j].textureName = AutoNamingTex(bPCustomizer[i].baseTextures[j].texture, bPCustomizer[i].modelName);
                    }
                }

                if(bPCustomizer[i].patterns.Count > 0)
                {
                    for(int j = 0; j < bPCustomizer[i].patterns.Count; j++)
                    {
                        if(bPCustomizer[i].patterns[j].textureName == "None")
                            bPCustomizer[i].patterns[j].isPurchased = true;
                        else
                            bPCustomizer[i].patterns[j].isPurchased = false;

                        if(bPCustomizer[i].patterns[j].autoNaming)
                        {
                            bPCustomizer[i].patterns[j].textureName = AutoNamingTex(bPCustomizer[i].patterns[j].texture, bPCustomizer[i].modelName);
                        }
                    }
                }

                if(bPCustomizer[i].logos.Count > 0)
                {
                    for(int j = 0; j < bPCustomizer[i].logos.Count; j++)
                    {
                        if(bPCustomizer[i].logos[j].autoNaming)
                        {
                            bPCustomizer[i].logos[j].textureName = AutoNamingTex(bPCustomizer[i].logos[j].texture, bPCustomizer[i].modelName);
                        }

                        if(bPCustomizer[i].logos[j].textureName == "None")
                            bPCustomizer[i].logos[j].isPurchased = true;
                        else if(bPCustomizer[i].logos[j].textureName == "D3")
                            bPCustomizer[i].logos[j].isPurchased = true;
                        else
                            bPCustomizer[i].logos[j].isPurchased = false;
                    }
                }
            }
        }

        if(shoeCustomizer.baseTextures.Count > 0)
        {
            shoeCustomizer.baseTextures[0].isPurchased = true;
            if(shoeCustomizer.baseTextures[0].autoNaming)
            {
                shoeCustomizer.baseTextures[0].textureName = AutoNamingTex(shoeCustomizer.baseTextures[0].texture, shoeCustomizer.modelName);
            }

            for(int j = 1; j < shoeCustomizer.baseTextures.Count; j++)
            {
                shoeCustomizer.baseTextures[j].isPurchased = false;

                if(shoeCustomizer.baseTextures[j].autoNaming)
                {
                    shoeCustomizer.baseTextures[j].textureName = AutoNamingTex(shoeCustomizer.baseTextures[j].texture, shoeCustomizer.modelName);
                }
            }
        }

        if(hairCustomizer.Count > 0)
        {
            for(int i = 0; i < hairCustomizer.Count; i++)
            {
                hairCustomizer[i].isPurchased = true;

                if(hairCustomizer[i].autoNaming)
                    hairCustomizer[i].modelName = AutoNamingModel(hairCustomizer[i].model);
            }
        }
    }

    public static string AutoNamingModel(GameObject obj)
	{
        int wordCount = 0;
		string name = "";
		string[] words = obj.name.Split(delimeters);

		foreach (string word in words)
		{
            wordCount++;

			if(word.ToLower() == "female" || word.ToLower() == "male")
			{
				continue;
			}
			
			name += word;
            if((words.Length - wordCount) >= 1)
			    name += ' ';
		}

		TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
		return myTI.ToTitleCase(name);
	}

	public static string AutoNamingTex(Texture texture, string modelName)
	{
        int wordCount = 0;
		bool isModelWord = false;
		string name = "";
		string[] words = texture.name.Split(delimeters);
		string[] modelWords = modelName.Split(delimeters);

		foreach (string word in words)
		{
            wordCount++;

			if(word.ToLower() == "female" || word.ToLower() == "male")
			{
				continue;
			}

            isModelWord = false;
			foreach(string modelWord in modelWords)
			{
				if(word.ToLower() == modelWord.ToLower())
				{
					isModelWord = true;
					break;
				}
			}

			if(isModelWord)
				continue;
			
			name += word;
            if((words.Length - wordCount) >= 1)
			    name += ' ';
		}

		TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
		return myTI.ToTitleCase(name);
	}
}
