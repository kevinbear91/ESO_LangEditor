using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using ESO_LangEditorGUI.Command;
using ESO_LangEditorGUI.Services;
using ESO_LangEditorGUI.Views;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ESO_LangEditorGUI.ViewModels
{
    public class PackFileViewModel : BindableBase
    {
        private string _addonVersion;
        private string _apiVersion;
        private string _addonVersionInt;
        private string _updateLog;
        private bool _buttonprogress;
        private static PackLangVersion AddonVersionConfig;

        private List<FilePaths> _copyFilePaths;
        private List<FilePaths> _filePaths;

        private PackToRelase _packToRelaseWindow;

        public string AddonVersion
        {
            get { return _addonVersion; }
            set { SetProperty(ref _addonVersion, value); }
        }

        public string ApiVersion
        {
            get { return _apiVersion; }
            set { SetProperty(ref _apiVersion, value); }
        }

        public string AddonVersionInt
        {
            get { return _addonVersionInt; }
            set { SetProperty(ref _addonVersionInt, value); }
        }

        public string UpdateLog
        {
            get { return _updateLog; }
            set { SetProperty(ref _updateLog, value); }
        }

        public bool ButtonProgress
        {
            get { return _buttonprogress; }
            set { SetProperty(ref _buttonprogress, value); }
        }

        public CHSorCHT ChsOrChtListSelected { get; set; }

        public IEnumerable<CHSorCHT> ChsOrChtList
        {
            get { return Enum.GetValues(typeof(CHSorCHT)).Cast<CHSorCHT>(); }
        }

        public ExcuteViewModelMethod PackFilesCommand => new ExcuteViewModelMethod(ProcessFilesAsync);


        public PackFileViewModel(PackToRelase packToRelaseWindow)
        {
            //exportToFileCommand = new ExportToFileCommand();
            _packToRelaseWindow = packToRelaseWindow;

            AddonVersionConfig = PackLangVersion.Load();

            AddonVersion = AddonVersionConfig.AddonVersion;
            ApiVersion = AddonVersionConfig.AddonApiVersion;
        }

        //private void Pack_button_Click()
        //{


        //    if (CheckResFolder())
        //    {
        //        //if (CheckField())
        //        //    packFiles.ProcessFiles(esoZhVersion, apiVersion);
        //        //else
        //        //    MessageBox.Show("汉化版本号与API版本号不得为空！");
        //    }
        //    else
        //    {
        //        MessageBox.Show("无法找到必要文件夹，非开放功能，请群内询问相关问题！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }



        //    //ModifyFiles();
        //    //CopyResList();

        //    //packFiles.ModifyFiles();

        //}

        public async void ProcessFilesAsync(object o)
        {
            //var packfile = new PackAllAddonFile(this);

            ButtonProgress = true;
            _packToRelaseWindow.Pack_button.IsEnabled = false;

            await Task.Run(() => ProcessFiles());

            AddonVersionConfig.AddonVersion = AddonVersion;
            AddonVersionConfig.AddonApiVersion = ApiVersion;
            PackLangVersion.Save(AddonVersionConfig);

            ButtonProgress = false;
            _packToRelaseWindow.Pack_button.IsEnabled = true;
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

            if (ChsOrChtListSelected == CHSorCHT.chs)
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
            CreateFileList(ChsOrChtListSelected);

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
            if (ChsOrChtListSelected == CHSorCHT.chs)
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

            if (ChsOrChtListSelected == CHSorCHT.chs)
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

            string dirPath = @"_tmp\pack\" + ChsOrChtListSelected.ToString();

            if (ChsOrChtListSelected == CHSorCHT.chs)
                zipPath = @"Export\微攻略汉化" + _addonVersion + "_简体.zip";
            else
                zipPath = @"Export\微攻略汉化" + _addonVersion + "_繁体.zip";

            ZipFile.CreateFromDirectory(dirPath, zipPath);

        }

    }
}
