using PdfSharpCore.Pdf;

namespace PDF_TOC.Proccessing.Processors;

public class CompressionProcessor : PdfProcessor
{
    public override void Invoke(PdfDocument document, PdfProccessor processor)
    {
        document.Options.FlateEncodeMode = PdfFlateEncodeMode.BestCompression;
        document.Options.UseFlateDecoderForJpegImages = PdfUseFlateDecoderForJpegImages.Automatic;
        document.Options.NoCompression = false;

        document.Options.CompressContentStreams = true;
    }
}