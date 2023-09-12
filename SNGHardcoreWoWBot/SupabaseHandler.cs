using Supabase;

namespace SupabaseStuff
{
    internal class SupabaseHandler
    {
        private static readonly Client supabase = new("https://xsnpijpuuaxcfwkwxjxi.supabase.co",
            "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InhzbnBpanB1dWF4Y2Z3a3d4anhpIiwicm9sZSI6InNlcnZpY2Vfcm9sZSIsImlhdCI6MTY5NDQ3NzQ2MCwiZXhwIjoyMDEwMDUzNDYwfQ.XdePbVEOsSWH6OEVRvDeYJ6nStStyEeF3UVkuqSriws",
            new() { AutoConnectRealtime = true });

        public static Client SupabaseClient { get; private set; } = supabase;

        public static async Task InitializeSupabase()
        {
            await supabase.InitializeAsync();
        }
    }
}