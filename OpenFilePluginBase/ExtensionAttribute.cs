namespace OpenFilePluginBase
{
    [AttributeUsage(System.AttributeTargets.Class)]
    public class ExtensionAttribute : Attribute
    {
        public ExtensionAttribute(string extension)
        {
            this.Extension = extension;
        }

        public string Extension { get; set; }
    }
}
