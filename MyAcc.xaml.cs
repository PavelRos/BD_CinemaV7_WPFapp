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
using System.Data.Entity;
using BD_CinemaV7.Models;
using Hangfire.Annotations;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MyAcc.xaml
    /// </summary>
    public partial class MyAcc : Window, INotifyPropertyChanged
    {
        public MyAcc()
        {
            nameTextBox = new TextBox();
            surnameTextBox = new TextBox();
            otcTextBox = new TextBox();
            ageBox = new TextBox();
            InitializeComponent();
            LoadAcc();
        }
        public static User SelectedUser { get; set; }
        public void selectuser()
        {
            using (MsSqlContext db=new MsSqlContext())
            {
                SelectedUser = db.Users.Where(c => c.Id == LoginView.id_user).FirstOrDefault();
            }
        }
        private void LoadAcc()
        {
            selectuser();
            DataContext = this;
            nameTextBox.Text = MyAcc.SelectedUser.user_name;
            surnameTextBox.Text = MyAcc.SelectedUser.user_surname;
            otcTextBox.Text = MyAcc.SelectedUser.user_otc;
            ageBox.Text = MyAcc.SelectedUser.Age.ToString();

        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {

            Application.Current.Shutdown();
        }

        private void Edit_OnClick(object sender, RoutedEventArgs e)
        {
            var editacc = new EditAcc();
            editacc.ShowDialog();
            LoadAcc();
        }

        private void HallView_OnClick(object sender, RoutedEventArgs e)
        {
            var hallview = new HallView_();
            hallview.Show();
            Close();
        }

        private void SessionView_OnClick(object sender, RoutedEventArgs e)
        {
            var sessionview = new SessionView();
            sessionview.Show();
            Close();
        }
        private void FilmView_OnClick(object sender, RoutedEventArgs e)
        {
            var filmview = new FilmView();
            filmview.Show();
            Close();
        }

        private void Report_OnClick(object sender, RoutedEventArgs e)
        {
            var report = new ReportView();
            report.Show();
            Close();
        }
        private void User_OnClick(object sender, RoutedEventArgs e)
        {
            UsersControls userview = new UsersControls();
            userview.Show();
            Close();
        }
    }
}
