using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using QuanLyCuaHangBanSach;

namespace QuanLyNhaSachNhom4
{
    public partial class FormMain : Form
    {
        private DataProvider dataProvider = new DataProvider();
        private int maSachLoaiSach;
        private int maSachSach;
        private int maLoaiSachLoaiSach;
        private int maHoaDonHoaDon;
        private int maPhieuNhapPhieuNhap;
        public FormMain()
        {
            InitializeComponent();
            init();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void init()
        {
            initSach();
            initLoaiSach();
            initHoaDon();
            initPhieuNhap();
        }
        private void initSach()
        {
            loadDgSach();
            loadcbSachLoaiSach();
        }
        private void loadDgSach()
        {
            DataTable dt = new DataTable();
            //StringBuilder query = new StringBuilder("SELECT * FROM tbl_sach");
            StringBuilder query = new StringBuilder("SELECT ma_sach as [Mã Sách]");
            query.Append(",ten_sach as [Tên Sách]");
            query.Append(",ten_loai_sach as [Loại Sách]");
            query.Append(",tac_gia as [Tác Giả]");
            query.Append(",so_luong as [Số Lượng]");
            query.Append(",gia_ban as [Giá Bán]");
            query.Append(" FROM tbl_sach, tbl_loai_sach");
            query.Append(" WHERE tbl_sach.ma_loai_sach = tbl_loai_sach.ma_loai_sach;");
            dt = dataProvider.execQuery(query.ToString());
            dgSach.DataSource = dt;
        }
        private void loadcbSachLoaiSach()
        {
            DataTable dt = new DataTable();
            dt = dataProvider.execQuery("SELECT * FROM tbl_loai_sach");
            cbSachLoaiSach.DisplayMember = "ten_loai_sach";
            cbSachLoaiSach.ValueMember = "ma_loai_sach";
            cbSachLoaiSach.DataSource = dt;
        }
        private void initLoaiSach()
        {
            loadDgLoaiSach();
        }
        private void loadDgLoaiSach()
        {
            DataTable dt = new DataTable();
            StringBuilder query = new StringBuilder("SELECT ma_loai_sach as [Mã Loại Sách]");
            query.Append(",ten_loai_sach as [Tên Loại Sách]");
            query.Append("FROM tbl_loai_sach");
            dt = dataProvider.execQuery(query.ToString());
            dgLoaiSach.DataSource = dt;
        }
        private void initHoaDon()
        {
            loadDgHoaDon();
        }
        private void loadDgHoaDon()
        {
            DataTable dt = new DataTable();
            StringBuilder query = new StringBuilder("SELECT ma_hoa_don as [Mã Hóa Đơn]");
            query.Append(",ngay_lap_hoa_don as [Ngày Lập Hóa Đơn]");
            query.Append(",ten_khach_hang as [Tên Khách Hàng]");
            query.Append(",sdt_khach_hang as [SĐT Khách Hàng]");
            query.Append("FROM tbl_hoa_don");
            dt = dataProvider.execQuery(query.ToString());
            dgHoaDon.DataSource = dt;
        }
        private void initPhieuNhap()
        {
            loadDgPhieuNhap();
        }
        private void loadDgPhieuNhap()
        {
            DataTable dt = new DataTable();
            StringBuilder query = new StringBuilder("SELECT ma_phieu_nhap as [Mã Phiếu Nhập]");
            query.Append(",ngay_lap_phieu_nhap as [Ngày Lập Phiếu Nhập]");
            query.Append(",ten_nha_cung_cap as [Tên Nhà Cung Cấp]");
            query.Append(" FROM tbl_phieu_nhap");
            dt = dataProvider.execQuery(query.ToString());
            dgPhieuNhap.DataSource = dt;
            //maPhieuNhapPhieuNhap = (int)dt.Rows[0][0];
        }
        private void dgSach_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            int rowId = e.RowIndex;
            if (rowId < dgSach.RowCount - 1 && rowId > 0)
            {
                DataGridViewRow row = dgSach.Rows[rowId];
                maSachSach = (int)row.Cells[0].Value;
                txtSachTenSach.Text = row.Cells[1].Value.ToString();
                cbSachLoaiSach.Text = row.Cells[2].Value.ToString();
                txtSachTacGia.Text = row.Cells[3].Value.ToString();
                numSachSoLuong.Value = (int)row.Cells[4].Value;
                numSachGiaBan.Value = Convert.ToInt32(row.Cells[5].Value);

                maSachLoaiSach = (int)dataProvider.execScaler("SELECT ma_loai_sach FROM tbl_loai_sach WHERE ten_loai_sach = N'" + cbSachLoaiSach.Text + "'");
            }
        }

