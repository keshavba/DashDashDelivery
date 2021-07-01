using UnityEngine;

public class CustomizationSearchPurchased : MonoBehaviour
{
    public GameObject femaleCustomizer;
    public GameObject maleCustomizer;

    [Header("Male Clothes")]
    public GameObject male_longshirt;
    public Texture male_longshirt_d3logo;
    public GameObject male_shortshirt;
    public Texture male_shortshirt_base;
    public Texture male_shortshirt_d3logo;
    public GameObject male_shorts;
    public Texture male_shorts_base;
    public GameObject male_pants;
    public GameObject male_shoe;
    public Texture male_shoe_base;
    public GameObject male_basicbp;
    public GameObject male_slingbp;

    [Header("Female Clothes")]
    public GameObject female_longshirt;
    public Texture female_longshirt_d3logo;
    public GameObject female_shortshirt;
    public Texture female_shortshirt_base;
    public Texture female_shortshirt_d3logo;
    public GameObject female_shorts;
    public Texture female_shorts_base;
    public GameObject female_pants;
    public GameObject female_shoe;
    public Texture female_shoe_base;
    public GameObject female_basicbp;
    public GameObject female_slingbp;

    [Header("Common Textures")]
    public Texture no_selection;
    public Texture basicbp_d3logo;
    public Texture slingbp_d3logo;

    private CharacterLists lists;
    private ClockUI clock;

    void Start()
    {
        clock = GameObject.FindObjectOfType<ClockUI>();
        if(clock.inEditor)
        {
            SetMaleOutfit();
            SetFemaleOutfit();
            return;
        }
        
        if(SaveSystem.Instance)
        {
            SaveSystem.Instance.onNewGame += SetMaleOutfit;
            SaveSystem.Instance.onNewGame += SetFemaleOutfit;
        }
    }
    private void OnDestroy()
    {
        if (SaveSystem.Instance)
        {
            SaveSystem.Instance.onNewGame -= SetMaleOutfit;
            SaveSystem.Instance.onNewGame -= SetFemaleOutfit;
        }
    }

