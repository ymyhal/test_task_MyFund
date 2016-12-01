using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using MyFund.Infrastructure.Enums;
using MyFund.Infrastructure.Interfaces;
using MyFund.Infrastructure.Models;
using MyFund.Infrastructure.ViewModels;
using Prism.Commands;
using Prism.Events;

namespace MyFund.Modules.Stock.AddStock
{
    [Export(typeof(IAddStockViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AddStockViewModel : BaseViewModel, IAddStockViewModel
    {
        public const string ErrorPriceIsLessOrEquallThanZerro = "Price cannot be negative or equal to zero";
        public const string ErrorQuantityIsLessOrEquallThanZerro = "Quantity cannot be negative or equal to zero";

        private readonly IFundService _fundService;
        private StockType _stockTypeSelected;
        private decimal _price;
        private int _quantity;

        [ImportingConstructor]
        public AddStockViewModel(IEventAggregator eventAggregator, IFundService fundService):
            base(eventAggregator)
        {
            _fundService = fundService;

            this.AddStockCommand = new DelegateCommand<object>(this.AddStock, this.CanAddStock);

            StockTypes = Enum.GetValues(typeof(StockType)).Cast<StockType>().ToList();

            OnPriceChanged();
            OnQuantityChanged();
        }

        public  DelegateCommand<object> AddStockCommand { get; private set; }

        public IEnumerable<StockType> StockTypes { get; private set; }

        public StockType StockTypeSelected
        {
            get
            {
                return _stockTypeSelected;
            }
            set
            {
                _stockTypeSelected = value;
            }
        }

        public decimal Price
        {
            get { return _price; }
            set
            {
                _price = value;
                OnPriceChanged();
            }
        }

        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                OnQuantityChanged();
            }
        }

        private void AddStock(object parameter)
        {
            if (CanAddStock(null))
            {
                _fundService.AddStock(new BaseStockModel()
                {
                    Type = StockTypeSelected,
                    Price = Price,
                    Quantity = Quantity
                });
            }
        }

        private bool CanAddStock(object parameter)
        {
            return !HasErrors;
        }

        private void OnPriceChanged()
        {
            ValidatePrice();
            AddStockCommand.RaiseCanExecuteChanged();
        }

        private void OnQuantityChanged()
        {
            ValidateQuantity();
            AddStockCommand.RaiseCanExecuteChanged();
        }

        private void ValidatePrice()
        {
            if (Price <= 0)
            {
                AddError(() => Price, ErrorPriceIsLessOrEquallThanZerro);
            }
            else
            {
                RemoveError(() => Price, ErrorPriceIsLessOrEquallThanZerro);
            }
        }

        private void ValidateQuantity()
        {
            if (Quantity <= 0)
            {
                AddError(() => Quantity, ErrorQuantityIsLessOrEquallThanZerro);
            }
            else
            {
                RemoveError(() => Quantity, ErrorQuantityIsLessOrEquallThanZerro);
            }
        }
    }
}