        private void btnSachThem_Click_1(object sender, EventArgs e)
        {
            StringBuilder query = new StringBuilder("EXEC proc_them_sach");
            dgSach.FirstDisplayedScrollingRowIndex = dgSach.RowCount - 1;
            query.Append(" @tenSach = N'" + txtSachTenSach.Text + "'");
            query.Append(",@maLoaiSach = " + maSachLoaiSach);
            query.Append(",@tacGia = N'" + txtSachTacGia.Text + "'");
            query.Append(",@soLuong = " + numSachSoLuong.Value);
            query.Append(",@giaBan = " + numSachGiaBan.Value);
            int result = dataProvider.execNonQuery(query.ToString());
            if (result > 0)
            {
                loadDgSach();
                MessageBox.Show("Thêm sách thành công !", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                MessageBox.Show("Thêm sách không thành công !", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSachSua_Click_1(object sender, EventArgs e)
        {
            StringBuilder query = new StringBuilder("EXEC proc_cap_nhat_sach");
            query.Append(" @maSach = " + maSachSach);
            query.Append(",@tenSach = N'" + txtSachTenSach.Text + "'");
            query.Append(",@maLoaiSach = " + maSachLoaiSach);
            query.Append(",@tacGia = N'" + txtSachTacGia.Text + "'");
            query.Append(",@soLuong = " + numSachSoLuong.Value);
            query.Append(",@giaBan = " + numSachGiaBan.Value);
            int result = dataProvider.execNonQuery(query.ToString());
            if (result > 0)
            {
                loadDgSach();
                MessageBox.Show("Cập nhật sách thành công !", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                MessageBox.Show("Cập nhật sách không thành công !", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSachXoa_Click_1(object sender, EventArgs e)
        {
            DialogResult check = MessageBox.Show("Bạn có chắc chắn muốn xóa sách " + txtSachTenSach.Text + "?", "Cảnh Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (check == DialogResult.Yes)
            {
                string query = "DELETE FROM tbl_sach WHERE ma_sach = " + maSachSach;
                int result = dataProvider.execNonQuery(query.ToString());
                if (result > 0)
                {
                    loadDgSach();
                    MessageBox.Show("Xóa sách thành công !", "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    MessageBox.Show("Xóa sách không thành công !", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cbSachLoaiSach_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            maSachLoaiSach = (int)comboBox.SelectedValue;
        }
        //xu ly loai sach
        private void dgLoaiSach_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            int rowId = e.RowIndex;
            if (rowId < dgLoaiSach.RowCount - 1 && rowId > 0)
            {
                DataGridViewRow row = dgLoaiSach.Rows[rowId];
                maLoaiSachLoaiSach = (int)row.Cells[0].Value;
                txtLoaiSachTenLoaiSach.Text = row.Cells[1].Value.ToString();
            }
        }

        private void btnLoaiSachThem_Click_1(object sender, EventArgs e)
        {
            StringBuilder query = new StringBuilder("EXEC proc_them_loai_sach");
            query.Append(" @tenLoaiSach = N'" + txtLoaiSachTenLoaiSach.Text + "'");
            int result = dataProvider.execNonQuery(query.ToString());
            if (result > 0)
            {
                loadDgLoaiSach();
                loadcbSachLoaiSach();
                MessageBox.Show("Thêm loại sách thành công !", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                MessageBox.Show("Thêm loại sách không thành công !", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLoaiSachSua_Click_1(object sender, EventArgs e)
        {
            StringBuilder query = new StringBuilder("EXEC proc_cap_nhat_loai_sach");
            query.Append(" @tenLoaiSach = N'" + txtLoaiSachTenLoaiSach.Text + "'");
            query.Append(",@maLoaiSach = " + maLoaiSachLoaiSach);
            int result = dataProvider.execNonQuery(query.ToString());
            if (result > 0)
            {
                loadDgLoaiSach();
                loadDgSach();
                loadcbSachLoaiSach();
                MessageBox.Show("Cập nhật loại sách thành công !", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                MessageBox.Show("Cập nhật loại sách không thành công !", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLoaiSachXoa_Click_1(object sender, EventArgs e)
        {
            DialogResult check = MessageBox.Show("Bạn có chắc chắn muốn xóa loại sách " + txtLoaiSachTenLoaiSach.Text + "?", "Cảnh Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (check == DialogResult.Yes)
            {
                string query = "DELETE FROM tbl_loai_sach WHERE ma_loai_sach = " + maLoaiSachLoaiSach;
                int result = dataProvider.execNonQuery(query.ToString());
                if (result > 0)
                {
                    loadDgLoaiSach();
                    loadcbSachLoaiSach();
                    MessageBox.Show("Xóa loại sách thành công !", "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    MessageBox.Show("Xóa loại sách không thành công !", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        //xu ly hoa don
        private void dgHoaDon_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            int rowId = e.RowIndex;
            if (rowId < dgHoaDon.RowCount - 1 && rowId > 0)
            {
                DataGridViewRow row = dgHoaDon.Rows[rowId];
                maHoaDonHoaDon = (int)row.Cells[0].Value;
                dateNgayLapHoaDon.Value = DateTime.Parse(row.Cells[1].Value.ToString());
                txtHoaDonTenKhachHang.Text = row.Cells[2].Value.ToString();
                txtHoaDonSDTKH.Text = row.Cells[3].Value.ToString();
            }
        }

        private void btnHoaDonThem_Click_1(object sender, EventArgs e)
        {
            StringBuilder query = new StringBuilder("EXEC proc_them_hoa_don");
            query.Append(" @ngayLapHoaDon = '" + dateNgayLapHoaDon.Value + "'");
            query.Append(",@tenKhachHang = N'" + txtHoaDonTenKhachHang.Text + "'");
            query.Append(",@sdtKhachHang = '" + txtHoaDonSDTKH.Text + "'");
            int result = dataProvider.execNonQuery(query.ToString());
            if (result > 0)
            {
                loadDgHoaDon();
                MessageBox.Show("Thêm hóa đơn thành công !", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                MessageBox.Show("Thêm hóa đơn không thành công !", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHoaDonSua_Click_1(object sender, EventArgs e)
        {
            StringBuilder query = new StringBuilder("EXEC proc_cap_nhat_hoa_don");
            query.Append(" @ngayLapHoaDon = '" + dateNgayLapHoaDon.Value + "'");
            query.Append(",@tenKhachHang = N'" + txtHoaDonTenKhachHang.Text + "'");
            query.Append(",@sdtKhachHang = '" + txtHoaDonSDTKH.Text + "'");
            query.Append(",@maHoaDon = " + maHoaDonHoaDon);
            int result = dataProvider.execNonQuery(query.ToString());
            if (result > 0)
            {
                loadDgHoaDon();
                MessageBox.Show("Cập nhật hóa đơn thành công !", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                MessageBox.Show("Cập nhật hóa đơn không thành công !", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHoaDonXoa_Click_1(object sender, EventArgs e)
        {
            DialogResult check = MessageBox.Show("Bạn có chắc chắn muốn xóa hóa đơn có mã là " + maHoaDonHoaDon + "?", "Cảnh Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (check == DialogResult.Yes)
            {
                string query = "DELETE FROM tbl_hoa_don WHERE ma_hoa_don = " + maHoaDonHoaDon;
                int result = dataProvider.execNonQuery(query.ToString());
                if (result > 0)
                {
                    loadDgHoaDon();
                    MessageBox.Show("Xóa hóa đơn thành công !", "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    MessageBox.Show("Xóa hóa đơn không thành công !", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txtHoaDonSDTKH_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
        //xu ly phieu nhap
        private void dgPhieuNhap_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            int rowId = e.RowIndex;
            if (rowId < dgPhieuNhap.RowCount - 1 && rowId > 0)
            {
                DataGridViewRow row = dgPhieuNhap.Rows[rowId];
                maPhieuNhapPhieuNhap = (int)row.Cells[0].Value;
                dateNgayLapPhieuNhap.Value = DateTime.Parse(row.Cells[1].Value.ToString());
                txtPhieuNhapNhaCungCap.Text = row.Cells[2].Value.ToString();
            }
        }

        private void btnPhieuNhapThem_Click_1(object sender, EventArgs e)
        {
            StringBuilder query = new StringBuilder("EXEC proc_them_phieu_nhap");
            query.Append(" @ngayLapPhieuNhap = '" + dateNgayLapPhieuNhap.Value + "'");
            query.Append(",@tenNhaCungCap = N'" + txtPhieuNhapNhaCungCap.Text + "'");
            int result = dataProvider.execNonQuery(query.ToString());
            if (result > 0)
            {
                loadDgPhieuNhap();
                MessageBox.Show("Thêm phiếu nhập thành công !", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                MessageBox.Show("Thêm phiếu nhập không thành công !", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPhieuNhapSua_Click_1(object sender, EventArgs e)
        {
            StringBuilder query = new StringBuilder("EXEC proc_cap_nhat_phieu_nhap");
            query.Append(" @ngayLapPhieuNhap = '" + dateNgayLapPhieuNhap.Value + "'");
            query.Append(",@tenNhaCungCap = N'" + txtPhieuNhapNhaCungCap.Text + "'");
            query.Append(",@maPhieuNhap = " + maPhieuNhapPhieuNhap);
            int result = dataProvider.execNonQuery(query.ToString());
            if (result > 0)
            {
                loadDgPhieuNhap();
                MessageBox.Show("Cập nhật phiếu nhập thành công !", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                MessageBox.Show("Cập nhật phiếu nhập không thành công !", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPhieuNhapXoa_Click_1(object sender, EventArgs e)
        {
            DialogResult check = MessageBox.Show("Bạn có chắc chắn muốn xóa phiếu nhập có mã là " + maPhieuNhapPhieuNhap + "?", "Cảnh Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (check == DialogResult.Yes)
            {
                string query = "DELETE FROM tbl_phieu_nhap WHERE ma_phieu_nhap = " + maPhieuNhapPhieuNhap;
                int result = dataProvider.execNonQuery(query.ToString());
                if (result > 0)
                {
                    loadDgPhieuNhap();
                    MessageBox.Show("Xóa phiếu nhập thành công !", "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    MessageBox.Show("Xóa phiếu nhập không thành công !", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnPhieuNhapChiTiet_Click_1(object sender, EventArgs e)
        {
            FormChiTietPhieuNhap form = new FormChiTietPhieuNhap(maPhieuNhapPhieuNhap);
            form.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormChiTietHoaDon form = new FormChiTietHoaDon(maHoaDonHoaDon);
            form.ShowDialog();
        }
        public void loadGridByKeyWord()
        {
            DataTable dt = new DataTable();
            //StringBuilder query = new StringBuilder("SELECT * FROM tbl_sach");
            StringBuilder query = new StringBuilder("SELECT * FROM tbl_sach Where ten_sach like N'%" + tbKeyWord.Text + "%'");
            /*query.Append(",ten_sach as [Tên Sách]");
              query.Append(",ten_loai_sach as [Loại Sách]");
              query.Append(",tac_gia as [Tác Giả]");
              query.Append(",so_luong as [Số Lượng]");
              query.Append(",gia_ban as [Giá Bán]");
              query.Append(" FROM tbl_sach, tbl_loai_sach");
              query.Append(" WHERE tbl_sach.ma_loai_sach = tbl_loai_sach.ma_loai_sach;");*/
            dt = dataProvider.execQuery(query.ToString());
            dgSach.DataSource = dt;
        }

        private void tbKeyWord_TextChanged(object sender, EventArgs e)
        {
            loadGridByKeyWord();
            this.cbSachLoaiSach.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.cbSachLoaiSach.DropDownStyle = ComboBoxStyle.DropDown;
            loadDgSach();
        }
    }
}
