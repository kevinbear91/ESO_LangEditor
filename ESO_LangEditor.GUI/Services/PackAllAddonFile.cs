using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.GUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Windows;

namespace ESO_LangEditor.GUI.Services
{
    public class PackAllAddonFile
    {
        private string _addonVersion;
        private string _apiVersion;
        private string _addonVersionInt;
        private string _updateLog;
        private CHSorCHT _chsOrChtListSelected;
        private List<FilePaths> _copyFilePaths;
        private List<FilePaths> _filePaths;

        

        public PackAllAddonFile(PackFileViewModel packFileViewModel)
        {
            _addonVersion = packFileViewModel.AddonVersion;
            _apiVersion = packFileViewModel.ApiVersion;
            _addonVersionInt = packFileViewModel.AddonVersionInt;
            _updateLog = packFileViewModel.UpdateLog;
            _chsOrChtListSelected = packFileViewModel.ChsOrChtListSelected;
        }

        public void ProcessFiles()
        {
            try
            {
                ExportDbFiles();
                CopyResList();
                ModifyFiles();
                PackTempFiles();
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
            //catch (Exception ex)
            //{
            //    MessageBox.Show("发生错误，信息：" + Environment.NewLine + ex.ToString(), "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }

        private async void ExportDbFiles()
        {
            var readDb = new LangTextRepoClientService();
            var export = new ExportDbToFile();
            var tolang = new ThirdPartSerices();

            var langtexts = await readDb.GetAlltLangTexts(0);
            var langlua = await readDb.GetAlltLangTexts(1);

            export.ExportText(langtexts);
            export.ExportLua(langlua);

            if (_chsOrChtListSelected == CHSorCHT.chs)
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

        private void ModifyFiles()
        {
            //List<FilePaths> copyFilePaths, filePaths;
            CreateFileList(_chsOrChtListSelected);

            foreach (var f in _filePaths)
            {
                modifyfile(f.SourcePath, f.DestPath);
                Debug.WriteLine("{0},{1}", f.SourcePath, f.DestPath);
            }

            foreach (var c in _copyFilePaths)
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

        }

        private void modifyfile(string readPath, string outputPath)

        {
            string modifyText;
            using (var sr = new StreamReader(readPath, Encoding.UTF8))
            {
                modifyText = sr.ReadToEnd();
            }

            modifyText = modifyText.Replace("{EsoZhVersion}", _addonVersion);
            modifyText = modifyText.Replace("{EsoApiVersion}", _apiVersion);
            modifyText = modifyText.Replace("{EsoZhVersionInt}", _addonVersionInt);
            modifyText = modifyText.Replace("{UpdateLog}", _updateLog);

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

        private void CreateFileList(CHSorCHT chsOrcht)
        {
            var esozhPath = chsOrcht;
            _copyFilePaths = new List<FilePaths>();
            _filePaths = new List<FilePaths>
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

                new FilePaths
                {
                    SourcePath = @"Resources\" + esozhPath + @"\EsoZH\EsoZH.lua",
                    DestPath = @"_tmp\pack\" + esozhPath + @"\EsoZH\EsoZH.lua",
                },
                //new FilePaths
                //{
                //    SourcePath = @"Export\zh_pregame.str",
                //    DestPath = @"_tmp\pack\" + esozhPath + @"\EsoUI\lang\zh_pregame.str",
                //}
            };
            if (_chsOrChtListSelected == CHSorCHT.chs)
            {
                _filePaths.Add(new FilePaths
                {
                    SourcePath = @"Export\zh_pregame.str",
                    DestPath = @"_tmp\pack\" + esozhPath + @"\EsoUI\lang\zh_pregame.str",
                });
                _copyFilePaths.Add(new FilePaths
                {
                    SourcePath = @"Export\zh_client.str",
                    DestPath = @"_tmp\pack\" + esozhPath + @"\EsoUI\lang\zh_client.str",
                });

                _copyFilePaths.Add(new FilePaths
                {
                    SourcePath = @"Export\zh.lang",
                    DestPath = @"_tmp\pack\" + esozhPath + @"\gamedata\lang\zh.lang",
                });

                //File.Copy(@"Export\zh_client.str", @"_tmp\pack\" + esozhPath + @"\EsoUI\lang\zh_client.str", true);
                //File.Copy(@"Export\zh.lang", @"_tmp\pack\" + esozhPath + @"\gamedata\lang\zh.lang", true);
            }
            else
            {
                _filePaths.Add(new FilePaths
                {
                    SourcePath = @"Export\zht_pregame.str",
                    DestPath = @"_tmp\pack\" + esozhPath + @"\EsoUI\lang\zh_pregame.str",
                });
                _copyFilePaths.Add(new FilePaths
                {
                    SourcePath = @"Export\zht_client.str",
                    DestPath = @"_tmp\pack\" + esozhPath + @"\EsoUI\lang\zh_client.str",
                });

                _copyFilePaths.Add(new FilePaths
                {
                    SourcePath = @"Export\zht.lang",
                    DestPath = @"_tmp\pack\" + esozhPath + @"\gamedata\lang\zh.lang",
                });
                //File.Copy(@"Export\zht_client.str", @"_tmp\pack\" + esozhPath + @"\EsoUI\lang\zh_client.str", true);
            }
        }

        private void CopyResList()
        {
            List<string> fileList;

            if (_chsOrChtListSelected == CHSorCHT.chs)
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

        private void PackTempFiles()
        {
            //string chsOrCht = GetEsoZhPath();
            string zipPath;

            string dirPath = @"_tmp\pack\" + _chsOrChtListSelected.ToString();

            if (_chsOrChtListSelected == CHSorCHT.chs)
                zipPath = @"Export\微攻略汉化" + _addonVersion + "_简体.zip";
            else
                zipPath = @"Export\微攻略汉化" + _addonVersion + "_繁体.zip";

            ZipFile.CreateFromDirectory(dirPath, zipPath);

        }
    }
}
