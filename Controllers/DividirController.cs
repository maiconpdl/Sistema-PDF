using Microsoft.AspNetCore.Mvc;
using iTextSharp.text.pdf;
using iTextSharp.text;
using SimplePDF.Models;
using System.IO.Compression;

namespace SimplePDF.Controllers
{
    public class DividirController : Controller
    {
        public static string nomePasta = "";
        public static String resource;
        public string PastaDestino = "";
        private readonly IWebHostEnvironment _web;
        
        public IActionResult Index()
        {
            return View();
        }

        public DividirController(IWebHostEnvironment web)
        {
            nomePasta = ControlePastas.GeraNomePasta();
            _web = web;
            PastaDestino = Path.Combine(_web.WebRootPath + @"\Arquivos\Dividir\" + nomePasta);
        }

       public IActionResult Download()
        {
            return View();
        }

        public IActionResult DividirPDF()
        {
            Dividir.dividindo = "";
            return View();
        }

           
        [HttpPost]
        public async Task<IActionResult> DividirPDF(IFormFile[] filesupload, int pagInicio, int pagFim)
        {
            var pastaDownload = ControlePastas.GeraNomePasta();
            if (filesupload == null || filesupload.Length == 0)
        {
            ViewData["Message"] = "Selecione o arquivo!";
                return View();
        }
        else
        {
            Directory.CreateDirectory(PastaDestino);
                
                foreach (IFormFile file in filesupload)
                {

                    var savefile = Path.Combine(PastaDestino, "upload.pdf");
                    {
                        var fileselected = new FileStream(savefile, FileMode.Create);
                        await file.CopyToAsync(fileselected);
                        ViewData["Message"] = "Mensagem sucesso";
                        fileselected.Close();
                    }
                resource = file.FileName;
                }


                ExtraiPagina(pagInicio, pagFim);
               
                Directory.CreateDirectory(Path.Combine(_web.WebRootPath , @"Arquivos" , @"Download", pastaDownload));
                ZipFile.CreateFromDirectory(PastaDestino, Path.Combine(_web.WebRootPath, @"Arquivos" ,@"Download",pastaDownload, @"Arquivo.zip"));
            }
            
            Dividir.nomeArquivo = @"../Arquivos/Download/" + pastaDownload + @"/Arquivo.zip";
            Dividir.dividindo = "http-equiv=refresh";
            
            return Download();
        }


        


        public void ExtraiPagina(int pagInicio, int pagFim)
        {
            
            PdfReader pdfReader = new PdfReader(PastaDestino+ @"\" + @"upload.pdf");
            
            Document document = new Document();
            

            if (pdfReader.NumberOfPages > 0)
            {
                if ((pagInicio != 0) && (pagFim != 0))
                {
                    for (int i = 1; i <= pdfReader.NumberOfPages; i++)
                    {
                        if ((i >= pagInicio) && (i <= pagFim))
                        {
                            PdfCopy pdfCopy = new PdfCopy(document, new FileStream(Path.Combine(PastaDestino, string.Format("Pagina_{0}.pdf", i)), FileMode.Create));
                            document.Open();
                            pdfCopy.AddPage(pdfCopy.GetImportedPage(pdfReader, i));
                        }
                    }
                }
                else
                {
                    for (int i = 1; i <= pdfReader.NumberOfPages; i++)
                    {
                        
                            PdfCopy pdfCopy = new PdfCopy(document, new FileStream(Path.Combine(PastaDestino, string.Format("Pagina_{0}.pdf", i)), FileMode.Create));
                            document.Open();
                            pdfCopy.AddPage(pdfCopy.GetImportedPage(pdfReader, i));
                        
                    }
                }
                document.Close();
            }
            else
            {
                return;
            }
                
        }

    }
}
