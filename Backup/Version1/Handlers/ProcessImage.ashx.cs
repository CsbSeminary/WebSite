using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Web;

using Csbs.Web;

using Csbs.Utilities;

namespace Csbs.Services
{
    public class ProcessImage : IHttpHandler
    {
        #region Constants

        private const String ErrorResponseStatusDescription = "404 Not Found";
        private const Int32 ErrorResponseStatusCode = 404;

        #endregion

        #region IHttpHandler definition

        public void ProcessRequest(HttpContext context)
        {
            String fileName = context.Request.QueryString["fn"];
            String type = context.Request.QueryString["tp"];

            if (!String.IsNullOrEmpty(fileName))
            {
                String filePhysPath = context.Server.MapPath(Settings.Services.ProcessImage.ImagesPath + fileName);

                if (File.Exists(filePhysPath))
                {
                    String sourceDirPhysPath = Path.GetDirectoryName(filePhysPath);

                    if (!sourceDirPhysPath.EndsWith("\\"))
                        sourceDirPhysPath += "\\";

                    String destinationDirPhysPath = context.Server.MapPath(Settings.Services.ProcessImage.ProcessedImagesPath);
                    String fileDirName = sourceDirPhysPath.Replace(context.Server.MapPath(Settings.Services.ProcessImage.ImagesPath), String.Empty);

                    if (!String.IsNullOrEmpty(fileDirName))
                        destinationDirPhysPath = Path.Combine(destinationDirPhysPath, fileDirName);

                    String imagePath = GetImagePath(context, filePhysPath, destinationDirPhysPath, type);

                    if (!String.IsNullOrEmpty(imagePath))
                    {
                        context.Response.Clear();
                        context.Response.ContentType = "image/png";
                        context.Response.WriteFile(imagePath);

                        return;
                    }
                }
            }

            context.Response.Status = ErrorResponseStatusDescription;
            context.Response.StatusCode = ErrorResponseStatusCode;
            context.Response.StatusDescription = ErrorResponseStatusDescription;
            context.Response.ContentType = "text/html";
            context.Response.Write(ErrorResponseStatusDescription);
            context.Response.End();
        }

        public Boolean IsReusable
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region Process Image

        private String GetImagePath(HttpContext context, String sourcePhysPath, String destDirPhysPath, String type)
        {
            try
            {
                switch (type)
                {
                    case "reflect":
                        return ReflectImage(context, sourcePhysPath, destDirPhysPath);
                }
            }
            catch
            {

            }

            return null;
        }

        private String ReflectImage(HttpContext context, String sourcePhysPath, String destDirPhysPath)
        {
            String fileExtension = Path.GetExtension(sourcePhysPath);
            String fileName = Path.GetFileNameWithoutExtension(sourcePhysPath) + "_reflected.png";
            String procPhysicalPath = Path.Combine(destDirPhysPath, fileName);

            if (!File.Exists(procPhysicalPath))
            {
                if (!Directory.Exists(destDirPhysPath))
                    Directory.CreateDirectory(destDirPhysPath);

                using (Image img = Image.FromFile(sourcePhysPath))
                {
                    Int32 height = img.Height > 3 ? img.Height / 3 - 1 : 1;

                    using (Bitmap bitmap = new Bitmap(img.Width, height))
                    {
                        using (Graphics graphics = Graphics.FromImage(bitmap))
                        {
                            using (Matrix matrix = new Matrix(1, 0, 0, -1, 0, img.Height - 1))
                            {
                                graphics.Transform = matrix;

                                using (LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, img.Height - bitmap.Height, bitmap.Width, bitmap.Height + 1), Color.White, Color.Transparent, LinearGradientMode.Vertical))
                                {
                                    brush.SetSigmaBellShape(1.0f, 0.45f);

                                    graphics.DrawImage(img, new Point(0, 0));
                                    graphics.FillRectangle(brush, new Rectangle(0, img.Height - bitmap.Height - 1, img.Width, img.Height));

                                    bitmap.Save(procPhysicalPath, System.Drawing.Imaging.ImageFormat.Png);
                                }
                            }
                        }
                    }
                }
            }

            return procPhysicalPath;
        }

        #endregion
    }
}