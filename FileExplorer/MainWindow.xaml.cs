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
        List<string> subList2 = new List<string>();
        TreeViewItem driveitem = new TreeViewItem();

        public MainWindow()
        {
            InitializeComponent();
            GetDrives();

            //Icon icon = System.Drawing.Icon.ExtractAssociatedIcon("4일차 스터디.pdf");
            ImageSource imgSource = null;

            using (System.Drawing.Icon sysicon = System.Drawing.Icon.ExtractAssociatedIcon("4일차 스터디.pdf"))
            {
                imgSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(sysicon.Handle,
                System.Windows.Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            }

            image.Source = imgSource;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process run = new System.Diagnostics.Process();
            run.StartInfo.FileName = "4일차 스터디.pdf";
            run.Start();
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
                                subItem.Expanded += SubList;
                                driveitem.Items.Add(UpperList(subItem));
                                subList1.Add(subItem.Tag.ToString());
                                lbx.Items.Add(subItem.Tag);
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



        private void SubList(object sender, RoutedEventArgs e)
        {
            try
            {
                subList1.Clear();
                int count1 = 0;
                lbx.Items.Clear();
                TreeViewItem subList = (TreeViewItem)sender;

                DirectoryInfo Di;
                Di = new DirectoryInfo(subList.Tag.ToString());
                DirectoryInfo baseDi = new DirectoryInfo(Di.FullName); //경로 받아오기
                DirectoryInfo[] childrenDI = baseDi.GetDirectories(); //경로안에 디렉토리 모두 알려주기

                currentLocation.Text = Di.FullName;

                if (childrenDI.Length != 0)
                {
                    subList.Items.Clear();
                    for (int i = 0; i < childrenDI.Length; i++)
                    {  //폴더
                        if ((childrenDI[i].Attributes & FileAttributes.Hidden) != FileAttributes.Hidden) //숨겨진 파일 아닌것만
                        {
                            TreeViewItem subItem = new TreeViewItem(); //자식들 추가
                            subItem.Header = childrenDI[i].Name;
                            subItem.Tag = childrenDI[i].FullName;
                            subItem.ToolTip = childrenDI[i].Name;
                            subList.Items.Add(UpperList(subItem));
                            subList1.Add(subItem.Tag.ToString());
                            lbx.Items.Add(subItem.Tag);
                            count1++;
                        }
                    }
                    count.Text = $"{count1} 항목입니다.";
                }

                else
                {
                    lbx.Items.Add("빈 파일 입니다.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("에러 발생 : " + ex.Message);
            }
        }


        private void lbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lbx.Items.Count != 0)
                {
                    int a = lbx.SelectedIndex;
                    ClickSubList(a);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("에러 발생 : " + ex.Message);
            }

        }

        private void ClickSubList(int a)
        {
            try
            {
                int count1 = 0;
                lbx.Items.Clear();
                TreeViewItem subList = new TreeViewItem();
                DirectoryInfo Di;
                Di = new DirectoryInfo(subList1[a]);
                subList1.Clear();
                DirectoryInfo baseDi = new DirectoryInfo(Di.FullName); //경로 받아오기                
                DirectoryInfo[] childrenDI = baseDi.GetDirectories(); //경로안에 디렉토리 모두 알려주기

                currentLocation.Text = Di.FullName;

                for (int i = 0; i < childrenDI.Length; i++)
                {  //폴더
                    if ((childrenDI[i].Attributes & FileAttributes.Hidden) != FileAttributes.Hidden) //숨겨진 파일 아닌것만
                    {
                        TreeViewItem subItem = new TreeViewItem(); //자식들 추가
                        subItem.Header = childrenDI[i].Name;
                        subItem.Tag = childrenDI[i].FullName;
                        subItem.ToolTip = childrenDI[i].Name;
                        subList.Items.Add(subItem);                        
                        lbx.Items.Add(subItem.Tag);
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

        private TreeViewItem UpperList(TreeViewItem subList)
        {
            try
            {
                subList1.Clear();

                DirectoryInfo Di;
                Di = new DirectoryInfo(subList.Tag.ToString());
                DirectoryInfo baseDi = new DirectoryInfo(Di.FullName); //경로 받아오기
                DirectoryInfo[] childrenDI = baseDi.GetDirectories(); //경로안에 디렉토리 모두 알려주기

                currentLocation.Text = Di.FullName;

                for (int i = 0; i < childrenDI.Length; i++)
                {  //폴더
                    if ((childrenDI[i].Attributes & FileAttributes.Hidden) != FileAttributes.Hidden) //숨겨진 파일 아닌것만
                    {
                        TreeViewItem subItem = new TreeViewItem(); //자식들 추가
                        subItem.Header = childrenDI[i].Name;
                        subItem.Tag = childrenDI[i].FullName;
                        subItem.ToolTip = childrenDI[i].Name;
                        subList.Items.Add(subItem);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("에러 발생 : " + ex.Message);
            }
            return subList;
        }
    }
}
