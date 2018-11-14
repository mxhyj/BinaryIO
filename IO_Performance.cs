using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BinaryFileIO
{
    public partial class IO_Performance : Form
    {
        const string FileName = "TestFile";
        const int FileSize = 1024 * 1024 * 20;
        const int ChunkSize = 1024 * 4;
        const int NumChunks = FileSize / ChunkSize;
        static readonly byte[] Chunk = new byte[ChunkSize];
        string report = "";

        public IO_Performance()
        {
            InitializeComponent();
        }

        private void IO_Performance_Load(object sender, EventArgs e)
        {
            Start();
        }

        private void Start()
        {
            var path = Path.Combine(Application.StartupPath, FileName);

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            var stopwatch = new System.Diagnostics.Stopwatch();
            long readChunkTime;
            long readByteTime;
            long writeChunkTime;
            long writeByteTime;
            long seekTime;
            long openCloseTime;

            using (var stream = File.Open(path, FileMode.OpenOrCreate))
            {
                stopwatch.Start();
                for (var i = 0; i < NumChunks; ++i)
                {
                    stream.Write(Chunk, 0, ChunkSize);
                }
                writeChunkTime = stopwatch.ElapsedMilliseconds;
            }

            using (var stream = File.Open(path, FileMode.OpenOrCreate))
            {
                stopwatch.Reset();
                stopwatch.Start();
                for (var i = 0; i < FileSize; ++i)
                {
                    stream.WriteByte(0);
                }
                writeByteTime = stopwatch.ElapsedMilliseconds;
            }

            using (var stream = File.Open(path, FileMode.OpenOrCreate))
            {
                stopwatch.Reset();
                stopwatch.Start();
                for (var i = 0; i < NumChunks; ++i)
                {
                    var numBytesRemain = ChunkSize;
                    var offset = 0;

                    while (numBytesRemain > 0)
                    {
                        var read = stream.Read(Chunk, offset, numBytesRemain);
                        numBytesRemain -= read;
                        offset += read;
                    }
                }
                readChunkTime = stopwatch.ElapsedMilliseconds;
            }

            using (var stream = File.Open(path, FileMode.OpenOrCreate))
            {
                stopwatch.Reset();
                stopwatch.Start();
                for (var i = 0; i < FileSize; ++i)
                {
                    stream.ReadByte();
                }
                readByteTime = stopwatch.ElapsedMilliseconds;
            }

            using (var stream = File.Open(path, FileMode.OpenOrCreate))
            {
                stopwatch.Reset();
                stopwatch.Start();
                for (var i = 0; i < NumChunks; ++i)
                {
                    stream.Position = stream.Position;
                }
                seekTime = stopwatch.ElapsedMilliseconds;
            }

            stopwatch.Reset();
            stopwatch.Start();
            for (var i = 0; i < NumChunks; ++i)
            {
                using (var stream = File.Open(path, FileMode.OpenOrCreate))
                {
                }
            }
            openCloseTime = stopwatch.ElapsedMilliseconds;

            //File.Delete(path);

            report = "Operation,Time\n"
            + "Write Chunk," + writeChunkTime + "\n"
            + "Write Byte," + writeByteTime + "\n"
            + "Read Chunk," + readChunkTime + "\n"
            + "Read Byte," + readByteTime + "\n"
            + "Seek," + seekTime + "\n"
            + "Open+Close," + openCloseTime + "\n";

            richTextBox.AppendText(report);
            richTextBox.AppendText(Environment.NewLine);

            byte[] bytes = new byte[16];
            using (BinaryReader reader = new BinaryReader(new FileStream(@"O:\Database\Export.mdf", FileMode.Open)))
            {
                reader.Read(bytes, 0, 16);
            }

            richTextBox.AppendText(BitConverter.ToString(bytes));
            //richTextBox.AppendText(BitConverter.GetBytes(256).ToString());

        }
    }
}
