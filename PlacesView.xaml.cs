using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
using BD_CinemaV7.Context;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Data.Entity;
using BD_CinemaV7.Models;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using iText.IO.Font;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using TextAlignment = iText.Layout.Properties.TextAlignment;
using Hangfire.Annotations;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для PlacesView.xaml
    /// </summary>
    public partial class PlacesView : Window, INotifyPropertyChanged
    {
        public static string FONT = "C:/Users/pavel/source/repos/WpfApp1/WpfApp1/Resurses/arial.ttf";
        public PlacesView()
        {
            LoadPlaces();
            InitializeComponent();
        }
        private ObservableCollection<places> places;

        public ObservableCollection<places> Places
        {
            get => places;
            set
            {
                places = value;
                OnPropertyChanged();
            }
        }
        private void MyAcc_OnClick(object sender, RoutedEventArgs e)
        {
            var acc = new MyAcc();
            acc.Show();
            Close();
        }
        private void LoadPlaces()
        {
            DataContext = this;
            using(MsSqlContext db=new MsSqlContext())
            {
     
                
                //sessions ses = db.session.Where(c => c.Id == sess).FirstOrDefault();
                Places = new ObservableCollection<places>(db.places_list
                 .Include(sessions=>sessions.sessions).Include(user=>user.User).Where(f => f.sessionsId == SessionView.SelectedSession.Id)
                 .ToList());
            }
        }


        private void User_OnClick(object sender, RoutedEventArgs e)
        {
            UsersControls userview = new UsersControls();
            userview.Show();
            Close();
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
        public places SelectedPlace { get; set; }

        private void FilmView_OnClick(object sender, RoutedEventArgs e)
        {
            var filmview = new FilmView();
            filmview.Show();
            Close();
        }
        private void SessionView_OnClick(object sender, RoutedEventArgs e)
        {
            var sesview = new SessionView();
            sesview.Show();
            Close();
        }
       
        private void Report_OnClick(object sender, RoutedEventArgs e)
        {
            var report = new ReportView();
            report.Show();
            Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
