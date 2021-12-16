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
        List<string> subList1 = new List<string>();
        Stack<string> log = new Stack<string>();
        Stack<string> beforeLog = new Stack<string>();
        TreeViewItem driveitem = new TreeViewItem();


        string c;

        public MainWindow()
        {
            InitializeComponent();
            GetDrives();

            ////Icon icon = System.Drawing.Icon.ExtractAssociatedIcon("4일차 스터디.pdf");
            //ImageSource imgSource = null;

            //using (System.Drawing.Icon sysicon = System.Drawing.Icon.ExtractAssociatedIcon("4일차 스터디.pdf"))
            //{
            //    imgSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(sysicon.Handle,
            //    System.Windows.Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            //}

            //image.Source = imgSource;
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    System.Diagnostics.Process run = new System.Diagnostics.Process();
        //    run.StartInfo.FileName = "4일차 스터디.pdf";
        //    run.Start();
        //}

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
                                //subList1.Add(subItem.Tag.ToString());
                                //lbx.Items.Add(subItem.Tag);
                            }
                        }


                        tvlist.Items.Add(driveitem);//전체를 추가
                        count.Text = $"{tvlist.Items.Count} 개의 항목";
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("에러 발생 : " + ex.Message);
            }

        }



        //private void SubList(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        subList1.Clear();
        //        int count1 = 0;
        //        lbx.Items.Clear();
        //        TreeViewItem subList = sender as TreeViewItem;
        //        Console.WriteLine(subList);
        //        DirectoryInfo Di;
        //        Di = new DirectoryInfo(subList.Tag.ToString());
        //        DirectoryInfo baseDi = new DirectoryInfo(Di.FullName); //경로 받아오기
        //        DirectoryInfo[] childrenDI = baseDi.GetDirectories(); //경로안에 디렉토리 모두 알려주기

        //        currentLocation.Text = Di.FullName;

        //        if (childrenDI.Length != 0)
        //        {
        //            subList.Items.Clear();
        //            for (int i = 0; i < childrenDI.Length; i++)
        //            {  //폴더
        //                if ((childrenDI[i].Attributes & FileAttributes.Hidden) != FileAttributes.Hidden) //숨겨진 파일 아닌것만
        //                {
        //                    TreeViewItem subItem = new TreeViewItem(); //자식들 추가
        //                    subItem.Header = childrenDI[i].Name;
        //                    subItem.Tag = childrenDI[i].FullName;
        //                    subItem.ToolTip = childrenDI[i].Name;
        //                    subList.Items.Add(UpperList(subItem));
        //                    subList1.Add(subItem.Tag.ToString());
        //                    lbx.Items.Add(subItem.Tag);
        //                    count1++;
        //                }
        //            }
        //            count.Text = $"{count1} 항목입니다.";
        //        }

        //        else
        //        {
        //            lbx.Items.Add("빈 파일 입니다.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("에러 발생 : " + ex.Message);
        //    }
        //}


        private void lbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //try
            //{
            //    if (lbx.Items.Count != 0)
            //    {
            //        int a = lbx.SelectedIndex;
            //        ClickSubList(a);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("에러 발생 : " + ex.Message);
            //}

        }


        private void ClickSubList(int a)
        {
            try
            {
                int count1 = 0;
                //lbx.Items.Clear();
                TreeViewItem subList = new TreeViewItem();
                DirectoryInfo Di;
                Di = new DirectoryInfo(subList1[a]);
                subList1.Clear();
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
                        subList.Items.Add(subItem);
                        //lbx.Items.Add(subItem.Tag);
                        subList1.Add(childrenDI[i].FullName);
                        count1++;
                    }
                }

                count.Text = $"{count1} 항목입니다.";

            }
            catch (Exception ex)
            {
                MessageBox.Show("에러 발생 : " + ex.Message);
            }
        }

        private void SubList1(TreeViewItem subList)
        {
            if (subList == null || subList.Items.Count != 0)
            {
                return;
            }

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
                        subItem.Expanded += new RoutedEventHandler(List_Expanded);
                        subList.Items.Add(subItem);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("에러 발생 : " + ex.Message);
            }
        }

        private void List_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem subList = e.Source as TreeViewItem;
            //lbx.Items.Clear();
            createLocation(subList.Tag.ToString());
            subList1.Clear();
            if (subList == null || subList.Items.Count == 0)
            {
                return;
            }

            foreach (TreeViewItem item in subList.Items)
            {
                SubList1(item);
                subList1.Add(item.Tag.ToString());
            }
        }

        private void tvlist_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem subList = e.Source as TreeViewItem;

            lbx.Children.Clear();
            subList1.Clear();

            if (subList == null || subList.Items.Count == 0)
            {
                lbx.Children.Clear();
                return;
            }

            log.Push(subList.Tag.ToString()); // stack으로 log 기록!
            createLocation(subList.Tag.ToString());

            foreach (TreeViewItem item in subList.Items)
            {
                SubList1(item);
                //lbx.Items.Add(item.Tag);
                //lbx.Items.Add(makeFolder(item.Header));
                subList1.Add(item.Tag.ToString());
                WrapPanel wrapPanel = new WrapPanel();
                wrapPanel.Orientation = Orientation.Vertical;
                wrapPanel.Width = 100;
                wrapPanel.Height = 100;
                Button button = new Button();
                TextBlock textBlock = new TextBlock();
                textBlock.Text = item.Header.ToString();
                textBlock.Width = 90;
                textBlock.TextWrapping = TextWrapping.Wrap;
                button.Width = 100;
                button.Tag = item.Tag.ToString();
                button.ToolTip = item.ToolTip.ToString();
                button.Height = 60;
                button.PreviewMouseDoubleClick += Button_PreviewMouseDoubleClick;
                button.Background = new ImageBrush(new BitmapImage(new Uri("C:\\Users\\kty30\\source\\repos\\FileExplorer\\FileExplorer\\bin\\Debug\\folderImage.png")));
                //wrapPanel.Children.Add(button);
                wrapPanel.Children.Add(button);
                wrapPanel.Children.Add(textBlock);
                lbx.Children.Add(wrapPanel);
            }
        }

        private void Button_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Button button = sender as Button;
            lbx.Children.Clear();
            subList1.Clear();

            DirectoryInfo baseDi = new DirectoryInfo(button.Tag.ToString());
            DirectoryInfo[] childrenDI = baseDi.GetDirectories(); //경로안에 디렉토리 모두 알려주기

            createLocation(button.Tag.ToString());
            for (int i = 0; i < childrenDI.Length; i++)
            {  //폴더
                if ((childrenDI[i].Attributes & FileAttributes.Hidden) != FileAttributes.Hidden) //숨겨진 파일 아닌것만
                {
                    WrapPanel wrapPanel = new WrapPanel();
                    wrapPanel.Orientation = Orientation.Vertical;
                    wrapPanel.Width = 100;
                    wrapPanel.Height = 100;
                    button = new Button();
                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = childrenDI[i].Name.ToString();
                    textBlock.Width = 90;
                    textBlock.TextWrapping = TextWrapping.Wrap;
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
        }

        //private TreeViewItem UpperList(TreeViewItem subList)
        //{
        //    try
        //    {
        //        subList1.Clear();

        //        DirectoryInfo Di;
        //        Di = new DirectoryInfo(subList.Tag.ToString());
        //        DirectoryInfo baseDi = new DirectoryInfo(Di.FullName); //경로 받아오기
        //        DirectoryInfo[] childrenDI = baseDi.GetDirectories(); //경로안에 디렉토리 모두 알려주기

        //        currentLocation.Text = Di.FullName;

        //        for (int i = 0; i < childrenDI.Length; i++)
        //        {  //폴더
        //            if ((childrenDI[i].Attributes & FileAttributes.Hidden) != FileAttributes.Hidden) //숨겨진 파일 아닌것만
        //            {
        //                TreeViewItem subItem = new TreeViewItem(); //자식들 추가
        //                subItem.Header = childrenDI[i].Name;
        //                subItem.Tag = childrenDI[i].FullName;
        //                subItem.ToolTip = childrenDI[i].Name;
        //                subList.Items.Add(subItem);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("에러 발생 : " + ex.Message);
        //    }
        //    return subList;
        //}

        //private WrapPanel makeFolder(object item)
        //{
        //    Button button = new Button();
        //    TextBlock textBlock = new TextBlock();
        //    textBlock.Text = item.ToString() ;
        //    button.Width = 40;
        //    button.Height = 40;
        //    button.Background = new ImageBrush(new BitmapImage(new Uri("C:\\Users\\kty30\\source\\repos\\FileExplorer\\FileExplorer\\bin\\Debug\\folderImage.png")));

        //    subFolder.Children.Add(button);
        //    subFolder.Children.Add(textBlock);

        //    return subFolder;
        //}

        private void Click(object sender, MouseButtonEventArgs e)
        {
            Button button = sender as Button;
        }

        private void createLocation(string a)
        {
            currentLocation.Children.Clear();

            string[] split = a.Split('\\');
            string b;
            c = null;

            for (int i = 0; i < split.Length; i++)
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

        private void currentLaction_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Button button = sender as Button;
            lbx.Children.Clear();
            subList1.Clear();

            DirectoryInfo baseDi = new DirectoryInfo(button.Content.ToString());
            DirectoryInfo[] childrenDI = baseDi.GetDirectories(); //경로안에 디렉토리 모두 알려주기

            createLocation(button.Content.ToString());
            for (int i = 0; i < childrenDI.Length; i++)
            {  //폴더
                if ((childrenDI[i].Attributes & FileAttributes.Hidden) != FileAttributes.Hidden) //숨겨진 파일 아닌것만
                {
                    WrapPanel wrapPanel = new WrapPanel();
                    wrapPanel.Orientation = Orientation.Vertical;
                    wrapPanel.Width = 100;
                    wrapPanel.Height = 100;
                    button = new Button();
                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = childrenDI[i].Name.ToString();
                    textBlock.Width = 90;
                    textBlock.TextWrapping = TextWrapping.Wrap;
                    button.Width = 100;
                    button.Height = 60;
                    button.Tag = childrenDI[i].FullName.ToString();
                    button.PreviewMouseLeftButtonDown += Button_PreviewMouseDoubleClick;
                    button.Background = new ImageBrush(new BitmapImage(new Uri("C:\\Users\\kty30\\source\\repos\\FileExplorer\\FileExplorer\\bin\\Debug\\folderImage.png")));
                    wrapPanel.Children.Add(button);
                    wrapPanel.Children.Add(textBlock);
                    lbx.Children.Add(wrapPanel);
                }
            }
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
                DirectoryInfo[] childrenDI = baseDi.GetDirectories(); //경로안에 디렉토리 모두 알려주기

                createLocation(subList);
                for (int i = 0; i < childrenDI.Length; i++)
                {  //폴더
                    if ((childrenDI[i].Attributes & FileAttributes.Hidden) != FileAttributes.Hidden) //숨겨진 파일 아닌것만
                    {
                        WrapPanel wrapPanel = new WrapPanel();
                        wrapPanel.Orientation = Orientation.Vertical;
                        wrapPanel.Width = 100;
                        wrapPanel.Height = 100;
                        Button button = new Button();
                        TextBlock textBlock = new TextBlock();
                        textBlock.Text = childrenDI[i].Name.ToString();
                        textBlock.Width = 90;
                        textBlock.TextWrapping = TextWrapping.Wrap;
                        button.Width = 100;
                        button.Height = 60;
                        button.Tag = childrenDI[i].FullName.ToString();
                        button.PreviewMouseLeftButtonDown += Button_PreviewMouseDoubleClick;
                        button.Background = new ImageBrush(new BitmapImage(new Uri("C:\\Users\\kty30\\source\\repos\\FileExplorer\\FileExplorer\\bin\\Debug\\folderImage.png")));
                        wrapPanel.Children.Add(button);
                        wrapPanel.Children.Add(textBlock);
                        lbx.Children.Add(wrapPanel);
                    }
                }
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
                DirectoryInfo[] childrenDI = baseDi.GetDirectories(); //경로안에 디렉토리 모두 알려주기

                createLocation(subList);
                for (int i = 0; i < childrenDI.Length; i++)
                {  //폴더
                    if ((childrenDI[i].Attributes & FileAttributes.Hidden) != FileAttributes.Hidden) //숨겨진 파일 아닌것만
                    {
                        WrapPanel wrapPanel = new WrapPanel();
                        wrapPanel.Orientation = Orientation.Vertical;
                        wrapPanel.Width = 100;
                        wrapPanel.Height = 100;
                        Button button = new Button();
                        TextBlock textBlock = new TextBlock();
                        textBlock.Text = childrenDI[i].Name.ToString();
                        textBlock.Width = 90;
                        textBlock.TextWrapping = TextWrapping.Wrap;
                        button.Width = 100;
                        button.Height = 60;
                        button.Tag = childrenDI[i].FullName.ToString();
                        button.PreviewMouseLeftButtonDown += Button_PreviewMouseDoubleClick;
                        button.Background = new ImageBrush(new BitmapImage(new Uri("C:\\Users\\kty30\\source\\repos\\FileExplorer\\FileExplorer\\bin\\Debug\\folderImage.png")));
                        wrapPanel.Children.Add(button);
                        wrapPanel.Children.Add(textBlock);
                        lbx.Children.Add(wrapPanel);
                    }
                }
            }
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            string home = "C:";
            DirectoryInfo baseDi = new DirectoryInfo(home);
            DirectoryInfo[] childrenDI = baseDi.GetDirectories(); //경로안에 디렉토리 모두 알려주기
            lbx.Children.Clear();
            createLocation(home);
            for (int i = 0; i < childrenDI.Length; i++)
            {  //폴더
                if ((childrenDI[i].Attributes & FileAttributes.Hidden) != FileAttributes.Hidden) //숨겨진 파일 아닌것만
                {
                    WrapPanel wrapPanel = new WrapPanel();
                    wrapPanel.Orientation = Orientation.Vertical;
                    wrapPanel.Width = 100;
                    wrapPanel.Height = 100;
                    Button button = new Button();
                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = childrenDI[i].Name.ToString();
                    textBlock.Width = 90;
                    textBlock.TextWrapping = TextWrapping.Wrap;
                    button.Width = 100;
                    button.Height = 60;
                    button.Tag = childrenDI[i].FullName.ToString();
                    button.PreviewMouseLeftButtonDown += Button_PreviewMouseDoubleClick;
                    button.Background = new ImageBrush(new BitmapImage(new Uri("C:\\Users\\kty30\\source\\repos\\FileExplorer\\FileExplorer\\bin\\Debug\\folderImage.png")));
                    wrapPanel.Children.Add(button);
                    wrapPanel.Children.Add(textBlock);
                    lbx.Children.Add(wrapPanel);
                }
            }
        }
    }
}
