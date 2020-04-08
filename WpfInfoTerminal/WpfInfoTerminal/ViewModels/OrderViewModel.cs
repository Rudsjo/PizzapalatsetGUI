using BackendHandler;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WpfInfoTerminal.Models;

namespace WpfInfoTerminal.ViewModels
{
    public class OrderViewModel : ObservableObject
    {

		public static OrderViewModel Instance { get; set; }

		public IDatabase rep { get; set; } = Helpers.GetSelectedBackend();

		private ThreadSafeObservableCollection<OrderModel> _ordersReadyToServe;
		public ThreadSafeObservableCollection<OrderModel> OrdersReadyToServe
		{
			get { return _ordersReadyToServe; }
			set { OnPropertyChanged(ref _ordersReadyToServe, value); }
		}

		private ThreadSafeObservableCollection<OrderModel> _ordersOngoing;
		public ThreadSafeObservableCollection<OrderModel> OrdersOngoing
		{
			get { return _ordersOngoing; }
			set { OnPropertyChanged(ref _ordersOngoing, value); }
		}

		private IEnumerable _dataBaseList;
		public IEnumerable DataBaseList
		{
			get { return _dataBaseList; }
			set { OnPropertyChanged(ref _dataBaseList, value); }
		}

		public OrderViewModel()
		{
			Init();

			Instance = this;
			OrdersOngoing = new ThreadSafeObservableCollection<OrderModel>();
			OrdersReadyToServe = new ThreadSafeObservableCollection<OrderModel>();

			GetOngoingOrders();
			GetReadyOrders();
		}


		public async void GetOngoingOrders()
		{
			// Gets order status 0 and checks if an order has a pizza in it otherwise change
			// Order Status to two.
			DataBaseList = await rep.GetOrderByStatus(0);
			foreach (Order order in DataBaseList)
			{
				if (order.PizzaList.Count < 1)
					await rep.UpdateOrderStatusWhenOrderIsCooked(order);
			}

			DataBaseList = await rep.GetOrderByStatus(0);
			OrdersOngoing.Clear();
			foreach (Order item in DataBaseList)
			{
				OrderModel order = new OrderModel();

				order.OrderID = item.OrderID;

				OrdersOngoing.Add(order);
			}

		}
		public async void GetReadyOrders()
		{
			DataBaseList = await rep.GetOrderByStatus(2);
			OrdersReadyToServe.Clear();
			foreach (Order item in DataBaseList)
			{
				OrderModel order = new OrderModel();

				order.OrderID = item.OrderID;

				OrdersReadyToServe.Add(order);
			}
		}

		public async void Init()
		{
			ProgramState.IsRunning = true;

			ConfigFileHelpers.ReadServerConfigFile();

			if (await ProgramState.ServerConnection.ConnectAsync())
				MessageBox.Show("Terminal is connected to the server");

			else
				MessageBox.Show("Could not connect to the server, RUNNING IN OFFLINE MODE");
		}
	}
}
