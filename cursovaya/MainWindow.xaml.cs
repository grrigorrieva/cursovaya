using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
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

namespace cursovaya
{
    public partial class MainWindow : Window
    {
        List<MyPicture> pictures = new List<MyPicture>();
        List<MyPicture> TempPictures = new List<MyPicture>(); 

        static string NameFileJSON = "PicturesCatalog.json";
        public MainWindow()
        {
            InitializeComponent();

            categ.Items.Add("all");

            if (File.Exists(NameFileJSON))
            {
                List<MyPicture> readedPicturs = JsonSerializer.Deserialize<List<MyPicture>>(File.ReadAllText(NameFileJSON));

                foreach (MyPicture pic in readedPicturs)
                {
                    pictures.Add(pic);

                    listPic.Items.Add(pic.Name);

                    if (!(categ.Items.Contains(pic.Categ)))
                        categ.Items.Add(pic.Categ);

                }
            }
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            string jsonString = JsonSerializer.Serialize(pictures);
            File.WriteAllText(NameFileJSON, jsonString);
        }

        private void dlt_Click(object sender, RoutedEventArgs e)
        {
            if (listPic.SelectedIndex != -1)
            {
                pictures.Remove(pictures[listPic.SelectedIndex]);
                listPic.Items.Clear();

                foreach (MyPicture pic in pictures)
                    listPic.Items.Add(pic.Name);

                categ.Items.Clear();

                foreach (MyPicture pic in pictures)
                {
                    if (!(categ.Items.Contains(pic.Categ)))
                        categ.Items.Add(pic.Categ);
                }

                categ.SelectedIndex = 0;
            }
        }

        private void ld_frm_file_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (!(bool)dlg.ShowDialog())
                return;

            Uri fileUri = new Uri(dlg.FileName);

            FromFileWnd addPic = new FromFileWnd();

            if (addPic.ShowDialog() == true)
            {
                byte[] imageArray = System.IO.File.ReadAllBytes(dlg.FileName);
                string base64ImageRepresentation = Convert.ToBase64String(imageArray);

                MyPicture pic = new MyPicture(addPic.Name.Text, base64ImageRepresentation, addPic.Categr.Text);

                pictures.Add(pic);
                listPic.Items.Add(pic.Name);

                if (!(categ.Items.Contains(pic.Categ)))
                    categ.Items.Add(pic.Categ);
            }
        }

        private void ld_frm_url_Click(object sender, RoutedEventArgs e)
        {
            FromUrlWnd addPic = new FromUrlWnd();

            if (addPic.ShowDialog() == true)
            {
                WebClient client = new WebClient();

                string imageUrl = addPic.url.Text;

                byte[] imageArray = client.DownloadData(imageUrl);

                string base64ImageRepresentation = Convert.ToBase64String(imageArray);

                MyPicture pic = new MyPicture(addPic.Name.Text, base64ImageRepresentation, addPic.Categr.Text);

                pictures.Add(pic);
                listPic.Items.Add(pic.Name);

                if (!(categ.Items.Contains(pic.Categ)))
                    categ.Items.Add(pic.Categ);
            }
        }

        private void srch_btn_Click(object sender, RoutedEventArgs e)
        {
            listPic.Items.Clear();
            TempPictures.Clear();
            foreach (MyPicture pic in pictures)
            {
                if (pic.Name.ToLower().Contains(srch.Text.ToLower()))
                {
                    listPic.Items.Add(pic.Name);
                    TempPictures.Add(pic);
                }
            }
        }

        private void listPic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((categ.SelectedIndex == 0) && (srch.Text.Length == 0 && srch_by_tag_field.Text.Length == 0))
            {
                if (listPic.SelectedIndex != -1)
                    picImg.Source = ByteToImage(Convert.FromBase64String(pictures[listPic.SelectedIndex].Img));
            }
            else
                if (listPic.SelectedIndex != -1)
                    picImg.Source = ByteToImage(Convert.FromBase64String(TempPictures[listPic.SelectedIndex].Img));
        }

        private void categ_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (categ.SelectedIndex != -1)
            {
                listPic.Items.Clear();
                if (categ.SelectedItem.ToString().Equals("all"))
                {
                    foreach (MyPicture pic in pictures)
                        listPic.Items.Add(pic.Name);
                    return;
                }

                TempPictures.Clear();

                foreach (MyPicture pic in pictures)
                {
                    if (pic.Categ == categ.SelectedItem.ToString())
                    {
                        listPic.Items.Add(pic.Name);
                        TempPictures.Add(pic);
                    }
                }
            }
        }

        static ImageSource ByteToImage(byte[] imageData)
        {
            var bitmap = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);
            bitmap.BeginInit();
            bitmap.StreamSource = ms;
            bitmap.EndInit();

            return (ImageSource)bitmap;
        }

        private void add_tag_btn_Click(object sender, RoutedEventArgs e)
        {
            if (listPic.SelectedIndex != -1 && listPic.Items.Count == pictures.Count && add_tag_field.Text.Length > 0)
            {
                pictures[listPic.SelectedIndex].add_tag(add_tag_field.Text);
            }
            else
                MessageBox.Show("Select category all or fill in the tag field");
        }

        private void srch_by_tag_btn_Click(object sender, RoutedEventArgs e)
        {
            listPic.Items.Clear();
            TempPictures.Clear();
            foreach (MyPicture pic in pictures)
            {
                foreach (string tag in pic.Tags)
                {
                    if (tag.ToLower().Equals(srch_by_tag_field.Text.ToLower()))
                    {
                        listPic.Items.Add(pic.Name);
                        TempPictures.Add(pic);
                    }
                }
            }
        }
    }
}
