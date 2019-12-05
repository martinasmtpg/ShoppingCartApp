using ShoppingCartApp.Content;
using ShoppingCartApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using Outlook = Microsoft.Office.Interop.Outlook;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System.ComponentModel;
using System.Drawing;

namespace ShoppingCartApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MyContext myContext = new MyContext();
        int supplierId;
        int itemId;
        int userId;
        string pass;
        int totalpayment, submitpayment, getpay, idTrans;
        List<TransactionItem> cart = new List<TransactionItem>();
        string receipt = "Id\t" + "Product Name\t" + "Price\t" + "Quantity\t" + "Total\t" + "\n";
        public MainWindow()
        {
            InitializeComponent();
            showData();
            dataGridSupp.ItemsSource = myContext.Suppliers.ToList();
            dataGridItem.ItemsSource = myContext.Items.ToList();
            dataGridRole.ItemsSource = myContext.Auth.ToList();
            dataGridReg.ItemsSource = myContext.User.ToList();
            var cb = CbSupplier.SelectedValue;
            CbSupplier.ItemsSource = myContext.Suppliers.ToList();
            CbRole.ItemsSource = myContext.Auth.ToList();
            CbCart.ItemsSource = myContext.Items.ToList();
            CbCart.DisplayMemberPath = "Name";
            CbCart.SelectedValuePath = "Id";
            TxtOrderDate.Text = DateTimeOffset.Now.DateTime.ToString();
        }

        private void BtnLogin_Click_1(object sender, RoutedEventArgs e)
        {
            gLogin.Visibility = Visibility.Visible;
            CartIcon.Visibility = Visibility.Hidden;
            ChangePassIcon.Visibility = Visibility.Hidden;
            LogoutIcon.Visibility = Visibility.Hidden;
            gForgotPass.Visibility = Visibility.Hidden;
            menuPanel.Visibility = Visibility.Hidden;
            gSuppliers.Visibility = Visibility.Hidden;
            gProducts.Visibility = Visibility.Hidden;
            gCart.Visibility = Visibility.Hidden;
            gRegist.Visibility = Visibility.Hidden;
            gChangePass.Visibility = Visibility.Hidden;
            gRole.Visibility = Visibility.Hidden;

            try
            {
                var email = myContext.User.Where(u => u.Email == textBoxEmail.Text).FirstOrDefault();

                if ((textBoxEmail.Text == "") || (passwordBox1.Password == ""))
                {
                    if (textBoxEmail.Text == "")
                    {
                        MessageBox.Show("Email is Required!", "Caution", MessageBoxButton.OK);
                        textBoxEmail.Focus();
                    }
                    else if (passwordBox1.Password == "")
                    {
                        MessageBox.Show("Password is Required!", "Caution", MessageBoxButton.OK);
                        passwordBox1.Focus();
                    }
                }
                else
                {
                    if (email != null)
                    {
                        var pass = email.Password;
                        pass = passwordBox1.Password;
                        if (passwordBox1.Password == pass)
                        {
                            MessageBox.Show("Login Successfully!", "Login Succes", MessageBoxButton.OK);
                            gSuppliers.Visibility = Visibility.Visible;
                            menuPanel.Visibility = Visibility.Visible;
                            gLogin.Visibility = Visibility.Hidden;
                            CartIcon.Visibility = Visibility.Visible;
                            showData();
                        }
                        else
                        {
                            MessageBox.Show("Email and Password are wrong!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Email and Password is invalid");
                    }

                }
            }
            catch (Exception)
            {

            }
        }


        #region
        public void showData()
        {
            dataGridSupp.ItemsSource = myContext.Suppliers.ToList();
            dataGridItem.ItemsSource = myContext.Items.ToList();
            //dataGridCart.ItemsSource = myContext.TransactionItem.ToList();
        }
        private void BtnMenuSupp_Click(object sender, RoutedEventArgs e)
        {
            gLogin.Visibility = Visibility.Hidden;
            CartIcon.Visibility = Visibility.Visible;
            ChangePassIcon.Visibility = Visibility.Visible;
            LogoutIcon.Visibility = Visibility.Visible;
            gForgotPass.Visibility = Visibility.Hidden;
            menuPanel.Visibility = Visibility.Visible;
            gSuppliers.Visibility = Visibility.Visible;
            gProducts.Visibility = Visibility.Hidden;
            gCart.Visibility = Visibility.Hidden;
            gRegist.Visibility = Visibility.Hidden;
            gChangePass.Visibility = Visibility.Hidden;
            gRole.Visibility = Visibility.Hidden;
        }
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnSubmitSupp_Click(object sender, RoutedEventArgs e)
        {
            if (TxtNameSup.Text == "")
            {
                MessageBox.Show("Name Should be Filled!", "Warning!", MessageBoxButton.OK);
                TxtNameSup.Focus();
            }
            else if (TxtEmailSup.Text == "")
            {
                MessageBox.Show("Email Should be Filled!", "Warning!", MessageBoxButton.OK);
                TxtEmailSup.Focus();
            }
            else
            {
                var validEmail = myContext.Suppliers.FirstOrDefault(c => c.Email == TxtEmailSup.Text);
                if (validEmail == null)
                {
                    var push = new Supplier(TxtNameSup.Text, TxtEmailSup.Text);
                    myContext.Suppliers.Add(push);
                    var result = myContext.SaveChanges();
                    showData();
                    TxtNameSup.Text = "";
                    TxtEmailSup.Text = "";
                    if (result > 0)
                    {
                        MessageBox.Show(result + " row has been inserted");
                        dataGridSupp.ItemsSource = myContext.Suppliers.ToList();
                        try
                        {
                            //Outlook._Application _app = new Outlook.Application();
                            //Outlook.MailItem mail = (Outlook.MailItem)_app.CreateItem(Outlook.OlItemType.olMailItem);
                            ////sesuaikan dengan content yang di xaml
                            //mail.To = TxtEmail.Text;
                            //mail.Subject = "Try to Send Mail";//isi sendiri ini seperti subject pas mau ngirim email;
                            //mail.Body = "cobacobacoba";
                            //mail.Importance = Outlook.OlImportance.olImportanceNormal;
                            //((Outlook._MailItem)mail).Send();
                            MessageBox.Show("Message has been sent.", "Message", MessageBoxButton.OK);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK);
                            TxtNameSup.Text = "";
                            TxtEmailSup.Text = "";
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Email has been registered.", "Caution", MessageBoxButton.OK);
                }
            }
        }

        private void dataGridSupp_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var data = dataGridSupp.SelectedItem;
            string id = (dataGridSupp.SelectedCells[0].Column.GetCellContent(data) as TextBlock).Text;
            TxtIdSup.Text = id;
            string name = (dataGridSupp.SelectedCells[1].Column.GetCellContent(data) as TextBlock).Text;
            TxtNameSup.Text = name;
            string email = (dataGridSupp.SelectedCells[2].Column.GetCellContent(data) as TextBlock).Text;
            TxtEmailSup.Text = email;
        }

        private void BtnUpdateSupp_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(TxtIdSup.Text);
            var updt = myContext.Suppliers.Where(i => i.Id == id).FirstOrDefault();
            updt.Name = TxtNameSup.Text;
            updt.Email = TxtEmailSup.Text;
            myContext.SaveChanges();
            dataGridSupp.ItemsSource = myContext.Suppliers.ToList();
            MessageBox.Show("Data is Updated !");
        }

        private void BtnDelSupp_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are You Sure Want to Delete This Supplier?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    int id = Convert.ToInt32(TxtIdSup.Text);
                    var del = myContext.Suppliers.Where(i => i.Id == id).FirstOrDefault();
                    myContext.Suppliers.Remove(del);
                    myContext.SaveChanges();
                    dataGridSupp.ItemsSource = myContext.Suppliers.ToList();
                }
                catch (Exception)
                {
                    MessageBox.Show("Data has been delete.", "Caution", MessageBoxButton.OK);
                }
            }
        }

        private void TxtEmailSup_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^A-Za-z0-9.@]+$");
            e.Handled = regex.IsMatch(e.Text);
        }
        #endregion

        private void BtnMenuProd_Click_1(object sender, RoutedEventArgs e)
        {
            gLogin.Visibility = Visibility.Hidden;
            CartIcon.Visibility = Visibility.Visible;
            ChangePassIcon.Visibility = Visibility.Visible;
            LogoutIcon.Visibility = Visibility.Visible;
            gForgotPass.Visibility = Visibility.Hidden;
            menuPanel.Visibility = Visibility.Visible;
            gSuppliers.Visibility = Visibility.Hidden;
            gProducts.Visibility = Visibility.Visible;
            gCart.Visibility = Visibility.Hidden;
            gRegist.Visibility = Visibility.Hidden;
            gChangePass.Visibility = Visibility.Hidden;
            gRole.Visibility = Visibility.Hidden;
            CbSupplier.ItemsSource = myContext.Suppliers.ToList();
            CbCart.ItemsSource = myContext.Items.ToList();
        }

        private void BtnSubmitItem_Click(object sender, RoutedEventArgs e)
        {
            int Stock = Convert.ToInt32(TxtStock.Text);
            int Price = Convert.ToInt32(TxtPrice.Text);
            if (TxtNameItem.Text == "")
            {
                MessageBox.Show("Name Should be Filled!", "Warning!", MessageBoxButton.OK);
                TxtNameItem.Focus();
            }
            else if (TxtStock.Text == "")
            {
                MessageBox.Show("Stock Should be Filled!", "Warning!", MessageBoxButton.OK);
                TxtStock.Focus();
            }
            else if (TxtPrice.Text == "")
            {
                MessageBox.Show("Price Should be Filled!", "Warning!", MessageBoxButton.OK);
                TxtPrice.Focus();
            }
            else if (CbSupplier.Text == "")
            {
                MessageBox.Show("Select Supplier Name!", "Warning!", MessageBoxButton.OK);
            }
            else
            {
                var supplier = myContext.Suppliers.Where(s => s.Id == supplierId).FirstOrDefault();

                var push = new Item(TxtNameItem.Text, Stock, Price, supplier);
                myContext.Items.Add(push);
                var result = myContext.SaveChanges();

                if (result > 0)
                {
                    MessageBox.Show("1 row has been inserted");

                    TxtNameItem.Text = "";
                    TxtStock.Text = "";
                    TxtPrice.Text = "";
                    dataGridItem.ItemsSource = myContext.Items.ToList();
                }
            }
        }
        private void CbSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            supplierId = Convert.ToInt32(CbSupplier.SelectedValue.ToString());
        }

        private void dataGridItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var data = dataGridItem.SelectedItem;
            string id = (dataGridItem.SelectedCells[0].Column.GetCellContent(data) as TextBlock).Text;
            TxtIdItem.Text = id;
            string name = (dataGridItem.SelectedCells[1].Column.GetCellContent(data) as TextBlock).Text;
            TxtNameItem.Text = name;
            string stock = (dataGridItem.SelectedCells[2].Column.GetCellContent(data) as TextBlock).Text;
            TxtStock.Text = stock;
            string price = (dataGridItem.SelectedCells[3].Column.GetCellContent(data) as TextBlock).Text;
            TxtPrice.Text = price;
        }

        private void BtnUpdateItem_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(TxtIdItem.Text);
            var supp = myContext.Suppliers.Where(s => s.Id == supplierId).FirstOrDefault();
            var updt = myContext.Items.FirstOrDefault(i => i.Id == id);
            updt.Name = TxtNameItem.Text;
            updt.Stock = Convert.ToInt32(TxtStock.Text);
            updt.Price = Convert.ToInt32(TxtPrice.Text);
            updt.Supplier = supp;
            myContext.SaveChanges();
            dataGridItem.ItemsSource = myContext.Items.ToList();
            MessageBox.Show("Data is Updated");
        }

        private void BtnDelItem_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are You Sure Want to Delete This Product?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    int id = Convert.ToInt32(TxtIdItem.Text);
                    var del = myContext.Items.Where(i => i.Id == id).FirstOrDefault();
                    myContext.Items.Remove(del);
                    myContext.SaveChanges();
                    dataGridItem.ItemsSource = myContext.Items.ToList();
                }
                catch (Exception)
                {
                    MessageBox.Show("Delete Success!", "Success", MessageBoxButton.OK);
                }
            }
        }
        private void TxtNameItem_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^A-Za-z0-9!]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void TxtStock_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void TxtPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void BtnIconCart_Click_3(object sender, RoutedEventArgs e)
        {
            gLogin.Visibility = Visibility.Hidden;
            CartIcon.Visibility = Visibility.Visible;
            ChangePassIcon.Visibility = Visibility.Visible;
            LogoutIcon.Visibility = Visibility.Visible;
            gForgotPass.Visibility = Visibility.Hidden;
            menuPanel.Visibility = Visibility.Visible;
            gSuppliers.Visibility = Visibility.Hidden;
            gProducts.Visibility = Visibility.Hidden;
            gCart.Visibility = Visibility.Visible;
            gRegist.Visibility = Visibility.Hidden;
            gChangePass.Visibility = Visibility.Hidden;
            gRole.Visibility = Visibility.Hidden;
            CbSupplier.ItemsSource = myContext.Suppliers.ToList();
            CbCart.ItemsSource = myContext.Items.ToList();
        }

        private void TxtQtyCart_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void CbCart_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            itemId = Convert.ToInt32(CbCart.SelectedValue.ToString());
            var item = myContext.Items.Where(i => i.Id == itemId).FirstOrDefault();
            TxtPriceCart.Text = item.Price.ToString();
            TxtRecentStock.Text = item.Stock.ToString();
            CbSupplier.ItemsSource = myContext.Suppliers.ToList();
            CbCart.ItemsSource = myContext.Items.ToList();
        }
        private void BtnSubmitCart_Click(object sender, RoutedEventArgs e)
        {
            if (CbCart.Text == "")
            {
                MessageBox.Show("Select a Product Name!", "Warning!", MessageBoxButton.OK);
            }
            else if (TxtQtyCart.Text == "")
            {
                MessageBox.Show("Quantity Should be Filled!", "Warning!", MessageBoxButton.OK);
            }
            else
            {

                string cartId = itemId.ToString();
                int qty = Convert.ToInt32(TxtQtyCart.Text);
                int price = Convert.ToInt32(TxtPriceCart.Text);
                int stock = Convert.ToInt32(TxtRecentStock.Text);
                int updstk = stock - qty;
                int total = qty * price;
                totalpayment += total;

                idTrans = Convert.ToInt32(TxtIdTrans.Text);
                var trans = myContext.Transaction.Where(c => c.Id == idTrans).FirstOrDefault();
                var prod = myContext.Items.Where(p => p.Id == itemId).FirstOrDefault();
                prod.Stock = updstk;
                myContext.SaveChanges();
                showData();
                TxtTotalPayment.Text = totalpayment.ToString();
                TxtTotalCart.Text = total.ToString();

                cart.Add(new TransactionItem { Transaction = trans, Item = prod, Quantity = qty });
                dataGridCart.Items.Add(new { Name = CbCart.Text, Price = TxtPriceCart.Text, Quantity = TxtQtyCart.Text, Total = total.ToString() });
                TxtPriceCart.Text = "";
                TxtQtyCart.Text = "";
                TxtRecentStock.Text = "";
            }
        }

        private void BtnRemoveCart_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridCart.SelectedItem != null)
            {
                dataGridCart.Items.Remove(dataGridCart.SelectedItem);
            }
        }

        private void TxtPayment_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TxtPayment_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                int payment = Convert.ToInt32(TxtPayment.Text);
                int totalpayment = Convert.ToInt32(TxtTotalPayment.Text);
                TxtChange.Text = (payment - totalpayment).ToString();
            }
            catch (Exception)
            {

            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            gLogin.Visibility = Visibility.Hidden;
            CartIcon.Visibility = Visibility.Hidden;
            ChangePassIcon.Visibility = Visibility.Hidden;
            LogoutIcon.Visibility = Visibility.Hidden;
            gForgotPass.Visibility = Visibility.Visible;
            menuPanel.Visibility = Visibility.Hidden;
            gSuppliers.Visibility = Visibility.Hidden;
            gProducts.Visibility = Visibility.Hidden;
            gCart.Visibility = Visibility.Hidden;
            gRegist.Visibility = Visibility.Hidden;
            gChangePass.Visibility = Visibility.Hidden;
            gRole.Visibility = Visibility.Hidden;
        }

        private void Hyperlink_Click_1(object sender, RoutedEventArgs e)
        {
            gLogin.Visibility = Visibility.Visible;
            CartIcon.Visibility = Visibility.Hidden;
            ChangePassIcon.Visibility = Visibility.Hidden;
            LogoutIcon.Visibility = Visibility.Hidden;
            gForgotPass.Visibility = Visibility.Hidden;
            menuPanel.Visibility = Visibility.Hidden;
            gSuppliers.Visibility = Visibility.Hidden;
            gProducts.Visibility = Visibility.Hidden;
            gCart.Visibility = Visibility.Hidden;
            gRegist.Visibility = Visibility.Hidden;
            gChangePass.Visibility = Visibility.Hidden;
            gRole.Visibility = Visibility.Hidden;
        }

        private void BtnPayment_Click(object sender, RoutedEventArgs e)
        {
            submitpayment = Convert.ToInt32(TxtTotalPayment.Text);
            getpay = Convert.ToInt32(TxtPayment.Text);
            if (TxtPayment.Text == "")
            {
                MessageBox.Show("Payment Should be Filled!", "Caution");
            }
            else if (submitpayment <= getpay)
            {
                int idtrans = Convert.ToInt32(TxtIdTrans.Text);
                var prod = myContext.TransactionItem.FirstOrDefault(p => p.Transaction.Id == idtrans);
                var trans = myContext.Transaction.FirstOrDefault(t => t.Id == idtrans);
                int totalpay = Convert.ToInt32(TxtTotalPayment.Text);
                trans.Total = totalpay;
                foreach (var transCart in cart)
                {
                    myContext.TransactionItem.Add(transCart);
                    myContext.SaveChanges();
                    receipt += transCart.Transaction.Id.ToString() + "\t" + transCart.Item.Name + "\t" + transCart.Item.Price + "\t" + transCart.Quantity + "\t" + transCart.Transaction.Total + "\t";
                }
                MessageBox.Show("Transaction Successes!", "Notification", MessageBoxButton.OK);
                //Create a new PDF document
                using (PdfDocument document = new PdfDocument())
                {
                    //Add a page to the document
                    PdfPage page = document.Pages.Add();

                    //Create PDF graphics for the page
                    PdfGraphics graphics = page.Graphics;

                    //Set the standard font
                    PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);

                    //Draw the text
                    graphics.DrawString(receipt, font, PdfBrushes.Black, new PointF(0, 0));

                    //Save the document
                    document.Save("Receipt.pdf");

                    #region View the Workbook
                    //Message box confirmation to view the created document.
                    if (MessageBox.Show("Do you want to view the PDF?", "PDF has been created",
                        MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    {
                        try
                        {
                            //Launching the Excel file using the default Application.[MS Excel Or Free ExcelViewer]
                            System.Diagnostics.Process.Start("Receipt.pdf");

                            //Exit
                            Close();
                        }
                        catch (Win32Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                    else
                        Close();
                    #endregion
                }
            }
            else
            {
                MessageBox.Show("Transaction Failed!", "Caution", MessageBoxButton.OK);
            }
        }

        private void dataGridRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var data = dataGridRole.SelectedItem;
            string id = (dataGridRole.SelectedCells[0].Column.GetCellContent(data) as TextBlock).Text;
            TxtIdRole.Text = id;
            string email = (dataGridRole.SelectedCells[2].Column.GetCellContent(data) as TextBlock).Text;
            TxtEmailRole.Text = email;
        }

        private void BtnSubmitRole_Click(object sender, RoutedEventArgs e)
        {
            if (TxtRole.Text == "")
            {
                MessageBox.Show("Role Should be Filled!", "Warning!", MessageBoxButton.OK);
                TxtRole.Focus();
            }
            else if (TxtEmailRole.Text == "")
            {
                MessageBox.Show("Email Should be Filled!", "Warning!", MessageBoxButton.OK);
                TxtEmailRole.Focus();
            }
            else
            {
                var validEmail = myContext.Auth.FirstOrDefault(a => a.Email == TxtEmailRole.Text);
                if (validEmail == null)
                {
                    var push = new Auth(TxtRole.Text, TxtEmailRole.Text);
                    myContext.Auth.Add(push);
                    var result = myContext.SaveChanges();
                    showData();
                    TxtEmailRole.Text = "";
                    if (result > 0)
                    {
                        MessageBox.Show(result + " row has been inserted");
                        dataGridRole.ItemsSource = myContext.Auth.ToList();
                    }
                }
                else
                {
                    MessageBox.Show("Email has been registered.", "Caution", MessageBoxButton.OK);
                }
            }
        }

        private void BtnEditRole_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(TxtIdRole.Text);
            var edit = myContext.Auth.Where(ia => ia.Id == id).FirstOrDefault();
            edit.Email = TxtEmailRole.Text;
            myContext.SaveChanges();
            dataGridRole.ItemsSource = myContext.Auth.ToList();
            MessageBox.Show("Data is Updated !");
        }

        private void BtnDeleteRole_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are You Sure Want to Delete This Role?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    int id = Convert.ToInt32(TxtIdRole.Text);
                    var del = myContext.Auth.Where(ia => ia.Id == id).FirstOrDefault();
                    myContext.Auth.Remove(del);
                    myContext.SaveChanges();
                    dataGridRole.ItemsSource = myContext.Auth.ToList();
                }
                catch (Exception)
                {
                    MessageBox.Show("Data has been delete.", "Caution", MessageBoxButton.OK);
                }
            }
        }

        private void BtnSubmitRegist_Click(object sender, RoutedEventArgs e)
        {
            if (TxtEmailReg.Text == "")
            {
                MessageBox.Show("Email Should be Filled!", "Warning!", MessageBoxButton.OK);
                TxtEmailReg.Focus();
            }
            else if (CbRole.Text == "")
            {
                MessageBox.Show("Select a Role!", "Warning!", MessageBoxButton.OK);
            }
            else
            {
                var auth = myContext.User.Where(u => u.Email == TxtEmailReg.Text).FirstOrDefault();
                pass = Guid.NewGuid().ToString();
                var role = myContext.Auth.Where(a => a.Id == userId).FirstOrDefault();

                if(auth == null)
                {
                    var pushreg = new User(TxtNameReg.Text, TxtEmailReg.Text, pass, role);
                    myContext.User.Add(pushreg);
                    var result = myContext.SaveChanges();
                    if (result > 0)
                    {
                        MessageBox.Show("Register Successes");
                        try
                        {
                            Outlook._Application _app = new Outlook.Application();
                            Outlook.MailItem mail = (Outlook.MailItem)_app.CreateItem(Outlook.OlItemType.olMailItem);
                            mail.To = TxtEmailReg.Text;
                            mail.Subject = "New Registered Account Shopping Cart App";
                            mail.Body = "Hi There!" + "/n" + "Use this password to login in Shopping Cart App : " + pass;
                            mail.Importance = Outlook.OlImportance.olImportanceNormal;
                            ((Outlook._MailItem)mail).Send();
                            MessageBox.Show("Password Has Been Sent to The Email Address.", "Message", MessageBoxButton.OK);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK);
                        }
                    }
                    dataGridReg.ItemsSource = myContext.User.ToList();
                    showData();
                }
            }
        }

        private void CbRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            userId = Convert.ToInt32(CbRole.SelectedValue.ToString());
        }

        private void BtnSendNewPass_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TxtEmailForgotPass.Text == "")
                {
                    MessageBox.Show("Email Should be Filled!", "Warning!", MessageBoxButton.OK);
                    TxtEmailReg.Focus();
                }
                else
                {
                    var auth = myContext.User.Where(u => u.Email == TxtEmailForgotPass.Text).FirstOrDefault();
                    if (auth != null)
                    {
                        var emailverif = auth.Email;
                        if (TxtEmailForgotPass.Text == emailverif)
                        {
                            string newpass = Guid.NewGuid().ToString();
                            var forgotpass = myContext.User.Where(f => f.Email == TxtEmailForgotPass.Text).FirstOrDefault();
                            forgotpass.Password = newpass;
                            myContext.SaveChanges();
                            MessageBox.Show("Password Has Reset");
                            Outlook._Application _app = new Outlook.Application();
                            Outlook.MailItem mail = (Outlook.MailItem)_app.CreateItem(Outlook.OlItemType.olMailItem);
                            mail.To = TxtEmailReg.Text;
                            mail.Subject = "Your Reset Password Account Shopping Cart App";
                            mail.Body = "Hi There!" + TxtEmailForgotPass.Text + "\nHere you go, this is your reset password : " + newpass + "\nDon't forget to login again to Shopping Cart App";
                            mail.Importance = Outlook.OlImportance.olImportanceNormal;
                            ((Outlook._MailItem)mail).Send();
                            MessageBox.Show("Reset Password Has Been Sent to Your Email Address.", "Message", MessageBoxButton.OK);
                        }
                    }
                    //else
                    //{
                    //    MessageBox.Show("Your Email Doesn't Register", "Caution", MessageBoxButton.OK);
                    //}
                }
            }
            catch (Exception)
            {

            }
        }

        private void LogoutIcon_Click(object sender, RoutedEventArgs e)
        {
            gLogin.Visibility = Visibility.Visible;
            CartIcon.Visibility = Visibility.Hidden;
            ChangePassIcon.Visibility = Visibility.Hidden;
            LogoutIcon.Visibility = Visibility.Hidden;
            gForgotPass.Visibility = Visibility.Hidden;
            menuPanel.Visibility = Visibility.Hidden;
            gSuppliers.Visibility = Visibility.Hidden;
            gProducts.Visibility = Visibility.Hidden;
            gCart.Visibility = Visibility.Hidden;
            gRegist.Visibility = Visibility.Hidden;
            gChangePass.Visibility = Visibility.Hidden;
            gRole.Visibility = Visibility.Hidden;
        }

        private void BtnNewTrans_Click(object sender, RoutedEventArgs e)
        {
            var push = new Transaction();
            myContext.Transaction.Add(push);
            myContext.SaveChanges();
            TxtIdTrans.Text = Convert.ToString(push.Id);
        }

        private void BtnRegister_Click_2(object sender, RoutedEventArgs e)
        {
            gLogin.Visibility = Visibility.Hidden;
            CartIcon.Visibility = Visibility.Visible;
            ChangePassIcon.Visibility = Visibility.Visible;
            LogoutIcon.Visibility = Visibility.Visible;
            gForgotPass.Visibility = Visibility.Hidden;
            menuPanel.Visibility = Visibility.Visible;
            gSuppliers.Visibility = Visibility.Hidden;
            gProducts.Visibility = Visibility.Hidden;
            gCart.Visibility = Visibility.Hidden;
            gRegist.Visibility = Visibility.Visible;
            gChangePass.Visibility = Visibility.Hidden;
            gRole.Visibility = Visibility.Hidden;
        }

        private void BtnRole_Click(object sender, RoutedEventArgs e)
        {
            gLogin.Visibility = Visibility.Hidden;
            CartIcon.Visibility = Visibility.Visible;
            ChangePassIcon.Visibility = Visibility.Visible;
            LogoutIcon.Visibility = Visibility.Visible;
            gForgotPass.Visibility = Visibility.Hidden;
            menuPanel.Visibility = Visibility.Visible;
            gSuppliers.Visibility = Visibility.Hidden;
            gProducts.Visibility = Visibility.Hidden;
            gCart.Visibility = Visibility.Hidden;
            gRegist.Visibility = Visibility.Hidden;
            gChangePass.Visibility = Visibility.Hidden;
            gRole.Visibility = Visibility.Visible;
        }
    }
}
