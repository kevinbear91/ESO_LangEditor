using ESO_LangEditorGUI.View;
using ESO_LangEditorLib;
using ESO_LangEditorLib.Services.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Windows;

namespace ESO_LangEditorGUI.Services
{
    public class PackAddonFiles
    {
        readonly PackToRelase _window;

        public PackAddonFiles(PackToRelase window)
        {
            _window = window;
        }

        public void ProcessFiles(string esoZhVersion, string esoApiVersion)
        {
            try
            {
                ExportDbFiles();
                CopyResList();
                ModifyFiles(esoZhVersion, esoApiVersion);
                PackTempFiles(esoZhVersion);
                MessageBox.Show("打包完成！", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("无法找到必要文件夹，非开放功能，请群内询问相关问题！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("无法找到必要文件，非开放功能，请群内询问相关问题！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误，信息：" + Environment.NewLine + ex.ToString(), "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private string GetEsoZhPath()
        {
            int selectedIndex = _window.CHSorCHT_comboBox.SelectedIndex;
            string path = selectedIndex switch
            {
                0 => "chs",
                1 => "cht",
                _ => "chs",
            };
            return path;
        }

        private void ExportDbFiles()
        {
            var readDb = new LangTextRepository();
            var export = new ExportDbToFile();
            var tolang = new ThirdPartSerices();

            var langtexts = readDb.GetAlltLangTexts(0);
            var langlua = readDb.GetAlltLangTexts(1);

            export.ExportText(langtexts);
            export.ExportLua(langlua);

            if (GetEsoZhPath() == "chs")
                tolang.ConvertTxTtoLang(false);
            else
            {
                tolang.OpenCCtoCHT();
                tolang.ConvertTxTtoLang(true);
                tolang.LuaStrToCHT();
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void ModifyFiles(string esoZhVersion, string esoApiVersion)
        {
            string esozhPath = GetEsoZhPath();
            List<FilePaths> copyFilePaths = new List<FilePaths>();
            List<FilePaths> filePaths = new List<FilePaths>
            {
                new FilePaths
                {
                    SourcePath = @"Resources\" + esozhPath + @"\EsoZH\EsoZH.txt",
                    DestPath = @"_tmp\pack\" + esozhPath + @"\EsoZH\EsoZH.txt",
                },

                new FilePaths
                {
                    SourcePath = @"Resources\" + esozhPath + @"\EsoUI\lang\en_pregame.str",
                    DestPath = @"_tmp\pack\" + esozhPath + @"\EsoUI\lang\en_pregame.str",
                },
                //new FilePaths
                //{
                //    SourcePath = @"Export\zh_pregame.str",
                //    DestPath = @"_tmp\pack\" + esozhPath + @"\EsoUI\lang\zh_pregame.str",
                //}
            };

            if (esozhPath == "chs")
            {
                filePaths.Add(new FilePaths
                {
                    SourcePath = @"Export\zh_pregame.str",
                    DestPath = @"_tmp\pack\" + esozhPath + @"\EsoUI\lang\zh_pregame.str",
                });
                copyFilePaths.Add(new FilePaths
                {
                    SourcePath = @"Export\zh_client.str",
                    DestPath = @"_tmp\pack\" + esozhPath + @"\EsoUI\lang\zh_client.str",
                });

                copyFilePaths.Add(new FilePaths
                {
                    SourcePath = @"Export\zh.lang",
                    DestPath = @"_tmp\pack\" + esozhPath + @"\gamedata\lang\zh.lang",
                });

                //File.Copy(@"Export\zh_client.str", @"_tmp\pack\" + esozhPath + @"\EsoUI\lang\zh_client.str", true);
                //File.Copy(@"Export\zh.lang", @"_tmp\pack\" + esozhPath + @"\gamedata\lang\zh.lang", true);
            }
            else
            {
                filePaths.Add(new FilePaths
                {
                    SourcePath = @"Export\zht_pregame.str",
                    DestPath = @"_tmp\pack\" + esozhPath + @"\EsoUI\lang\zh_pregame.str",
                });
                copyFilePaths.Add(new FilePaths
                {
                    SourcePath = @"Export\zht_client.str",
                    DestPath = @"_tmp\pack\" + esozhPath + @"\EsoUI\lang\zh_client.str",
                });

                copyFilePaths.Add(new FilePaths
                {
                    SourcePath = @"Export\zht.lang",
                    DestPath = @"_tmp\pack\" + esozhPath + @"\gamedata\lang\zh.lang",
                });
                //File.Copy(@"Export\zht_client.str", @"_tmp\pack\" + esozhPath + @"\EsoUI\lang\zh_client.str", true);
            }

            foreach(var f in filePaths)
            {
                modifyfile(f.SourcePath,f.DestPath, esoZhVersion, esoApiVersion);
                Debug.WriteLine("{0},{1}",f.SourcePath,f.DestPath);
            }

            foreach(var c in copyFilePaths)
            {
                if (Directory.Exists(Path.GetDirectoryName(c.DestPath)))
                {
                    File.Copy(c.SourcePath, c.DestPath, true);
                }
                else
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(c.DestPath));
                    File.Copy(c.SourcePath, c.DestPath, true);
                }
            }

            #region 修改文件方法
            static void modifyfile(string readPath, string outputPath, string esoZhVersion, string esoApiVersion)
            
            {
                string modifyText;
                using (var sr = new StreamReader(readPath, Encoding.UTF8))
                {
                    modifyText = sr.ReadToEnd();
                }

                modifyText = modifyText.Replace("{EsoZhVersion}", esoZhVersion);
                modifyText = modifyText.Replace("{EsoApiVersion}", esoApiVersion);


                using (var sw = new StreamWriter(outputPath))
                {
                    if (Directory.Exists(Path.GetDirectoryName(outputPath)))
                    {
                        sw.Write(modifyText);
                    }
                    else
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
                        sw.Write(modifyText);
                    }
                }
            }
            #endregion

        }

        private void CopyResList()
        {
            List<string> fileList;

            if (GetEsoZhPath() == "chs")
                fileList = File.ReadAllLines(@"Resources\PackCHS.txt").ToList();
            else
                fileList = File.ReadAllLines(@"Resources\PackCHT.txt").ToList();

            foreach (var source in fileList)
            {
                string destPath = source.Replace("Resources", @"_tmp\pack");

                if (Directory.Exists(destPath))
                {
                    File.Copy(source, destPath, true);
                }
                else
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(destPath));
                    File.Copy(source, destPath, true);
                }
                //Debug.WriteLine(s);
            }

        }

        private void PackTempFiles(string esoZhVersion)
        {
            string chsOrCht = GetEsoZhPath();
            string zipPath;

            string dirPath = @"_tmp\pack\" + chsOrCht;

            if (chsOrCht == "chs")
                zipPath = @"Export\微攻略汉化" + esoZhVersion + "_简体.zip";
            else
                zipPath = @"Export\微攻略汉化" + esoZhVersion + "_繁体.zip";

            ZipFile.CreateFromDirectory(dirPath, zipPath);

        }
    }
    class FilePaths
    {
        public string SourcePath { get; set; }
        public string DestPath { get; set; }
    }
}
