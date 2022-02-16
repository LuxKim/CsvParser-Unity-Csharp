# CSV Parser for C#

- class to CSV save directly / csv to class load directly

## Method

load data from csv file, and change to class type array.
```c#
public static T[] Deserialize<T>(string csvFilePath)
```

save data as csv file
```c#
public static string[] ToCsv(object[] data, string savePath = null)
```

## Examples

```c#
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Tester : MonoBehaviour
{
    [SerializeField]
    private List<CsvSampleClass> sampleLoadedList = new List<CsvSampleClass>();

    // Use this for initialization
    void Start()
    {
        var person = new CsvSampleClass()
        {
            name = "제임스",
            hp = 12.45f,
            atk = 10,
            level = Level.High
        };
        List<CsvSampleClass> list = new List<CsvSampleClass>();
        list.Add(person);
        list.Add(person);

        CsvParser.ToCsv(list.ToArray(), Application.dataPath + "/.." + "/samplePeople.csv");

        sampleLoadedList = CsvParser.Deserialize<CsvSampleClass>(Application.dataPath + "/.." + "/samplePeople.csv").ToList();
    }
}

[System.Serializable]
public class CsvSampleClass : ModelBase
{
    public string name;
    public float hp;
    public int atk;
    public Level level;
}

[System.Serializable]
public enum Level
{ Low, High}
```
