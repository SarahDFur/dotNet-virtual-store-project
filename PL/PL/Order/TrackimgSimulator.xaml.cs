using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.Order
{    
    /// <summary>
    /// Interaction logic for TrackimgSimulator.xaml
    /// </summary>
    #region Simulator
    public partial class TrackimgSimulator : Window
    {
        BackgroundWorker worker;
        public BlApi.IBl? bl = BlApi.Factory.GetBl();
        ObservableCollection<PO.OrderForList?> list;
        public DateTime Time = DateTime.Now;
        public TrackimgSimulator()
        {
            InitializeComponent();
            list = Castings.OrderForList_ConvertIEnumerableToObservable(bl.Order.GetOrders());
            DataContext= list;
            worker = new BackgroundWorker();

            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;

            worker.WorkerSupportsCancellation = true;
            worker.WorkerReportsProgress = true;
        }

        #region background worker       
        
        private void Worker_DoWork(object? sender, DoWorkEventArgs e)
        {
            try
            {
                while (worker.CancellationPending != true)
                {
                    PO.OrderForList? myOFL = new();

                    foreach (PO.OrderForList? order in list) //go over all list
                    {
                        if (worker.CancellationPending == true)//if canceled
                        {
                            e.Cancel = true;
                            break;
                        }

                        if (order != null)
                        {
                            switch (order?.OrderStatus)
                            {
                                case PO.Status.OrderConfirmed:
                                    //if (((bl?.Order.GetOrder(order.ID))?.OrderDate + new TimeSpan(new Random().Next(1,10), 0, 0, 0)) >= Time)
                                    if(Time - (bl?.Order.GetOrder(order.ID))?.OrderDate > new TimeSpan(new Random().Next(1, 10),0,0,0))
                                    {
                                        bl?.Order.UpdateShipDate(order.ID);
                                        System.Threading.Thread.Sleep(500);
                                    }
                                    break;

                                case PO.Status.OrderSent:
                                    //if (((bl?.Order.GetOrder(order.ID))?.ShipDate + new TimeSpan(10, 0, 0, 0)) >= (bl?.Order.GetOrder(order.ID))?.OrderDate)
                                    if(Time - (bl?.Order.GetOrder(order.ID))?.ShipDate > new TimeSpan(new Random().Next(1, 10), 0, 0, 0))
                                    {
                                        bl?.Order.UpdateDeliveryDate(order.ID);
                                        System.Threading.Thread.Sleep(500);
                                    }
                                    break;

                                default: break;
                            }

                            myOFL = list.FirstOrDefault(x => x?.ID == order!.ID);
                            if (myOFL != null)
                            {
                                myOFL.OrderStatus = (PO.Status)bl!.Order.GetOrder(order!.ID)!.OrderStatus;
                            }
                        }
                    }
                    Time += new TimeSpan(1, 0, 0, 0);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.InnerException?.Message);
            }
        }


        private void Worker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            int id = e.ProgressPercentage;
            Time.AddHours(2);
        }
        private void Worker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            if(e.Cancelled == true)
                MessageBox.Show("Simulator stopped");
            else if(e.Error != null)
            {
                MessageBox.Show("Simulator stopped because of an error");
            }
            else MessageBox.Show("Simulator finished");
        }
        #endregion

        #region start/cancel/view tracking buttons
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (worker.IsBusy != true)
            {
                simulatorRunning.Visibility = Visibility.Visible;
                worker.RunWorkerAsync("argument"); // Start the asynchronous operation.
            }
        }
        
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (worker.WorkerSupportsCancellation == true)
            {
                simulatorRunning.Visibility = Visibility.Hidden;
                worker.CancelAsync(); // Cancel the asynchronous operation.
            }
        }

        private void ViewTrackingForOrder_Click(object sender, RoutedEventArgs e)
        {
            PO.OrderForList? ord = (PO.OrderForList)((sender as Button)!.DataContext);
            BO.OrderTracking? ordtrack = bl?.Order.TrackOrder(ord.ID);
            PO.OrderTracking? ordtr_display = new()
            {
                ID = ordtrack?.IdOfOrder ?? 0,
                Tracking = ordtrack?.Tracking ?? null,
                OrderStatus = (PO.Status)ordtrack!.OrderStatus
            };
            MessageBox.Show($"{ordtr_display}");
        }
        #endregion

        #region Add Order click
        private void AddOrderButton_Click(object sender, RoutedEventArgs e)
        {
            bl?.Order.AddOrderForSimulator();
            list = Castings.OrderForList_ConvertIEnumerableToObservable(bl!.Order.GetOrders());
            DataContext = list;
        }
        #endregion

    }

    #endregion
}
