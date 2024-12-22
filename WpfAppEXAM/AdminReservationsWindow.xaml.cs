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
using WpfAppEXAM.Services;

namespace WpfAppEXAM
{
    /// <summary>
    /// Interaction logic for AdminReservationsWindow.xaml
    /// </summary>
    public partial class AdminReservationsWindow : Window
    {
        private readonly ReservationService reservationService;
        private readonly LoggerService loggerService;

        public AdminReservationsWindow(ReservationService reservationService, LoggerService loggerService)
        {
            this.reservationService = reservationService;
            this.loggerService = loggerService;
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadReservationsToGrid();
        }

        private async Task LoadReservationsToGrid()
        {
            try
            {
                ReservationDataGrid.ItemsSource = await reservationService.GetAllReservationsAsync();
            }
            catch (Exception ex)
            {
                loggerService.LogError($"Failed to load reservations: {ex.Message}");
            }
        }
    }
}
