namespace Lab4
{
    public abstract class Connector
    {
        public abstract Resource Create();
    }
    
    
    public class DBConnector: Connector
    {
        public override Resource Create()
        {
            return new MySQLDAO("localhost", 3306, "sqllab4", "root", "yolo2802");
        }
    }
    
    public class FileConnector: Connector
    {
        public override Resource Create()
        {
            return new FileDAO(@"..\stores.csv", @"..\products.csv");
        }
    }
}