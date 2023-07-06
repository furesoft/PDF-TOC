using PdfSharpCore.Pdf;

namespace PDF_TOC.Proccessing.Processors;

public class ProtectionProcessor : IPdfProcessor
{
    private readonly string userPw, ownerPw;

    public ProtectionProcessor(string userPw, string ownerPw)
    {
        this.userPw = userPw;
        this.ownerPw = ownerPw;
    }

    public void Invoke(PdfDocument document, PdfProccessor processor)
    {
        document.SecuritySettings.UserPassword = userPw;
        document.SecuritySettings.OwnerPassword = ownerPw;
    }
}