using PDF_TOC.Proccessing;

var processor = new PdfProccessor();

processor.Proccessors.Add(new DocumentInclude("g.pdf"));
processor.Proccessors.Add(new MetadataProccessor("Chris", "GK - Test"));

var toc = new TocItems
{
    new("Demografischer Wandel", 2),
    new("Familie", 9),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),new("Demografischer Wandel", 2),
    new("Familie", 9),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Konfliktlösung im Berufsleben", 17),
    new("Mediennutzung", 19)
};

processor.Proccessors.Add(new ToCProccessor(toc));

processor.Proccessors.Add(new PageNumberRenderer(PdfPagePosition.BottomRight, "X von Y"));

processor.Invoke();