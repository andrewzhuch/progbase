using System;
using Terminal.Gui;
using System.Collections.Generic;
using System.Collections;

namespace UserInterface
{
    class MainWindow : Window
    {
        private int pageLength = 10;
        private ListView pageOfProductsListView;
        private List<Product.Product> productsList;
        private int page = 1;
        private Label totalPagesPabel;
        private Label pagePabel;
        public MainWindow()
        {
            this.Title = "Products DB";
            pageOfProductsListView = new ListView(new List<Product.Product>())
            {
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            pageOfProductsListView.OpenSelectedItem += OnOpenProduct;
            Button prevButton = new Button(2, 6, "Prev");
            prevButton.Clicked += OnPreviousPage;
            pagePabel = new Label("?")
            {
                X = Pos.Right(prevButton) + 2,
                Y = Pos.Top(prevButton),
                Width = 5,
            };
            totalPagesPabel = new Label("?")
            {
                X = Pos.Right(pagePabel) + 2,
                Y = Pos.Top(prevButton),
                Width = 5,
            };
            Button nextButton = new Button("Next")
            {
                X = Pos.Right(totalPagesPabel) + 2,
                Y = Pos.Top(prevButton),
            };
            nextButton.Clicked += OnNextPage;
            this.Add(prevButton, nextButton, totalPagesPabel, pagePabel);
            FrameView frameView = new FrameView("Products")
            {
                X = 2,
                Y = 8,
                Width = Dim.Fill() -  4,
                Height = pageLength + 2
            };
            frameView.Add(pageOfProductsListView);
            this.Add(frameView);
            Button createNewProductBtn = new Button(2, 4, "Create new Product");
            createNewProductBtn.Clicked += OnCreateButtonCLicked;
            this.Add(createNewProductBtn);
        }
        public void SetList(List<Product.Product> list)
        {
            this.productsList = list;
            this.ShowCurrentPage();
        }
        private void OnPreviousPage()
        {
            if(page == 1)
            {
                return;
            }
            this.page--;
            ShowCurrentPage();
        }
        private void OnNextPage()
        {
            int totalPages = ProductRepo.ProductRepo.GetTotalPages();
            if(page >= totalPages)
            {
                return;
            }
            this.page++;
            ShowCurrentPage();
        }
        public void ShowCurrentPage()
        {
            this.pagePabel.Text = page.ToString();
            this.totalPagesPabel.Text = ProductRepo.ProductRepo.GetTotalPages().ToString();
            this.pageOfProductsListView.SetSource(ProductRepo.ProductRepo.GetPage(page));
        }
        private void OnCreateButtonCLicked()
        {
            CreateProductDialog dialog = new CreateProductDialog();
            Application.Run(dialog);
            if(!dialog.canceled)
            {
                Product.Product product = dialog.GetProduct();
                if(product == null)
                {
                    return;
                }
                long ooo = ProductRepo.ProductRepo.Insert(product);
                OpenProductDialog dialog2 = new OpenProductDialog();//
                product.id = ooo;
                dialog2.SetProduct(product);//
                Application.Run(dialog2);//
                pageOfProductsListView.SetSource(ProductRepo.ProductRepo.GetPage(page));
                ShowCurrentPage();
            }
        }
        private void OnOpenProduct(ListViewItemEventArgs args)
        {
            Product.Product product = (Product.Product)args.Value;
            OpenProductDialog dialog = new OpenProductDialog();
            dialog.SetProduct(product);
            Application.Run(dialog);
            if(dialog.deleted)
            {
                bool result = ProductRepo.ProductRepo.DeleteById(product);
                if(result)
                {
                    int pages = ProductRepo.ProductRepo.GetTotalPages();
                    if(page > pages && page > 1)
                    {
                        page--;
                        this.ShowCurrentPage();
                    }
                    pageOfProductsListView.SetSource(ProductRepo.ProductRepo.GetPage(page));
                }
                else
                {
                    MessageBox.ErrorQuery("Delete product", "Cannot delete product", "Ok");
                }
            }
            if(dialog.edited)
            {
                ProductRepo.ProductRepo.UpdateProduct(dialog.GetProduct(), product.id, product.createdAt);
                pageOfProductsListView.SetSource(ProductRepo.ProductRepo.GetPage(page));
                this.ShowCurrentPage();
            }
        }
    }
    class EditProductDialog : CreateProductDialog
    {
        public EditProductDialog()
        {
            this.Title = "Edit product";
        }
        public void SetProduct(Product.Product product)
        {
            this.productNameInput.Text = product.name; 
            this.productInfoInput.Text = product.info;
            this.isOnStorageCheckbox.Checked = product.onStorage;
            this.productPriceInput.Text = product.price.ToString();
        }

    }
    class OpenProductDialog : Dialog
    {
        public bool edited;
        protected Product.Product product;
        public bool deleted;
        private TextField productId;
        private TextField productNameInput;
        private TextView productInfoInput;
        private TextField productPriceInput;
        private TextField isOnStorageLabel;
        private TextField createdAt;
        public OpenProductDialog()
        {
            this.Title = "Open product";
            Button okBtn = new Button("Back");
            okBtn.Clicked += OnCreatedDialogSubmit;
            this.AddButton(okBtn);
            Label productNameLabel = new Label(2, 2, "Name");
            productNameInput = new TextField("")
            {
                X = 20, Y = Pos.Top(productNameLabel),
                Width = 40,
                ReadOnly = true,
            };
            this.Add(productNameLabel, productNameInput);
            Label productInfoLabel = new Label(2, 4, "Info");
            productInfoInput = new TextView()
            {
                X = 20,
                Y = Pos.Top(productInfoLabel),
                Width = Dim.Fill(5),
                Height = 10,
                ReadOnly = true,
            };
            this.Add(productInfoLabel, productInfoInput);
            Label productPriceLabel = new Label(2, 16, "Price");
            productPriceInput = new TextField("")
            {
                X = 20, Y = Pos.Top(productPriceLabel),
                Width = 40,
                ReadOnly = true,
            };
            this.Add(productPriceLabel, productPriceInput);
            Label productIdLabel = new Label(2, 20, "id");
            productId = new TextField("")
            {
                X = 20, Y = Pos.Top(productIdLabel), Width = 40, ReadOnly = true,
            };
            this.Add(productIdLabel, productId);
            Label productOnStorafeLabel = new Label(2, 18, "On storage");
            isOnStorageLabel = new TextField("")
            {
                X = 20, Y = Pos.Top(productOnStorafeLabel), Width = 40,
                ReadOnly = true,
            };
            this.Add(productOnStorafeLabel, isOnStorageLabel);
            Label productCreationTime = new Label(2, 22, "Created at");
            createdAt = new TextField("")
            {
                X = 20, Y = Pos.Top(productCreationTime), Width = 40,
                ReadOnly = true,
            };
            this.Add(productCreationTime, createdAt);
            Button button = new Button(2, 24, "Delete");
            Button button1 = new Button(12, 24, "Edit");
            button1.Clicked += OnProductEdit;
            button.Clicked += OnProductDelete;
            this.Add(button);
            this.Add(button1);
        }
        public Product.Product GetProduct()
        {
            return this.product;
        }
        private void OnProductEdit()
        {
            EditProductDialog dialog = new EditProductDialog();
            dialog.SetProduct(this.product);
            Application.Run(dialog);
            if(!dialog.canceled)
            {
                Product.Product newProduct = dialog.GetProduct();
                this.edited = true;
                this.SetProduct(newProduct);
            }
        }
        private void OnProductDelete()
        {
            int index = MessageBox.Query("Delete book", "Are you sure?", "No", "Yes");
            if(index == 1)
            {
                this.deleted = true;
                Application.RequestStop();
            }
        }
        private void OnCreatedDialogSubmit()
        {
            Application.RequestStop();
        }
        public void SetProduct(Product.Product product)
        {
            if(product == null)
            {
                return;
            }
            this.product = product;
            this.productInfoInput.Text = product.info;
            this.productNameInput.Text = product.name;
            this.productPriceInput.Text = product.price.ToString();
            this.isOnStorageLabel.Text = product.onStorage.ToString();
            this.productId.Text = product.id.ToString();
            this.createdAt.Text = product.createdAt.ToString();
        }
    }
    class CreateProductDialog : Dialog
    {
        public bool canceled;
        protected TextField productNameInput;
        protected TextView productInfoInput;
        protected TextField productPriceInput;
        protected CheckBox isOnStorageCheckbox;
        public CreateProductDialog()
        {
            this.Title = "Create product";
            Button okBtn = new Button("OK");
            okBtn.Clicked += OnCreatedDialogSubmit;
            Button cancelBtn = new Button("Cancel");
            cancelBtn.Clicked += OnCreatedDialogCanceled;
            this.AddButton(okBtn);
            this.AddButton(cancelBtn);
            Label productNameLabel = new Label(2, 2, "Name");
            productNameInput = new TextField("")
            {
                X = 20, Y = Pos.Top(productNameLabel),
                Width = 40,
            };
            this.Add(productNameLabel, productNameInput);
            Label productInfoLabel = new Label(2, 4, "Info");
            productInfoInput = new TextView()
            {
                X = 20,
                Y = Pos.Top(productInfoLabel),
                Width = Dim.Fill(5),
                Height = 10,
            };
            this.Add(productInfoLabel, productInfoInput);
            Label productPriceLabel = new Label(2, 16, "Price");
            productPriceInput = new TextField("")
            {
                X = 20, Y = Pos.Top(productPriceLabel),
                Width = 40,
            };
            this.Add(productPriceLabel, productPriceInput);
            Label productOnStorafeLabel = new Label(2, 18, "On storage");
            isOnStorageCheckbox = new CheckBox("")
            {
                X = 20, Y = Pos.Top(productOnStorafeLabel), Width = 40,
            };
            this.Add(productOnStorafeLabel, isOnStorageCheckbox);
        }
        private void OnCreatedDialogCanceled()
        {
            this.canceled = true;
            Application.RequestStop();
        }
        private void OnCreatedDialogSubmit()
        {
            this.canceled = false;
            Application.RequestStop();
        }
        public Product.Product GetProduct()
        {
            int k;
            bool TryParse = int.TryParse(productPriceInput.Text.ToString(), out k);
            if(TryParse == false)
            {
                MessageBox.ErrorQuery("Error", "Wrong data inputed", "OK");
                return null;
            }
            return new Product.Product()
            {
                name = productNameInput.Text.ToString(),
                info = productInfoInput.Text.ToString(),
                price = k,
                onStorage = isOnStorageCheckbox.Checked,
                createdAt = DateTime.Now,
            };
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            List<Product.Product> a = ProductRepo.ProductRepo.GetPage(1);
            Application.Init();
            MenuBar menu = new MenuBar(new MenuBarItem[] {
            new MenuBarItem ("_File", new MenuItem [] {
               new MenuItem ("_Quit", "", OnQuit)
                }), new MenuBarItem("_Help", new MenuItem[]{new MenuItem("_About", "", GetInfo)})
            });
            Toplevel top = Application.Top;
            MainWindow win = new MainWindow();
            win.SetList(a);
            top.Add(win, menu);
            Application.Run();
        }
        static void GetInfo()
        {
            Button btn = new Button("OK");
            btn.Clicked += StopModal;
            Dialog w = new Dialog("About", btn);
            w.Add(btn);
            Label productPriceLabel = new Label(2, 16, "Developed by Andrew Zhuchenko");
            Label productPriceLabel1 = new Label(2, 18, "Program works with database");
            w.Add(productPriceLabel);
            w.Add(productPriceLabel1);
            Application.Run(w);
        }
         static void StopModal()
        {
            Application.RequestStop();
        }
        static void OnQuit()
        {
            Application.RequestStop();
        }
    }
}