    public void SearchPurchased(ObjectType objectType, ModelType model, string productName, Texture texture, Mesh mesh, bool male)
    {
        if(male)
            lists = maleCustomizer.GetComponent<CharacterLists>();
        else
            lists = femaleCustomizer.GetComponent<CharacterLists>();

        if(objectType == ObjectType.Model)
        {
            if(model != ModelType.Shoe)
            {
                string[] modelWords = model.ToString().Split(CharacterLists.delimeters);
                string modelName = "";
                int count = 0;

                foreach(string modelWord in modelWords)
                {
                    count++;
                    modelName += modelWord;
                    
                    if((modelWords.Length - count) > 0)
                        modelName += ' ';
                }
                
                string[] textureWords = productName.Split(CharacterLists.delimeters);
                bool remove = false;
                string textureName = "";
                count = 0;
                
                foreach(string word in textureWords)
                {
                    count++;

                    remove = false;
                    foreach(string modelWord in modelWords)
                    {
                        if(word.ToLower() == modelWord.ToLower())
                        {
                            remove = true;
                            break;
                        }
                    }
                    
                    if(remove)
                        continue;

                    textureName += word;
                }

                string baseTextName = "";

                if(model == ModelType.Short_Sleeved_Shirt || model == ModelType.Long_Sleeved_Shirt)
                {
                    for(int i = 0; i < lists.shirtCustomizer.Count; i++)
                    {
                        if(modelName.ToLower() == lists.shirtCustomizer[i].modelName.ToLower())
                        {
                            if(!lists.shirtCustomizer[i].isPurchased)
                            {
                                lists.shirtCustomizer[i].isPurchased = true;
                            }
                            
                            for(int j = 0; j < lists.shirtCustomizer[i].baseTextures.Count; j++)
                            {
                                baseTextName = "";
                                string[] textWords = lists.shirtCustomizer[i].baseTextures[j].textureName.Split(CharacterLists.delimeters);
                                
                                foreach(string word in textWords)
                                {
                                    baseTextName += word;
                                }

                                if(textureName.ToLower() == baseTextName.ToLower())
                                {
                                    lists.shirtCustomizer[i].baseTextures[j].isPurchased = true;
                                    return;
                                }
                            }

                            Debug.Log("Base texture has not been properly assigned!");
                        }
                    }
                }

                if(model == ModelType.Shorts || model == ModelType.Pants)
                {
                    for(int i = 0; i < lists.pantCustomizer.Count; i++)
                    {
                        if(modelName.ToLower() == lists.pantCustomizer[i].modelName.ToLower())
                        {
                            if(!lists.pantCustomizer[i].isPurchased)
                            {
                                lists.pantCustomizer[i].isPurchased = true;
                            }
                            
                            for(int j = 0; j < lists.pantCustomizer[i].baseTextures.Count; j++)
                            {
                                baseTextName = "";
                                string[] textWords = lists.pantCustomizer[i].baseTextures[j].textureName.Split(CharacterLists.delimeters);

                                foreach(string word in textWords)
                                {
                                    baseTextName += word;
                                }

                                if(textureName.ToLower() == baseTextName.ToLower())
                                {
                                    lists.pantCustomizer[i].baseTextures[j].isPurchased = true;
                                    return;
                                }
                            }
                            
                            Debug.Log("Base texture has not been properly assigned!");
                        }
                    }
                }

                if(model == ModelType.Basic_Backpack || model == ModelType.Sling_Backpack)
                {
                    for(int i = 0; i < lists.bPCustomizer.Count; i++)
                    {
                        if(modelName.ToLower() == lists.bPCustomizer[i].modelName.ToLower())
                        {
                            if(!lists.bPCustomizer[i].isPurchased)
                            {
                                lists.bPCustomizer[i].isPurchased = true;
                            }
                            
                            for(int j = 0; j < lists.bPCustomizer[i].baseTextures.Count; j++)
                            {
                                if(texture == lists.bPCustomizer[i].baseTextures[j].texture)
                                {
                                    lists.bPCustomizer[i].baseTextures[j].isPurchased = true;
                                    return;
                                }
                            }

                            Debug.Log("Base texture has not been properly assigned!");
                        }
                    }
                }
            }
            else
            {
                for(int i = 0; i < lists.shoeCustomizer.baseTextures.Count; i++)
                {
                    if(texture == lists.shoeCustomizer.baseTextures[i].texture)
                    {
                        lists.shoeCustomizer.baseTextures[i].isPurchased = true;
                        return;
                    }
                }

                Debug.Log("Base texture has not been properly assigned!");
            }
        }
        else if(objectType == ObjectType.Pattern)
        {
            bool isFound = false;

            for(int i = 0; i < lists.shirtCustomizer.Count; i++)
            {   
                for(int j = 0; j < lists.shirtCustomizer[i].patterns.Count; j++)
                {
                    if(texture == lists.shirtCustomizer[i].patterns[j].texture)
                    {
                        lists.shirtCustomizer[i].patterns[j].isPurchased = true;
                        isFound = true;
                        break;
                    }
                }
            }

            if(!isFound)
                Debug.Log("Pattern has not been properly assigned!");

            isFound = false;

            for(int i = 0; i < lists.pantCustomizer.Count; i++)
            {   
                for(int j = 0; j < lists.pantCustomizer[i].patterns.Count; j++)
                {
                    if(texture == lists.pantCustomizer[i].patterns[j].texture)
                    {
                        lists.pantCustomizer[i].patterns[j].isPurchased = true;
                        isFound = true;
                        break;
                    }
                }
            }

            if(!isFound)
                Debug.Log("Pattern has not been properly assigned!");
                
            isFound = false;

            for(int i = 0; i < lists.bPCustomizer.Count; i++)
            {   
                for(int j = 0; j < lists.bPCustomizer[i].patterns.Count; j++)
                {
                    if(texture == lists.bPCustomizer[i].patterns[j].texture)
                    {
                        lists.bPCustomizer[i].patterns[j].isPurchased = true;
                        isFound = true;
                        break;
                    }
                }
            }

            if(!isFound)
                Debug.Log("Pattern has not been properly assigned!");
        }
        else
        {
            int wordCount = 0;
            string name = "";
            string[] words = productName.Split(CharacterLists.delimeters);
            
            foreach(string word in words)
            {
                wordCount++;

                if(word.ToLower() == "logo" || word.ToLower() == "sling" || word.ToLower() == "basic" || word.ToLower() == "backpack")
                {
                    continue;
                }
                else if(word.ToLower() == "broken")
                {
                    name += word;
                    name += ' ';
                }
                else
                    name += word;
            }
            
            for(int i = 0; i < lists.shirtCustomizer.Count; i++)
            {
                for(int j = 0; j < lists.shirtCustomizer[i].logos.Count; j++)
                {
                    if(name.ToLower() == lists.shirtCustomizer[i].logos[j].textureName.ToLower())
                    {
                        lists.shirtCustomizer[i].logos[j].isPurchased = true;
                        break;
                    }
                }
            }

            for(int i = 0; i < lists.bPCustomizer.Count; i++)
            {   
                for(int j = 0; j < lists.bPCustomizer[i].logos.Count; j++)
                {
                    if(name.ToLower() == lists.bPCustomizer[i].logos[j].textureName.ToLower())
                    {
                        lists.bPCustomizer[i].logos[j].isPurchased = true;
                        break;
                    }
                }
            }
        }
    }

