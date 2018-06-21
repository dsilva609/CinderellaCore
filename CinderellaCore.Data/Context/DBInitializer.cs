namespace CinderellaCore.Data.Context
{
    public class DBInitializer
    {
        public static void Initialize(CinderellaCoreContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}