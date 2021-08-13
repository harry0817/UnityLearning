using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class SolitaireXmlUtil {

    [MenuItem("Solitaire适配/修改Android xml文件")]
    public static void Handle() {
        var rootDic = new Dictionary<string, Dictionary<string, string>>();
        var layoutDir = Path.Combine(Application.dataPath, "layout");
        var layoutDirInfo = new DirectoryInfo(layoutDir);
        var layoutFiles = layoutDirInfo.GetFiles();

        var minusDpRegex = new Regex("\\-\\d+dp");
        var dpRegex = new Regex("\\d+dp");
        var spRegex = new Regex("\\d+sp");
        foreach (var layoutFileInfo in layoutFiles) {
            var layoutTxt = File.ReadAllText(layoutFileInfo.FullName);

            var match = minusDpRegex.Match(layoutTxt);
            while (match.Groups.Count > 0) {
                var group = match.Groups[0].Value;
                if (string.IsNullOrEmpty(group)) {
                    break;
                }
                var dpValue = Mathf.Abs(int.Parse(group.Replace("dp", "")));
                layoutTxt = minusDpRegex.Replace(layoutTxt, "@dimen/dp_m_" + dpValue, 1);
                match = minusDpRegex.Match(layoutTxt);
            }

            match = dpRegex.Match(layoutTxt);
            while (match.Groups.Count > 0) {
                var group = match.Groups[0].Value;
                if (string.IsNullOrEmpty(group)) {
                    break;
                }
                var dpValue = int.Parse(group.Replace("dp", ""));
                layoutTxt = dpRegex.Replace(layoutTxt, "@dimen/dp_" + dpValue, 1);
                match = dpRegex.Match(layoutTxt);
            }

            match = spRegex.Match(layoutTxt);
            while (match.Groups.Count > 0) {
                var group = match.Groups[0].Value;
                if (string.IsNullOrEmpty(group)) {
                    break;
                }
                var spValue = int.Parse(group.Replace("sp", ""));
                layoutTxt = spRegex.Replace(layoutTxt, "@dimen/sp_" + spValue, 1);
                match = spRegex.Match(layoutTxt);
            }

            File.WriteAllText(Path.Combine(layoutDir, "result/" + layoutFileInfo.Name), layoutTxt);
        }

        Debug.Log("complete");
    }

}
