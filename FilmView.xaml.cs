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
namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для FilmView.xaml
    /// </summary>
    public partial class FilmView : Window, INotifyPropertyChanged
    {
        public FilmView()
        {
            LoadFilm();
            InitializeComponent();
        }

        private ObservableCollection<film> films;

        public ObservableCollection<film> Films
        {
            get => films;
            set
            {
                films = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<country> countries;
        public ObservableCollection<country> Countries
        {
            get => countries;
            set
            {
                countries = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<styles> styles;

        public ObservableCollection<styles> Styles
        {
            get => styles;
            set
            {
                styles = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<regis> regis;

        public ObservableCollection<regis> Regis
        {
            get => regis;
            set
            {
                regis = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<distr> distr;

        public ObservableCollection<distr> Distr
        {
            get => distr;
            set
            {
                distr = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<ages> age_rating;

        public ObservableCollection<ages> Age_rating
        {
            get => age_rating;
            set
            {
                age_rating = value;
                OnPropertyChanged();
            }
        }
        public static film SelectedFilm { get; set; }
       
        private void LoadFilm()
        {
            DataContext = this;
            using (MsSqlContext db = new MsSqlContext())
            {
                Films = new ObservableCollection<film>(db.films
                    .Include(countries => countries.theCountry)
                    .Include(styles => styles.theStyle)
                    .Include(regis=>regis.theRegis)
                    .Include(distr=>distr.theDistr)
                    .Include(age=>age.the_rating)
                    .ToList());
                countries = new ObservableCollection<country>(db.countries_list.ToList());
                Styles = new ObservableCollection<styles>(db.styles_list.ToList());
                Regis = new ObservableCollection<regis>(db.regs_list.ToList());
                Distr= new ObservableCollection<distr>(db.distr_list.ToList());
                Age_rating = new ObservableCollection<ages>(db.age_rating_list.ToList());

            }
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                using (MsSqlContext db = new MsSqlContext())
                {
                    db.Entry(SelectedFilm).State = EntityState.Deleted;
                    db.SaveChanges();
                    LoadFilm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Edit_OnClick(object sender, RoutedEventArgs e)
        {
            EditFilmView edit = new EditFilmView();
            edit.ShowDialog();
            LoadFilm();
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
        private void MyAcc_OnClick(object sender, RoutedEventArgs e)
        {
            var acc = new MyAcc();
            acc.Show();
            Close();
        }
        private void User_OnClick(object sender, RoutedEventArgs e)
        {
            UsersControls userview = new UsersControls();
            userview.Show();
            Close();
        }
        private void Report_OnClick(object sender, RoutedEventArgs e)
        {
            var report = new ReportView();
            report.Show();
            Close();
        }

        private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NameBox.Text))
            {
                MessageBox.Show("Введите название фильма!");
                return;
            }
            if (AgeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите возрастной рейтинг!");
                return;
            }
            if (styleComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите жанр!");
                return;
            }
            if (CountryComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите страну!");
                return;
            }
            if (!int.TryParse(budgetTextBox.Text, out var mln))
            {
                MessageBox.Show("Введите правильное значение бюджета! Больше 0");
                return;
            }
            if (mln <=0)
            {
                MessageBox.Show("Введите правильное значение бюджета! Больше 0");
                return;
            }
            if (datePicker1.SelectedDate == null)
            {
                MessageBox.Show("Выберите дату!");
                return;
            }
            if (regisComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите режиссёра!");
                return;
            }
            if (distrComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите дистрибьютора!");
                return;
            }
            if (!int.TryParse(durationTextBox.Text, out var minutes))
            {
                MessageBox.Show("Введите правильное значение длительности!  Не больше 300 и не меньше 1)");
                return;
            }
            if (minutes <= 0|| minutes>301)
            {
                MessageBox.Show("Введите правильное значение длительности!  Не больше 300 и не меньше 1)");
                return;
            }
            int  duration = minutes;
            string film = NameBox.Text;
            int age_code = int.Parse(AgeComboBox.SelectedValue.ToString());
            int dis_code = int.Parse(distrComboBox.SelectedValue.ToString());
            int regis_code = int.Parse(regisComboBox.SelectedValue.ToString());
            int country_code = int.Parse(CountryComboBox.SelectedValue.ToString());
            int style_code = int.Parse(styleComboBox.SelectedValue.ToString());
            string release_date = datePicker1.Text;
            int budget = mln;
            using(MsSqlContext db=new MsSqlContext())
            {
                var rating = db.age_rating_list.Where(a => a.Age_Code == age_code).FirstOrDefault();
                var dis = db.distr_list.Where(f => f.dis_Code == dis_code).FirstOrDefault();
                var reg = db.regs_list.Where(d => d.reg_Code == regis_code).FirstOrDefault();
                var cout = db.countries_list.Where(d => d.CountryCode == country_code).FirstOrDefault();
                var st = db.styles_list.Where(f => f.Style_Code == style_code).FirstOrDefault();
                film new_film = new film
                {
                    film_name = film,
                    duration = duration,
                    budget = budget,
                    style = st.film_style,
                    age_rating = rating.Age_Rating,
                    regisseur = reg.regisseur,
                    Country = cout.Country_Name,
                    distributor = dis.distributor,
                    release_date = release_date,
                };
                db.Entry(new_film).State = EntityState.Added;
                db.SaveChanges();
            }
            LoadFilm();
        }

        //private void CustomerView_OnClick(object sender, RoutedEventArgs e)
        //{
        //    var customerView = new CustomerView();
        //    customerView.Show();
        //    Close();
        //}
    }
}
