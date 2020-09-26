using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace ESO_LangEditorGUI.ViewModels
{
    public class DataGridEx : DataGrid
    {
        public DataGridEx() : base()
        {
            // Create event for right click on headers
            var style = new Style { TargetType = typeof(DataGridColumnHeader) };
            var eventSetter = new EventSetter(MouseRightButtonDownEvent, new MouseButtonEventHandler(HeaderClick));
            style.Setters.Add(eventSetter);
            ColumnHeaderStyle = style;
        }

        private void HeaderClick(object sender, MouseButtonEventArgs e)
        {
            ContextMenu menu = new ContextMenu();
            // Fill context menu with column names and checkboxes
            var visibleColumns = this.Columns.Where(c => c.Visibility == Visibility.Visible).Count();
            foreach (var column in this.Columns)
            {
                var menuItem = new MenuItem
                {
                    Header = column.Header.ToString(),
                    IsChecked = column.Visibility == Visibility.Visible,
                    IsCheckable = true,
                    // Don't allow user to hide all columns
                    IsEnabled = visibleColumns > 1 || column.Visibility != Visibility.Visible
                };
                // Bind events
                menuItem.Checked += (object a, RoutedEventArgs ea)
                    => column.Visibility = Visibility.Visible;
                menuItem.Unchecked += (object b, RoutedEventArgs eb)
                    => column.Visibility = Visibility.Collapsed;
                menu.Items.Add(menuItem);
            }
            // Open it
            menu.IsOpen = true;
        }
    }
}
