using System;
using System.IO;
using System.Windows.Forms;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Forms;
using iText.Forms.Fields;


namespace AssinaturaPDF
{
    
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            
            using (OpenFileDialog openFile = new OpenFileDialog())
            {
                
                openFile.Filter = "pdf files(*.pdf)|*.pdf";
                openFile.Title = "Escolha o arquivo PDF para adicionar uma assinatura.";
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    
                    string filePath = openFile.FileName;
                    string outPath = RemoveExtension(filePath);
                    AddSignature(filePath, outPath + "-Assinatura.pdf");                    
                    MessageBox.Show("Arquivo com assinatura salvo em " + outPath + "-Assinatura.pdf",
                                        "Arquivo Salvo",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);

                }
            }
        }
        private void AddSignature(string filePath, string outPath)
        {
            Random rand = new Random();
            int rndNum = rand.Next();
            decimal idec = this.numericUpDown1.Value;
            float signatureHeight =(float)idec;
            PdfReader reader = new PdfReader(filePath);
            FileStream fs = new FileStream(outPath, FileMode.Create, FileAccess.Write);
            PdfWriter writer = new PdfWriter(fs);
            PdfDocument pdfDoc = new PdfDocument(reader, writer);            
            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);
            Rectangle rect = new Rectangle(0, signatureHeight, 800, 50);            
            PdfFormField signature = PdfFormField.CreateSignature(pdfDoc, rect);
            signature.SetFieldName("Assinatura"+rndNum);
            form.AddField(signature, pdfDoc.GetFirstPage());
            pdfDoc.Close();

        }

        private string RemoveExtension(string filePath)
        {
            int lio = filePath.LastIndexOf(@".pdf");
            string outPut = filePath.Substring(0, lio);
            Console.WriteLine(outPut);
            return outPut;
        }
               

        private void Form1_Load(object sender, EventArgs e)
        {

        }

       
    }
    public static class StringExtensions
    {
        public static string Left(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            maxLength = Math.Abs(maxLength);

            return (value.Length <= maxLength
                   ? value
                   : value.Substring(0, maxLength)
                   );
        }
    }
}
