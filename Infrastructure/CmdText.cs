namespace Infrastructure
{
    internal class CmdText
    {
        public static string CreateTable = @"CREATE TABLE IF NOT EXISTS public.messages (
                                              Id SERIAL PRIMARY KEY,
                                              Content VARCHAR(128) NOT NULL,
                                              SavedAt TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
                                              SentAt TIMESTAMP WITH TIME ZONE NOT NULL
                                            );";
    }
}
