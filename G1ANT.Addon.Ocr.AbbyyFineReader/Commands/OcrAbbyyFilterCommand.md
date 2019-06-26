# ocrabbyy.filter

**Syntax:**

```G1ANT
ocrabbyy.filter  documentid ‴‴ filter ‴‴
```

**Description:**

Command `ocrabbyy.filter` allows to filter text from a document by font style. Required arguments: documentid, filter.

| Argument | Type | Required | Default Value | Description |
| -------- | ---- | -------- | ------------- | ----------- |
|`documentid`| [variable](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Special-Characters/variable.md) | yes |  | id of a processed document returned by a call to `processfile` command. If not specified, last processed document is used. |
|`filter`| [string](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/string.md) | yes |  | flags of filter to apply, separated by ❚, could be: italic, bold |
|`result`| [variable](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Special-Characters/variable.md) | no | [♥result](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Common-Arguments.md)  | name of variable (of type AbbyyDocument) where command’s result will be stored  |
|`if`| [bool](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/bool.md) | no | true | runs the command only if condition is true |
|`timeout`| [variable](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Special-Characters/variable.md) | no | [♥timeoutcommand](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Variables/Special-Variables.md)  | specifies time in milliseconds for G1ANT.Robot to wait for the command to be executed |
|`errorjump` | [label](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/label.md) | no | | name of the label to jump to if given `timeout` expires |
|`errormessage`| [string](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/string.md) | no |  | message that will be shown in case error occurs and no `errorjump` argument is specified |

For more information about `if`, `timeout`, `errorjump` and `errormessage` arguments, please visit [Common Arguments](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Common-Arguments.md)  manual page.

This command is contained in **G1ANT.Addon.Ocr.AbbyyFineReader.dll**.
See: [https://github.com/G1ANT-Robot/G1ANT.Addon.Ocr.AbbyyFineReader](https://github.com/G1ANT-Robot/G1ANT.Addon.Ocr.AbbyyFineReader)

**Example 1:**

In order to use `ocrabbyy.filter` command, we first need to process the file using `ocrabbyy.processfile` command. In the example below, we are filtering some text by font style. As a result, the dialog window will show a list of words which are bold in chosen text.

```G1ANT
ocrabbyy.processfile path ‴♥environment⟦HOMEDRIVE⟧♥environment⟦HOMEPATH⟧\Tests\document2.jpg‴ result ♥file1
ocrabbyy.filter documentid ♥file1 filter ‴bold‴ result ♥documentFiltered
dialog ♥documentFiltered
```