    private void SetMaleOutfit()
    {
        Material temp;

        //Set default models of the outfit
        male_shortshirt.SetActive(true);
        male_longshirt.SetActive(false);
        male_shorts.SetActive(true);
        male_pants.SetActive(false);
        male_basicbp.SetActive(false);
        male_slingbp.SetActive(false);

        //Set shirt materials
        temp = male_shortshirt.GetComponent<SkinnedMeshRenderer>().material;
        temp.SetTexture("baseMap", male_shortshirt_base);
        temp.SetTexture("additionalColorMap", no_selection);
        temp.SetTexture("optionalLogo", male_shortshirt_d3logo);
        temp = male_longshirt.GetComponent<SkinnedMeshRenderer>().material;
        temp.SetTexture("baseMap", no_selection);
        temp.SetTexture("additionalColorMap", no_selection);
        temp.SetTexture("optionalLogo", male_longshirt_d3logo);

        //Set pants materials
        temp = male_shorts.GetComponent<SkinnedMeshRenderer>().material;
        temp.SetTexture("baseMap", male_shorts_base);
        temp.SetTexture("additionalColorMap", no_selection);
        temp.SetTexture("optionalLogo", no_selection);
        temp = male_pants.GetComponent<SkinnedMeshRenderer>().material;
        temp.SetTexture("baseMap", no_selection);
        temp.SetTexture("additionalColorMap", no_selection);
        temp.SetTexture("optionalLogo", no_selection);

        //Set shoe materials
        temp = male_shoe.GetComponent<SkinnedMeshRenderer>().sharedMaterial;
        temp.SetTexture("baseMap", male_shoe_base);
        temp.SetTexture("additionalColorMap", no_selection);
        temp.SetTexture("optionalLogo", no_selection);
        temp.SetColor("baseMapColor", Color.blue);

        //Set backpack materials
        temp =  male_basicbp.GetComponent<SkinnedMeshRenderer>().material;
        temp.SetTexture("baseMap", no_selection);
        temp.SetTexture("additionalColorMap", no_selection);
        temp.SetTexture("optionalLogo", basicbp_d3logo);
        temp = male_slingbp.GetComponent<SkinnedMeshRenderer>().material;
        temp.SetTexture("baseMap", no_selection);
        temp.SetTexture("additionalColorMap", no_selection);
        temp.SetTexture("optionalLogo", slingbp_d3logo);
    }

    private void SetFemaleOutfit()
    {
        Material temp;

        //Set default models of the outfit
        female_shortshirt.SetActive(true);
        female_longshirt.SetActive(false);
        female_shorts.SetActive(true);
        female_pants.SetActive(false);
        female_basicbp.SetActive(false);
        female_slingbp.SetActive(false);

        //Set shirt materials
        temp = female_shortshirt.GetComponent<SkinnedMeshRenderer>().material;
        temp.SetTexture("baseMap", female_shortshirt_base);
        temp.SetTexture("additionalColorMap", no_selection);
        temp.SetTexture("optionalLogo", female_shortshirt_d3logo);
        temp = female_longshirt.GetComponent<SkinnedMeshRenderer>().material;
        temp.SetTexture("baseMap", no_selection);
        temp.SetTexture("additionalColorMap", no_selection);
        temp.SetTexture("optionalLogo", female_longshirt_d3logo);

        //Set pants materials
        temp = female_shorts.GetComponent<SkinnedMeshRenderer>().material;
        temp.SetTexture("baseMap", female_shorts_base);
        temp.SetTexture("additionalColorMap", no_selection);
        temp.SetTexture("optionalLogo", no_selection);
        temp = female_pants.GetComponent<SkinnedMeshRenderer>().material;
        temp.SetTexture("baseMap", no_selection);
        temp.SetTexture("additionalColorMap", no_selection);
        temp.SetTexture("optionalLogo", no_selection);

        //Set shoe materials
        temp = female_shoe.GetComponent<SkinnedMeshRenderer>().sharedMaterial;
        temp.SetTexture("baseMap", female_shoe_base);
        temp.SetTexture("additionalColorMap", no_selection);
        temp.SetTexture("optionalLogo", no_selection);
        temp.SetColor("baseMapColor", Color.blue);

        //Set backpack materials
        temp = female_basicbp.GetComponent<SkinnedMeshRenderer>().material;
        temp.SetTexture("baseMap", no_selection);
        temp.SetTexture("additionalColorMap", no_selection);
        temp.SetTexture("optionalLogo", basicbp_d3logo);
        temp = female_slingbp.GetComponent<SkinnedMeshRenderer>().material;
        temp.SetTexture("baseMap", no_selection);
        temp.SetTexture("additionalColorMap", no_selection);
        temp.SetTexture("optionalLogo", slingbp_d3logo);
    }
}
