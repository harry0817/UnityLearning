using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using UnityEditor;
using UnityEngine;

public class SolitaireXmlUtil {

    [MenuItem("Solitaire/屏幕适配 修改Android xml文件")]
    public static void ScreenFit() {
        string resDirName = "layout";
        var layoutDir = Path.Combine(Application.dataPath, "AndroidUtil/" + resDirName);
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

            var outputPath = Path.Combine(Application.dataPath, "AndroidUtil/" + resDirName + "-result" + "/" + layoutFileInfo.Name);
            File.WriteAllText(outputPath, layoutTxt);
        }

        Debug.Log("complete");
    }

    [MenuItem("Solitaire/复制strings.xml")]
    public static void CopyStringsXml() {
        string[] toCopy = {
            "title_continous_undo_prompt",
            "msg_continous_undo_prompt",
            "prompt_msg_sound_off",
            "prompt_msg_orientation_lock_by_system",
            "prompt_msg_orientation_lock_by_game",
        };
        var fromResDirPath = Path.Combine(Application.dataPath, "AndroidUtil/strings_xml_copy/from/res");
        var fromResDirInfo = new DirectoryInfo(fromResDirPath);
        var valuesDirInfos = fromResDirInfo.GetDirectories();
        foreach (var valuesDirInfo in valuesDirInfos) {
            var fromXmlFile = new FileInfo(Path.Combine(valuesDirInfo.FullName, "strings.xml"));
            var toXmlFile = new FileInfo(fromXmlFile.FullName.Replace("from", "to"));
            if (!fromXmlFile.Exists || !toXmlFile.Exists) {
                continue;
            }
            var fromXml = new XmlDocument();
            var toXml = new XmlDocument();
            fromXml.Load(fromXmlFile.FullName);
            toXml.Load(toXmlFile.FullName);
            var fromResourcesNode = fromXml.SelectSingleNode("resources");
            var toResourcesNode = toXml.SelectSingleNode("resources");
            for (int i = 0; i < fromResourcesNode.ChildNodes.Count; i++) {
                var stringNode = fromResourcesNode.ChildNodes[i];
                if (!stringNode.Name.Equals("string"))
                    continue;
                for (int j = 0; j < toCopy.Length; j++) {
                    if (stringNode.Attributes["name"].Value.Equals(toCopy[i])) {
                        var element = toXml.CreateElement("string");
                        element.SetAttribute("name", stringNode.Attributes["name"].Value);
                        string innerText = stringNode.InnerText;
                        if (stringNode.Attributes["name"].Value.Equals("prompt_msg_orientation_lock_by_game")) {
                            innerText = innerText.Replace("#", "\"%s\"");
                            Debug.Log("匹配: " + innerText);
                        }
                        element.InnerText = innerText;
                        toResourcesNode.AppendChild(element);
                        break;
                    }
                }
            }
            Debug.Log(toXml.OuterXml);
            toXml.Save(toXmlFile.FullName);
            FileUtil.DeleteFileOrDirectory(toXmlFile.FullName + ".meta");
        }

    }

    [MenuItem("Solitaire/改文件后缀")]
    public static void ChangeName() {
        string from = "10_experiment";
        string to = "11";
        var dirPath = Path.Combine(Application.dataPath, "AndroidUtil/change_suffix");
        var dir = new DirectoryInfo(dirPath);
        var files = dir.GetFiles();
        foreach (var file in files) {
            var fileName = file.FullName;
            fileName = fileName.Replace(from, to);
            file.CopyTo(fileName);
            file.Delete();
        }
    }
}
