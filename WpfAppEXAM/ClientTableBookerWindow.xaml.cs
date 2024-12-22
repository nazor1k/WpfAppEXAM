using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
//using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfAppEXAM.Models;
using WpfAppEXAM.Services;
using WpfAppEXAM.Services.AuthorizeHelperServices;

namespace WpfAppEXAM
{
    /// <summary>
    /// Interaction logic for ClientTableBookerWindow.xaml
    /// </summary>
    public partial class ClientTableBookerWindow : Window
    {




        private readonly UserService userService;
        private readonly TableService tableService;
        private readonly ReservationService reservationService;
        private readonly LoggerService loggerService;
        public ClientTableBookerWindow(UserService userService, TableService tableService, ReservationService reservationService, LoggerService loggerService)
        {
            this.userService = userService;
            this.tableService = tableService;
            this.reservationService = reservationService;
            this.loggerService = loggerService;
            InitializeComponent();
        }


        private async void LoadAvailableTables()
        {
            try
            {
                TableDataGrid.ItemsSource = await tableService.GetAllTablesAsync();
            }
            catch (Exception ex)
            {
                loggerService.LogError($"Помилка завантаження столиків: {ex.Message}");
            }
        }

        private async void ApplyFilters_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var tables = await tableService.GetAllTablesAsync();

               
                if (int.TryParse(FilterSeatsInput.Text, out int seats))
                {
                    tables = tables.Where(t => t.Seats >= seats).ToList();
                }

               
                //if (DateTime.TryParse(FilterTimeInput.Text, out DateTime filterTime))
                //{
                //    tables = tables.Where(t => !t.Reservations.Any(r => r.DateFrom <= filterTime && r.DateTo >= filterTime)).ToList();
                //}

                TableDataGrid.ItemsSource = tables;
            }
            catch (Exception ex)
            {
                loggerService.LogError($"Помилка застосування фільтрів: {ex.Message}");
            }
        }
        private DateTime? CombineDateAndTime(DatePicker datePicker, TextBox timePicker)
        {
            if (datePicker.SelectedDate.HasValue && TimeSpan.TryParse(timePicker.Text, out TimeSpan time))
            {
                return datePicker.SelectedDate.Value.Date + time;
            }
            return null;
        }


        private async void CreateReservation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TableDataGrid.SelectedItem is Table selectedTable)
                {
                    var dateFrom = CombineDateAndTime(DateFromPicker, TimeFromPicker);
                    var dateTo = CombineDateAndTime(DateToPicker, TimeToPicker);

                    if (dateFrom.HasValue && dateTo.HasValue && dateFrom < dateTo)
                    {
                        
                        var reservations = await reservationService.GetReservationsForTableAsync(selectedTable.Id);
                        if (reservations.Any(r => r.DateFrom < dateTo && r.DateTo > dateFrom))
                        {
                            loggerService.LogError("Цей столик вже заброньовано на вибраний час.");
                            return;
                        }

                        
                        var reservation = new Reservation
                        {
                            TableId = selectedTable.Id,
                            UserId = AuthorizeHelperService.User.Id,
                            DateFrom = dateFrom.Value,
                            DateTo = dateTo.Value
                        };

                        await reservationService.CreateReservationAsync(reservation);
                        loggerService.LogInfo("Бронювання створено успішно.");
                        LoadAvailableTables();
                    }
                    else
                    {
                        loggerService.LogWarning("Перевірте правильність введення дати та часу.");
                    }
                }
                else
                {
                    loggerService.LogWarning("Виберіть столик.");
                }
            }
            catch (Exception ex)
            {
                loggerService.LogError($"Помилка створення бронювання: {ex.Message}");
            }
        }


        private async void CancelReservation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TableDataGrid.SelectedItem is Models.Table selectedTable)
                {
                    var reservations = await reservationService.GetReservationsForUserAsync(AuthorizeHelperService.User.Id);
                    var reservation = reservations.FirstOrDefault(r => r.TableId == selectedTable.Id);
                    if (reservation != null)
                    {
                        await reservationService.DeleteReservationAsync(reservation.Id);
                        loggerService.LogInfo("Бронювання скасовано.");
                        LoadAvailableTables();
                    }
                    else
                    {
                        loggerService.LogInfo("Бронювання для цього столика немає.");
                    }
                }
                else
                {
                    loggerService.LogInfo("Виберіть столик для скасування бронювання.");
                }
            }
            catch (Exception ex)
            {
                loggerService.LogError($"Помилка скасування бронювання: {ex.Message}");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAvailableTables();
        }

        private void MyReservationsButton_Click(object sender, RoutedEventArgs e)
        {
            var reservationsWindow = App.ServiceProvider.GetService<UserEditReservationWindow>();
            reservationsWindow.ShowDialog();
        }
    }
}
