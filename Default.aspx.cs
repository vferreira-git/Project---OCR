using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Tesseract;
using System.Web.Hosting;
using Ghostscript.NET.Rasterizer;
using System.Drawing;
using Ghostscript.NET;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using WebApplication6.Classes;

namespace WebApplication6
{
    public partial class Default : System.Web.UI.Page
    {
        private GhostscriptVersionInfo gvi;
        protected void Page_Load(object sender, EventArgs e)
        {
            gvi = new GhostscriptVersionInfo(Server.MapPath("./gsdll32.dll"));
            
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            if (!checkBoxOCR.Checked)
            {
                if (fileUpload.HasFile)
                {

                    if (Path.GetExtension(fileUpload.FileName) == ".pdf")
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(Server.MapPath("~/xml.xml"));
                        using (var rasterizer = new GhostscriptRasterizer())
                        {
                            rasterizer.Open(fileUpload.FileContent, gvi, true);
                            for (int i = 1; i <= 5; i++)
                            {
                                var ocr = new TesseractEngine(Server.MapPath("./tessdata"), "eng");
                                var pdf2jpg = (System.Drawing.Image)rasterizer.GetPage(200, 200, i).Clone();
                                Tesseract.Page page = ocr.Process((Bitmap)pdf2jpg);

                                List<Field> fields = new List<Field>();
                                switch (i)
                                {
                                    case 1:
                                        fields = Constants.fields1;
                                        break;
                                    case 2:
                                        fields = Constants.fields2;
                                        break;
                                    case 3:
                                        fields = Constants.fields3;
                                        break;
                                    case 4:
                                        fields = Constants.fields4;
                                        break;
                                    case 5:
                                        fields = Constants.fields5;
                                        break;
                                }
                                foreach (Field field in fields)
                                {
                                    string txtRetrieved = "";
                                    if (field.isCross)
                                    {
                                        if (field.specialTreatment)
                                            txtRetrieved = !Methods.IsBlank(Methods.CropImage(new Bitmap(pdf2jpg), field.rect.X1, field.rect.Y1, field.rect.Width, field.rect.Height)) ? field.value : "";
                                        else
                                            txtRetrieved = !Methods.IsBlank(Methods.CropImage(new Bitmap(pdf2jpg), field.rect.X1, field.rect.Y1, field.rect.Width, field.rect.Height)) ? "Y" : field.CrossNo ? "N" : "";
                                    }
                                    else
                                    {
                                        page.RegionOfInterest = field.rect;
                                        txtRetrieved = page.GetText();
                                    }
                                    doc.SelectSingleNode(field.xmlField).InnerText = txtRetrieved;
                                }
                            }
                            Response.Clear();
                            Response.AddHeader("Content-Disposition", "attachment;filename=MyXmlDocument.xml");
                            Response.AddHeader("Content-Length", doc.OuterXml.Length.ToString());
                            Response.ContentType = "application/octet-stream";
                            Response.Write(doc.OuterXml);
                            Response.End();

                        }
                    }
                }
            }
            else
            {
                if (page1Upload.HasFile && page2Upload.HasFile && page3Upload.HasFile && page4Upload.HasFile && page5Upload.HasFile)
                {
                    XmlDocument doc = new XmlDocument();
                    List<System.Drawing.Image> pages = new List<System.Drawing.Image>() { ResizeImage(System.Drawing.Image.FromStream(new MemoryStream(page1Upload.FileBytes)),1700,2200),
                                                                                        ResizeImage(System.Drawing.Image.FromStream(new MemoryStream(page2Upload.FileBytes)),1700,2200),
                                                                                        ResizeImage(System.Drawing.Image.FromStream(new MemoryStream(page3Upload.FileBytes)),1700,2200),
                                                                                        ResizeImage(System.Drawing.Image.FromStream(new MemoryStream(page4Upload.FileBytes)),1700,2200),
                                                                                        ResizeImage(System.Drawing.Image.FromStream(new MemoryStream(page5Upload.FileBytes)),1700,2200)};

                    using (var rasterizer = new GhostscriptRasterizer())
                    {
                        doc.Load(Server.MapPath("~/xml.xml"));
                        List<Field> fields = new List<Field>();
                        for (int i = 0; i <= 4; i++)
                        {
                            using (var page = pages[i])
                            {
                            var ocr = new TesseractEngine(Server.MapPath("./tessdata"), "eng");
                            Tesseract.Page pageOCR = ocr.Process((Bitmap)page);

                                switch (i)
                                {
                                    case 0:
                                        fields = Constants.fields1;
                                        break;
                                    case 1:
                                        fields = Constants.fields2;
                                        break;
                                    case 2:
                                        fields = Constants.fields3;
                                        break;
                                    case 3:
                                        fields = Constants.fields4;
                                        break;
                                    case 4:
                                        fields = Constants.fields5;
                                        break;
                                }
                                foreach (Field field in fields)
                                {
                                    string text = "";
                                    if (field.isCross)
                                    {
                                        if (field.specialTreatment)
                                            text = !Methods.IsBlank(Methods.CropImage(new Bitmap(page), field.rect.X1, field.rect.Y1, field.rect.Width, field.rect.Height)) ? field.value : "";
                                        else
                                            text = !Methods.IsBlank(Methods.CropImage(new Bitmap(page), field.rect.X1, field.rect.Y1, field.rect.Width, field.rect.Height)) ? "Y" : field.CrossNo ? "N" : "";
                                    }
                                    else
                                    {
                                        pageOCR.RegionOfInterest = field.rect;
                                        text = pageOCR.GetText();
                                    }
                                    doc.SelectSingleNode(field.xmlField).InnerText = text;
                                }
                            }
                        }
                        MemoryStream stream = new MemoryStream();
                        doc.Save(stream);
                        StreamReader reader = new StreamReader(stream);
                        Response.Clear();
                        Response.AddHeader("Content-Disposition", "attachment;filename=MyXmlDocument.xml");
                        Response.AddHeader("Content-Length", doc.OuterXml.Length.ToString());
                        Response.ContentType = "application/octet-stream";
                        Response.Write(reader.ReadToEnd());
                        Response.End();
                    }
                }
            }


        }

        public static Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }



        protected void checkBoxOCR_CheckedChanged(object sender, EventArgs e)
        {
            fileUploadHolder.Visible = checkBoxOCR.Checked;
            fileUpload.Visible = !checkBoxOCR.Checked;
        }

    }
}