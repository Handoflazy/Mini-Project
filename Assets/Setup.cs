using System.IO;
using UnityEditor;
using UnityEngine;


public static class Setup
{
	[MenuItem("Tools/Setup/Create Default Folders")]
	public static void CreateDefaultFolders()
	{
		Folders.CreateDefault ("_Project", "Animation", "Art", "Materials", "Prefabs", "Resoureces", "_Scripts", "Settings","Scene");
		UnityEditor.AssetDatabase.Refresh();
	}
}

static class Folders
{
	public static void CreateDefault(string root, params string[] folders)
	{
		
		var fullPath = Path.Combine(Application.dataPath, root);
		foreach (var folder in folders)
		{
			var path = Path.Combine(fullPath, folder);
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
		}
	}
}