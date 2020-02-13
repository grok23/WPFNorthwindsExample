﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFNorthwindsExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NORTHWNDEntities db = new NORTHWNDEntities();

        public MainWindow()
        {
            InitializeComponent();
        }

        //shows products on a datagrid
        private void ShowProducts(DataGrid currentGrid)
        {
            var query = from p in db.Products
                        where p.Category.CategoryName.Equals("Beverages")
                        orderby p.ProductID descending
                        select new
                        {
                            p.ProductID,
                            p.ProductName,
                            p.Category.CategoryName,
                            p.UnitPrice
                        };
            currentGrid.ItemsSource = query.ToList();
        }

        //shows customer names
        private void btnQueryEx1_Click(object sender, RoutedEventArgs e)
        {
            var query = from c in db.Customers
                        select c.CompanyName;
            lbxCustomersEx1.ItemsSource = query.ToList();
        }

        //retrieves customer objects
        private void btnQueryEx2_Click(object sender, RoutedEventArgs e)
        {
            var query = from c in db.Customers 
                        select c;
            DGCustomersEx2.ItemsSource = query.ToList();
        }

        //retrieves order information
        private void btnQueryEx3_Click(object sender, RoutedEventArgs e)
        {
            var query = from o in db.Orders
                        where o.Customer.City.Equals("London")
                        || o.Customer.City.Equals("Paris")
                        || o.Customer.Country.Equals("USA")
                        orderby o.Customer.CompanyName
                        select new
                        {
                            CustomerName = o.Customer.CompanyName,
                            City = o.Customer.City,
                            Address = o.ShipAddress
                        };
            DGCustomersEx3.ItemsSource = query.ToList().Distinct();
        }

        //displays product information
        private void btnQueryEx4_Click(object sender, RoutedEventArgs e)
        {
            var query = from p in db.Products
                        where p.Category.CategoryName.Equals("Beverages")
                        orderby p.ProductID descending
                        select new
                        {
                            p.ProductID,
                            p.ProductName,
                            p.Category.CategoryName,
                            p.UnitPrice
                        };
            DGCustomersEx4.ItemsSource = query.ToList();
        }

        //inserts information to a table
        private void btnQueryEx5_Click(object sender, RoutedEventArgs e)
        {
            Product p = new Product()
            {
                ProductName = "Kickapoo Jungle Joy Juice",
                UnitPrice = 12.49m,
                CategoryID = 1
            };
            db.Products.Add(p);
            db.SaveChanges();

            ShowProducts(DGCustomersEx5);
        }

        //updates product information
        private void btnQueryEx6_Click(object sender, RoutedEventArgs e)
        {
            Product p1 = (db.Products
                .Where(p => p.ProductName.StartsWith("Kick"))
                .Select(p => p)).First();

            p1.UnitPrice = 100m;

            db.SaveChanges();
            ShowProducts(DGCustomersEx6);
        }

        //makes updates to multiple products
        private void btnQueryEx7_Click(object sender, RoutedEventArgs e)
        {
            var products = from p in db.Products
                           where p.ProductName.StartsWith("Kick")
                           select p;

            foreach (var item in products)
            {
                item.UnitPrice = 100m;
            }

            db.SaveChanges();
            ShowProducts(DGCustomersEx7);

        }

        //to delete items 
        private void btnQueryEx8_Click(object sender, RoutedEventArgs e)
        {
            var products = from p in db.Products
                           where p.ProductName.StartsWith("Kick")
                           select p;

            db.Products.RemoveRange(products);
            db.SaveChanges();
            ShowProducts(DGCustomersEx8);
        }

        //using a sproc
        private void btnQueryEx9_Click(object sender, RoutedEventArgs e)
        {
            var query = db.Customers_By_City("London");

            DGCustomersEx9.ItemsSource = query.ToList();
        }
    }

}
