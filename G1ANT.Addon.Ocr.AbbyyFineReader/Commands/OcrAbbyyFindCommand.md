# ocrabbyy.find

## Syntax

```G1ANT
ocrabbyy.find search ⟦text⟧ documentid ⟦integer⟧
```

## Description

This command finds a specified text in a given document and returns a list of matches’ positions in a [rectangle](https://manual.g1ant.com/link/G1ANT.Language/G1ANT.Language/Structures/RectangleStructure.md) format. If the text is not found, the result will be `-1⫽-1⫽-2⫽-2`.

> **Note:** The OCR ABBYY addon is in the beta phase and was not tested with ABBYY FineReader Engine 12.

| Argument | Type | Required | Default Value | Description |
| -------- | ---- | -------- | ------------- | ----------- |
|`search`| [text](https://manual.g1ant.com/link/G1ANT.Language/G1ANT.Language/Structures/TextStructure.md) | yes |  | Text to be found in a document |
|`documentid`| [integer](https://manual.g1ant.com/link/G1ANT.Language/G1ANT.Language/Structures/IntegerStructure.md) | yes |  | ID of a processed document. If not specified, the last processed document is used |
| `result`       | [variable](https://manual.g1ant.com/link/G1ANT.Language/G1ANT.Language/Structures/VariableStructure.md) | no       | `♥result`                                                   | Name of a variable where the command's result (a list of [rectangle](https://manual.g1ant.com/link/G1ANT.Language/G1ANT.Language/Structures/RectangleStructure.md) elements) will be stored |
| `if`           | [bool](https://manual.g1ant.com/link/G1ANT.Language/G1ANT.Language/Structures/BooleanStructure.md) | no       | true                                                        | Executes the command only if a specified condition is true   |
| `timeout`      | [timespan](https://manual.g1ant.com/link/G1ANT.Language/G1ANT.Language/Structures/TimeSpanStructure.md) | no       | [♥timeoutcommand](https://manual.g1ant.com/link/G1ANT.Language/G1ANT.Addon.Core/Variables/TimeoutCommandVariable.md) | Specifies time in milliseconds for G1ANT.Robot to wait for the command to be executed |
| `errorcall`    | [procedure](https://manual.g1ant.com/link/G1ANT.Language/G1ANT.Language/Structures/ProcedureStructure.md) | no       |                                                             | Name of a procedure to call when the command throws an exception or when a given `timeout` expires |
| `errorjump`    | [label](https://manual.g1ant.com/link/G1ANT.Language/G1ANT.Language/Structures/LabelStructure.md) | no       |                                                             | Name of the label to jump to when the command throws an exception or when a given `timeout` expires |
| `errormessage` | [text](https://manual.g1ant.com/link/G1ANT.Language/G1ANT.Language/Structures/TextStructure.md) | no       |                                                             | A message that will be shown in case the command throws an exception or when a given `timeout` expires, and no `errorjump` argument is specified |
| `errorresult`  | [variable](https://manual.g1ant.com/link/G1ANT.Language/G1ANT.Language/Structures/VariableStructure.md) | no       |                                                             | Name of a variable that will store the returned exception. The variable will be of [error](https://manual.g1ant.com/link/G1ANT.Language/G1ANT.Language/Structures/ErrorStructure.md) structure  |

For more information about `if`, `timeout`, `errorcall`, `errorjump`, `errormessage` and `errorresult` arguments, see [Common Arguments](https://manual.g1ant.com/link/G1ANT.Manual/appendices/common-arguments.md) page.

## Example

In order to use the `ocrabbyy.find` command, it’s necessary to process the file first with the [`ocrabbyy.processfile`](https://manual.g1ant.com/link/G1ANT.Addon.Ocr.AbbyyFineReader/G1ANT.Addon.Ocr.AbbyyFineReader/Commands/OcrAbbyyProcessFileCommand.md) command. In the example below, text from a sample file located on user’s Desktop is opened and searched for a word “*robots*”. The result — a list of matches (their rectangles’ coordinates) — is then displayed in a dialog box:

```G1ANT
ocrabbyy.processfile ♥environment⟦USERPROFILE⟧\Desktop\document5.jpg result ♥file
ocrabbyy.find robots documentid ♥file
dialog ♥result
```
