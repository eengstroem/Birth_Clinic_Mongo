namespace Library.Config
{
    public class MongoConnection
    {
        public string ConnString;
        public MongoConnection(string connString)
        {
            ConnString = connString;
        }
    }
}
