using PdfSharpCore.Pdf;

namespace PDF_TOC.Proccessing.Proccessors;

public class ProtectionProccessor : IPdfProccessor
{
    private readonly string userPw, ownerPw;

    public ProtectionProccessor(string userPw, string ownerPw)
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