using System;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace BinaryFileIO
{
    public partial class BinaryFile : Form
    {
        DataTable tableBinary = new DataTable();

        const string fileName = @"\\SLUMBERAD\CompanyData\Admin\Vision.net conversion\CustomerProduct.bin";
        //const string fileName = @"C:\Users\mxhyj\Desktop\Output\CustomerProduct.bin";

        const int Size_Id = 4;
        const int Size_AccountId = 4;
        const int Size_AccountSortCodeId = 4;
        const int Size_ProductId = 4;
        const int Size_IsActive = 1;
        const int Size_Delimiter = 4 * 2;
        const int Size_Chunk = Size_Id + Size_AccountId + Size_AccountSortCodeId + Size_ProductId + Size_IsActive + Size_Delimiter;
        byte[] Chunk = new byte[Size_Chunk];

        char[] delimiterChars_Chunk = { '/' };
        char[] delimiterChars_Field = { ',' };
        long Size_File = 0;
        long Number_Chunk = 0;

        int Id;
        string string_Transaction = "";


        //Vision
        int AccountId = 185;
        int AccountSortCodeId = 0;
        string paraStringProductId = "7501, 7496, 7495, 7503, 7502, 7493, 7494, 7490, 7492, 7491, 7500, 7497, 7498, 7499";



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
            tableBinary = new DataTable();
            tableBinary.Columns.Add("Id", typeof(int));
            tableBinary.Columns.Add("AccountId", typeof(int));
            tableBinary.Columns.Add("AccountSortCodeId", typeof(int));
            tableBinary.Columns.Add("ProductId", typeof(int));
            tableBinary.Columns.Add("IsActive", typeof(bool));
            tableBinary.Columns.Add("Action", typeof(string));

            DataColumn[] keys = new DataColumn[1];
            keys[0] = tableBinary.Columns["Id"];
            tableBinary.PrimaryKey = keys;

            using (FileStream fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None))
            {
                Size_File = fileStream.Length;
                Number_Chunk = Size_File / Size_Chunk;
            }

            if (Size_File > 0)
            {
                ReadAllTransactions();
            }

            UpdateText();
            dgv.DataSource = tableBinary;
        }

        private void ReadAllTransactions()
        {
            try
            {
                string[] chunks = Encoding.UTF8.GetString(File.ReadAllBytes(fileName)).Split(delimiterChars_Chunk);

                foreach (string str in chunks)
                {
                    string[] field = str.Trim().Split(delimiterChars_Field);

                    if (field.Length > 1)
                    {
                        tableBinary.Rows.Add(Convert.ToInt32(field[0]), Convert.ToInt32(field[1]), Convert.ToInt32(field[2]), Convert.ToInt32(field[3]), Convert.ToBoolean(field[4]));
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void btInActivate_Click(object sender, EventArgs e)
        {
            Inactivate_AccountId(AccountId, paraStringProductId);
        }

        private void AddTransaction(int AccountId, int AccountSortCodeId, int ProductId, bool IsActive)
        {
            try
            {
                if (tableBinary.Rows.Count == 0)
                {
                    Id = 0;
                }
                else
                {
                    Id = Convert.ToInt32(tableBinary.Compute("MAX([Id])", ""));
                }
                Id = Id + 1;

                Chunk = GetTransactionBytes(Id, AccountId, AccountSortCodeId, ProductId, IsActive);
                using (BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.Append)))
                {
                    writer.Write(Chunk, 0, Size_Chunk);
                    tableBinary.Rows.Add(Id, AccountId, AccountSortCodeId, ProductId, IsActive);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void btUpdate_Click(object sender, EventArgs e)
        {
            UpdateBinaryFile(paraStringProductId);
        }

        private void btReWrite_Click(object sender, EventArgs e)
        {
            Id = Convert.ToInt32(tbIndex.Text);
            DataRow dr = tableBinary.Rows.Find(Id);

            InactivateTransaction(Id, dr.Field<int>("AccountId"), dr.Field<int>("AccountSortCodeId"), dr.Field<int>("ProductId"), false);
        }

        private byte[] GetTransactionBytes(int Id, int AccountId, int AccountSortCodeId, int ProductId, bool IsActive)
        {
            string_Transaction = Id.ToString() + "," + AccountId.ToString() + "," + AccountSortCodeId.ToString() + "," + ProductId.ToString() + "," + IsActive.ToString() + '/';
            return Encoding.UTF8.GetBytes(string_Transaction.PadRight(Size_Chunk));
        }

        private void InactivateTransaction(int Id, int AccountId, int AccountSortCodeId, int ProductId, bool IsActive)
        {
            int position = Size_Chunk * (Id - 1);

            using (BinaryWriter bw = new BinaryWriter(File.Open(fileName, FileMode.Open)))
            {
                bw.Seek(position, SeekOrigin.Begin);
                Chunk = GetTransactionBytes(Id, AccountId, AccountSortCodeId, ProductId, IsActive);
                bw.Write(Chunk, 0, Size_Chunk);

                DataRow dr = tableBinary.Rows.Find(Id);
                dr.BeginEdit();
                dr["IsActive"] = false;
                dr.EndEdit();
            }
        }

        private void UpdateText()
        {
            Text = string.Format("File size: {0} bytes, info Chunks: {1}", Size_File, Number_Chunk);
        }

        private void UpdateBinaryFile(string paraStringProductId)
        {
            DialogResult result = MessageBox.Show(string.Format("Yes: Save selected products as a new list and remove the old list.{0}No: Add selected products into saved list?", Environment.NewLine), "Save as new or Add", MessageBoxButtons.YesNoCancel);

            if (result == DialogResult.Yes && AccountId > 0)
            {
                Inactivate_AccountId(AccountId, paraStringProductId);
                Add_Products(paraStringProductId);
            }

            if (result == DialogResult.Yes && AccountSortCodeId > 0)
            {
                Inactivate_AccountSortCodeId(AccountSortCodeId, paraStringProductId);
                Add_Products(paraStringProductId);
            }

            if (result == DialogResult.No)
            {
                Add_Products(paraStringProductId);
            }
        }

        private void Inactivate_AccountId(int AccountId, string paraStringProductId)
        {
            DataRow[] drs = tableBinary.Select(string.Format("AccountId = {0} AND AccountSortCodeId = 0 AND IsActive = 1", AccountId));
            foreach (DataRow dr in drs)
            {
                InactivateTransaction(dr.Field<int>("Id"), dr.Field<int>("AccountId"), dr.Field<int>("AccountSortCodeId"), dr.Field<int>("ProductId"), false);
            }
        }

        private void Inactivate_AccountSortCodeId(int AccountSortCodeId, string paraStringProductId)
        {
            DataRow[] drs = tableBinary.Select(string.Format("AccountId = 0 AND AccountSortCodeId = {0} AND IsActive = 1", AccountId));
            foreach (DataRow dr in drs)
            {
                InactivateTransaction(dr.Field<int>("Id"), dr.Field<int>("AccountId"), dr.Field<int>("AccountSortCodeId"), dr.Field<int>("ProductId"), false);
            }
        }

        private void Add_Products(string paraStringProductId)
        {
            string[] strings = paraStringProductId.Trim().Split(',');
            DataRow[] drs;

            if (AccountId > 0)
            {
                foreach (string str in strings)
                {
                    drs = tableBinary.Select(string.Format("AccountId = {0} AND AccountSortCodeId = 0 AND ProductId = {1} AND IsActive = 1", AccountId, Convert.ToInt32(str)));
                    if (drs.Length == 0)
                    {
                        AddTransaction(AccountId, 0, Convert.ToInt32(str), true);
                    }
                }
            }

            if (AccountSortCodeId > 0)
            {
                foreach (string str in strings)
                {
                    drs = tableBinary.Select(string.Format("AccountId = 0 AND AccountSortCodeId = {0} AND ProductId = {1} AND IsActive = 1", AccountSortCodeId, Convert.ToInt32(str)));
                    if (drs.Length == 0)
                    {
                        AddTransaction(0, AccountSortCodeId, Convert.ToInt32(str), true);
                    }
                }
            }
        }

        private string GetProductIdFromBinary(int AccountId, int AccountSortCodeId)
        {
            DataRow[] drs;
            string str = "";

            drs = tableBinary.Select(string.Format("AccountId = {0} AND AccountSortCodeId = {1} AND IsActive = 1", AccountId, AccountSortCodeId));
            foreach (DataRow dr in drs)
            {
                str = str + dr.Field<int>("ProductId").ToString() + ", ";
            }

            if (str.Length > 0)
            {
                str = str.Substring(0, str.Length - 2);
            }
            else
            {
                str = "";
            }

            return str;
        }

        private void tbGo_Click(object sender, EventArgs e)
        {
            Text = GetProductIdFromBinary(Convert.ToInt32(tbIndex.Text), 0);
        }
    }
}
