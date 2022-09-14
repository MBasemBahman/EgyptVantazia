namespace API.ActionFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AllowAnonymousAttribute : Attribute
    { }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AllowAllAttribute : Attribute
    { }
}
