using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace X_Guide.CustomControls
{
    /// <summary>
    /// Interaction logic for _9PointCalibLayout.xaml
    /// </summary>
    ///

    public partial class NinePointCalibLayout : UserControl
    {
        public class BoxItem
        {
            public int Row { get; set; }
            public int Column { get; set; }
            public int Number { get; set; }
        }

        public List<int> Orders
        {
            get { return (List<int>)GetValue(OrdersProperty); }
            set { SetValue(OrdersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Orders.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrdersProperty =
            DependencyProperty.Register("Orders", typeof(List<int>), typeof(NinePointCalibLayout), new PropertyMetadata(new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 }));

        public Dictionary<int, Border> BoxItems
        {
            get { return (Dictionary<int, Border>)GetValue(BoxItemsProperty); }
            set { SetValue(BoxItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BoxItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BoxItemsProperty =
            DependencyProperty.Register("BoxItems", typeof(Dictionary<int, Border>), typeof(NinePointCalibLayout), new PropertyMetadata(new Dictionary<int, Border>(), OnBoxItemChanged));

        private static void OnBoxItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NinePointCalibLayout ninePoint = d as NinePointCalibLayout;
            if (ninePoint.BoxItems.Count > 0) ninePoint.BoxItems.Clear();
            int index = 0;
            for (int c = 0; c < 3; c++)
            {
                for (int r = 0; r < 3; r++)
                {
                    var border = new Border();
                    var boxTextBlock = new TextBlock();
                    border.Child = boxTextBlock;
                    ninePoint.BoxItems.Add(index, border);
                    boxTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
                    boxTextBlock.VerticalAlignment = VerticalAlignment.Center;
                    boxTextBlock.Text = $"{index}";
                    border.SetValue(Grid.RowProperty, c);
                    border.SetValue(Grid.ColumnProperty, r);
                    ninePoint.NinePointGrid.Children.Add(border);
                    index++;
                }
            }
        }

        public NinePointCalibLayout()
        {
            InitializeComponent();
            CreateTextBlock();
        }

        private void RenderCalibration()
        {
            BoxItems[Orders[0]].BorderBrush = (Brush)Application.Current.FindResource("PrimaryBlueColor");
            BoxItems[Orders[0]].BorderThickness = new Thickness
            {
                Top = 3,
                Bottom = 3,
                Left = 3,
                Right = 3,
            };

            for (int i = 0; i < Orders.Count(); i++)
            {
                var currentItem = BoxItems[Orders[i]];

                MessageBoxResult result = MessageBox.Show("Proceed to next?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Question);

                if (result == MessageBoxResult.Cancel)
                {
                    CreateTextBlock();
                    break;
                }

                currentItem.BorderBrush = (Brush)FindResource("ConnectedColor");

                (currentItem.Child as TextBlock).Text = "Done";

                if (i + 1 != Orders.Count())
                {
                    var nextItem = BoxItems[Orders[i + 1]]; nextItem.BorderBrush = (Brush)FindResource("PrimaryBlueColor");
                    nextItem.BorderThickness = new Thickness
                    {
                        Top = 3,
                        Bottom = 3,
                        Left = 3,
                        Right = 3,
                    };
                }
            }
        }

        private void ClearTextBlock()
        {
            if (BoxItems.Count > 0)
            {
                List<Border> bordersToRemove = NinePointGrid.Children.OfType<Border>().ToList();

                foreach (var border in bordersToRemove)
                {
                    NinePointGrid.Children.Remove(border);
                }
                BoxItems.Clear();
            }
        }

        private void CreateTextBlock()
        {
            ClearTextBlock();
            int index = 0;
            for (int c = 0; c < 3; c++)
            {
                for (int r = 0; r < 3; r++)
                {
                    var i = new Border();
                    var boxTextBlock = new TextBlock();
                    i.Child = boxTextBlock;
                    BoxItems.Add(index, i);
                    boxTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
                    boxTextBlock.VerticalAlignment = VerticalAlignment.Center;
                    boxTextBlock.Text = $"{index}";
                    i.SetValue(Grid.RowProperty, c);
                    i.SetValue(Grid.ColumnProperty, r);
                    NinePointGrid.Children.Add(i);
                    index++;
                }
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            CreateTextBlock();
        }
    }
}