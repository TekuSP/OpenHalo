//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OpenHalo.Resources
{
    
    internal partial class ResourceDictionary
    {
        private static System.Resources.ResourceManager manager;
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if ((ResourceDictionary.manager == null))
                {
                    ResourceDictionary.manager = new System.Resources.ResourceManager("OpenHalo.Resources.ResourceDictionary", typeof(ResourceDictionary).Assembly);
                }
                return ResourceDictionary.manager;
            }
        }
        internal static nanoFramework.UI.Bitmap GetBitmap(ResourceDictionary.BitmapResources id)
        {
            return ((nanoFramework.UI.Bitmap)(nanoFramework.Runtime.Native.ResourceUtility.GetObject(ResourceManager, id)));
        }
        internal static nanoFramework.UI.Font GetFont(ResourceDictionary.FontResources id)
        {
            return ((nanoFramework.UI.Font)(nanoFramework.Runtime.Native.ResourceUtility.GetObject(ResourceManager, id)));
        }
        [System.SerializableAttribute()]
        internal enum FontResources : short
        {
            courierregular10 = -25095,
            small = 13070,
            NinaB = 18060,
        }
        [System.SerializableAttribute()]
        internal enum BitmapResources : short
        {
            settings = -20938,
            wifiConnect = -18698,
            heatbed = -3953,
            reboot = 9673,
            nozzle = 11401,
            moonraker = 15675,
            wifi = 17980,
        }
    }
}
