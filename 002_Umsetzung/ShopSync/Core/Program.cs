using Core.io;
using Core.testdata;
using DbAccess;
using DBModel;
using HttpAccess;
using HttpModel;

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
