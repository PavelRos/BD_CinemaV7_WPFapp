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
    /// Логика взаимодействия для HallView_.xaml
    /// </summary>
    public partial class HallView_ : Window, INotifyPropertyChanged
    {
        public HallView_()
        {
            LoadHalls();
            InitializeComponent();
        }
        private ObservableCollection<types> types_of_the_hall;

        public ObservableCollection<types> Types_of_the_hall
        {
            get => types_of_the_hall;
            set
            {
                types_of_the_hall = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<hall> halls;

        public ObservableCollection<hall> Halls
        {
            get => halls;
            set
            {
                halls = value;
                OnPropertyChanged();
            }
        }
        public static hall SelectedHall { get; set; }

        private void LoadHalls()
        {
            DataContext = this;
            using (MsSqlContext db = new MsSqlContext())
            {
                Halls = new ObservableCollection<hall>(db.halls
                    .Include(difficulty => difficulty.theHall_Type)
                    .ToList());

                Types_of_the_hall = new ObservableCollection<types>(db.types.ToList());
            }

        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                using (MsSqlContext db = new MsSqlContext())
                {
                    db.Entry(SelectedHall).State = EntityState.Deleted;
                    db.SaveChanges();
                }
                LoadHalls();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void FilmView_OnClick(object sender, RoutedEventArgs e)
        {
            var filmview = new FilmView();
            filmview.Show();
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void Edit_OnClick(object sender, RoutedEventArgs e)
        {
            EditHallView edithall = new EditHallView();
            edithall.ShowDialog();
            LoadHalls();
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
            using (MsSqlContext db = new MsSqlContext())
            {
                if (string.IsNullOrEmpty(nameTextBox.Text))
                {
                    MessageBox.Show("Введите название зала!");
                    return;
                }
                if (TypeComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Выберите тип зала!");
                    return;
                }
                if (!int.TryParse(RowsTextBox.Text, out var rows))
                {
                    MessageBox.Show("Введите правильное значение количества рядов(от 1 до 40)!");
                    return;
                }
                if (rows <= 0 || rows > 40)
                {
                    MessageBox.Show("Введите правильное значение количества рядов(от 1 до 40)!");
                    return;
                }
                if (!int.TryParse(PlaceTextBox.Text, out var places))
                {
                    MessageBox.Show("Введите правильное значение количества мест в ряду(от 1 до 40)!");
                    return;
                }
                if (places <= 0 || places > 40)
                {
                    MessageBox.Show("Введите правильное значение количества мест в ряду(от 1 до 40)!");
                }

                try
                {

                    var hallItem = new hall
                    {
                        hall_name = nameTextBox.Text,
                        type = TypeComboBox.SelectedValue.ToString(),
                        number_of_rows = rows,
                        number_of_seats_in_a_row = places
                    };
                    db.Entry(hallItem).State = EntityState.Added;
                    db.SaveChanges();
                    LoadHalls();
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
