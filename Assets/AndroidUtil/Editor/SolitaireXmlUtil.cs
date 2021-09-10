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
        string[] toCopy = { "recover_billing", "recover_billing_completed" };
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
                if (stringNode.Attributes["name"].Value.Equals("recover_billing")) {
                    //Debug.Log(stringNode.Attributes["name"].Value);
                    //Debug.Log(stringNode.InnerText);
                    //Debug.Log(stringNode.OuterXml);
                    var element = toXml.CreateElement("string");
                    element.SetAttribute("name", stringNode.Attributes["name"].Value);
                    element.InnerText = stringNode.InnerText;
                    toResourcesNode.AppendChild(element);
                }
            }
            Debug.Log(toXml.OuterXml);
            toXml.Save(toXmlFile.FullName);
        }

    }
}
