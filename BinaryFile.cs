using System;
using System.Data;
using System.Windows.Forms;

namespace BinaryFileIO
{
    public partial class BinaryFile : Form
    {
        const string fileName = @"\\SLUMBERAD\CompanyData\Admin\Vision.net conversion\CustomerProduct1.bin";

        private DataTable tableBinary = new DataTable();
        private Bin_Old bo = new Bin_Old(fileName);

        int AccountId;
        int AccountSortCodeId;

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
            dgv.DataSource = bo.tableBinary;
            UpdateText();
        }


        private void btInActivate_Click(object sender, EventArgs e)
        {

            bo.Inactivate_AccountId(AccountId, paraStringProductId);
        }


        private void btUpdate_Click(object sender, EventArgs e)
        {
            bo.UpdateBinaryFile(paraStringProductId);
        }

        private void btReWrite_Click(object sender, EventArgs e)
        {
            Id = Convert.ToInt32(tbIndex.Text);
            DataRow dr = tableBinary.Rows.Find(Id);

            bo.InactivateTransaction(Id, dr.Field<int>("AccountId"), dr.Field<int>("AccountSortCodeId"), dr.Field<int>("ProductId"), false);
        }

        private void UpdateText()
        {
            Text = string.Format("File size: {0} bytes, info Chunks: {1}", bo.Size_File, bo.Number_Chunk);
        }

        private void tbGo_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(GetProductId(258, 0));
        }

        private void CreateNewFile(int Size_Chunk_New)
        {



        }
    }
}
