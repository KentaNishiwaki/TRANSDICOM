using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace TRANSDICOM.Behavior
{
    /// <summary>
    /// ListViewカスタマイズ
    /// </summary>
    /// <remarks>
    /// SelectedItemsをBinding可
    /// </remarks>
    public class CustomListView : ListView
    {
        #region DependencyProperty

        /// <summary>
        /// SelectedItems DependencyProperty
        /// </summary>
        public static readonly DependencyProperty CustomSelectedItemsProperty
            = DependencyProperty.Register(
                nameof(CustomSelectedItems)
                , typeof(IList)
                , typeof(CustomListView)
                , new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
                );

        #endregion

        #region Property

        /// <summary>
        /// SelectedItemsプロパティ
        /// </summary>
        public IList CustomSelectedItems
        {
            get { return (IList)GetValue(CustomSelectedItemsProperty); }
            set { SetValue(CustomSelectedItemsProperty, value); }
        }

        #endregion

        #region Event

        /// <summary>
        /// SelectedItems変更イベント
        /// </summary>
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {

            base.OnSelectionChanged(e);

            CustomSelectedItems = SelectedItems;

        }

        #endregion

    }
}
