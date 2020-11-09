using ESO_LangEditorModels.Enum;
using System.Windows;
using System.Windows.Data;

namespace ESO_LangEditorGUI.Services
{
    public class ComparisonValue : DependencyObject
    {
        public SearchTextType Value
        {
            get { return (SearchTextType)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(SearchTextType),
            typeof(ComparisonValue),
            new PropertyMetadata(default(SearchTextType), OnValueChanged));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ComparisonValue comparisonValue = (ComparisonValue)d;
            BindingExpressionBase bindingExpressionBase = BindingOperations.GetBindingExpressionBase(comparisonValue, BindingToTriggerProperty);
            bindingExpressionBase?.UpdateSource();
        }

        public object BindingToTrigger
        {
            get { return GetValue(BindingToTriggerProperty); }
            set { SetValue(BindingToTriggerProperty, value); }
        }
        public static readonly DependencyProperty BindingToTriggerProperty = DependencyProperty.Register(
            nameof(BindingToTrigger),
            typeof(object),
            typeof(ComparisonValue),
            new FrameworkPropertyMetadata(default(object), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    }
}
