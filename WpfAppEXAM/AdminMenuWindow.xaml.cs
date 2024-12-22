using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using WpfAppEXAM.Models;

//using System.Windows.Documents;
using WpfAppEXAM.Services;

namespace WpfAppEXAM
{
    /// <summary>
    /// Interaction logic for AdminMenuWindow.xaml
    /// </summary>
    public partial class AdminMenuWindow : Window
    {
        private readonly UserService userService;
        private readonly TableService tableService;
        private readonly ReservationService reservationService;
        private readonly LoggerService loggerService;
        public AdminMenuWindow(UserService userService, TableService tableService, ReservationService reservationService, LoggerService loggerService)
        {
            this.userService = userService;
            this.tableService = tableService;
            this.reservationService = reservationService;
            this.loggerService = loggerService;
            InitializeComponent();
        }

        private void ViewReservations_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var reservationWindow = App.ServiceProvider.GetService<AdminReservationsWindow>();
                if (reservationWindow != null)
                {
                    reservationWindow.ShowDialog();
                }



            }
            catch (Exception ex)
            {
                loggerService.LogError($"Failed open reservations window! {ex.Message}");

            }
        }

        private async void AddTable_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(SeatsInput.Text, out int seats) && !string.IsNullOrWhiteSpace(DescriptionInput.Text))
            {
                var table = new Table
                {
                    Seats = seats,
                    Description = DescriptionInput.Text
                };

                try
                {
                    await tableService.AddTableAsync(table);
                    loggerService.LogInfo("Table added successfully!");
                    await LoadTablesToGrid();
                }
                catch (Exception ex)
                {
                    loggerService.LogError($"Failed to add table {ex.Message}");
                }
            }
            else
            {
                loggerService.LogError("Invalid input for table");
                
            }
        }

        private async void EditTable_Click(object sender, RoutedEventArgs e)
        {
            if (TableDataGrid.SelectedItem is Table selectedTable)
            {
                if (int.TryParse(SeatsInput.Text, out int seats) && !string.IsNullOrWhiteSpace(DescriptionInput.Text))
                {
                    selectedTable.Seats = seats;
                    selectedTable.Description = DescriptionInput.Text;

                    try
                    {
                        await tableService.UpdateTableAsync(selectedTable, selectedTable.Id);
                        loggerService.LogInfo($"Table updated successfully");
                        await LoadTablesToGrid();
                    }
                    catch (Exception ex)
                    {
                        loggerService.LogError($"Failed update table {ex.Message}");
                    }
                }
                else
                {
                    loggerService.LogError("Invalid input for updating table");
                    
                }
            }
            else
            {
                loggerService.LogError("No table selected for update");
                
            }
        }

        private async void DeleteTable_Click(object sender, RoutedEventArgs e)
        {
            if (TableDataGrid.SelectedItem is Table selectedTable)
            {
                var result = MessageBox.Show($"Are you sure you want to delete table {selectedTable.Id}?",
                                              "Delete Confirmation",
                                              MessageBoxButton.YesNo,
                                              MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        await tableService.DeleteTableAsync(selectedTable.Id);
                        loggerService.LogInfo($"Table {selectedTable.Id} deleted successfully.");
                        await LoadTablesToGrid();
                    }
                    catch (Exception ex)
                    {
                        loggerService.LogError($"Failed to delete table: {ex.Message}");
                    }
                }
            }
            else
            {
                loggerService.LogError("No table selected for delete");
                
            }
        }
        private async Task LoadTablesToGrid()
        {
            try
            {
                TableDataGrid.ItemsSource =await tableService.GetAllTablesAsync();
            }
            catch (Exception ex)
            {
                loggerService.LogError($"Failed to load tables {ex.Message}");
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadTablesToGrid();
        }
    }
}
