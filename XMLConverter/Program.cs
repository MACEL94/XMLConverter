using XMLConverter.Classes;

namespace XMLConverter
{
    public class Program
    {
        #region Private Methods

        private static void Main()
        {
            var manager = new XmlConverterManager();
            manager.Start();
        }

        #endregion Private Methods
    }
}