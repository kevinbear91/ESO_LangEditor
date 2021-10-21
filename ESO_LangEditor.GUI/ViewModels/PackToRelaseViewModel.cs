using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.GUI.Command;
using ESO_LangEditor.GUI.Services;
using ESO_LangEditor.GUI.Views;
using Microsoft.Extensions.Logging;
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

namespace ESO_LangEditor.GUI.ViewModels
{
    public class PackToRelaseViewModel : BindableBase
    {
        private string _addonVersion;
        private string _apiVersion;
        private string _addonVersionInt;
        private string _updateLog;
        private bool _buttonprogress;
        private static PackLangVersion AddonVersionConfig;

        private List<FilePaths> _copyFilePaths;
        private List<FilePaths> _filePaths;
        private List<string> _roleList = new List<string>();

        //private PackToRelase _packToRelaseWindow;

        public string AddonVersion
        {
            get => _addonVersion;
            set => SetProperty(ref _addonVersion, value);
        }

        public string ApiVersion
        {
            get => _apiVersion;
            set => SetProperty(ref _apiVersion, value);
        }

        public string AddonVersionInt
        {
            get => _addonVersionInt;
            set => SetProperty(ref _addonVersionInt, value);
        }

        public string UpdateLog
        {
            get => _updateLog;
            set => SetProperty(ref _updateLog, value);
        }

        public bool ButtonProgress
        {
            get => _buttonprogress;
            set => SetProperty(ref _buttonprogress, value);
        }

        public Visibility IsAdmin => RoleToVisibility("Admin");

        public CHSorCHT ChsOrChtListSelected { get; set; }

        public IEnumerable<CHSorCHT> ChsOrChtList
        {
            get { return Enum.GetValues(typeof(CHSorCHT)).Cast<CHSorCHT>(); }
        }

        public ExcuteViewModelMethod PackFilesCommand => new ExcuteViewModelMethod(ProcessFilesAsync);
        public ExcuteViewModelMethod ExportLangCommand => new ExcuteViewModelMethod(ExportLangTextToLang);
        public ExcuteViewModelMethod ExportLuaCommand => new ExcuteViewModelMethod(ExportLangLuaToStr);
        public ExcuteViewModelMethod ExportLocationCommand => new ExcuteViewModelMethod(ExportTypeDataToLua);


        private ILangTextRepoClient _langTextRepo;
        private ILangFile _langFile;
        private ILogger _logger;
        private IUserAccess _userAccess;

        public PackToRelaseViewModel(ILangTextRepoClient langTextRepoClient, ILangFile langFile, 
            IUserAccess userAccess, ILogger logger)
        {
            _langTextRepo = langTextRepoClient;
            _langFile = langFile;
            _userAccess = userAccess;
            _logger = logger;

            AddonVersionConfig = PackLangVersion.Load();

            AddonVersion = AddonVersionConfig.AddonVersion;
            ApiVersion = AddonVersionConfig.AddonApiVersion;

            _roleList = _userAccess.GetUserRoleFromToken(App.LangConfig.UserAuthToken);

            var rev = Task.Run(() => _langTextRepo.GetRevNumber(1)).Result;

            AddonVersionInt = rev.Rev.ToString();
        }

        public async void ProcessFilesAsync(object o)
        {
            ButtonProgress = true;

            await Task.Run(() => ProcessFiles().ContinueWith(ExportDoneNotify));

            AddonVersionConfig.AddonVersion = AddonVersion;
            AddonVersionConfig.AddonApiVersion = ApiVersion;
            PackLangVersion.Save(AddonVersionConfig);
        }

