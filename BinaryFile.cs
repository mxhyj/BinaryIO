using System;
using System.Data;
using System.Windows.Forms;

namespace BinaryFileIO
{
    public partial class BinaryFile : Form
    {
        const string fileName_old = @"\\SLUMBERAD\CompanyData\Admin\Vision.net conversion\CustomerProduct1.bin";
        const string fileName_new = @"\\SLUMBERAD\CompanyData\Admin\Vision.net conversion\CustomerProduct_New.bin";

        private DataTable TB_Bridge = new DataTable();

        private Bin_Old bo = new Bin_Old(fileName_old);
        private Bin bin = new Bin(fileName_new);

        string paraStringProductId = "";

        public BinaryFile()
        {
            InitializeComponent();
        }

        private void BinaryFile_Load(object sender, EventArgs e)
        {
            InitializeData();
        }

        private void InitializeData()
        {
            dgv.DataSource = bo.TableBinary;
            UpdateText();
        }

        private void btInActivate_Click(object sender, EventArgs e)
        {
            bo.Inactivate_AccountId(Convert.ToInt32(tbAccountId.Text), Convert.ToInt32(tbAccountSortCodeId.Text));
        }
        private void btUpdate_Click(object sender, EventArgs e)
        {
            //bo.UpdateBinaryFile(Convert.ToInt32(tbAccountId.Text), Convert.ToInt32(tbAccountSortCodeId.Text), paraStringProductId);
        }

        private void btReWrite_Click(object sender, EventArgs e)
        {
            bo.UpdateBinaryFile(Convert.ToInt32(tbAccountId.Text), Convert.ToInt32(tbAccountSortCodeId.Text), paraStringProductId);

            //DataRow dr = TableBinary.Rows.Find(Convert.ToInt32(tbAccountId.Text));
            //bo.InactivateTransaction(Convert.ToInt32(tbAccountId.Text), dr.Field<int>("AccountId"), dr.Field<int>("AccountSortCodeId"), dr.Field<int>("ProductId"), false);
        }

        private void UpdateText()
        {
            Text = string.Format("File size: {0} bytes, info Chunks: {1}", bo.Size_File, bo.Number_Chunk);
        }

        private void tbGetProductIds_Click(object sender, EventArgs e)
        {
            paraStringProductId = bo.GetProductIdString(Convert.ToInt32(tbAccountId.Text), Convert.ToInt32(tbAccountSortCodeId.Text));
            MessageBox.Show(paraStringProductId);
        }

        private void CreateNewFile(int Size_Chunk_New)
        {



        }
    }
}
