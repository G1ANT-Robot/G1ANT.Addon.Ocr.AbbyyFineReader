# ocrabbyy.processfile

**Syntax:**

```G1ANT
ocrabbyy.processfile  path ‴‴  pages ‴‴
```

**Description:**

Command `ocrabbyy.processfile` allows to assign project information to a variable in order to extract different types of data from it.

| Argument | Type | Required | Default Value | Description |
| -------- | ---- | -------- | ------------- | ----------- |
|`path`| [string](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/string.md) | yes | | path to a file to be processed |
|`pages`| [list](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/list.md) | yes | 1❚5❚7 | list of numbers of pages to be processed |
|`result`| [variable](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Special-Characters/variable.md) | no | [♥result](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Common-Arguments.md)  | name of a variable (of type AbbyyDocument) where command’s result will be stored  |
|`tablescountresult`| [integer](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/integer.md) | no | | info about the number of tables found in the processed file |
|`language`| [string](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/string.md) | no |  | the language which should be considered  trying to recognise text |
|`languageweight`| [integer](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/integer.md) | no | | importance of the chosen language from 0 to 100 |
|`dictionary`| [list](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/list.md) | no | | list of possible key words that exist in processed document that will have higher priority than random character strings while OCR processing |
|`dictionaryweight`| [integer](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/integer.md) | no | | importance of words in chosen dictionary |
|`if`| [bool](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/bool.md) | no | true | runs the command only if condition is true |
|`timeout`| [variable](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Special-Characters/variable.md) | no | [♥timeoutcommand](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Variables/Special-Variables.md)  | specifies time in milliseconds for G1ANT.Robot to wait for the command to be executed |
|`errorjump` | [label](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/label.md) | no | | name of the label to jump to if given `timeout` expires |
|`errormessage`| [string](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/string.md) | no |  | message that will be shown in case error occurs and no `errorjump` argument is specified |

For more information about `if`, `timeout`, `errorjump` and `errormessage` arguments, please visit [Common Arguments](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Common-Arguments.md)  manual page.

This command is contained in **G1ANT.Addon.Ocr.AbbyyFineReader.dll**.
See: [https://github.com/G1ANT-Robot/G1ANT.Addon.Ocr.AbbyyFineReader](https://github.com/G1ANT-Robot/G1ANT.Addon.Ocr.AbbyyFineReader)

**Example 1:**

Processes given file through Abbyy engine. It is necessary to process file before performing any other action on it.

```G1ANT
ocrabbyy.processfile path ‴♥environment⟦HOMEDRIVE⟧♥environment⟦HOMEPATH⟧\Tests\document5.jpg‴ result ♥file1
```
