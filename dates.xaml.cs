using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using BD_CinemaV7.Context;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using BD_CinemaV7.Models;
using System.Data.Entity;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для dates.xaml
    /// </summary>
    public partial class dates : Window
    {
        public static DateTime begin;
        public static DateTime end;
        public dates()
        {
            InitializeComponent();
        }
        private void Back_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void save_OnClick(object sender, RoutedEventArgs e)
        {
            begin = begindate.SelectedDate.Value;
            end = enddate.SelectedDate.Value;
            Close();

        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
