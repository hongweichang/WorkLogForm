﻿using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.IO;

namespace ChattingCtrl._ChatListBox
{
    //有待解决
    //[TypeConverter(typeof(ExpandableObjectConverter))]
    public class ChatListSubItem : IComparable
    {
        private int id;
        /// <summary>
        /// 获取或者设置用户账号
        /// </summary>
        public int ID {
            get { return id; }
            set { id = value; }
        }

        private string nicName;
        /// <summary>
        /// 获取或者设置用户昵称
        /// </summary>
        public string NicName {
            get { return nicName; }
            set { 
                nicName = value;
                RedrawSubItem(); 
            }
        }

        private string displayName;
        /// <summary>
        /// 获取或者设置用户备注名称
        /// </summary>
        public string DisplayName {
            get { return displayName; }
            set { displayName = value; RedrawSubItem(); }
        }

        private string personalMsg;
        /// <summary>
        /// 获取或者设置用户签名信息
        /// </summary>
        public string PersonalMsg {
            get { return personalMsg; }
            set { personalMsg = value; RedrawSubItem(); }
        }

        private string ipAddress;
        /// <summary>
        /// 获取或者设置用户IP地址
        /// </summary>
        public string IpAddress {
            get { return ipAddress; }
            set {
                if (!CheckIpAddress(value))
                    throw new ArgumentException("Cannot format " + value + " to IPAddress");
                ipAddress = value;
            }
        }

        private int updPort;
        /// <summary>
        /// 获取或者设置用户Upd端口
        /// </summary>
        public int UpdPort {
            get { return updPort; }
            set { updPort = value; }
        }

        private int tcpPort;
        /// <summary>
        /// 获取或者设置用户Tcp端口
        /// </summary>
        public int TcpPort {
            get { return tcpPort; }
            set { tcpPort = value; }
        }

        private Image headImage;
        /// <summary>
        /// 获取或者设置用户头像
        /// </summary>
        public Image HeadImage {
            get 
            {
                return headImage; 
            }
            set
            {
                headImage = value; 
                RedrawSubItem(); 
            }
        }

        private int theuserid;
        public int userid 
        {
            get
            {
                return theuserid;
            }
            set
            {
                theuserid = value;
                HeadImage = LoadPicture.GetHeadIcon(value);
            }
        }
        private UserStatus status;
        /// <summary>
        /// 获取或者设置用户当前状态
        /// </summary>
        public UserStatus Status {
            get { return status; }
            set {
                if (status == value) return;
                status = value;
                if (this.ownerListItem != null)
                    this.ownerListItem.SubItems.Sort();
            }
        }

        private bool isTwinkle;
        /// <summary>
        /// 获取或者设置是否闪动
        /// </summary>
        public bool IsTwinkle {
            get { return isTwinkle; }
            set {
                if (isTwinkle == value) return;
                if (this.ownerListItem == null) return;
                isTwinkle = value;
                if (isTwinkle)
                    this.ownerListItem.TwinkleSubItemNumber++;
                else
                    this.ownerListItem.TwinkleSubItemNumber--;
            }
        }

        private bool isTwinkleHide;
        internal bool IsTwinkleHide {
            get { return isTwinkleHide; }
            set { isTwinkleHide = value; }
        }

        private Rectangle bounds;
        /// <summary>
        /// 获取列表子项显示区域
        /// </summary>
        [Browsable(false)]
        public Rectangle Bounds {
            get { return bounds; }
            internal set { bounds = value; }
        }

        private Rectangle headRect;
        /// <summary>
        /// 获取头像显示区域
        /// </summary>
        [Browsable(false)]
        public Rectangle HeadRect {
            get { return headRect; }
            internal set { headRect = value; }
        }

        private ChatListItem ownerListItem;
        /// <summary>
        /// 获取当前列表子项所在的列表项
        /// </summary>
        [Browsable(false)]
        public ChatListItem OwnerListItem {
            get { return ownerListItem; }
            internal set { ownerListItem = value; }
        }

