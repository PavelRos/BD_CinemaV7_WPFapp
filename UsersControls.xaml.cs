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
    /// Логика взаимодействия для UsersControls.xaml
    /// </summary>
    public partial class UsersControls : Window, INotifyPropertyChanged
    {
        public UsersControls()
        {
            LoadUsers();
            InitializeComponent();
        }
        private ObservableCollection<User> users;

        public ObservableCollection<User> Users
        {
            get => users;
            set
            {
                users = value;
                OnPropertyChanged();
            }
        }
        public static User SelectedUser { get; set; }
        private void LoadUsers()
        {
            DataContext = this;
            using (MsSqlContext db = new MsSqlContext())
            {
                Users = new ObservableCollection<User>(db.Users
                    .Include(role => role.Role)
                    .ToList());
            }

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
        private void AddRole_OnClick(object sender, RoutedEventArgs e)
        {
            using(MsSqlContext db=new MsSqlContext())
            {
                User user = db.Users.Where(c => c.Id == SelectedUser.Id).FirstOrDefault();
                if (user.RoleId == 1)
                {
                    user.RoleId = 0;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    LoadUsers();
                }
                else
                {
                    MessageBox.Show("Пользователь обладает правами администратора");
                    return;
                }
            }
        }
        private void RemoveRole_OnClick(object sender, RoutedEventArgs e)
        {
            using (MsSqlContext db = new MsSqlContext())
            {
                User user = db.Users.Where(c => c.Id == SelectedUser.Id).FirstOrDefault();
                if(user.RoleId==0)
                {
                    user.RoleId = 1;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    LoadUsers();
                }
                else
                {
                    MessageBox.Show("Данный пользователь не обладает правами администратора");
                    return;
                }
               
            }
        }
        private void Report_OnClick(object sender, RoutedEventArgs e)
        {
            var report = new ReportView();
            report.Show();
            Close();
        }
        private void MyAcc_OnClick(object sender, RoutedEventArgs e)
        {
            var acc = new MyAcc();
            acc.Show();
            Close();
        }
    }
}
