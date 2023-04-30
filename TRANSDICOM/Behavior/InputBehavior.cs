using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace TRANSDICOM.Behavior
{
    public class InputBehavior
    {
        public static readonly DependencyProperty DefInputProperty =
                DependencyProperty.RegisterAttached(
                        "DefInput",
                        typeof(bool),
                        typeof(InputBehavior),
                        new UIPropertyMetadata(false, DefInputChanged)
                );

        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static bool GetDefInput(DependencyObject obj)
        {
            return (bool)obj.GetValue(DefInputProperty);
        }

        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static void SetDefInput(DependencyObject obj, bool value)
        {
            obj.SetValue(DefInputProperty, value);
        }

        private static void DefInputChanged(DependencyObject sender, DependencyPropertyChangedEventArgs evt)
        {

            if (sender.GetType() == typeof(TextBox))
            {
                TextBox textBox = sender as TextBox;
                if (textBox == null)
                    return;
                textBox.PreviewKeyDown -= OnPreviewKeyDown;
                textBox.GotFocus -= OnTextBoxGotFocus;
                if ((bool)evt.NewValue)
                    textBox.PreviewKeyDown += OnPreviewKeyDown;
                textBox.GotFocus += OnTextBoxGotFocus;

            }
            else if (sender.GetType() == typeof(ComboBox))
            {
                ComboBox comboBox = sender as ComboBox;
                if (comboBox == null)
                    return;

                comboBox.GotFocus -= OnTextBoxGotFocus;
                comboBox.PreviewKeyDown -= OnPreviewKeyDown;
                if ((bool)evt.NewValue)
                    comboBox.GotFocus += OnTextBoxGotFocus;
                comboBox.PreviewKeyDown += OnPreviewKeyDown;
            }

        }

        private static void OnTextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            if (sender.GetType() == typeof(TextBox))
            {
                TextBox textBox = sender as TextBox;

                textBox.Dispatcher.BeginInvoke((Action)(() => textBox.SelectAll()));
            }
            else if (sender.GetType() == typeof(ComboBox))
            {
                ComboBox comboBox = sender as ComboBox;
                if (comboBox == null)
                    return;
            }
        }
        private static void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (sender.GetType() == typeof(TextBox))
            {


            }
            else if (sender.GetType() == typeof(ComboBox))
            {
            }
            switch (e.Key)
            {
                case Key.Enter:
                    e.Handled = true;
                    //e = new KeyEventArgs(e.KeyboardDevice, e.InputSource, e.Timestamp, Key.Tab);
                    UIElement element = sender as UIElement;
                    element.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next)); break;
                default:
                    break;
            }
        }
    }
}
