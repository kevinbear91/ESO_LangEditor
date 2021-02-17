using ESO_LangEditorGUI.Command;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ESO_LangEditorGUI.ViewModels
{
    public class MainMenuListViewModel : BindableBase
    {
        private MenuItem _mainMenuList;
        public ObservableCollection<MenuItemContent> TopMenu { get; set; }


        public Window WindowLink(string windowClassName)
        {
            Assembly windowClass = GetType().Assembly;

            object window = windowClass.CreateInstance(windowClassName);
            if (window == null)
            {
                throw new TypeLoadException("Unable to create window: " + windowClassName);
            }
            return (Window)window;
        }

        public MainMenuListViewModel()
        {
            //WindowLink("ESO_LangEditorGUI.Views.ExportTranslate").Show();
            

            TopMenu = new ObservableCollection<MenuItemContent>
            {
                new MenuItemContent
                {
                    Header="导入", ChildMenuItems=new ObservableCollection<MenuItemContent>
                    {
                        new MenuItemContent 
                        { 
                            Header = "导入翻译文本",
                            Command = new ExcuteViewModelMethod(OpenWindowByICommand),
                            CommandParameter = "ESO_LangEditorGUI.Views.ImportTranslateDB",
                        },
                        new MenuItemContent 
                        { 
                            Header = "CSV和Lua与数据库对比",
                            Command = new ExcuteViewModelMethod(OpenWindowByICommand),
                            CommandParameter = "ESO_LangEditorGUI.Views.CompareWithDBWindow",
                        }
                    }

                },
                new MenuItemContent
                {
                    Header="导出", ChildMenuItems=new ObservableCollection<MenuItemContent>
                    {
                        new MenuItemContent   //导出译文窗口参数
                        { 
                            Header = "导出已翻译内容",                                        //菜单标题
                            Command = new ExcuteViewModelMethod(OpenWindowByICommand),      //菜单Command 打开窗口
                            CommandParameter = "ESO_LangEditorGUI.Views.ExportTranslate",  //窗口名 - 必须包含命名空间
                        }
                    }

                },
                new MenuItemContent
                {
                    Header="高级", ChildMenuItems=new ObservableCollection<MenuItemContent>
                    {
                        new MenuItemContent 
                        {
                            Header = "导出文本至.lang", 
                            //Command = new ExcuteViewModelMethod(OpenWindowByICommand), 
                            //CommandParameter = "ESO_LangEditorGUI.Views.ExportTranslate"
                        },
                        new MenuItemContent 
                        {
                            Header = "导出UI Str内容", 
                            //Command = new ExcuteViewModelMethod(OpenWindowByICommand), 
                            //CommandParameter = "ESO_LangEditorGUI.Views.ExportTranslate"
                        },
                        new MenuItemContent {
                            Header = "一键发布", 
                            Command = new ExcuteViewModelMethod(OpenWindowByICommand), 
                            CommandParameter = "ESO_LangEditorGUI.Views.PackToRelase"
                        }
                    }

                },
                new MenuItemContent
                {
                    Header="网络", ChildMenuItems=new ObservableCollection<MenuItemContent>
                    {
                        new MenuItemContent { Header = "服务器线路" , ChildMenuItems=new ObservableCollection<MenuItemContent>
                        {
                            new MenuItemContent { Header = "IKDC2(IPv4)" },
                            new MenuItemContent { Header = "IKDC2(IPv6)" }
                        }},
                        //new MenuItemContent {Header="导出UI Str内容" },
                        //new MenuItemContent {Header="一键发布" }
                    }

                },
                new MenuItemContent
                {
                    Header="帮助", ChildMenuItems=new ObservableCollection<MenuItemContent>
                    {
                        new MenuItemContent { Header = "使用说明" }
                    }

                },
            };
        }

        private async void OpenWindowByICommand(object o)
        {
            WindowLink(o.ToString()).Show();
        }

        

    }


    public class MenuItemContent : BindableBase
    {
        public ObservableCollection<MenuItemContent> ChildMenuItems { get; set; }
        private string _header;
        private ICommand _command;
        private object _commandParameter;

        public string Header
        {
            get { return _header; }
            set { SetProperty(ref _header, value); }
        }

        public ICommand Command
        {
            get { return _command; }
            set { SetProperty(ref _command, value); }
        }

        public object CommandParameter
        {
            get { return _commandParameter; }
            set { SetProperty(ref _commandParameter, value); }
        }


    }


}
