using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Agents.AI;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace ClientManagement.DataAccessLayer.Helpers;

/// <summary>
/// Tooling helpers exposed to the agent framework via <see cref="AIFunctionFactory"/>.
/// Each method streams the document as <see cref="IAsyncEnumerable{T}"/> of structurally
/// meaningful chunks — paragraphs, sentences, and bullet points — so callers can feed
/// agents and embedding pipelines without imposing a fixed character budget.
///
/// Note: chunks are sized by document structure, not characters. A bullet point can be
/// 3 words; a paragraph can be 600 chars. If a downstream model requires a strict token
/// cap, wrap the output with a length-bounded splitter.
/// </summary>
public static class DocumentToolKit
{
    // Sentence terminators: ".", "!", "?" followed by whitespace (or EOL).
    // We keep the delimiter attached to the sentence so the original punctuation survives.
    private static readonly Regex SentenceSplitter = new(
        @"(?<=[.!?])\s+(?=[A-Z(\[""'""«»\u2014])",
        RegexOptions.Compiled);

    // Lines that look like bullet points: leading whitespace, then a bullet glyph or
    // a hyphen / asterisk followed by a space.
    private static readonly Regex BulletLine = new(
        @"^\s*[•●◦▪▫–—\-\*\u2022\u2023\u2043\u204C\u204D]\s+",
        RegexOptions.Compiled);

    /// <summary>
    /// Extracts text from a PDF file and yields structurally meaningful chunks:
    /// bullet-point lines, paragraphs, and (inside paragraphs) sentences.
    /// </summary>
    /// <param name="filePath">Absolute or working-directory-relative path to the PDF.</param>
    /// <param name="cancellationToken">Token used to cancel the streaming operation.</param>
    
    public static async IAsyncEnumerable<string> ReadPdf(
        byte[] pdfContents,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await Task.Yield();

        using var document = PdfDocument.Open(pdfContents);
        foreach (var page in document.GetPages())
        {
            cancellationToken.ThrowIfCancellationRequested();

            var pageText = page.Text ?? string.Empty;
            foreach (var chunk in ChunkFreeText(pageText))
            {
                cancellationToken.ThrowIfCancellationRequested();
                yield return chunk;
                await Task.Yield();
            }
        }
    }

    [Description("Breaks down a document into smaller chunks")]
    public static async IAsyncEnumerable<string> DocumentClipper([Description("Path to the document that will be broken down to smaller chunks")] string filePath, [Description("number of chunks that the document will be broken down to.")] int n)
    {

       

        using var stream = File.OpenRead(filePath);
        var fileSize = stream.Length;
        var batchSize = fileSize % n > 0 ? (fileSize / n) + 1 : fileSize / n;
        List<byte[]> fileChunks = new();
        int index = 0;
        while (index < n)
        {
            var chunkCollection = new byte[batchSize];
            stream.Seek(index * (int)batchSize, SeekOrigin.Begin);
            if (index < n - 1)
            {
                await stream.ReadExactlyAsync(chunkCollection, 0, chunkCollection.Length);

            }
            else
            {
                var remainder = fileSize & n;
                var ratio = remainder / (float)n;
                var itemsInLastCollection = Math.Ceiling(ratio * batchSize);

                //int sizeOfLastChunk = Math.Ceiling(batchSize*(batchSize/()))
                await stream.ReadAsync(chunkCollection, 0, chunkCollection.Length);
            }
            fileChunks.Add(chunkCollection);
            index += 1;
        }

        FileInfo fInfo = new FileInfo(filePath);
        int chunkIndex = 0;
        foreach (var chunk in fileChunks)
        {
            var chunkName = new Regex(@".+(?=\.)").Match(fInfo.Name).Value;
            var chunkPath = Path.Combine(AppContext.BaseDirectory, $"{chunkName}_{(chunkIndex + 1):D5}{fInfo.Extension}");
            using var chunkStream = File.OpenWrite(chunkPath);
            await chunkStream.WriteAsync(chunk, 0, chunk.Length);
            yield return $"chunk copied to ${chunkPath}\n";
            chunkIndex += 1;
        }

        yield return "chunking process completed!";
    }

