
using System.Reflection;
using System.Text;
namespace BO;

public static class Tools
{

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


    //public static void ToStringProperty<T>(this T t)
    //{
    //    string str = "";
    //    foreach (PropertyInfo item in t.GetType().GetProperties())
    //        str += "\n" + item.Name + ":" + item.GetValue(t, null);
    //    Console.WriteLine(str);

    //}

}

