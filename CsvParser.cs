using System.Collections.Generic;
using System.Reflection;

public class CsvParser
{
    /// <summary>
    /// load data from csv file, and change to T array.
    /// </summary>
    /// <param name="csvFilePath">ex) System.IO.Path.Combine(Application.dataPath, "samplePeople.csv")</param>
    public static T[] Deserialize<T>(string csvFilePath) where T : new()
    {
        string[] csvData = System.IO.File.ReadAllLines(csvFilePath);

        if(csvData == null || csvData.Length < 2)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.Log("csv data is null or too short");
#endif
            return default;
        }

        List<T> tList = new List<T>();
        string[] memeberNames = csvData[0].Split(',');

        //for(int i = 0; i < memeberNames.Length; i++)
        // {
        for (int j = 1; j < csvData.Length; j++)
        {
            string[] data = csvData[j].Split(',');

            var fields = typeof(T).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            T t = new T();

            foreach (var fInfo in fields)
            {
                for (int i = 0; i < memeberNames.Length; i++)
                {
                    if (fInfo.Name == memeberNames[i])
                        SetValue(ref t, fInfo, data[i]);//fInfo.SetValue(t, data[i]);
                }
                    
            }

            tList.Add(t);
        }
        //}

        return tList.ToArray();
    }

    /// <summary>
    /// change string value in csv to each data type (nested class is not supported)
    /// </summary>
    private static void SetValue<T>(ref T t, FieldInfo fInfo, string value)
    {
        //정수 (integers)
        if (fInfo.FieldType == typeof(int))
        {
            fInfo.SetValue(t, int.Parse(value)); //data[i]);
        }
        else if (fInfo.FieldType == typeof(long))
        {
            fInfo.SetValue(t, long.Parse(value));
        }
        else if (fInfo.FieldType == typeof(sbyte))
        {
            fInfo.SetValue(t, sbyte.Parse(value));
        }
        else if (fInfo.FieldType == typeof(byte))
        {
            fInfo.SetValue(t, byte.Parse(value));
        }
        else if (fInfo.FieldType == typeof(short))
        {
            fInfo.SetValue(t, short.Parse(value));
        }
        else if (fInfo.FieldType == typeof(ushort))
        {
            fInfo.SetValue(t, ushort.Parse(value));
        }
        else if (fInfo.FieldType == typeof(uint))
        {
            fInfo.SetValue(t, uint.Parse(value));
        }
        else if (fInfo.FieldType == typeof(ulong))
        {
            fInfo.SetValue(t, ulong.Parse(value));
        }
        else if (fInfo.FieldType == typeof(System.IntPtr))
        {
            fInfo.SetValue(t, System.Convert.ChangeType(value, typeof(System.IntPtr)));
        }
        else if (fInfo.FieldType == typeof(System.UIntPtr))
        {
            fInfo.SetValue(t, System.Convert.ChangeType(value, typeof(System.UIntPtr)));
        }
        //소수 (rational numbers)
        else if (fInfo.FieldType == typeof(float))
        {
            fInfo.SetValue(t, float.Parse(value));
        }
        else if (fInfo.FieldType == typeof(double))
        {
            fInfo.SetValue(t, double.Parse(value));
        }
        else if (fInfo.FieldType == typeof(decimal))
        {
            fInfo.SetValue(t, decimal.Parse(value));
        }
        //문자 (literal)
        else if (fInfo.FieldType == typeof(char))
        {
            fInfo.SetValue(t, char.Parse(value));
        }
        else if (fInfo.FieldType == typeof(string))
        {
            fInfo.SetValue(t, value);
        }
        //열거 / 그외 (enum / etc)
        else
        {
            try
            {
                if (System.Enum.IsDefined(fInfo.FieldType, value))
                {
                    fInfo.SetValue(t, System.Enum.Parse(fInfo.FieldType, value));
                }
            }
            catch
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError("Unsupported type is exist");
#endif
            }
            return;
        }
    }

    /// <summary>
    /// save data as csv file
    /// </summary>
    /// <param name="data">data for saving to csv</param>
    /// <param name="savePath">ex) System.IO.Path.Combine(Application.dataPath, "samplePeople.csv")</param>
    /// <returns></returns>
    public static string[] ToCsv(object[] data, string savePath = null)
    {
        var fields = data[0].GetType().GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        List<string[]> fileContents = new List<string[]>();
        List<string> memberNames = new List<string>();

        for (int i = 0; i < data.Length; i++)
        {
            List<string> content = new List<string>();
            for (int j = 0; j < fields.Length; j++)
            {
                if(i == 0)
                    memberNames.Add(fields[j].Name);

                var value = fields[j].GetValue(data[i]);
                content.Add(value.ToString());
            }
            fileContents.Add(content.ToArray());
        }

        fileContents.Insert(0, memberNames.ToArray());

        List<string> fileFinal = new List<string>();
        foreach(var content in fileContents)
        {
            string line = "";
            for(int i = 0; i < content.Length; i++)
            {
                if (i == content.Length - 1)
                    line += content[i];
                else
                    line += string.Format("{0},", content[i]);
            }
            fileFinal.Add(line);
        }

        if (!string.IsNullOrEmpty(savePath))
            System.IO.File.WriteAllLines(savePath, fileFinal);

        return fileFinal.ToArray();
    }
}
