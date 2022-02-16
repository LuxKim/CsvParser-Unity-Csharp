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

}
