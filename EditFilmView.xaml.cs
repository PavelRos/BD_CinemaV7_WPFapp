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
    /// Логика взаимодействия для EditFilmView.xaml
    /// </summary>
    public partial class EditFilmView : Window, INotifyPropertyChanged
    {
        public EditFilmView()
        {
            NameBox = new TextBox();
            AgeComboBox = new ComboBox();
            styleComboBox = new ComboBox();
            CountryComboBox = new ComboBox();
            budgetTextBox = new TextBox();
            datePicker1 = new DatePicker();
            regisComboBox = new ComboBox();
            distrComboBox = new ComboBox();
            durationTextBox = new TextBox();
           
            InitializeComponent();
            LoadFilm();
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

        private void LoadFilm()
        {
            DataContext = this;
            NameBox.Text = FilmView.SelectedFilm.film_name;
            budgetTextBox.Text = FilmView.SelectedFilm.budget.ToString();
            durationTextBox.Text = FilmView.SelectedFilm.duration.ToString();
            datePicker1.SelectedDate = DateTime.Parse(FilmView.SelectedFilm.release_date);
            using (MsSqlContext db = new MsSqlContext())
            {
                ages age = db.age_rating_list.Where(c => c.Age_Rating == FilmView.SelectedFilm.age_rating).FirstOrDefault();
                country country = db.countries_list.Where(c => c.Country_Name == FilmView.SelectedFilm.Country).FirstOrDefault();
                styles styles = db.styles_list.Where(c => c.film_style == FilmView.SelectedFilm.style).FirstOrDefault();
                regis regis = db.regs_list.Where(c => c.regisseur == FilmView.SelectedFilm.regisseur).FirstOrDefault();
                distr distr = db.distr_list.Where(c => c.distributor == FilmView.SelectedFilm.distributor).FirstOrDefault();
               
                countries = new ObservableCollection<country>(db.countries_list.ToList());
                Styles = new ObservableCollection<styles>(db.styles_list.ToList());
                Regis = new ObservableCollection<regis>(db.regs_list.ToList());
                Distr = new ObservableCollection<distr>(db.distr_list.ToList());
                Age_rating = new ObservableCollection<ages>(db.age_rating_list.ToList());
                AgeComboBox.SelectedValue = age.Age_Rating;
                CountryComboBox.SelectedValue = country.Country_Name;
                styleComboBox.SelectedValue = styles.film_style;
                regisComboBox.SelectedValue = regis.regisseur;
                distrComboBox.SelectedValue = distr.distributor;

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
        private void Back_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
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
            if (mln <= 0)
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
            if (minutes <= 0 || minutes > 301)
            {
                MessageBox.Show("Введите правильное значение длительности!  Не больше 300 и не меньше 1)");
                return;
            }
           
            
            using (MsSqlContext db = new MsSqlContext())
            {
                film cur_film = db.films.Where(c => c.Id == FilmView.SelectedFilm.Id).FirstOrDefault();
                

                cur_film.film_name = NameBox.Text;
                cur_film.duration = int.Parse(durationTextBox.Text);
                cur_film.budget = int.Parse(budgetTextBox.Text);
                cur_film.style = styleComboBox.SelectedValue.ToString();
                cur_film.age_rating = AgeComboBox.SelectedValue.ToString();
                cur_film.regisseur = regisComboBox.SelectedValue.ToString();
                cur_film.Country = CountryComboBox.SelectedValue.ToString(); 
                cur_film.distributor = distrComboBox.SelectedValue.ToString();
                cur_film.release_date = datePicker1.SelectedDate.ToString();
                db.Entry(cur_film).State = EntityState.Modified;
                db.SaveChanges();
            }
            LoadFilm();
            Close();
        }

     
    }
}
