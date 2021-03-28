using UnityEngine;
using JotunnLib.Managers;

namespace JotunnLib.Entities
{
    /// <summary>
    /// Used to add new ItemConversions to a <see cref="CookingStation"/>
    /// </summary>
    public class ItemConversionConfig
    {
        /// <summary>
        /// Amount of time it takes to perform the conversion
        /// </summary>
        public float CookTime { get; set; } = 10f;

        /// <summary>
        /// The name of the item prefab you need to put on the CookingStation
        /// </summary>
        public string FromItem { get; set; }
        
        /// <summary>
        /// The name of the item prefab that your "FromItem" will be turned into
        /// </summary>
        public string ToItem { get; set; }

        /// <summary>
        /// Turns the ItemConversionConfig into a Valheim <see cref="CookingStation.ItemConversion"/> item
        /// </summary>
        /// <returns></returns>
        public CookingStation.ItemConversion GetItemConversion()
        {
            CookingStation.ItemConversion conv = new CookingStation.ItemConversion()
            {
                m_cookTime = CookTime,
                m_from = ObjectManager.Instance.GetItemDrop(FromItem),
                m_to = ObjectManager.Instance.GetItemDrop(ToItem)
            };

            if (conv.m_from == null || conv.m_to == null)
            {
                Debug.LogError("Failed to create ItemConversion from ItemConversionConfig with null From or To items");
                return null;
            }

            return conv;
        }
    }
}
