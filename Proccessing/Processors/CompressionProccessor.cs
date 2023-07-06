using PdfSharpCore.Pdf;

namespace PDF_TOC.Proccessing.Processors;

public class CompressionProccessor : IPdfProccessor
{
    public void Invoke(PdfDocument document, PdfProccessor processor)
    {
        document.Options.FlateEncodeMode = PdfFlateEncodeMode.BestCompression;
        document.Options.UseFlateDecoderForJpegImages = PdfUseFlateDecoderForJpegImages.Automatic;
        document.Options.NoCompression = false;

        document.Options.CompressContentStreams = true;
    }
}