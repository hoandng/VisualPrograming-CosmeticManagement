using Main.Models;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Main.BaoCao
{
    public partial class Report_HDB : Form
    {
        ProcessDatabase db;
        User _user;
        DateTime _tu = DateTime.Now;
        DateTime _den = DateTime.Now;
        public Report_HDB(User user)
        {
            _user = new User(user);
            db = new ProcessDatabase();
            InitializeComponent();
        }


        private void Report_HDB_Load(object sender, EventArgs e)
        {
            this.rv_HoaDonBan.RefreshReport();
            Load_Report();
        }

        private void Load_Report()
        {
            string nguoilap = "";
            string query = "Select TenNV From [NhanVien] Where MaNV = @manv";
            var parameters = new Dictionary<string, object>
            {
                {"@manv", _user.EmployeeID }
            };
            DataTable dt = db.ExecuteQuery(query, parameters);
            if (dt.Rows.Count > 0)
            {
                string name = dt.Rows[0]["TenNV"].ToString();
                if (name.Trim() != "")
                {
                    nguoilap = name;
                }
                else
                {
                    nguoilap = "Unknow";
                }
            }
            else
            {
                nguoilap = _user.Username;
            }

            Console.WriteLine(nguoilap);
            // Truyền parameter tên người lập báo cáo

            query = "Select * From [HoaDonBan] Where NgayBan >= @tu And NgayBan <= @den";
            parameters = new Dictionary<string, object>
            {
                {"@tu", _tu},
                {"@den", _den},
            };
            dt = db.ExecuteQuery(query, parameters);
            rv_HoaDonBan.LocalReport.DataSources.Clear();
            ReportDataSource source = new ReportDataSource("DataSetHoaDonBan", dt);
            rv_HoaDonBan.LocalReport.ReportPath = "D:\\VisualPrograming\\BTL\\Winform-QuanLyMyPham\\Main\\ReportHoaDonBan.rdlc";
            rv_HoaDonBan.LocalReport.DataSources.Add(source);
            
            var reportParameters = new ReportParameterCollection
            {
                new ReportParameter("NguoiLapBaoCao", nguoilap)
            };
            
            rv_HoaDonBan.LocalReport.SetParameters(reportParameters);
            rv_HoaDonBan.RefreshReport();
        }

        private void dtp_Tu_ValueChanged(object sender, EventArgs e)
        {
            _tu = dtp_Tu.Value;
            Load_Report();
        }

        private void dtp_Den_ValueChanged(object sender, EventArgs e)
        {
            _den = dtp_Den.Value;
            Load_Report();
        }
    }
}
