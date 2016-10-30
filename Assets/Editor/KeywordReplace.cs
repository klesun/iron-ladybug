//Assets/Editor/KeywordReplace.cs
using UnityEngine;
using UnityEditor;
using System.Collections;

/** 
 * @see http://forum.unity3d.com/threads/c-script-template-how-to-make-custom-changes.273191/
 */
public class KeywordReplace : UnityEditor.AssetModificationProcessor 
{
    public static void OnWillCreateAsset(string relPath)
    {
        var path = relPath.Replace(".meta", "");
        int extIndex = path.LastIndexOf(".");
        string ext = extIndex > -1 ? path.Substring(extIndex) : null;
        if (ext != ".cs" && ext != ".js" && ext != ".boo") return;
        int index = Application.dataPath.LastIndexOf("Assets");
        path = Application.dataPath.Substring(0, index) + path;
        string file = System.IO.File.ReadAllText(path);

        file = file.Replace("#NAMESPACE#", relPathToNs(relPath));

        System.IO.File.WriteAllText(path, file);
        AssetDatabase.Refresh();
    }

    static string relPathToNs(string relPath)
    {
        return relPath
            .Substring (0, relPath.LastIndexOf("/"))
            .Substring ("Assets/Scripts/".Length)
            .Replace("/", ".")
            ;
    }
}
