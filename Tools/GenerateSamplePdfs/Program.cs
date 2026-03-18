using IronPdf;

// ── Configuration ──────────────────────────────────────────────────────────
// Output directories relative to the main demo project
var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".."));
var examplesDir = Path.Combine(projectRoot, "IronOCR-Demos", "Demos", "LanguagesGuide", "Examples");
var expectedDir = Path.Combine(projectRoot, "IronOCR-Demos", "Demos", "LanguagesGuide", "ExpectedText");

Directory.CreateDirectory(examplesDir);
Directory.CreateDirectory(expectedDir);

Console.WriteLine($"Generating sample PDFs to: {examplesDir}");
Console.WriteLine($"Generating expected text to: {expectedDir}\n");

// ── Sample Text Content ────────────────────────────────────────────────────
var samples = new Dictionary<string, (string html, string expectedText)>
{
    ["english"] = (
        html: @"
        <html><body style='font-family: Arial, sans-serif; font-size: 14px; padding: 40px;'>
        <h2>Business Correspondence</h2>
        <p>Dear Mr. Thompson,</p>
        <p>Thank you for your inquiry regarding our enterprise software solutions.
        We are pleased to present our comprehensive OCR platform, which has been designed
        to meet the demanding requirements of modern document processing workflows.</p>
        <p>Our solution supports over 125 languages and delivers industry-leading accuracy
        across a wide range of document types, including invoices, contracts, and technical
        specifications.</p>
        <p>We look forward to discussing how our technology can streamline your operations.</p>
        <p>Best regards,<br/>Sarah Mitchell<br/>Director of Sales Engineering</p>
        </body></html>",
        expectedText: @"Business Correspondence
Dear Mr. Thompson,
Thank you for your inquiry regarding our enterprise software solutions. We are pleased to present our comprehensive OCR platform, which has been designed to meet the demanding requirements of modern document processing workflows.
Our solution supports over 125 languages and delivers industry-leading accuracy across a wide range of document types, including invoices, contracts, and technical specifications.
We look forward to discussing how our technology can streamline your operations.
Best regards,
Sarah Mitchell
Director of Sales Engineering"
    ),

    ["spanish"] = (
        html: @"
        <html><body style='font-family: Arial, sans-serif; font-size: 14px; padding: 40px;'>
        <h2>Descripción del Producto</h2>
        <p>Nuestra plataforma de reconocimiento óptico de caracteres ofrece una solución
        completa para el procesamiento de documentos empresariales. Con soporte para más
        de 125 idiomas, nuestra tecnología garantiza una precisión excepcional en la
        extracción de texto.</p>
        <p>Las características principales incluyen procesamiento por lotes, detección
        automática de idioma y generación de documentos PDF con capacidad de búsqueda.
        Nuestros clientes han reportado una mejora significativa en la eficiencia de sus
        flujos de trabajo documentales.</p>
        <p>Para obtener más información, póngase en contacto con nuestro equipo de ventas.</p>
        </body></html>",
        expectedText: @"Descripción del Producto
Nuestra plataforma de reconocimiento óptico de caracteres ofrece una solución completa para el procesamiento de documentos empresariales. Con soporte para más de 125 idiomas, nuestra tecnología garantiza una precisión excepcional en la extracción de texto.
Las características principales incluyen procesamiento por lotes, detección automática de idioma y generación de documentos PDF con capacidad de búsqueda. Nuestros clientes han reportado una mejora significativa en la eficiencia de sus flujos de trabajo documentales.
Para obtener más información, póngase en contacto con nuestro equipo de ventas."
    ),

    ["chinese"] = (
        html: @"
        <html><body style='font-family: SimSun, Microsoft YaHei, sans-serif; font-size: 14px; padding: 40px;'>
        <h2>商务文件</h2>
        <p>我们的光学字符识别平台为企业文档处理提供了全面的解决方案。该技术支持超过一百二十五种语言，
        确保在文本提取方面具有卓越的准确性。</p>
        <p>主要功能包括批量处理、自动语言检测以及可搜索的文档生成。我们的客户报告称其文档处理
        工作流程的效率有了显著提高。</p>
        <p>如需了解更多信息，请联系我们的销售团队。</p>
        </body></html>",
        expectedText: @"商务文件
我们的光学字符识别平台为企业文档处理提供了全面的解决方案。该技术支持超过一百二十五种语言， 确保在文本提取方面具有卓越的准确性。
主要功能包括批量处理、自动语言检测以及可搜索的文档生成。我们的客户报告称其文档处理 工作流程的效率有了显著提高。
如需了解更多信息，请联系我们的销售团队。"
    ),

    ["russian"] = (
        html: @"
        <html><body style='font-family: Arial, sans-serif; font-size: 14px; padding: 40px;'>
        <h2>Техническая спецификация</h2>
        <p>Наша платформа оптического распознавания символов предлагает комплексное решение
        для обработки корпоративных документов. Технология поддерживает более ста двадцати
        пяти языков и обеспечивает исключительную точность при извлечении текста.</p>
        <p>Основные функции включают пакетную обработку, автоматическое определение языка
        и создание документов с возможностью поиска. Наши клиенты сообщают о значительном
        повышении эффективности рабочих процессов обработки документов.</p>
        <p>Для получения дополнительной информации свяжитесь с нашей командой продаж.</p>
        </body></html>",
        expectedText: @"Техническая спецификация
Наша платформа оптического распознавания символов предлагает комплексное решение для обработки корпоративных документов. Технология поддерживает более ста двадцати пяти языков и обеспечивает исключительную точность при извлечении текста.
Основные функции включают пакетную обработку, автоматическое определение языка и создание документов с возможностью поиска. Наши клиенты сообщают о значительном повышении эффективности рабочих процессов обработки документов.
Для получения дополнительной информации свяжитесь с нашей командой продаж."
    ),

    ["multi-language"] = (
        html: @"
        <html><body style='font-family: Arial, SimSun, sans-serif; font-size: 14px; padding: 40px;'>
        <h2>Multi-Language Document Sample</h2>

        <h3>English</h3>
        <p>Our optical character recognition platform delivers industry-leading accuracy
        across multiple languages and document types.</p>

        <h3>Español</h3>
        <p>Nuestra plataforma de reconocimiento óptico de caracteres ofrece una precisión
        líder en la industria en múltiples idiomas y tipos de documentos.</p>

        <h3>中文</h3>
        <p>我们的光学字符识别平台在多种语言和文档类型中提供行业领先的准确性。</p>

        <h3>Русский</h3>
        <p>Наша платформа оптического распознавания символов обеспечивает лидирующую
        в отрасли точность на нескольких языках и типах документов.</p>
        </body></html>",
        expectedText: @"Multi-Language Document Sample
English
Our optical character recognition platform delivers industry-leading accuracy across multiple languages and document types.
Español
Nuestra plataforma de reconocimiento óptico de caracteres ofrece una precisión líder en la industria en múltiples idiomas y tipos de documentos.
中文
我们的光学字符识别平台在多种语言和文档类型中提供行业领先的准确性。
Русский
Наша платформа оптического распознавания символов обеспечивает лидирующую в отрасли точность на нескольких языках и типах документов."
    )
};

// ── Generate PDFs ──────────────────────────────────────────────────────────
var renderer = new ChromePdfRenderer();
renderer.RenderingOptions.MarginTop = 20;
renderer.RenderingOptions.MarginBottom = 20;

foreach (var (name, (html, expectedText)) in samples)
{
    Console.Write($"  Generating {name}-sample.pdf... ");
    var pdf = renderer.RenderHtmlAsPdf(html);
    var pdfPath = Path.Combine(examplesDir, $"{name}-sample.pdf");
    pdf.SaveAs(pdfPath);
    Console.WriteLine("OK");

    var txtPath = Path.Combine(expectedDir, $"{name}-expected.txt");
    File.WriteAllText(txtPath, expectedText.Trim());
}

Console.WriteLine("\nDone! All sample PDFs and expected text files generated.");
