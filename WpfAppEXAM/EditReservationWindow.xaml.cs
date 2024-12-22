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
using WpfAppEXAM.Models;
using WpfAppEXAM.Services;
using WpfAppEXAM.Services.AuthorizeHelperServices;

namespace WpfAppEXAM
{
    /// <summary>
    /// Interaction logic for EditReservationWindow.xaml
    /// </summary>
    public partial class EditReservationWindow : Window
    {
        private readonly Reservation _reservation;
        private readonly ReservationService _reservationService;
        private readonly LoggerService _loggerService;

        public EditReservationWindow( ReservationService reservationService, LoggerService loggerService)
        {
            InitializeComponent();
            _reservation = AuthorizeHelperService.Reservation;
            _reservationService = reservationService;
            _loggerService = loggerService;

            
            DateFromPicker.SelectedDate = _reservation.DateFrom.Date;
            TimeFromPicker.Text = _reservation.DateFrom.ToString("HH:mm:ss");
            DateToPicker.SelectedDate = _reservation.DateTo.Date;
            TimeToPicker.Text = _reservation.DateTo.ToString("HH:mm:ss");
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var newDateFrom = CombineDateAndTime(DateFromPicker, TimeFromPicker);
            var newDateTo = CombineDateAndTime(DateToPicker, TimeToPicker);

            if (newDateFrom.HasValue && newDateTo.HasValue && newDateFrom < newDateTo)
            {
                try
                {
                    _reservation.DateFrom = newDateFrom.Value;
                    _reservation.DateTo = newDateTo.Value;
                    await _reservationService.UpdateReservationAsync(_reservation,_reservation.Id);

                    
                    _loggerService.LogInfo("Бронювання оновлено.");
                    Close();
                }
                catch (Exception ex)
                {
                    
                    _loggerService.LogError($"Помилка оновлення бронювання: {ex.Message}");
                }
            }
            else
            {
                _loggerService.LogError("Перевірте правильність введення дати та часу.");
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
    }
}
