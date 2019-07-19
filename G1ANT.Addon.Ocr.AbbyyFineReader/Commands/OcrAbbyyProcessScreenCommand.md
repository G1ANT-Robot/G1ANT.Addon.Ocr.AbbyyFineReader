# ocrabbyy.processscreen

## Syntax

```G1ANT
ocrabbyy.processscreen area ⟦rectangle⟧ pages ⟦list⟧ tablescountresult ⟦text⟧ language ⟦text⟧ languageweight ⟦integer⟧ dicionary ⟦list⟧ dictionaryweight ⟦integer⟧
```

## Description

This command processes a part of the screen for further data extraction.

> **Note:** The OCR ABBYY addon is in the beta phase and was not tested with ABBYY FineReader Engine 12.

| Argument | Type | Required | Default Value | Description |
| -------- | ---- | -------- | ------------- | ----------- |
|`area`| [rectangle](https://manual.g1ant.com/link/G1ANT.Language/G1ANT.Language/Structures/RectangleStructure.md) | no | (primary screen resolution) | Area of the screen to be processed specified in [rectangle](https://manual.g1ant.com/link/G1ANT.Language/G1ANT.Language/Structures/RectangleStructure.md) format, eg. `2⫽4⫽12⫽40` |
|`relative`| [bool](https://manual.g1ant.com/link/G1ANT.Language/G1ANT.Language/Structures/BooleanStructure.md) | no | false | If set to true, the area coordinates are relative to the active window |
|`language`| [text](https://manual.g1ant.com/link/G1ANT.Language/G1ANT.Language/Structures/TextStructure.md) | no | English | Language which should be considered during text recognition |
|`languageweight`| [integer](https://manual.g1ant.com/link/G1ANT.Language/G1ANT.Language/Structures/IntegerStructure.md) | no | 100 | Importance of the chosen language (0-100 range) |
|`dictionary`| [list](https://manual.g1ant.com/link/G1ANT.Language/G1ANT.Language/Structures/ListStructure.md) | no | | List of possible keywords existing in the processed document that will have higher priority than random character strings while OCR processing |
|`dictionaryweight`| [integer](https://manual.g1ant.com/link/G1ANT.Language/G1ANT.Language/Structures/IntegerStructure.md) | no | 100 | Importance of words in the chosen dictionary (0-100 range) |
| `result`       | [variable](https://manual.g1ant.com/link/G1ANT.Language/G1ANT.Language/Structures/VariableStructure.md) | no       | `♥result`                                                   | Name of a variable where the command's result (document ID) will be stored |
| `if`           | [bool](https://manual.g1ant.com/link/G1ANT.Language/G1ANT.Language/Structures/BooleanStructure.md) | no       | true                                                        | Executes the command only if a specified condition is true   |
| `timeout`      | [timespan](https://manual.g1ant.com/link/G1ANT.Language/G1ANT.Language/Structures/TimeSpanStructure.md) | no       | [♥timeoutcommand](https://manual.g1ant.com/link/G1ANT.Language/G1ANT.Addon.Core/Variables/TimeoutCommandVariable.md) | Specifies time in milliseconds for G1ANT.Robot to wait for the command to be executed |
| `errorcall`    | [procedure](https://manual.g1ant.com/link/G1ANT.Language/G1ANT.Language/Structures/ProcedureStructure.md) | no       |                                                             | Name of a procedure to call when the command throws an exception or when a given `timeout` expires |
| `errorjump`    | [label](https://manual.g1ant.com/link/G1ANT.Language/G1ANT.Language/Structures/LabelStructure.md) | no       |                                                             | Name of the label to jump to when the command throws an exception or when a given `timeout` expires |
| `errormessage` | [text](https://manual.g1ant.com/link/G1ANT.Language/G1ANT.Language/Structures/TextStructure.md) | no       |                                                             | A message that will be shown in case the command throws an exception or when a given `timeout` expires, and no `errorjump` argument is specified |
| `errorresult`  | [variable](https://manual.g1ant.com/link/G1ANT.Language/G1ANT.Language/Structures/VariableStructure.md) | no       |                                                             | Name of a variable that will store the returned exception. The variable will be of [error](https://manual.g1ant.com/link/G1ANT.Language/G1ANT.Language/Structures/ErrorStructure.md) structure  |

For more information about `if`, `timeout`, `errorcall`, `errorjump`, `errormessage` and `errorresult` arguments, see [Common Arguments](https://manual.g1ant.com/link/G1ANT.Manual/appendices/common-arguments.md) page.

## Example

The script below makes robot open G1ANT website in Chrome, maximize the browser window, capture a specified screen area, process it with OCR and assign the resulting content to the `♥document` variable. Using this variable with the indexes of [abbyydocument](https://manual.g1ant.com/link/G1ANT.Addon.Ocr.AbbyyFineReader/G1ANT.Addon.Ocr.AbbyyFineReader/Structures/AbbyyDocumentStructure.md) structure, the first page of the content is displayed in a dialog box:

```G1ANT
chrome g1ant.com
window ✱G1ANT✱ style maximize
ocrabbyy.processscreen 294⫽207⫽1602⫽798 result ♥document
♥page1 = ♥document⟦0⟧
dialog ♥page1
```
