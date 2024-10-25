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

	[MenuItem("Tools/Setup/Import Default Assets")]
	public static void ImportDefaultAssets()
	{
		Assets.ImportAssets("DOTween Pro.unitypackage","Default Assets");
		Assets.ImportAssets("Odin Inspector and Serializer.unitypackage","Default Assets");
		Assets.ImportAssets("Play Mode Save.unitypackage","Default Assets");
		Assets.ImportAssets("Text Animator for Unity.unitypackage","Default Assets");
		Assets.ImportAssets("vFolders 2.unitypackage","Default Assets");
		Assets.ImportAssets("vHierarchy 2.unitypackage","Default Assets");
		Assets.ImportAssets("vTabs 2.unitypackage","Default Assets");
		
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

static class Assets
{
	public static void ImportAssets(string asset, string subfolder, string folder = "C:/Users/aclon/OneDrive - ut.edu.vn/UnityAssetCollector")
	{
		AssetDatabase.ImportPackage(Path.Combine(asset, subfolder, folder),false);
	}
}