namespace ToDoList.Properties
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.CompilerServices;

    [CompilerGenerated, GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"), DebuggerNonUserCode]
    internal class Resources
    {
        private static CultureInfo resourceCulture;
        private static System.Resources.ResourceManager resourceMan;

        internal Resources()
        {
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }
            set
            {
                resourceCulture = value;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (resourceMan == null)
                {
                    System.Resources.ResourceManager temp = new System.Resources.ResourceManager("ToDoList.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
    }
}

