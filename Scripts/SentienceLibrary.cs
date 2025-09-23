using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;

namespace Sentience
{
    public static class SentienceLibrary
    {
        public static string GetNumbersFromString(string input)
        {
            return new string(input.Where(c => char.IsDigit(c)).ToArray());
        }

        public static List<T> GetListFromJsonData<T>(string description)
        {
            List<T> list = new();
            try
            {
                string json = description.Replace("```json", "").Replace("```", "").Replace("\n", "").Replace("`", "").Replace("“", "\"");
                try
                {
                    if (!json.StartsWith("[") && json.Contains("["))
                    {
                        json = json.TrimStart(json.Split("[")[0]);
                        json = json.TrimEnd(json.Split("]")[json.Split("]").Length - 1]);
                        json = json.Replace(" ", "");
                        json = json.Trim();
                        if (!json.StartsWith("[")) json = "[" + json;
                        if (!json.EndsWith("]")) json = json + "]";
                    }
                    string jsonParseList = json.Replace("\"div\": \"0\",", "").Replace("\"div\": 0,", "");
                    list = JsonConvert.DeserializeObject<T[]>(jsonParseList).ToList();
                }
                catch
                {
                    Debug.Log($"Couldn't parse full Json list:\n{json}");
                    json = json.Trim().TrimStart('[').TrimEnd(']').Trim();
                    List<string> items = new();
                    if (json.Contains("\"div\": \"0\",")) items = json.Split("\"div\": \"0\",").ToList();
                    else items = json.Split("\"div\": 0,").ToList();
                    items = items.Where(x => x.Trim().StartsWith("{") == false).ToList();
                    for (int i = 0; i < items.Count; i++)
                    {
                        items[i] = "{" + items[i].Trim().TrimEnd('{').Trim().TrimEnd(',');
                        string item = items[i];
                        T data = JsonConvert.DeserializeObject<T>(item);
                        if (data != null) list.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
            // if (list.Count > 0) list = RemoveEmptyFieldsFromList(list);
            return list;
        }

        public static List<T> RemoveEmptyFieldsFromList<T>(List<T> list)
        {
            List<T> toRemove = new();
            foreach (var item in list)
            {
                if (HasEmptyFields(item)) toRemove.Add(item);
            }
            foreach (var item in toRemove)
            {
                Debug.Log($"Removed item with empty fields from list:\n{item}");
                list.Remove(item);
            }
            return list;
        }

        public static bool HasEmptyFields(object item)
        {
            foreach (FieldInfo field in item.GetType().GetFields())
            {
                if (field.GetValue(item) is ICollection collection)
                {
                    foreach (var x in collection)
                    {
                        if (HasEmptyFields(x)) return true;
                    }
                }
                else if (String.IsNullOrWhiteSpace(field.GetValue(item).ToString()))
                {
                    return true;
                }
            }
            return false;
        }
    }
}