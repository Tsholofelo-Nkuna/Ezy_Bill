
using System.Text.Json;


namespace ClientManagement.Utils
{ 
    public static class ObjectExtensions
    {
        public static string ToJsonString(this object obj)
        {
            return JsonSerializer.Serialize(obj, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        }

        public static TObject Clone<TObject>(this object obj) where TObject : new()
        {
            var cloned = new TObject();
            if (obj is TObject toBeCloned)
            {
                cloned.GetType().GetProperties().Where(p => p is { CanWrite: true, CanRead: true }).ToList().ForEach(prop => {
                    prop.SetValue(cloned, prop.GetValue(toBeCloned));
                });
                return cloned;
            }
            else
            {
                return new TObject();
            }
            
        }
    }
}
