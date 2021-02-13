using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace ESO_LangEditorGUI.ViewModels
{
    public class MainMenuListViewModel : BindableBase
    {
        private MenuItem _mainMenuList;
        public ObservableCollection<MenuItemContent> TopMenu { get; set; }

        public MainMenuListViewModel()
        {
            TopMenu = new ObservableCollection<MenuItemContent>
            {
                new MenuItemContent
                {
                    Header="导入", ChildMenuItems=new ObservableCollection<MenuItemContent>
                    {
                        new MenuItemContent { Header = "导入翻译文本" },
                        new MenuItemContent { Header = "CSV和Lua与数据库对比" }
                    }

                },
                new MenuItemContent
                {
                    Header="导出", ChildMenuItems=new ObservableCollection<MenuItemContent>
                    {
                        new MenuItemContent { Header = "导出已翻译内容" }
                    }

                },
                new MenuItemContent
                {
                    Header="高级", ChildMenuItems=new ObservableCollection<MenuItemContent>
                    {
                        new MenuItemContent { Header = "导出文本至.lang" },
                        new MenuItemContent { Header = "导出UI Str内容" },
                        new MenuItemContent { Header = "一键发布" }
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



    }


    public class MenuItemContent : BindableBase
    {
        public ObservableCollection<MenuItemContent> ChildMenuItems { get; set; }
        private string _header;
        private ICommand _command;

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


    }


}
