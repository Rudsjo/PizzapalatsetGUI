using BackendHandler;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WpfInfoTerminal.Models;

namespace WpfInfoTerminal.ViewModels
{
    public class OrderViewModel : ObservableObject
    {


		private IDatabase rep { get; set; } = Helpers.GetSelectedBackend();


		private ObservableCollection<OrderModel> _ordersReadyToServe;
		public ObservableCollection<OrderModel> OrdersReadyToServe
		{
			get { return _ordersReadyToServe; }
			set { OnPropertyChanged(ref _ordersReadyToServe, value); }
		}


		private ObservableCollection<OrderModel> _ordersOngoing;
		public ObservableCollection<OrderModel> OrdersOngoing
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
			GetOngoingOrders();
			GetReadyOrders();
		}


		public async Task<ObservableCollection<OrderModel>> GetOngoingOrders()
		{
			DataBaseList = await rep.GetOrderByStatus(1);
			OrdersOngoing = new ObservableCollection<OrderModel>();

			foreach (Order item in DataBaseList)
			{
				OrderModel order = new OrderModel();

				order.OrderID = item.OrderID;

				OrdersOngoing.Add(order);
			}
			return OrdersOngoing;
		}
		public async Task<ObservableCollection<OrderModel>> GetReadyOrders()
		{
			DataBaseList = await rep.GetOrderByStatus(2);
			OrdersReadyToServe = new ObservableCollection<OrderModel>();

			foreach (Order item in DataBaseList)
			{
				OrderModel order = new OrderModel();

				order.OrderID = item.OrderID;

				OrdersReadyToServe.Add(order);
			}
			return OrdersReadyToServe;
		}

	}
}
