using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;

public class CSVLoader : MonoBehaviour
{
  private TextAsset _csvFile;
  private readonly char lineSeperator = '\n';
  private readonly char surround = '"'; // Символ определение записи
  private readonly string[] fieldSeperator = { "\",\"" };

  //======================================

  public Dictionary<string, string> GetDictionaryValues(string attributeId)
  {
    Dictionary<string, string> dictionary = new Dictionary<string, string>();

    string[] lines = _csvFile.text.Split(lineSeperator);

    int attributeIndex = -1;

    string[] headers = lines[0].Split(fieldSeperator, StringSplitOptions.None);

    for (int i = 0; i < headers.Length; i++)
    {
      if (headers[i].Contains(attributeId))
      {
        attributeIndex = i;
        break;
      }
    }

    Regex CSVParser = new(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

    for (int i = 1; i < lines.Length; i++)
    {
      string line = lines[i];

      string[] fields = CSVParser.Split(line);

      for (int j = 0; j < fields.Length; j++)
      {
        fields[j] = fields[j].TrimStart(' ', surround);
        fields[j] = fields[j].TrimEnd(surround);
      }

      if (fields.Length > attributeIndex)
      {
        var key = fields[0];

        if (dictionary.ContainsKey(key)) { continue; }

        var value = fields[attributeIndex];

        dictionary.Add(key, value);
      }
    }

    return dictionary;
  }

  //======================================

  public void LoadCSV()
  {
    _csvFile = Resources.Load<TextAsset>("Localisation");
  }

  //======================================
}