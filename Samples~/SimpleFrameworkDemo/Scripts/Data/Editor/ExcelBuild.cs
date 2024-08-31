using System.IO;
using UnityEngine;
using OfficeOpenXml;
using System.Collections.Generic;
using System;
using UnityEditor;
using System.Text;

namespace SimpleFramework
{
    public class ExcelBuild : Editor
    {
        /// <summary>
        /// ���excel���ļ��еĵ�·��
        /// </summary
        public static readonly string ExcelPath = Application.dataPath + "/Samples/SimpleFrameworkDemo/DataTables/";

        /// <summary>
        /// ���Excelת����Assest�ļ����ļ���·��
        /// [Resources/AssetsFile �������޸�]
        /// </summary>
        public static readonly string AssetPath = "Assets/Samples/SimpleFrameworkDemo/Resources/AssetsFile/";

        /// <summary>
        /// UI��������Ӧ��ö�������ļ�·��
        /// </summary>
        public static readonly string EnumUIPath = "Assets/Samples/SimpleFrameworkDemo/Scripts/UI/";

        [MenuItem("RPT/���������ļ�")]
        public static void Excel2Asset()
        {
            // ����ļ����Ƿ����
            if (Directory.Exists(ExcelBuild.ExcelPath))
            {
                // ��ȡ�ļ�����Ϣ
                DirectoryInfo direction = new DirectoryInfo(ExcelBuild.ExcelPath);
                FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);
                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].Name.EndsWith(".xlsx"))
                    {
                        ExportExcelToAsset(ExcelBuild.ExcelPath + files[i].Name, files[i].Name.Substring(0, files[i].Name.IndexOf('.')));
                    }
                }
            }
            else
            {
                Debug.Log($"���޸Ľű���·������");
            }
        }

        public static void ExportExcelToAsset(string path, string name)
        {
            //UI����Ӧ��ö����Ϣ
            List<string> EnumUIString = new List<string>();
            List<string> EnumSoundString = new List<string>();

            ItemManager manager = CreateInstance<ItemManager>();

            // ��ֵ
            manager.dataArray = CreateItemArrayWithExcel(path);

            // ȷ���ļ����µ�
            if (File.Exists(ExcelBuild.AssetPath + name + ".asset"))
            {
                File.Delete(ExcelBuild.AssetPath + name + ".asset");
            }

            if (!File.Exists(AssetPath))
            {
                Directory.CreateDirectory(AssetPath);
            }

            //asset�ļ���·�� Ҫ��"Assets/..."��ʼ������ᱨ��
            string assetPath = string.Format("{0}{1}.asset", ExcelBuild.AssetPath, name);

            // Debug.Log("ת��������:" + manager.dataArray.Length);

            // ����Asset�ļ�
            AssetDatabase.CreateAsset(manager, assetPath);
            AssetDatabase.SaveAssets();
            Debug.Log($"{name}.asset �����ļ�������ϣ�·����{AssetPath}");

            //����UIö�ٶ��������
            if (name == "UIFormInfo")
            {
                foreach (ExItem exItem in manager.dataArray)
                {
                    EnumUIString.Add(exItem.itemData[2] + " = " + exItem.itemId + ",");
                }

                StringBuilder ui_contentStringBuilder = new StringBuilder();
                ui_contentStringBuilder.Append("//���ļ����Զ����ɵģ������ֶ��޸ģ�\n");
                ui_contentStringBuilder.Append("namespace SimpleFrameworkDemo\n");
                ui_contentStringBuilder.Append("{\n");
                ui_contentStringBuilder.Append("    public enum EnumUI\n");
                ui_contentStringBuilder.Append("    {\n");
                foreach (string msg in EnumUIString)
                {
                    ui_contentStringBuilder.Append("        " + msg + "\n");
                }
                ui_contentStringBuilder.Append("    }\n");
                ui_contentStringBuilder.Append("}\n");
                if (!File.Exists(EnumUIPath))
                {
                    Directory.CreateDirectory(EnumUIPath);
                }
                File.WriteAllText(EnumUIPath + "EnumUI.cs", ui_contentStringBuilder.ToString());
                Debug.Log($"EnumUI.cs �ű�������ϣ�·����{EnumUIPath} + EnumUI.cs");
            }

            AssetDatabase.Refresh();
        }

        public static ExItem[] CreateItemArrayWithExcel(string filePath)
        {
            ExItem[] array;

            //��ñ�����
            int columnNum = 0, rowNum = 0;

            FileInfo fileInfo = new FileInfo(filePath);

            using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[1];
                columnNum = worksheet.Dimension.End.Column;
                rowNum = worksheet.Dimension.End.Row;

                //����excel�Ķ��壬�ڶ��п�ʼ��������
                array = new ExItem[rowNum - 1];
                for (int i = 2; i < rowNum + 1; i++)
                {
                    ExItem item = new ExItem();
                    //����ÿ�е�����
                    item.itemId = uint.Parse(worksheet.Cells[i, 1].Value.ToString());
                    item.itemData = new List<string>();
                    for (int j = 1; j < columnNum + 1; j++)
                    {
                        item.itemData.Add(Convert.ToString(worksheet.Cells[i, j].Value));
                    }
                    array[i - 2] = item;
                }
            }

            return array;
        }
    }
}