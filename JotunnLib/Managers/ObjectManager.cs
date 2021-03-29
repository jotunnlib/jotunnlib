using System;
using System.Collections.Generic;
using UnityEngine;
using JotunnLib.Utils;
using JotunnLib.Entities;

namespace JotunnLib.Managers
{
    public class ObjectManager : Manager
    {
        /// <summary>
        /// Single global instance of the ObjectManager. Use this to interact with the ObjectManager.
        /// </summary>
        public static ObjectManager Instance { get; private set; }

        /// <summary>
        /// Event called when it's time to register your objects (items, recipes, etc.) into the game. <br/>
        /// All items <b>MUST</b> be registered in a handler for this event.
        /// </summary>
        public event EventHandler ObjectRegister;
        
        /// <summary>
        /// Event called after objects (items, recipes, etc.) from the base game and all mods have all been loaded. <br/>
        /// Use this event handler if you wish to modify existing items, recipes, etc.
        /// </summary>
        public event EventHandler ObjectsRegistered;
        
        internal List<GameObject> Items = new List<GameObject>();
        internal List<Recipe> Recipes = new List<Recipe>();
        internal Dictionary<CookingStation, CookingStation.ItemConversion> ItemConversions =
            new Dictionary<CookingStation, CookingStation.ItemConversion>();
        
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Error, two instances of singleton: " + this.GetType().Name);
                return;
            }

            Instance = this;
        }

        internal override void Register()
        {
            Debug.Log("---- Registering custom objects ----");

            // Clear existing items and recipes
            Items.Clear();
            Recipes.Clear();

            // Register new items and recipes
            ObjectRegister?.Invoke(null, EventArgs.Empty);

            // Call post-register hook
            ObjectsRegistered?.Invoke(null, EventArgs.Empty);
        }

        internal override void Load()
        {
            Debug.Log("---- Loading custom objects ----");

            // Load items
            foreach (GameObject obj in Items)
            {
                ObjectDB.instance.m_items.Add(obj);
                Debug.Log("Loaded item: " + obj.name);
            }

            // Load recipes
            foreach (Recipe recipe in Recipes)
            {
                ObjectDB.instance.m_recipes.Add(recipe);
                Debug.Log("Loaded item recipe: " + recipe.name);
            }

            // Update hashes
            ReflectionUtils.InvokePrivate(ObjectDB.instance, "UpdateItemHashes");

            // Load item conversions
            foreach (var pair in ItemConversions)
            {
                if (pair.Key == null || pair.Value == null)
                {
                    Debug.LogError($"Failed to load invalid ItemConversion pair: ${pair.Key}, ${pair.Value}");
                    continue;
                }

                Debug.Log($"Loaded item conversion for CookingStation: ${pair.Key.name}");
                pair.Key.m_conversion.Add(pair.Value);
            }
        }

        /// <summary>
        /// Registers a new item from given prefab name
        /// </summary>
        /// <param name="prefabName">Name of prefab to use for item</param>
        public void RegisterItem(string prefabName)
        {
            Items.Add(PrefabManager.Instance.GetPrefab(prefabName));
        }

        /// <summary>
        /// Registers new item from given GameObject. GameObject MUST be also registered as a prefab
        /// </summary>
        /// <param name="item">GameObject to use for item</param>
        public void RegisterItem(GameObject item)
        {
            // Set layer if not already set
            if (item.layer == 0)
            {
                item.layer = LayerMask.NameToLayer("item");
            }

            Items.Add(item);
        }

        /// <summary>
        /// Registers a new recipe
        /// </summary>
        /// <param name="recipeConfig">Recipe details</param>
        public void RegisterRecipe(RecipeConfig recipeConfig)
        {
            if (recipeConfig == null)
            {
                Debug.LogError("Failed to register null RecipeConfig");
                return;
            }

            RegisterRecipe(recipeConfig.GetRecipe());
        }

        /// <summary>
        /// Registers a new recipe
        /// </summary>
        /// <param name="recipe">Recipe item to register</param>
        public void RegisterRecipe(Recipe recipe)
        {
            if (recipe == null)
            {
                Debug.LogError("Failed to register null recipe");
                return;
            }

            if (Recipes.Exists(r => r.name == recipe.name))
            {
                Debug.LogError($"Failed to register recipe with duplicate name: ${recipe.name}");
                return;
            }

            Recipes.Add(recipe);
        }

        /// <summary>
        /// Registers a new item conversion for any prefab that has a CookingStation component (such as a "piece_cookingstation"). <br/>
        /// <b>MUST</b> be called within a handler for the <see cref="ObjectRegister"/> event.
        /// </summary>
        /// <param name="prefabName">The name of the prefab that has the CookingStation</param>
        /// <param name="itemConversion">Item conversion details</param>
        public void RegisterItemConversion(ItemConversionConfig itemConversion)
        {
            if (itemConversion == null)
            {
                Debug.LogError("Failed to register ItemConversion with null config");
                return;
            }

            GameObject prefab = PrefabManager.Instance.GetPrefab(itemConversion.CookingStation);
            CookingStation cookingStation = prefab?.GetComponent<CookingStation>();

            if (!prefab || !cookingStation)
            {
                Debug.LogError($"Failed to register ItemConversion for invalid CookingStation prefab: ${itemConversion.CookingStation}");
                return;
            }

            CookingStation.ItemConversion conv = itemConversion.GetItemConversion();
            
            if (conv == null)
            {
                Debug.LogError($"Failed to register ItemConversion on ${itemConversion.CookingStation} with invalid ItemConversionConfig");
                return;
            }

            if (cookingStation.m_conversion.Exists(c => c.m_from == conv.m_from))
            {
                Debug.LogError($"Failed to register ItemConversion on ${itemConversion.CookingStation} for item with existing conversion: ${conv.m_from.name}");
                return;
            }

            if (!conv.m_from.transform.Find("attach"))
            {
                Debug.LogWarning($"Warning, 'from' item for ItemConversion ${itemConversion.FromItem} does not have 'attach' child. This item may not show up correctly when placed on the CookingStation. See our docs for more info");
            }

            if (!conv.m_to.transform.Find("attach"))
            {
                Debug.LogWarning($"Warning, 'to' item for ItemConversion ${itemConversion.ToItem} does not have 'attach' child. This item may not show up correctly when placed on the CookingStation. See our docs for more info");
            }

            ItemConversions.Add(cookingStation, conv);
        }

        /// <summary>
        /// Gets an existing item prefab
        /// </summary>
        /// <param name="name">Prefab name</param>
        /// <returns>Existing prefab, or null if it does not exist</returns>
        public GameObject GetItemPrefab(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            return ObjectDB.instance.GetItemPrefab(name);
        }

        /// <summary>
        /// Gets an existing item by prefab name
        /// </summary>
        /// <param name="name">Prefab name</param>
        /// <returns>ItemDrop component on item prefab</returns>
        public ItemDrop GetItemDrop(string name)
        {
            return GetItemPrefab(name)?.GetComponent<ItemDrop>();
        }

        /// <summary>
        /// Searches for a recipe by name
        /// </summary>
        /// <param name="name">Name of the recipe to look for</param>
        /// <returns>The recipe if it exists, or null otherwise</returns>
        public Recipe GetRecipe(string name)
        {
            Recipe recipe = Recipes.Find(r => r.name == name);

            if (recipe != null)
            {
                return recipe;
            }

            recipe = ObjectDB.instance.m_recipes.Find(r => r.name == name);
            return recipe;
        }
    }
}
