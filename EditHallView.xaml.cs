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
    /// Логика взаимодействия для EditHallView.xaml
    /// </summary>
    public partial class EditHallView : Window, INotifyPropertyChanged
    {
        public EditHallView()
        {
            nameTextBox = new TextBox();
            placeBox = new TextBox();
            rowsTextBox = new TextBox();
            TypeComboBox = new ComboBox();
            InitializeComponent();
            LoadEditHall();
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
        private void LoadEditHall()
        {
            
            nameTextBox.Text = HallView_.SelectedHall.hall_name;
            placeBox.Text = HallView_.SelectedHall.number_of_seats_in_a_row.ToString();
            rowsTextBox.Text = HallView_.SelectedHall.number_of_rows.ToString();
            using (MsSqlContext db = new MsSqlContext())
            {
                DataContext = this;
                Types_of_the_hall = new ObservableCollection<types>(db.types.ToList());
                types typeh = db.types.Where(c => c.Hall_Type == HallView_.SelectedHall.type).FirstOrDefault();
                TypeComboBox.SelectedValue = typeh.Hall_Type;
            }
        }

        private void save_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(nameTextBox.Text) || string.IsNullOrEmpty(placeBox.Text)
                                                      || string.IsNullOrEmpty(rowsTextBox.Text)|| string.IsNullOrEmpty(TypeComboBox.Text))
            {
                MessageBox.Show("Пожалуйста заполните поля!");
                return;
            }
           
            try
            {
                using (MsSqlContext db = new MsSqlContext())
                {
                    HallView_.SelectedHall.hall_name = nameTextBox.Text;
                    if (!int.TryParse(rowsTextBox.Text, out var rows))
                    {
                        MessageBox.Show("Введите правильное значение количества рядов(от 1 до 30)!");
                        return;
                    }
                    if (rows <= 0 || rows > 40)
                    {
                        MessageBox.Show("Введите правильное значение количества рядов(от 1 до 30)!");
                        return;
                    }
                    if (!int.TryParse(placeBox.Text, out var places))
                    {
                        MessageBox.Show("Введите правильное значение количества мест в ряду(от 1 до 30)!");
                        return;
                    }
                    if (places <= 0 || places > 30)
                    {
                        MessageBox.Show("Введите правильное значение количества мест в ряду(от 1 до 30)!");
                    }
                    HallView_.SelectedHall.number_of_seats_in_a_row = int.Parse(placeBox.Text);
                    HallView_.SelectedHall.number_of_rows = int.Parse(rowsTextBox.Text);
                    HallView_.SelectedHall.type = TypeComboBox.Text;
                    db.Entry(HallView_.SelectedHall).State=EntityState.Modified;
                    db.SaveChanges();
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Close();
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Back_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
