using Newtonsoft.Json;
using System.Data;

public static class DataTableExtensions
{
    public static string ToJson(this DataTable dataTable)
    {
        return JsonConvert.SerializeObject(dataTable, Formatting.Indented, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full
        });
    }

    public static DataTable ToDataTable(this string json)
    {
        return JsonConvert.DeserializeObject<DataTable>(json, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full
        });
    }
}
