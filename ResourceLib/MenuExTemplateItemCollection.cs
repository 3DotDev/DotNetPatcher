﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;

namespace Vestris.ResourceLib
{
    /// <summary>
    /// A collection of menu items.
    /// </summary>
    public class MenuExTemplateItemCollection : List<MenuExTemplateItem>
    {
        /// <summary>
        /// A collection of extended menu items.
        /// </summary>
        public MenuExTemplateItemCollection()
        {

        }

        /// <summary>
        /// Read the menu item collection.
        /// </summary>
        /// <param name="lpRes">Address in memory.</param>
        /// <returns>End of the menu item structure.</returns>
        internal IntPtr Read(IntPtr lpRes)
        {
            while(true)
            {
                lpRes = ResourceUtil.Align(lpRes.ToInt64());

                User32.MENUEXITEMTEMPLATE childItem = (User32.MENUEXITEMTEMPLATE)Marshal.PtrToStructure(
                    lpRes, typeof(User32.MENUEXITEMTEMPLATE));

                MenuExTemplateItem childMenu = null;
                if ((childItem.bResInfo & (uint) User32.MenuResourceType.Sub) > 0)
                    childMenu = new MenuExTemplateItemPopup();
                else
                    childMenu = new MenuExTemplateItemCommand();

                lpRes = childMenu.Read(lpRes);
                Add(childMenu);

                if ((childItem.bResInfo & (uint) User32.MenuResourceType.Last) > 0)
                    break;
            }

            return lpRes;
        }

        /// <summary>
        /// Write the menu collection to a binary stream.
        /// </summary>
        /// <param name="w">Binary stream.</param>
        internal void Write(BinaryWriter w)
        {
            foreach (MenuExTemplateItem menuItem in this)
            {
                ResourceUtil.PadToDWORD(w);
                menuItem.Write(w);
            }
        }

        /// <summary>
        /// String representation in the MENU format.
        /// </summary>
        /// <returns>String representation.</returns>
        public override string ToString()
        {
            return ToString(0);
        }

        /// <summary>
        /// String representation in the MENU format.
        /// </summary>
        /// <param name="indent">Indent.</param>
        /// <returns>String representation.</returns>
        public string ToString(int indent)
        {
            StringBuilder sb = new StringBuilder();
            if (Count > 0)
            {
                sb.AppendLine(string.Format("{0}BEGIN", new String(' ', indent)));
                foreach (MenuExTemplateItem child in this)
                {
                    sb.Append(child.ToString(indent + 1));
                }
                sb.AppendLine(string.Format("{0}END", new String(' ', indent)));
            }
            return sb.ToString();
        }
    }
}
