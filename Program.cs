using PDF_TOC;
using PDF_TOC.Proccessing;

var tree = HyperlambdaEvaluator.Evaluate(@"include-document: g.pdf
metadata:
   author: Chris
   title: GK-Test
compress
page-numbers: X von Y
table-of-contents: Inhaltsverzeichnis
   Demografischer Wandel: 1
   Familie: 8
   Konfliktlösung im Berufsleben: 16");

var processor = PdfProccessor.FromNode(tree);

processor.Invoke();