using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace BinaryFileIO
{
    public class Bin
    {
        string fileName;

        const int Size_Id = 4;
        const int Size_AccountId = 4;
        const int Size_AccountSortCodeId = 4;
        const int Size_ProductId = 4;
        const int Size_IsActive = 1;
        const int Size_Reserved = 13;

        const int Size_Block = Size_Id + Size_AccountId + Size_AccountSortCodeId + Size_ProductId + Size_IsActive + Size_Reserved;

        byte[] byte_Id = new byte[Size_Id];
        byte[] byte_AccountId = new byte[Size_AccountId];
        byte[] byte_AccountSortCodeId = new byte[Size_AccountSortCodeId];
        byte[] byte_ProductId = new byte[Size_ProductId];
        byte[] byte_IsActive = new byte[Size_IsActive];
        byte[] byte_Reserved = new byte[Size_Reserved];

        #region Properties
        public DataTable TableBinary { get; private set; } = new DataTable();
        public long Size_File { get; private set; } = 0;
        public long Number_Block { get; private set; } = 0;

        #endregion

        public Bin(string name)
        {
            fileName = name;
            TableBinary = GetData();
        }
        public DataTable GetData()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("AccountId", typeof(int));
            table.Columns.Add("AccountSortCodeId", typeof(int));
            table.Columns.Add("ProductId", typeof(int));
            table.Columns.Add("IsActive", typeof(bool));
            table.Columns.Add("Action", typeof(string));

            DataColumn[] keys = new DataColumn[1];
            keys[0] = table.Columns["Id"];
            table.PrimaryKey = keys;

            using (FileStream fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None))
            {
                Size_File = fileStream.Length;
                Number_Block = Size_File / Size_Block;
            }

            if (Size_File > 0)
            {
                return ReadAllTransactions(table);
            }
            return table;
        }
        public void InactivateTransaction(int Id)
        {
            try
            {
                int position = Size_Block * (Id - 1);

                using (BinaryWriter bw = new BinaryWriter(File.Open(fileName, FileMode.Open)))
                {
                    byte_IsActive = BitConverter.GetBytes(false);

                    bw.Seek(position, SeekOrigin.Begin);
                    bw.Write(byte_IsActive, Size_Id + Size_AccountId + Size_AccountSortCodeId + Size_ProductId, Size_IsActive);

                    DataRow dr = TableBinary.Rows.Find(Id);
                    dr.BeginEdit();
                    dr["IsActive"] = false;
                    dr.EndEdit();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString() + Environment.NewLine + e.Message + Environment.NewLine + e.StackTrace);
            }
        }
        public void UpdateBinaryFile(int AccountId, int AccountSortCodeId, string paraStringProductId)
        {
            DialogResult result = MessageBox.Show(string.Format("Yes: Save selected products as a new list and remove the old list.{0}No: Add selected products into saved list?", Environment.NewLine), "Save as new or Add", MessageBoxButtons.YesNoCancel);

            if (result == DialogResult.Yes)
            {
                Inactivate_AccountId(AccountId, AccountSortCodeId);
            }
            Add_Products(AccountId, AccountSortCodeId, paraStringProductId);
        }
        public void Inactivate_AccountId(int AccountId, int AccountSortCodeId)
        {
            DataRow[] drs = TableBinary.Select(string.Format("AccountId = {0} AND AccountSortCodeId = {1} AND IsActive = 1", AccountId, AccountSortCodeId));
            foreach (DataRow dr in drs)
            {
                InactivateTransaction(dr.Field<int>("Id"));
            }
        }
        public string GetProductIdString(int AccountId, int AccountSortCodeId)
        {
            DataRow[] drs;
            string str = "";

            drs = TableBinary.Select(string.Format("AccountId = {0} AND AccountSortCodeId = {1} AND IsActive = 1", AccountId, AccountSortCodeId));
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
        public void AddTransaction(int AccountId, int AccountSortCodeId, int ProductId, bool IsActive)
        {
            try
            {
                int id;
                if (TableBinary.Rows.Count == 0)
                {
                    id = 0;
                }
                else
                {
                    id = Convert.ToInt32(TableBinary.Compute("MAX([Id])", ""));
                }
                id = id + 1;

                using (BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.Append)))
                {
                    writer.Write(GetTransactionBytes(id, AccountId, AccountSortCodeId, ProductId, IsActive), 0, Size_Block);
                    TableBinary.Rows.Add(id, AccountId, AccountSortCodeId, ProductId, IsActive);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private DataTable ReadAllTransactions(DataTable table)
        {
            try
            {
                int position = 0;
                int id;
                int accountId;
                int sortcodeId;
                int productId;
                bool isActive;
                int reserved;
                byte[] file = File.ReadAllBytes(fileName);

                if (Number_Block >= 0)
                {
                    for (int i = 0; i < Number_Block; i++)
                    {
                        position = 0;
                        Buffer.BlockCopy(file, i * Size_Block + position, byte_Id, 0, Size_Id);
                        position = position + Size_Id;
                        id = BitConverter.ToInt32(byte_Id, 0);

                        Buffer.BlockCopy(file, i * Size_Block + position, byte_AccountId, 0, Size_AccountId);
                        position = position + Size_AccountId;
                        accountId = BitConverter.ToInt32(byte_AccountId, 0);

                        Buffer.BlockCopy(file, i * Size_Block + position, byte_AccountSortCodeId, 0, Size_AccountSortCodeId);
                        position = position + Size_AccountSortCodeId;
                        sortcodeId = BitConverter.ToInt32(byte_AccountSortCodeId, 0);

                        Buffer.BlockCopy(file, i * Size_Block + position, byte_ProductId, 0, Size_ProductId);
                        position = position + Size_ProductId;
                        productId = BitConverter.ToInt32(byte_ProductId, 0);

                        Buffer.BlockCopy(file, i * Size_Block + position, byte_IsActive, 0, Size_IsActive);
                        position = position + Size_IsActive;
                        isActive = BitConverter.ToBoolean(byte_IsActive, 0);

                        Buffer.BlockCopy(file, i * Size_Block + position, byte_Reserved, 0, Size_Reserved);
                        position = position + Size_Reserved;
                        reserved = BitConverter.ToInt32(byte_Reserved, 0);

                        if (i == id - 1)
                        {
                            table.Rows.Add(id, accountId, sortcodeId, productId, isActive);
                        }
                        else
                        {
                            MessageBox.Show("Data has been compromised, operation aborted.");
                            return table;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Nothing in the file.");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString() + Environment.NewLine + e.Message + Environment.NewLine + e.StackTrace);
            }
            return table;
        }
        private byte[] GetTransactionBytes(int Id, int AccountId, int AccountSortCodeId, int ProductId, bool IsActive)
        {
            byte_Id = BitConverter.GetBytes(Id);
            byte_AccountId = BitConverter.GetBytes(AccountId);
            byte_AccountSortCodeId = BitConverter.GetBytes(AccountSortCodeId);
            byte_ProductId = BitConverter.GetBytes(ProductId);
            byte_IsActive = BitConverter.GetBytes(IsActive);

            byte[] block = new byte[Size_Block];
            int position = 0;

            Buffer.BlockCopy(byte_Id, 0, block, position, Size_Id);
            position = position + Size_Id;

            Buffer.BlockCopy(byte_AccountId, 0, block, position, Size_AccountId);
            position = position + Size_AccountId;

            Buffer.BlockCopy(byte_AccountSortCodeId, 0, block, position, Size_AccountSortCodeId);
            position = position + Size_AccountSortCodeId;

            Buffer.BlockCopy(byte_ProductId, 0, block, position, Size_ProductId);
            position = position + Size_ProductId;

            Buffer.BlockCopy(byte_IsActive, 0, block, position, Size_IsActive);
            position = position + Size_IsActive;

            Buffer.BlockCopy(byte_Reserved, 0, block, position, Size_Reserved);
            
            return block;
        }
        private void Add_Products(int AccountId, int AccountSortCodeId, string paraStringProductId)
        {
            string[] strings = paraStringProductId.Trim().Split(',');
            DataRow[] drs;

            if (AccountId > 0 || AccountSortCodeId > 0)
            {
                foreach (string str in strings)
                {
                    drs = TableBinary.Select(string.Format("AccountId = {0} AND AccountSortCodeId = {1} AND ProductId = {2} AND IsActive = 1", AccountId, AccountSortCodeId, Convert.ToInt32(str)));
                    if (drs.Length == 0)
                    {
                        AddTransaction(AccountId, 0, Convert.ToInt32(str), true);
                    }
                }
            }
        }
    }
}
