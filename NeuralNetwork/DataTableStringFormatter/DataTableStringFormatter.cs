using System.Data;
using System.Text;

namespace DataTableStringFormatter
{
    public static class DataTableStringFormatter
    {

        public static string Format(DataTable table)
        {
            StringBuilder stringBuilder = new StringBuilder();
            
            foreach (DataColumnCollection x in table.Columns)
            {
                
            }
            
            return stringBuilder.ToString();
        }
        
    }
}