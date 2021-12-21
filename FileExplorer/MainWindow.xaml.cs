using System;
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
using System.IO;
using System.Drawing;

namespace FileExplorer
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {/// <summary>
     /// MainWindow.xaml에 대한 상호 작용 논리
     /// </summary>
        public List<string> subList1 = new List<string>();
        public Stack<string> log = new Stack<string>();
        public Stack<string> beforeLog = new Stack<string>();
        public TreeViewItem driveitem = new TreeViewItem();


        string c;

        public MainWindow()
        {
            InitializeComponent();
            GetDrives();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button viewImage = (Button)e.Source;
                System.Diagnostics.Process run = new System.Diagnostics.Process();
                run.StartInfo.FileName = viewImage.Tag.ToString();
                run.Start();
            }

            catch (Exception ex)
            {
                MessageBox.Show("에러 발생 : " + ex.Message);
            }
        }

        private void GetDrives() //드라이브 검색
        {
            try
            {
                DriveInfo[] drives = DriveInfo.GetDrives(); // 내컴퓨터 모든 드라이브 정보 얻기
                DirectoryInfo Di;
                Expander expander = new Expander();

                foreach (DriveInfo drive in drives)
                {
                    //자식들을 추가해주려고
                    if (drive.IsReady == true) // 드라이브가 준비되었으면
                    {
                        Di = new DirectoryInfo(drive.Name); //경로정보 갖고오기
                        driveitem.Header = drive.Name;//제일처음에 보이는거
                        driveitem.Tag = Di.FullName;//전체경로를 테그에 넣고 
                        driveitem.ToolTip = drive.Name;//마우스 위에 올려놓았을때 보이는거

                        DirectoryInfo baseDi = new DirectoryInfo(Di.FullName); //경로 받아오기
                        DirectoryInfo[] childrenDI = baseDi.GetDirectories(); //경로안에 디렉토리 모두 알려주기

                        for (int i = 0; i < childrenDI.Length; i++)
                        {  //폴더
                            if ((childrenDI[i].Attributes & FileAttributes.Hidden) != FileAttributes.Hidden) //숨겨진 파일 아닌것만
                            {
                                TreeViewItem subItem = new TreeViewItem(); //자식들 추가
                                subItem.Header = childrenDI[i].Name;
                                subItem.Tag = childrenDI[i].FullName;
                                subItem.ToolTip = childrenDI[i].Name;
                                subItem.Expanded += new RoutedEventHandler(List_Expanded); // 하위 폴더 

                                driveitem.Items.Add(subItem);
                                SubList1(subItem);

                            }
                        }


                        tvlist.Items.Add(driveitem);//전체를 추가
                        count.Text = $"{tvlist.Items.Count} 개의 항목";
                        lbx.Children.Clear();
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("에러 발생 : " + ex.Message);
            }

        }

        private void SubList1(TreeViewItem subList)
        {
            lbx.Children.Clear();

            try
            {
                DirectoryInfo baseDi = new DirectoryInfo(subList.Tag.ToString());
                DirectoryInfo[] childrenDI = baseDi.GetDirectories(); //경로안에 디렉토리 모두 알려주기

                for (int i = 0; i < childrenDI.Length; i++)
                {  //폴더
                    if ((childrenDI[i].Attributes & FileAttributes.Hidden) != FileAttributes.Hidden) //숨겨진 파일 아닌것만
                    {
                        TreeViewItem subItem = new TreeViewItem();
                        subItem.Header = childrenDI[i].Name;
                        subItem.Tag = childrenDI[i].FullName;
                        subItem.ToolTip = childrenDI[i].Name;
                        //subItem.Expanded += new RoutedEventHandler(List_Expanded);
                        subList.Items.Add(subItem);
                    }
                }

                //    // 파일
                //    FileInfo[] files = baseDi.GetFiles();

                //    var filtered = files.Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden));

                //    foreach (var file in filtered)
                //    {
                //        WrapPanel wrapPanel = new WrapPanel();
                //        wrapPanel.Width = 100;
                //        wrapPanel.Height = 100;

                //        Button button = new Button();
                //        button.Background = new ImageBrush(GetImage(file.FullName));
                //        button.BorderThickness = new Thickness(0);
                //        button.Height = 80;
                //        button.Width = 60;
                //        button.Tag = file.ToString();
                //        button.MouseDoubleClick += Button_Click;
                //        wrapPanel.Children.Add(button);

                //        TextBlock textBlock = new TextBlock();
                //        textBlock.Text = file.Name;
                //        textBlock.TextWrapping = TextWrapping.Wrap;
                //        textBlock.Width = 90;
                //        wrapPanel.Children.Add(textBlock);
                //        lbx.Children.Add(wrapPanel);
                //    }

                //항목 개수 표시
                count.Text = $"{lbx.Children.Count}개 항목";
            }
            catch (Exception ex)
            {
                MessageBox.Show("에러 발생 : " + ex.Message);
            }
        }

        private void List_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem subList = e.Source as TreeViewItem;


            //if (subList == null || subList.Items.Count == 0)
            //{
            //    return;
            //}

            DirectoryInfo directoryInfo = new DirectoryInfo(subList.Tag.ToString());
            DirectoryInfo[] childrenDI = directoryInfo.GetDirectories(); // 하위 디렉토리 불러오기

            subList.Items.Clear();

            lbx.Children.Clear();
            createLocation(subList.Tag.ToString());
            subList1.Clear();

            foreach (DirectoryInfo childDI in childrenDI)
            {
                if ((childDI.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)  // 숨겨진 파일 아닌 것만 
                {
                    TreeViewItem subItem = new TreeViewItem(); //자식들 추가
                    subItem.Header = childDI.Name;
                    subItem.Tag = childDI.FullName;
                    subItem.ToolTip = childDI.Name;
                    subItem.Expanded += List_Expanded;
                    subItem.PreviewMouseLeftButtonUp += tvlist_MouseLeftButtonDown;

                    SubList1(subItem);
                    subList.Items.Add(subItem);

                }
            }

            DirectoryInfo baseDi = new DirectoryInfo(subList.Tag.ToString());
            viewList(baseDi);
        }


        private void tvlist_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem subList = e.Source as TreeViewItem;
            lbx.Children.Clear();
            subList1.Clear();


            if (subList == null)
            {
                lbx.Children.Clear();
                return;
            }

            DirectoryInfo baseDi = new DirectoryInfo(subList.Tag.ToString());
            log.Push(subList.Tag.ToString()); // stack으로 log 기록!
            createLocation(subList.Tag.ToString());

            viewList(baseDi);
        }

        //View에서 클릭시 List표시
        private void Button_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Button button = sender as Button;
            lbx.Children.Clear();
            subList1.Clear();
            TreeViewItem temp  = new TreeViewItem();
            temp.ToolTip = button.Name;
            temp.Tag = button.Tag;
            temp.Name = button.Name;

            DirectoryInfo baseDi = new DirectoryInfo(button.Tag.ToString());

            createLocation(button.Tag.ToString());

            viewList(baseDi);

        }

        private void createLocation(string a)
        {
            currentLocation.Children.Clear();

            string[] split = a.Split('\\');
            string b;
            c = null;

            for (int i = 0; i < split.Length; i++)
            {
                if (split[i] != "")
                {
                    b = split[i] + '\\';
                    c += b;
                    WrapPanel wrapPanel = new WrapPanel();
                    wrapPanel.Orientation = Orientation.Vertical;
                    wrapPanel.Height = 20;
                    Button button = new Button();
                    button.Height = 20;
                    button.Tag = c;
                    button.Content = split[i];
                    button.PreviewMouseLeftButtonDown += Button_PreviewMouseDoubleClick;
                    wrapPanel.Children.Add(button);
                    currentLocation.Children.Add(wrapPanel);
                }

            }
        }

        private void currentLaction_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Button button = sender as Button;
            lbx.Children.Clear();
            subList1.Clear();

            DirectoryInfo baseDi = new DirectoryInfo(button.Content.ToString());

            createLocation(button.Content.ToString());
            viewList(baseDi);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (log.Count == 0)
                return;
            else
            {
                lbx.Children.Clear();
                subList1.Clear();
                string subList = log.Pop();
                beforeLog.Push(subList);
                DirectoryInfo baseDi = new DirectoryInfo(subList);

                createLocation(subList);
                viewList(baseDi);
            }
        }

        private void FrontButton_Click(object sender, RoutedEventArgs e)
        {
            if (beforeLog.Count == 0)
                return;
            else
            {
                lbx.Children.Clear();
                subList1.Clear();
                string subList = beforeLog.Pop();
                log.Push(subList);
                DirectoryInfo baseDi = new DirectoryInfo(subList);

                createLocation(subList);
                viewList(baseDi);
            }
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            string home = "C:";
            DirectoryInfo baseDi = new DirectoryInfo(home);
            DirectoryInfo[] childrenDI = baseDi.GetDirectories(); //경로안에 디렉토리 모두 알려주기
            lbx.Children.Clear();
            createLocation(home);
            viewList(baseDi);
        }

        private ImageSource GetImage(string imageTag)
        {
            System.Drawing.Icon sysicon = System.Drawing.Icon.ExtractAssociatedIcon(imageTag);
            ImageSource imageSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(sysicon.Handle,
            System.Windows.Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

            return imageSource;
        }

        private void viewList(DirectoryInfo directoryInfo)
        {
            DirectoryInfo[] childrenDI = directoryInfo.GetDirectories();
            lbx.Children.Clear();

            try
            {
                for (int i = 0; i < childrenDI.Length; i++)
                {  //폴더
                    if ((childrenDI[i].Attributes & FileAttributes.Hidden) != FileAttributes.Hidden) //숨겨진 파일 아닌것만
                    {
                        TreeViewItem subItem = new TreeViewItem();
                        subItem.Header = childrenDI[i].Name;
                        subItem.Tag = childrenDI[i].FullName;
                        subItem.ToolTip = childrenDI[i].Name;
                        subItem.Expanded += new RoutedEventHandler(List_Expanded);

                        WrapPanel wrapPanel = new WrapPanel();
                        wrapPanel.Orientation = Orientation.Vertical;
                        wrapPanel.Width = 100;
                        wrapPanel.Height = 100;

                        TextBlock textBlock = new TextBlock();
                        textBlock.Text = childrenDI[i].Name.ToString();
                        textBlock.Width = 90;
                        textBlock.TextWrapping = TextWrapping.Wrap;

                        Button button = new Button();
                        button.Width = 100;
                        button.Height = 60;
                        button.Tag = childrenDI[i].FullName.ToString();
                        button.ToolTip = childrenDI[i].Name.ToString();
                        button.PreviewMouseDoubleClick += Button_PreviewMouseDoubleClick;
                        button.Background = new ImageBrush(new BitmapImage(new Uri("C:\\Users\\kty30\\source\\repos\\FileExplorer\\FileExplorer\\bin\\Debug\\folderImage.png")));
                        
                        wrapPanel.Children.Add(button);
                        wrapPanel.Children.Add(textBlock);
                        lbx.Children.Add(wrapPanel);
                    }
                }

                // 파일 불러오기
                FileInfo[] files = directoryInfo.GetFiles();

                var filtered = files.Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden));

                foreach (var file in filtered)
                {
                    WrapPanel wrapPanel = new WrapPanel();
                    wrapPanel.Width = 100;
                    wrapPanel.Height = 100;

                    Button button = new Button();
                    button.Background = new ImageBrush(GetImage(file.FullName));
                    button.Margin = new Thickness(5);
                    button.Height = 60;
                    button.Width = 60;
                    button.Tag = file.FullName.ToString();
                    button.MouseDoubleClick += Button_Click;
                    wrapPanel.Children.Add(button);

                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = file.Name;
                    textBlock.TextWrapping = TextWrapping.Wrap;
                    textBlock.Width = 90;

                    //Button button = new Button();
                    //button.Width = 100;
                    //button.Height = 60;
                    //button.Tag = file.FullName.ToString();
                    //button.ToolTip = file.Name.ToString();
                    //button.PreviewMouseDoubleClick += Button_PreviewMouseDoubleClick;
                    //button.Background = new ImageBrush(GetImage(file.FullName));
                    
                    //wrapPanel.Children.Add(button);
                    wrapPanel.Children.Add(textBlock);
                    lbx.Children.Add(wrapPanel);
                }

                // 항목 개수 표시
                count.Text = $"{lbx.Children.Count}개 항목";
            }
            catch (Exception ex)
            {
                MessageBox.Show("에러 발생 : " + ex.Message);
            }
        }
    }
}