        private void RedrawSubItem() {
            if (this.ownerListItem != null)
                if (this.ownerListItem.OwnerChatListBox != null)
                    this.ownerListItem.OwnerChatListBox.Invalidate(this.bounds);
        }
        /// <summary>
        /// 获取当前用户的黑白头像
        /// </summary>
        /// <returns>黑白头像</returns>
        public Bitmap GetDarkImage() 
        {
            
            string str = headImage.Tag.ToString() + "gray"+".png";
            str = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"icons" + @"\" + str;
            if (!File.Exists(str))
            {
                Bitmap b = new Bitmap(headImage);
                Bitmap bmp = b.Clone(new Rectangle(0, 0, headImage.Width, headImage.Height), PixelFormat.Format24bppRgb);
                b.Dispose();
                BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
                byte[] byColorInfo = new byte[bmp.Height * bmpData.Stride];
                Marshal.Copy(bmpData.Scan0, byColorInfo, 0, byColorInfo.Length);
                for (int x = 0, xLen = bmp.Width; x < xLen; x++)
                {
                    for (int y = 0, yLen = bmp.Height; y < yLen; y++)
                    {
                        byColorInfo[y * bmpData.Stride + x * 3] =
                            byColorInfo[y * bmpData.Stride + x * 3 + 1] =
                            byColorInfo[y * bmpData.Stride + x * 3 + 2] =
                            GetAvg(
                            byColorInfo[y * bmpData.Stride + x * 3],
                            byColorInfo[y * bmpData.Stride + x * 3 + 1],
                            byColorInfo[y * bmpData.Stride + x * 3 + 2]);
                    }
                }
                Marshal.Copy(byColorInfo, 0, bmpData.Scan0, byColorInfo.Length);
                bmp.UnlockBits(bmpData);
                bmp = ToRoundPic(bmp);
                string add = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"icons";
                bool exi = File.Exists(add);
                if (!Directory.Exists(add))
                    Directory.CreateDirectory(add);
               
                bmp.Save(str);

                return bmp;

            }
            else
            {
                System.Drawing.Bitmap ybitmap = new System.Drawing.Bitmap(str);
                return ybitmap;
            }

           
        }

        private Bitmap ToRoundPic(Bitmap addd)
        {
            //保存图象   
            //string filename = addd;//如果不是png类型，须转换
            System.Drawing.Bitmap ybitmap = addd;// new System.Drawing.Bitmap(filename);
            for (int y = 0; y < ybitmap.Width; y++)
            {
                for (int x = 0; x < ybitmap.Height; x++)
                {
                    Color c = ybitmap.GetPixel(x, y);
                    //int wagrb = Color.White.ToArgb();
                    if (c.R == 255&& c.G==255 && 255 == c.B)
                    {
                        ybitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(0,255,254,254));
                    }
                    if ((x - ybitmap.Width / 2) * (x - ybitmap.Width / 2) + (y - ybitmap.Width / 2) * (y - ybitmap.Width / 2) > ybitmap.Width / 2 * ybitmap.Width / 2)
                    {
                        ybitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(0,Color.White));
                    }
                }
            }

            Graphics g = Graphics.FromImage(ybitmap);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.DrawImage(ybitmap, new Point(0, 0));
            g.DrawEllipse(new Pen(Color.LightBlue), 0, 0, ybitmap.Width, ybitmap.Width);
            g.Dispose();
            ybitmap.MakeTransparent(Color.FromArgb(0,Color.White));
            return ybitmap;
        }

        private byte GetAvg(byte b, byte g, byte r) {
            return (byte)((r + g + b) / 3);
        }

        private bool CheckIpAddress(string str) {
            string[] strIp = str.Split('.');
            if (strIp.Length != 4)
                return false;
            for (int i = 0; i < 4; i++) {
                try {
                    if(Convert.ToInt32(str[i]) > 255)
                        return false;
                } catch (FormatException) {
                    return false;
                }
            }
            return true;
        }
        //实现排序接口
        int IComparable.CompareTo(object obj) {
            if (!(obj is ChatListSubItem))
                throw new NotImplementedException("obj is not ChatListSubItem");
            ChatListSubItem subItem = obj as ChatListSubItem;
            return (this.status).CompareTo(subItem.status);
        }

        public ChatListSubItem() { 
            this.status = UserStatus.Online;
            this.displayName = "displayName";
            this.nicName = "nicName";
            this.personalMsg = "Personal Message ...";
        }
        public ChatListSubItem(string nicname) {
            this.nicName = nicname;
        }
        public ChatListSubItem(string nicname, UserStatus status) {
            this.nicName = nicname;
            this.status = status;
        }
        public ChatListSubItem(string nicname, string displayname, string personalmsg) {
            this.nicName = nicname;
            this.displayName = displayname;
            this.personalMsg = personalmsg;
        }
        public ChatListSubItem(string nicname, string displayname, string personalmsg, UserStatus status) {
            this.nicName = nicname;
            this.displayName = displayname;
            this.personalMsg = personalmsg;
            this.status = status;
        }
        public ChatListSubItem(int id, string nicname, string displayname, string personalmsg, UserStatus status, Bitmap head) {
            this.id = id;
            this.nicName = nicname;
            this.displayName = displayname;
            this.personalMsg = personalmsg;
            this.status = status;
            this.headImage = head;
        }
        //在线状态
        public enum UserStatus
        {
            QMe, Online, Away, Busy, DontDisturb, OffLine   //貌似对于列表而言 没有隐身状态
        }
    }
}
