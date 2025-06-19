using DbAccess;
using HttpAccess;

namespace Core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var db = new DbClient();
            var api = new Client();

            SyncService syncService = new SyncService(db, api);
            syncService.Run();
        }
    }
}