        public async Task ProcessFiles()
        {
            try
            {
                await ExportLang(ChsOrChtListSelected);
                await ExportLua(ChsOrChtListSelected);
                await ExportTypeDataToLua();

                CopyResList();
                ModifyFiles();
                PackTempFiles();
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("无法找到必要文件夹，非开放功能，请群内询问相关问题！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("无法找到必要文件，非开放功能，请群内询问相关问题！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task ExportLang(CHSorCHT chsOrcht)
        {
            var langtext = await _langTextRepo.GetAlltLangTextsDictionaryAsync(2);

            if (chsOrcht == CHSorCHT.chs)
            {
                await _langFile.ExportLangTextsToLang(langtext, @"Export\zh.lang");
            }
            else
            {
                await _langFile.ExportLangTextsToText(langtext, @"_tmp\zh.txt");

                OpenccToCHT(@"_tmp\zh.txt", @"_tmp\zht.txt");

                var langtextCHT = await _langFile.ParseTextFile(@"_tmp\zht.txt");

                await _langFile.ExportLangTextsToLang(langtextCHT, @"Export\zht.lang");
            }
        }

        private async Task ExportLua(CHSorCHT chsOrcht)
        {
            var langLua = await _langTextRepo.GetAlltLangTextsDictionaryAsync(1);

            if (chsOrcht == CHSorCHT.chs)
            {
                await _langFile.ExportLuaToStr(langLua.Values.ToList());
            }
            else
            {
                await _langFile.ExportLuaToStr(langLua.Values.ToList());

                OpenccToCHT(@"Export\zh_pregame.str", @"Export\zht_pregame.str");
                OpenccToCHT(@"Export\zh_client.str", @"Export\zht_client.str");
            }
            
        }

        private void ExportLangTextToLang(object o)
        {
            ButtonProgress = true;
            Task.Run(() => ExportLang(ChsOrChtListSelected).ContinueWith(ExportDoneNotify));
        }

        private void ExportLangLuaToStr(object o)
        {
            ButtonProgress = true;
            Task.Run(() => ExportLua(ChsOrChtListSelected).ContinueWith(ExportDoneNotify));
        }
        private void ExportDoneNotify(object o)
        {
            ButtonProgress = false;
            MessageBox.Show("导出完成！", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
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

                _copyFilePaths.Add(new FilePaths
                {
                    SourcePath = @"Export\location.lua",
                    DestPath = @"_tmp\pack\" + esozhPath + @"\EsoZH\data\location.lua",
                });

                _copyFilePaths.Add(new FilePaths
                {
                    SourcePath = @"Export\itemsname.lua",
                    DestPath = @"_tmp\pack\" + esozhPath + @"\EsoZH\data\itemsname.lua",
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

                _copyFilePaths.Add(new FilePaths
                {
                    SourcePath = @"Export\location_cht.lua",
                    DestPath = @"_tmp\pack\" + esozhPath + @"\EsoZH\data\location.lua",
                });

                _copyFilePaths.Add(new FilePaths
                {
                    SourcePath = @"Export\itemsname_cht.lua",
                    DestPath = @"_tmp\pack\" + esozhPath + @"\EsoZH\data\itemsname.lua",
                });
                //File.Copy(@"Export\zht_client.str", @"_tmp\pack\" + esozhPath + @"\EsoUI\lang\zh_client.str", true);
            }
        }

        private void CopyResList()
        {
            List<string> fileList;

            if (ChsOrChtListSelected == CHSorCHT.chs)
            {
                fileList = File.ReadAllLines(@"Resources\PackCHS.txt").ToList();
            }
            else 
            { 
                fileList = File.ReadAllLines(@"Resources\PackCHT.txt").ToList(); 
            }

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
            {
                zipPath = @"Export\微攻略汉化" + _addonVersion + "_简体.zip";
            }
            else
            {
                zipPath = @"Export\微攻略汉化" + _addonVersion + "_繁体.zip";
            }
            ZipFile.CreateFromDirectory(dirPath, zipPath);

        }

        private void OpenccToCHT(string inputPath, string outputPath)
        {
            ProcessStartInfo startOpenCCInfo = new ProcessStartInfo
            {
                FileName = @"opencc\opencc.exe",
                Arguments = @" -i " + inputPath + " -o " + outputPath + @" -c opencc\s2twp.json"
            };

            Process opencc = new Process
            {
                StartInfo = startOpenCCInfo
            };
            opencc.Start();
            opencc.WaitForExit();
        }

        private void ExportTypeDataToLua(object obj)
        {
            Task.Run(() => ExportTypeDataToLua());
        }

        private async Task ExportTypeDataToLua()
        {
            Dictionary<string, string> exportDict = new Dictionary<string, string>();
            string replaceWord = @"\" + "\x22";  //replace " to \" for export Lua.

            var locationNameList = await _langTextRepo.GetLangTextByConditionAsync("10860933", SearchTextType.Type, SearchPostion.Full);
            locationNameList.AddRange(await _langTextRepo.GetLangTextByConditionAsync("146361138", SearchTextType.Type, SearchPostion.Full));
            locationNameList.AddRange(await _langTextRepo.GetLangTextByConditionAsync("157886597", SearchTextType.Type, SearchPostion.Full));
            locationNameList.AddRange(await _langTextRepo.GetLangTextByConditionAsync("162658389", SearchTextType.Type, SearchPostion.Full));
            Debug.WriteLine($"locationNameList count: {locationNameList.Count}");

            foreach (var location in locationNameList)
            {
                if (location.TextZh != null && !exportDict.ContainsKey(location.TextZh))
                {
                    if (location.TextZh.Contains("\x22"))
                    {
                        location.TextZh = location.TextZh.Replace("\x22", replaceWord);
                        Debug.WriteLine(location.TextZh);
                    }
                    if (location.TextEn.Contains("\x22"))
                    {
                        location.TextEn = location.TextEn.Replace("\x22", replaceWord);
                        Debug.WriteLine(location.TextEn);
                    }

                    if (location.TextEn.Contains("^"))
                    {
                        location.TextEn = location.TextEn.Replace("^ns", "");
                        location.TextEn = location.TextEn.Replace("^np", "");
                        location.TextEn = location.TextEn.Replace("^n", "");
                        location.TextEn = location.TextEn.Replace("^p", "");
                        location.TextEn = location.TextEn.Replace("^m", "");
                    }

                    

                    exportDict.Add(location.TextZh, location.TextEn);
                }
            }
            await _langFile.ExportAddonDictToLua(exportDict, 0);
            exportDict.Clear();

            var itemName = await _langTextRepo.GetLangTextByConditionAsync("242841733", SearchTextType.Type, SearchPostion.Full);


            foreach (var item in itemName)
            {
                if (item.TextZh != null && !exportDict.ContainsKey(item.TextZh))
                {
                    if (item.TextZh.Contains("\x22"))
                    {
                        item.TextZh = item.TextZh.Replace("\x22", replaceWord);
                        Debug.WriteLine(item.TextZh);
                    }
                    if (item.TextEn.Contains("\x22"))
                    {
                        item.TextEn = item.TextEn.Replace("\x22", replaceWord);
                        Debug.WriteLine(item.TextEn);
                    }

                    if (item.TextEn.Contains("^"))
                    {
                        item.TextEn = item.TextEn.Replace("^ns", "");
                        item.TextEn = item.TextEn.Replace("^np", "");
                        item.TextEn = item.TextEn.Replace("^n", "");
                        item.TextEn = item.TextEn.Replace("^p", "");
                        item.TextEn = item.TextEn.Replace("^m", "");
                    }
                    exportDict.Add(item.TextZh, item.TextEn);
                }
            }

            await _langFile.ExportAddonDictToLua(exportDict, 1);

            if (ChsOrChtListSelected == CHSorCHT.cht)
            {
                OpenccToCHT(@"Export\location.lua", @"Export\location_cht.lua");
                OpenccToCHT(@"Export\itemsname.lua", @"Export\itemsname_cht.lua");
            }
        }


        private Visibility RoleToVisibility(string roleName)
        {
            return _roleList.Contains(roleName) ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
