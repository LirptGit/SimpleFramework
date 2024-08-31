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
        /// 存放excel表文件夹的的路径
        /// </summary
        public static readonly string ExcelPath = Application.dataPath + "/Samples/SimpleFrameworkDemo/DataTables/";

        /// <summary>
        /// 存放Excel转化的Assest文件的文件夹路径
        /// [Resources/AssetsFile 不可以修改]
        /// </summary>
        public static readonly string AssetPath = "Assets/Samples/SimpleFrameworkDemo/Resources/AssetsFile/";

        /// <summary>
        /// UI和声音对应的枚举配置文件路径
        /// </summary>
        public static readonly string EnumUIPath = "Assets/Samples/SimpleFrameworkDemo/Scripts/UI/";

        [MenuItem("RPT/生成配置文件")]
        public static void Excel2Asset()
        {
            // 检测文件夹是否存在
            if (Directory.Exists(ExcelBuild.ExcelPath))
            {
                // 获取文件夹信息
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
                Debug.Log($"请修改脚本的路径参数");
            }
        }

        public static void ExportExcelToAsset(string path, string name)
        {
            //UI面板对应的枚举信息
            List<string> EnumUIString = new List<string>();
            List<string> EnumSoundString = new List<string>();

            ItemManager manager = CreateInstance<ItemManager>();

            // 赋值
            manager.dataArray = CreateItemArrayWithExcel(path);

            // 确保文件是新的
            if (File.Exists(ExcelBuild.AssetPath + name + ".asset"))
            {
                File.Delete(ExcelBuild.AssetPath + name + ".asset");
            }

            if (!File.Exists(AssetPath))
            {
                Directory.CreateDirectory(AssetPath);
            }

            //asset文件的路径 要以"Assets/..."开始，否则会报错
            string assetPath = string.Format("{0}{1}.asset", ExcelBuild.AssetPath, name);

            // Debug.Log("转化的数据:" + manager.dataArray.Length);

            // 生成Asset文件
            AssetDatabase.CreateAsset(manager, assetPath);
            AssetDatabase.SaveAssets();
            Debug.Log($"{name}.asset 配置文件生成完毕，路径：{AssetPath}");

            //生成UI枚举定义的内容
            if (name == "UIFormInfo")
            {
                foreach (ExItem exItem in manager.dataArray)
                {
                    EnumUIString.Add(exItem.itemData[2] + " = " + exItem.itemId + ",");
                }

                StringBuilder ui_contentStringBuilder = new StringBuilder();
                ui_contentStringBuilder.Append("//该文件是自动生成的，请勿手动修改！\n");
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
                Debug.Log($"EnumUI.cs 脚本生成完毕，路径：{EnumUIPath} + EnumUI.cs");
            }

            AssetDatabase.Refresh();
        }

        public static ExItem[] CreateItemArrayWithExcel(string filePath)
        {
            ExItem[] array;

            //获得表数据
            int columnNum = 0, rowNum = 0;

            FileInfo fileInfo = new FileInfo(filePath);

            using (ExcelPackage excelPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[1];
                columnNum = worksheet.Dimension.End.Column;
                rowNum = worksheet.Dimension.End.Row;

                //根据excel的定义，第二行开始才是数据
                array = new ExItem[rowNum - 1];
                for (int i = 2; i < rowNum + 1; i++)
                {
                    ExItem item = new ExItem();
                    //解析每列的数据
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