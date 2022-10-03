namespace Dashboard.Utility
{
    public class LocalizationManager : ILocalizationManager
    {
        private readonly IStringLocalizer localizer;
        public LocalizationManager(IStringLocalizerFactory factory)
        {
            AssemblyName assemblyName = new(typeof(ResourcesFile).GetTypeInfo().Assembly.FullName);
            localizer = factory.Create(nameof(ResourcesFile), assemblyName.Name);
        }

        public string Get(string key)
        {
            return localizer[key];
        }
    }
}
