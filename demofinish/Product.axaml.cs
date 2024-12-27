using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using demofinish.Models;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Data;
using Avalonia.Interactivity;
using System;
using System.Globalization;

namespace demofinish
{
    public partial class Product : Window
    {
        private ObservableCollection<ProductPresenter> _products;
        private ObservableCollection<ProductPresenter> _filteredProducts;

        public Product()
        {
            InitializeComponent();
            LoadProducts();
            SetupEventHandlers();
            InitializeComboBoxes();
        }

        private void InitializeComboBoxes()
        {
            SortComboBox.SelectedIndex = 0;
            FilterCombobox.SelectedIndex = 0;
        }

        private void LoadProducts()
        {
            using var context = new DemodbContext();
            var dataSource = context.Products.Select(product => new ProductPresenter
            {
                Productarticlenumber = product.Productarticlenumber,
                Productname = product.Productname,
                Productdescription = product.Productdescription,
                Productcategory = product.Productcategory,
                Productcost = product.Productcost,
                Productdiscountamount = product.Productdiscountamount,
                Discountmax = product.Discountmax,
                Productmanufacturer = product.Productmanufacturer,
                Productphoto = product.Productphoto,
                Productquantityinstock = product.Productquantityinstock,
                Productstatus = product.Productstatus
            }).ToList();

            _products = new ObservableCollection<ProductPresenter>(dataSource);
            _filteredProducts = new ObservableCollection<ProductPresenter>(_products);
            ProductListBox.ItemsSource = _filteredProducts;
            UpdateCountText();
        }

        private void SetupEventHandlers()
        {
            SearchTextBox.TextChanged += OnSearchTextChanged;
            SortComboBox.SelectionChanged += OnSortSelectionChanged;
            FilterCombobox.SelectionChanged += OnFilterSelectionChanged;
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFiltersAndSearch();
        }

        private void OnSortSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFiltersAndSearch();
        }

        private void OnFilterSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFiltersAndSearch();
        }

        private void ApplyFiltersAndSearch()
        {
            var searchText = SearchTextBox.Text?.ToLower() ?? string.Empty;
            var selectedFilter = FilterCombobox.SelectedIndex;
            var selectedSort = SortComboBox.SelectedIndex;

            var filtered = _products.Where(p => p.Productname.ToLower().Contains(searchText));

            if (selectedFilter > 0)
            {
                filtered = filtered.Where(p => selectedFilter switch
                {
                    1 => p.Productdiscountamount >= 0 && p.Productdiscountamount < 10,
                    2 => p.Productdiscountamount >= 10 && p.Productdiscountamount < 15,
                    3 => p.Productdiscountamount >= 15,
                    _ => true
                });
            }

            filtered = selectedSort switch
            {
                1 => filtered.OrderBy(p => p.Productcost),
                2 => filtered.OrderByDescending(p => p.Productcost),
                _ => filtered
            };

            _filteredProducts.Clear();
            foreach (var product in filtered)
            {
                _filteredProducts.Add(product);
            }

            UpdateCountText();
        }

        private void UpdateCountText()
        {
            CountRowTextBlock.Text = $"{_filteredProducts.Count} из {_products.Count}";
        }

        public class ProductPresenter : Models.Product
        {
            public Bitmap? Image
            {
                get
                {
                    try
                    {
                        return new Bitmap(Productphoto);
                    }
                    catch
                    {
                        return null;
                    }
                }
            }

            public string FinalCost
            {
                get
                {
                    if (Productdiscountamount > 0)
                    {
                        return (Productcost * (1 - Productdiscountamount / 100)).ToString();
                    }
                    return string.Empty;
                }
            }

            public bool IsDiscounted => Productdiscountamount > 0;

            public IBrush BackgroundColor => Productdiscountamount > 15 ? Brush.Parse("#7fff00") : Brushes.Transparent;
        }
    }
}