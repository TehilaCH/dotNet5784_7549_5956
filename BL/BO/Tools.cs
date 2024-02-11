
using System.Reflection;
using System.Text;
namespace BO;

public static class Tools
{
    /// <summary>
    /// A function that overrides the ToString function and prints objects
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string ToStringProperty<T> (T obj)
    {
        string str = "";
        var properties=typeof(T).GetProperties();
        foreach (var property in properties) 
        { 
            var value = property.GetValue(obj);
            if(value is IEnumerable<object>items)
            {
                str += $"{property.Name}:\n";
                foreach (var item in items)
                {
                    str += $"-{item}\n";

                }
            }
            else
            {
                str += $"{property.Name}:{value}\n";
            }
        }
        return str;
    }



}

