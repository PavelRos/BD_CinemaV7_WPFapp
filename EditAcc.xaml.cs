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
    /// Логика взаимодействия для EditAcc.xaml
    /// </summary>
    public partial class EditAcc : Window
    {
        public EditAcc()
        {
            nameTextBox = new TextBox();
            surnameTextBox = new TextBox();
            otcTextBox = new TextBox();
            ageBox = new TextBox();
            InitializeComponent();
            LoadEditAcc();
        }
        private void LoadEditAcc()
        {
            DataContext = this;
            nameTextBox.Text = MyAcc.SelectedUser.user_name;
            surnameTextBox.Text = MyAcc.SelectedUser.user_surname;
            otcTextBox.Text = MyAcc.SelectedUser.user_otc;
            ageBox.Text = MyAcc.SelectedUser.Age.ToString();
            
        }

        private void save_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(nameTextBox.Text) || string.IsNullOrEmpty(nameTextBox.Text)
                                                      || string.IsNullOrEmpty(surnameTextBox.Text) || string.IsNullOrEmpty(otcTextBox.Text)|| string.IsNullOrEmpty(ageBox.Text))
            {
                MessageBox.Show("Пожалуйста заполните поля!");
                return;
            }

            try
            {
                using (MsSqlContext db = new MsSqlContext())
                {
                    if (!int.TryParse(ageBox.Text, out var ages)|| int.Parse(ageBox.Text)>150)
                    {
                        MessageBox.Show("Введите правильное значение возраста (от 1 до 150)!");
                        return;
                    }

                    MyAcc.SelectedUser.user_name = nameTextBox.Text;
                    MyAcc.SelectedUser.user_surname = surnameTextBox.Text;
                    MyAcc.SelectedUser.user_otc = otcTextBox.Text;
                    MyAcc.SelectedUser.Age = int.Parse(ageBox.Text);
                    db.Entry(MyAcc.SelectedUser).State = EntityState.Modified;
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