    public static IEnumerable<(IEnumerable<IPdfImage> images, string contexts)> ExtractPdfImage(byte[] pdfContents)
    {
        using var document = PdfDocument.Open(pdfContents);
        foreach (var page in document.GetPages())
        {
            var pageImages = page.GetImages();
            if (!pageImages.Any())
            {
                continue;
            }
            yield return (pageImages, page.Text);
        }
    }

    /// <summary>
    /// Extracts text from a Word (.docx) document and yields structurally meaningful chunks.
    /// List items (bulleted or numbered) become one chunk per item; other paragraphs become
    /// one chunk per paragraph or sentence depending on length.
    /// </summary>
    /// <param name="filePath">Absolute or working-directory-relative path to the .docx file.</param>
    /// <param name="cancellationToken">Token used to cancel the streaming operation.</param>
    public static async IAsyncEnumerable<string> ReadWordDocument(
        string filePath,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path must be provided.", nameof(filePath));

        if (!File.Exists(filePath))
            throw new FileNotFoundException("Word document not found.", filePath);

        await Task.Yield();

        using var document = WordprocessingDocument.Open(filePath, false);
        var body = document.MainDocumentPart?.Document?.Body
                   ?? throw new InvalidOperationException("The Word document has no readable body.");

        foreach (var paragraph in body.Elements<Paragraph>())
        {
            cancellationToken.ThrowIfCancellationRequested();

            var text = paragraph.InnerText;
            if (string.IsNullOrWhiteSpace(text))
                continue;

            // Numbering definitions are how Word marks bullet/numbered lists at the XML level;
            // checking paragraph properties is more reliable than scraping bullet glyphs from text.
            var isListItem = paragraph.ParagraphProperties?.NumberingProperties?.NumberingLevelReference is not null;

            if (isListItem || BulletLine.IsMatch(text))
            {
                yield return text.Trim();
            }
            else
            {
                foreach (var sentence in SplitParagraphIntoSentences(text))
                {
                    yield return sentence;
                }
            }

            await Task.Yield();
        }
    }

    /// <summary>
    /// Splits a flat block of extracted PDF text into structural chunks:
    /// bullet lines first, then paragraphs, then sentences inside paragraphs.
    /// </summary>
    private static IEnumerable<string> ChunkFreeText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            yield break;

        // Normalise CRLF/CR to LF so the line-based splitting behaves consistently.
        var normalised = text.Replace("\r\n", "\n").Replace('\r', '\n');

        foreach (var rawLine in normalised.Split('\n'))
        {
            var line = rawLine.TrimEnd();
            if (line.Length == 0)
                continue;

            if (BulletLine.IsMatch(line))
            {
                yield return line.Trim();
                continue;
            }

            foreach (var sentence in SplitParagraphIntoSentences(line))
                yield return sentence;
        }
    }

    /// <summary>
    /// Splits a paragraph into sentence-sized chunks. Short paragraphs are returned as a
    /// single chunk; longer ones are split on sentence terminators. Empty entries are skipped.
    /// </summary>
    private static IEnumerable<string> SplitParagraphIntoSentences(string paragraph)
    {
        var trimmed = paragraph.Trim();
        if (trimmed.Length == 0)
            yield break;

        // Very short paragraphs almost certainly aren't multi-sentence; keep them whole
        // to avoid fragmenting bullet-style or heading content.
        if (trimmed.Length < 80)
        {
            yield return trimmed;
            yield break;
        }

        var parts = SentenceSplitter.Split(trimmed);
        foreach (var part in parts)
        {
            var clean = part.Trim();
            if (clean.Length > 0)
                yield return clean;
        }
    }
}
