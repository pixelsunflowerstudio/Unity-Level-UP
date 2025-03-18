using System;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

#region Summary
// This is a quick summary of how the CSV file should be structured to work with this importer script.

// First Row structure:
// + First column is suggested to always be name of the data key variable (things like ID, or Key string)
// + Each column is the Name of variables in said class

// Second Row structure:
// + Each column is the Type of each variable. Such as: int, string, float[]...
// The second row can be omitted, because it's there to make sure the data is filled in with the correct Type.

// Third Row onwards structure:
// + First Column: Suggested to always be Data key
// + Second Column onwards: Data.

// Each row from the third row onwards will be parsed into an object.
// The CSV Importer will parse the CSV file into a list of data objects.

// Note:
// - This importer will work with Array fields. But the array data must be within one space, each element separated by a ';'
// For Example: a field of int[] will have this input: 1;2;3;4;5
#endregion

public static class CSVImporter
{
	// This row will be skipped.
	public const int ClassTypeRowIndex = 1;

	public static List<T> Parse<T>(string csv) where T : class
	{
		List<T> result = new();

		string[] rows = csv.Split('\n').Select(r => r.Replace("\r", string.Empty)).ToArray();

		string[] fieldNames = rows[0].Split(',');

		string[] dataRows = rows.Where((val, index) => index > ClassTypeRowIndex).ToArray();
		foreach (string row in dataRows)
		{
			string[] data = ParseLineData(row);
			result.Add(CreateObjectInstance(typeof(T), fieldNames, data) as T);
		}

		return result;
	}

	private static object CreateObjectInstance(Type objectType, string[] fieldNames, string[] rowData)
	{
		object instance = Activator.CreateInstance(objectType);

		FieldInfo[] fieldInfos = objectType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

		foreach (FieldInfo field in fieldInfos)
		{
			if (fieldNames.Contains(field.Name))
			{
				int index = Array.IndexOf(fieldNames, field.Name);
				SetObjectValue(instance, field, rowData[index]);
			}
		}

		return instance;
	}

	private static void SetObjectValue(object instance, FieldInfo fieldInfo, string value)
	{
		if (fieldInfo.FieldType.IsArray)
		{
			SetArrayValue(instance, fieldInfo, value);
		}
		else
		{
			SetFieldValue(instance, fieldInfo, value);
		}
	}

	private static void SetArrayValue(object instance, FieldInfo fieldInfo, string value)
	{
		Type elementType;
		try
		{
			elementType = fieldInfo.FieldType.GetElementType();
		}
		catch
		{
			throw new InvalidOperationException();
		}

		string[] elements = value.Split(';');
		Array arrayValue = Array.CreateInstance(elementType, elements.Length);
		for (int i = 0; i < elements.Length; i++)
		{
			arrayValue.SetValue(SetType(elements[i], elementType), i);
		}

		fieldInfo.SetValue(instance, arrayValue);
	}

	private static void SetFieldValue(object instance, FieldInfo fieldInfo, string value)
	{
		fieldInfo.SetValue(instance, SetType(value, fieldInfo.FieldType));
	}

	private static object SetType(string value, Type targetType)
	{
		if (targetType.IsEnum)
		{
			return Enum.Parse(targetType, value);
		}

		if (targetType.IsNumericType())
		{
			TypeConverter typeConverter = TypeDescriptor.GetConverter(targetType);
			return typeConverter.ConvertFromString(value);
		}

		if (targetType == typeof(string))
		{
			return value;
		}

		try
		{
			return Convert.ChangeType(value, targetType);
		}
		catch
		{
			UnityEngine.Debug.LogError($"Can't Set data Type to: {value} -------- type: {targetType}");
		}
		return null;
	}

	private static bool IsNumericType(this Type type)
	{
		return type.IsPrimitive && type != typeof(bool) && type != typeof(char);
	}

	// Parse the string of the line to remove quotation marks from the CSV imported file while preserving nested quotations
	private static string[] ParseLineData(string data)
	{
		List<string> result = new List<string>();
		StringBuilder current = new StringBuilder();
		bool inQuotes = false;

		for (int i = 0; i < data.Length; i++)
		{
			char c = data[i];

			// Toggle inQuotes when we hit a quote not part of an escaped pair
			if (c == '"' && (i == data.Length - 1 || data[i + 1] != '"'))
			{
				inQuotes = !inQuotes;
			}
			// Handle escaped double quotes ("")
			else if (c == '"' && i < data.Length - 1 && data[i + 1] == '"')
			{
				current.Append('"');
				i++; // Skip the next quote
			}
			// If we hit a comma and we're not inside quotes, it's a new field
			else if (c == ',' && !inQuotes)
			{
				result.Add(current.ToString());
				current.Clear();
			}
			else
			{
				current.Append(c);
			}
		}

		result.Add(current.ToString()); // Add the last field
		return result.ToArray();
	}
}