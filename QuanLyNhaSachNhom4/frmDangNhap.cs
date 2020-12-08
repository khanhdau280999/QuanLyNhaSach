using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNhaSachNhom4
{
    public partial class frmDangNhap : Form
    {
        public frmDangNhap()
        {
            InitializeComponent();
        }

        private void frmDangNhap_Load(object sender, EventArgs e)
        {
            txtUsername.Focus();
            txtPassword.Focus();
        }

        private void btn_thoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void dangnhap()
        {
            if (txtUsername.Text.Length == 0 && txtPassword.Text.Length == 0)
                MessageBox.Show("Bạn chưa đăng nhập username và password ! vui lòng nhập tên đăng nhập và mật khẩu. ");
            else
                if (this.txtUsername.Text.Length == 0)
                MessageBox.Show("Bạn chưa đăng nhập Username");
            else
                if (this.txtPassword.Text.Length == 0)
                MessageBox.Show("Bạn chưa đăng nhập mật khẩu");
            else
                if (this.txtUsername.Text == "admin" && this.txtPassword.Text == "admin1999")
                MessageBox.Show("Đăng nhập thành công !");
            else
                MessageBox.Show("tài khoản hoặc mật khẩu của bạn không đúng ! vui lòng nhập lại tên đăng nhập và mật khẩu. ");
        }

        private void btn_dangnhap_Click(object sender, EventArgs e)
        {
            FormMain fm = new FormMain();
            if (this.txtUsername.Text == "admin" && this.txtPassword.Text == "admin1999")
            {
                fm.Show();
            }
            dangnhap();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
