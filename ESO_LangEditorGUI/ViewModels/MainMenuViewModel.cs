using ESO_LangEditorGUI.Command;
using ESO_LangEditorGUI.EventAggres;
using Prism.Events;
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
        private List<string> _roleList = new List<string>();
        private ObservableCollection<MenuItemContent> _topMenu;

        public ObservableCollection<MenuItemContent> TopMenu
        {
            get { return _topMenu; }
            set { SetProperty(ref _topMenu, value); }
        }

        IEventAggregator _ea;

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

        public MainMenuListViewModel(IEventAggregator ea)
        {
            //WindowLink("ESO_LangEditorGUI.Views.ExportTranslate").Show();

            _ea = ea;
            _ea.GetEvent<RoleListUpdateEvent>().Subscribe(UpdateRoleList);
            LoadMemu();
        }

        private void UpdateRoleList(List<string> roleList)
        {
            if(_roleList != roleList)
            {
                _roleList = roleList;
                LoadMemu();
                Debug.WriteLine("角色列表不相等");
            }
        }

        private void LoadMemu()
        {
            string windowNamespace = "ESO_LangEditorGUI.Views.";
            TopMenu = null;

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
                            CommandParameter = windowNamespace + "ImportTranslateDB",
                        },
                        new MenuItemContent
                        {
                            Header = "CSV和Lua与数据库对比",
                            Command = new ExcuteViewModelMethod(OpenWindowByICommand),
                            CommandParameter = windowNamespace + "CompareWithDBWindow",
                            Visible = RoleToVisibility("Admin"),
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
                            CommandParameter = windowNamespace + "ExportTranslate",  //窗口名 - 必须包含命名空间
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
                            CommandParameter = windowNamespace + "PackToRelase",
                            Visible = RoleToVisibility("Admin"),
                        }
                    }

                },
                new MenuItemContent
                {
                    Header="网络", ChildMenuItems=new ObservableCollection<MenuItemContent>
                    {
                        new MenuItemContent {
                            Header = "审核待通过文本",
                            Command = new ExcuteViewModelMethod(OpenWindowByICommand),
                            CommandParameter = windowNamespace + "LangTextReviewWindow",
                            //Visible = RoleToVisibility("Admin"),
                        },

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
                    Header="资料修改",
                    Command = new ExcuteViewModelMethod(OpenWindowByICommand),
                    CommandParameter = windowNamespace + "UserProfileSetting",

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

        private bool isRole(string roleName)
        {
            return _roleList.Contains(roleName);
        }

        private Visibility RoleToVisibility(string roleName)
        {
            if (_roleList.Contains(roleName))
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
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
        private bool _isVisible = true;
        private Visibility _visible = Visibility.Visible;

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

        public bool IsVisible
        {
            get { return _isVisible; }
            set { SetProperty(ref _isVisible, value); }
        }

        public Visibility Visible
        {
            get { return _visible; }
            set { SetProperty(ref _visible, value); }
        }


    }


}
