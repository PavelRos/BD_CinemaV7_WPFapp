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
    /// Логика взаимодействия для HallView.xaml
    /// </summary>
    public partial class SessionView : Window, INotifyPropertyChanged
    {
        private MsSqlContext db = new MsSqlContext();
        public static int id_ses;
        public SessionView()
        {
            LoadSessions();
            InitializeComponent();
        }

        private ObservableCollection<film> film;

        public ObservableCollection<film> Film
        {
            get => film;
            set
            {
                film = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<hall> hall;
        public ObservableCollection<hall> Hall
        {
            get => hall;
            set
            {
                hall = value;
                OnPropertyChanged();
            }
        }



        private ObservableCollection<sessions> sessions;

        public ObservableCollection<sessions> Sessions
        {
            get => sessions;
            set
            {
                sessions = value;
                OnPropertyChanged();
            }
        }
        public static sessions SelectedSession { get; set; }


        private void LoadSessions()
        {
            DataContext = this;
            Sessions = new ObservableCollection<sessions>(db.session
                .Include(films => films.film)
                .Include(hall => hall.hall)
                .ToList());
            Film = new ObservableCollection<film>(db.films.ToList());
            Hall = new ObservableCollection<hall>(db.halls.ToList());
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                using (MsSqlContext db = new MsSqlContext())
                {
                    db.Entry(SelectedSession).State = EntityState.Deleted;
                    db.SaveChanges();
                }
                LoadSessions();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void Places_OnClick(object sender, RoutedEventArgs e)
        {
            PlacesView ses_places = new PlacesView();
            ses_places.Show();
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

        private void FilmView_OnClick(object sender, RoutedEventArgs e)
        {
            var filmview = new FilmView();
            filmview.Show();
            Close();
        }

        private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            if (HallComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите зал!");
                return;
            }
            if (filmComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите фильм!");
                return;
            }
            if (datePicker1.SelectedDate == null)
            {
                MessageBox.Show("Выберите дату!");
                return;

            }
            if (string.IsNullOrEmpty(timePicker.Text))
            {
                MessageBox.Show("Введите время!");
                return;
            }
            if (!int.TryParse(PriceBox.Text, out var price))
            {
                MessageBox.Show("Введите правильное значение цены! (Не меньше 0)");
                return;
            }
            if (price < 0)
            {
                MessageBox.Show("Введите правильное значение цены! (Не меньше 0)");
                return;
            }
            film film1 =(film)filmComboBox.SelectedItem;
            int id = film1.Id;
            if (datePicker1.SelectedDate >= DateTime.Parse(film1.release_date))
            {
                string todaydate = DateTime.Today.ToString("yyyy.MM.dd");
                int dd = DateTime.Parse(todaydate).CompareTo(datePicker1.SelectedDate);
                 if (dd <= 0)
                {
                    int price_tickets = price;
                    int hallid = int.Parse(HallComboBox.SelectedValue.ToString());
                    int filmid = int.Parse(filmComboBox.SelectedValue.ToString());
                    using (MsSqlContext db = new MsSqlContext())
                    {
                        var hall = db.halls.Where(r => r.Id == hallid).FirstOrDefault();
                        sessions ses = new sessions
                        {
                            hallId = hallid,
                            filmId = filmid,
                            price_of_tickets = price_tickets,
                            date_of_session = datePicker1.Text,
                            time_of_session = timePicker.Text
                        };
                        db.Entry(ses).State = EntityState.Added;
                        db.SaveChanges();
                        hall current_hall = db.halls
                         .Where(d => d.Id == hallid).FirstOrDefault();
                        film current_film = db.films
                            .Where(d => d.Id == filmid).FirstOrDefault();
                        int number_rows = current_hall.number_of_rows;
                        int number_seats = current_hall.number_of_seats_in_a_row;
                        for (int i = 1; i < number_rows + 1; i++)
                        {
                            for (int j = 1; j < number_seats + 1; j++)
                            {
                                places place = new places();
                                {
                                    place.sessionsId = ses.Id;
                                    place.number_of_row = i;
                                    place.number_of_seat_in_a_row = j;
                                    place.status = "Свободно";

                                };

                                db.places_list.Add(place);
                                db.SaveChanges();
                            }
                        }

                        LoadSessions();
                    }
                }
                 else
                {
                    MessageBox.Show("Введите корректную дату(не раньше сегодняшнего дня)");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Введите корректную дату сеанса(после релиза данного фильма)");
                return;
            }
        }
        private void User_OnClick(object sender, RoutedEventArgs e)
        {
            UsersControls userview = new UsersControls();
            userview.Show();
            Close();
        }
        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}
