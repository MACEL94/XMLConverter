using XMLConverter.Classes;

namespace XMLConverter
{
    public class Program
    {
        #region Private Methods

        private static void Main()
        {
            XmlConverterManager manager = new XmlConverterManager();
            manager.Start();
        }

        #endregion Private Methods
    }
}