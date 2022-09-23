using Microsoft.AspNetCore.Mvc;
using iText.Kernel.Pdf;
using PdfDocument = iText.Kernel.Pdf.PdfDocument;
using iText.Kernel.Utils;
using SimplePDF.Models;

namespace SimplePDF.Controllers
{

    public class JuntarController : Controller
    {
        private readonly IWebHostEnvironment _web;
        public static string nomePasta = "";
        public string PastaDestino = "";
     
        
        public JuntarController(IWebHostEnvironment web)
        {
            nomePasta = ControlePastas.GeraNomePasta();
            _web = web;
            PastaDestino = Path.Combine(_web.WebRootPath + @"\Arquivos\Juntar\" + nomePasta);
            
        }
        public IActionResult JuntarPDF()
        {
            Juntar.juntando = "";
            
            return View();
        }
        public IActionResult Download()
        {

            return View();
        }
        public IActionResult Juntando()
        {

            return View();
        }

        
       

        [HttpPost]
        public async Task<IActionResult> JuntarPDF(IFormFile[] filesupload)
        {
            var numArquivo = 0;
            if(filesupload == null || filesupload.Length == 0)
            {
                ViewData["Message"] = "Selecione os arquivos!";
                return View();
            }
            else
            {
                Directory.CreateDirectory(PastaDestino);
                    foreach(IFormFile file in filesupload)
                    {
                    numArquivo = numArquivo + 1;
                    var savefile = Path.Combine(PastaDestino, @"pagina"+numArquivo);
                    {
                        var fileselected = new FileStream(savefile, FileMode.Create);
                        await file.CopyToAsync(fileselected);
                        ViewData["Message"] = "Mensagem sucesso";
                        fileselected.Close();
                    }
                    
                }
            }
            
            carregaArquivos();
            Juntar.nomeArquivo = @"../Arquivos/Juntar/" + nomePasta + @"/merged.pdf";
            Juntar.juntando = "http-equiv=refresh";
          
            return Download();
        }

        public void carregaArquivos()
        {
            
            var path = PastaDestino;
            var arquivos = GetFiles(path);
            var src0 = System.IO.Path.Combine(path, "merged.pdf");
            var wtr0 = new PdfWriter(src0);
            var pdf0 = new iText.Kernel.Pdf.PdfDocument(wtr0);
            var merger = new PdfMerger(pdf0);
            foreach (string arquivo in arquivos)
            {            

            var src1 = arquivo;
            var fi1 = new FileInfo(src1);
            var rdr1 = new iText.Kernel.Pdf.PdfReader(fi1);
            var pdf1 = new PdfDocument(rdr1);

            merger.Merge(pdf1, 1, pdf1.GetNumberOfPages());
            


            }
            merger.Close();
            pdf0.Close();

        }

        private string[] GetFiles(string caminho)
        {
            return Directory.GetFiles(caminho, "*.*", SearchOption.TopDirectoryOnly);
        }
    }
}
