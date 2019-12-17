# abbyydocument

This structure stores documents processed with Abbyy FineReader OCR engine and has two fields:

| Field   | Type    | Description                                                  |
| ------- | ------- | ------------------------------------------------------------ |
| `pages` | list    | List of all pages in a document; each list element is of [abbyypage](https://manual.g1ant.com/link/G1ANT.Addon.Ocr.AbbyyFineReader/G1ANT.Addon.Ocr.AbbyyFineReader/Structures/AbbyyPageStructure.md) structure |
| `count` | integer | Total number of pages in a document                          |

## Example
This example shows how to process a sample image file on user’s desktop with the `ocrabbyy.processfile` and `ocrabbyy.getdocument` commands, and then display the first page of an OCR-ed document along with the total number of its pages using the `abbyydocument` structure indexes:

```G1ANT
ocrabbyy.processfile ♥environment⟦USERPROFILE⟧\Desktop\document.jpg result ♥fileId
ocrabbyy.getdocument ♥fileId result ♥document
♥pagesCount = ♥document⟦count⟧
♥firstPage = ♥document⟦0⟧
dialog ‴Page 1 of ♥pagesCount: ♥firstPage‴
```

