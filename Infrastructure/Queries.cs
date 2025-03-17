namespace Infrastructure
{
    public class Queries
    {
        public static string CreateTable = @"CREATE TABLE IF NOT EXISTS public.messages (
                                              Id SERIAL PRIMARY KEY,
                                              Content VARCHAR(128) NOT NULL,
                                              SavedAt TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
                                              SentAt TIMESTAMP WITH TIME ZONE NOT NULL);";

        public static string CheckTableExist = @"SELECT EXISTS (
                                                SELECT FROM information_schema.tables 
                                                WHERE table_schema = 'public' 
                                                AND table_name = @tableName);";

        public static string SaveMessage = @"INSERT INTO messages (Content, SentAt) VALUES (@Content, @SentAt) RETURNING id, Content, SavedAt";

        public static string GetMessages = @"SELECT Id, Content, SentAt FROM messages WHERE SentAt >= NOW() - INTERVAL '10 minutes';";
    }
}
