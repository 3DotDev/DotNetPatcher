using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.IO;

namespace Vestris.ResourceLib
{
    /// <summary>
    /// This structure depicts the organization of data in a hardware-independent icon resource.
    /// </summary>
    public class IconDirectoryResource : DirectoryResource<IconResource>
    {
        /// <summary>
        /// A hardware-independent icon resource.
        /// </summary>
        /// <param name="hModule">Module handle.</param>
        /// <param name="hResource">Resource ID.</param>
        /// <param name="type">Resource type.</param>
        /// <param name="name">Resource name.</param>
        /// <param name="language">Language ID.</param>
        /// <param name="size">Resource size.</param>
        internal IconDirectoryResource(IntPtr hModule, IntPtr hResource, ResourceId type, ResourceId name, ushort language, int size)
            : base(hModule, hResource, type, name, language, size)
        {
            
        }

        /// <summary>
        /// A new hardware-independent icon resource.
        /// </summary>
        public IconDirectoryResource()
            : base(Kernel32.ResourceTypes.RT_GROUP_ICON)
        {

        }

        /// <summary>
        /// A new collection of icons that can be embedded into an executable file.
        /// </summary>
        public IconDirectoryResource(IconFile iconFile)
            : base(Kernel32.ResourceTypes.RT_GROUP_ICON)
        {            
            for (ushort id = 0; id < iconFile.Icons.Count; id++)
            {
                IconResource iconResource = new IconResource(
                    iconFile.Icons[id], new ResourceId(id), _language);
                Icons.Add(iconResource);
            }
        }
    }
}
