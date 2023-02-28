﻿using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using X_Guide.MVVM.View.CalibrationWizardSteps;

namespace X_Guide.MVVM.View
{
    /// <summary>
    /// Interaction logic for EngineeringView.xaml
    /// </summary>
    public partial class EngineeringView : UserControl
    {

        public EngineeringView()
        {
            InitializeComponent();



        }
        private void Button_Prev(object sender, RoutedEventArgs e)
        {
            step.Prev();
        }

        private void Button_Next(object sender, RoutedEventArgs e)
        {
            step.Next();
        }




    }



}
