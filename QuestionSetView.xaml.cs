using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SkillMeter.Views
{
    /// <summary>
    /// Interaction logic for QuestionSetView.xaml
    /// </summary>
    public partial class QuestionSetView : UserControl
    {
        public QuestionSetView()
        {
            InitializeComponent();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dGrid.UnselectAllCells();
        }
    }
}
