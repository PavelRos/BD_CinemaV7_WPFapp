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
    /// Логика взаимодействия для ReportView.xaml
    /// </summary>
    public partial class ReportView : Window
    {
        public static string FONT = "C:/Users/pavel/source/repos/WpfApp1/WpfApp1/Resurses/arial.ttf";
        public ReportView()
        {
            InitializeComponent();
        }
        private void ReportFilm_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                using (MsSqlContext db = new MsSqlContext())
                {
                    var film = db.films.Include(c => c.Sessions).ToList();
                    var dest = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Отчёт по продажам билетов на фильм.pdf";
                    var file = new FileInfo(dest);
                    file.Directory.Create();
                    var pdf = new PdfDocument(new PdfWriter(dest));
                    var document = new Document(pdf, PageSize.A2.Rotate());
                    var font = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H);

                    float[] columnWidths = { 10, 10, 10, 8 };
                    var table = new Table(UnitValue.CreatePercentArray(columnWidths));

                    var cell = new Cell(1, 4)
                        .Add(new Paragraph("Отчёт по фильмам"))
                        .SetFont(font)
                        .SetFontSize(13)
                        .SetFontColor(DeviceGray.WHITE)
                        .SetBackgroundColor(DeviceGray.BLACK)
                        .SetWidth(600)
                        .SetTextAlignment(TextAlignment.CENTER);
                    table.AddHeaderCell(cell);

                    Cell[] headerFooter =
                    {
                    new Cell().SetFont(font).SetWidth(100).SetBackgroundColor(new DeviceGray(0.75f)).Add(new Paragraph("Фильм")),
                    new Cell().SetFont(font).SetWidth(100).SetBackgroundColor(new DeviceGray(0.75f)).Add(new Paragraph("Количество сеансов")),
                    new Cell().SetFont(font).SetWidth(100).SetBackgroundColor(new DeviceGray(0.75f)).Add(new Paragraph("Количество купленных билетов")),
                    new Cell().SetFont(font).SetWidth(100).SetBackgroundColor(new DeviceGray(0.75f)).Add(new Paragraph("Общая сумма"))
                    };

                    foreach (var hfCell in headerFooter)
                    {
                        table.AddHeaderCell(hfCell);
                    }

                    int Count_of_places = 0;
                    int all_places = 0;
                    float Summ_price = 0;
                    for (int i = 0; i < film.Count; i++)
                    {
                        Summ_price = 0;
                        table.AddCell(new Cell().SetTextAlignment(TextAlignment.CENTER).SetFont(font).Add(new Paragraph(film[i].film_name)));
                        table.AddCell(new Cell().SetTextAlignment(TextAlignment.CENTER).SetFont(font).Add(new Paragraph($"{film[i].Sessions.Count}")));
                        for (int f = 0; f <= film[i].Sessions.Count - 1; f++)
                        {
                            ICollection<sessions> sess = film[i].Sessions;
                            sessions buff_ses = sess.ElementAt(f);
                            var places_list = db.places_list.Where(c => c.status == "Куплено").Where(c => c.sessionsId == buff_ses.Id).ToList();
                            Count_of_places = places_list.Count;
                            all_places = all_places + Count_of_places;
                            Summ_price = Count_of_places * buff_ses.price_of_tickets + Summ_price;
                        }
                        table.AddCell(new Cell().SetTextAlignment(TextAlignment.CENTER).SetFont(font).Add(new Paragraph(all_places.ToString())));
                        table.AddCell(new Cell().SetTextAlignment(TextAlignment.CENTER).SetFont(font).Add(new Paragraph(Summ_price.ToString())));
                        all_places = 0;
                        
                    }
                    document.Add(table);
                    document.Close();
                    var p = new Process
                    {
                        StartInfo = new ProcessStartInfo(dest)
                        {
                            UseShellExecute = true
                        }
                    };
                    p.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ReportHall_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                using (MsSqlContext db = new MsSqlContext())
                {
                    var hall = db.halls.Include(c => c.sessions).ToList();
                    var dest = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Отчёт по продажам в зале.pdf";
                    var file = new FileInfo(dest);
                    file.Directory.Create();
                    var pdf = new PdfDocument(new PdfWriter(dest));
                    var document = new Document(pdf, PageSize.A2.Rotate());
                    var font = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H);


                    float[] columnWidths = { 10, 10, 10, 8 };
                    var table = new Table(UnitValue.CreatePercentArray(columnWidths));

                    var cell = new Cell(1, 4)
                        .Add(new Paragraph("Отчёт по залам"))
                        .SetFont(font)
                        .SetFontSize(13)
                        .SetFontColor(DeviceGray.WHITE)
                        .SetBackgroundColor(DeviceGray.BLACK)
                        .SetWidth(600)
                        .SetTextAlignment(TextAlignment.CENTER);
                    table.AddHeaderCell(cell);

                    Cell[] headerFooter =
                    {
                    new Cell().SetFont(font).SetWidth(100).SetBackgroundColor(new DeviceGray(0.75f)).Add(new Paragraph("Зал")),
                    new Cell().SetFont(font).SetWidth(100).SetBackgroundColor(new DeviceGray(0.75f)).Add(new Paragraph("Количество сеансов")),
                    new Cell().SetFont(font).SetWidth(100).SetBackgroundColor(new DeviceGray(0.75f)).Add(new Paragraph("Количество купленных билетов")),
                    new Cell().SetFont(font).SetWidth(100).SetBackgroundColor(new DeviceGray(0.75f)).Add(new Paragraph("Общая сумма"))
                    };

                    foreach (var hfCell in headerFooter)
                    {
                        table.AddHeaderCell(hfCell);
                    }

                    int Count_of_places = 0;
                    float Summ_price = 0;
                    int Count_all_places = 0;

                    for (int i = 0; i < hall.Count; i++)
                    {
                        Summ_price = 0;
                        table.AddCell(new Cell().SetTextAlignment(TextAlignment.CENTER).SetFont(font).Add(new Paragraph(hall[i].hall_name)));
                        table.AddCell(new Cell().SetTextAlignment(TextAlignment.CENTER).SetFont(font).Add(new Paragraph($"{hall[i].sessions.Count}")));
                        for (int f = 0; f <= hall[i].sessions.Count - 1; f++)
                        {
                            ICollection<sessions> sess = hall[i].sessions;
                            sessions buff_ses = sess.ElementAt(f);
                            var places_list = db.places_list.Where(c => c.status == "Куплено").Where(c => c.sessionsId == buff_ses.Id).ToList();
                            Count_of_places = places_list.Count;
                            Count_all_places = Count_all_places+Count_of_places;
                            Summ_price = Count_of_places * buff_ses.price_of_tickets+Summ_price;
                        }
                        table.AddCell(new Cell().SetTextAlignment(TextAlignment.CENTER).SetFont(font).Add(new Paragraph(Count_all_places.ToString())));
                        table.AddCell(new Cell().SetTextAlignment(TextAlignment.CENTER).SetFont(font).Add(new Paragraph(Summ_price.ToString())));
                        Count_all_places = 0;
                     
                    }
                    document.Add(table);
                    document.Close();
                    var p = new Process
                    {
                        StartInfo = new ProcessStartInfo(dest)
                        {
                            UseShellExecute = true
                        }
                    };
                    p.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void MyAcc_OnClick(object sender, RoutedEventArgs e)
        {
            var acc = new MyAcc();
            acc.Show();
            Close();
        }
        private void ReportDate_OnClick(object sender, RoutedEventArgs e)
        {
            var date = new dates();
            date.ShowDialog();
            try
            {
                using (MsSqlContext db = new MsSqlContext())
                {
                    var places_list = db.places_list.Include(t => t.sessions).Include(g => g.User).Where(c => c.status == "Куплено").Where(x => x.date_of_operation >= dates.begin && x.date_of_operation <= dates.end).ToList();
                    var dest = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Отчёт по продажам за промежуток времени.pdf";
                    var file = new FileInfo(dest);
                    file.Directory.Create();
                    var pdf = new PdfDocument(new PdfWriter(dest));
                    var document = new Document(pdf, PageSize.A2.Rotate());
                    var font = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H);
                    
                    float[] columnWidths = { 10, 10, 10, 8 };
                    var table = new Table(UnitValue.CreatePercentArray(columnWidths));

                    var cell = new Cell(1, 4)
                        .Add(new Paragraph("Отчёт за промежуток времени"))
                        .SetFont(font)
                        .SetFontSize(13)
                        .SetFontColor(DeviceGray.WHITE)
                        .SetBackgroundColor(DeviceGray.BLACK)
                        .SetWidth(600)
                        .SetTextAlignment(TextAlignment.CENTER);
                    table.AddHeaderCell(cell);

                    Cell[] headerFooter =
                    {
                    new Cell().SetFont(font).SetWidth(100).SetBackgroundColor(new DeviceGray(0.75f)).Add(new Paragraph("С")),
                    new Cell().SetFont(font).SetWidth(100).SetBackgroundColor(new DeviceGray(0.75f)).Add(new Paragraph("По")),
                    new Cell().SetFont(font).SetWidth(100).SetBackgroundColor(new DeviceGray(0.75f)).Add(new Paragraph("Количество купленных билетов")),
                    new Cell().SetFont(font).SetWidth(100).SetBackgroundColor(new DeviceGray(0.75f)).Add(new Paragraph("Общая сумма"))
                    };

                    foreach (var hfCell in headerFooter)
                    {
                        table.AddHeaderCell(hfCell);
                    }

                    int Count_of_places = places_list.Count;
                    float Summ_price = 0;

                    for (int i = 0; i < places_list.Count; i++)
                    {
                        
                       
                        Summ_price =places_list[i].sessions.price_of_tickets + Summ_price;
                        
                    }
                    table.AddCell(new Cell().SetTextAlignment(TextAlignment.CENTER).SetFont(font).Add(new Paragraph(dates.begin.ToString())));
                    table.AddCell(new Cell().SetTextAlignment(TextAlignment.CENTER).SetFont(font).Add(new Paragraph(dates.end.ToString())));
                    table.AddCell(new Cell().SetTextAlignment(TextAlignment.CENTER).SetFont(font).Add(new Paragraph(Count_of_places.ToString())));
                    table.AddCell(new Cell().SetTextAlignment(TextAlignment.CENTER).SetFont(font).Add(new Paragraph(Summ_price.ToString())));
                    document.Add(table);
                    document.Close();
                    var p = new Process
                    {
                        StartInfo = new ProcessStartInfo(dest)
                        {
                            UseShellExecute = true
                        }
                    };
                    p.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void Places_OnClick(object sender, RoutedEventArgs e)
        {
            PlacesView ses_places = new PlacesView();
            ses_places.Show();
            Close();
        }
        private void SessionView_OnClick(object sender, RoutedEventArgs e)
        {
            var sessionview = new SessionView();
            sessionview.Show();
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
        private void User_OnClick(object sender, RoutedEventArgs e)
        {
            UsersControls userview = new UsersControls();
            userview.Show();
            Close();
        }

    }
}


