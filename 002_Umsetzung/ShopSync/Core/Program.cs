namespace Core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SyncService syncService = new SyncService();
            syncService.Run();
        }
    }
}
