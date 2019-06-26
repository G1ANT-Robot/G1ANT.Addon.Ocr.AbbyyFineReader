# ocrabbyy.gettableposition

**Syntax:**

```G1ANT
ocrabbyy.gettableposition  search ‴‴  documentid ‴‴  tableindex ‴‴
```

**Description:**

Command `ocrabbyy.gettableposition ` allows to find indexes.

| Argument | Type | Required | Default Value | Description |
| -------- | ---- | -------- | ------------- | ----------- |
|`search`| [string](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/string.md) | yes |  | text that you want to find in the screen |
|`documentid`| [integer](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/integer.md) | yes |  | Id of a processed document, if not specified last processed document is used |
|`tableindex`| [integer](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/integer.md) | yes | | index of a table in document |
|`result`| [variable](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Special-Characters/variable.md) | no | [♥result](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Common-Arguments.md)  | name of variable (of type AbbyyDocument) where command’s result will be stored  |
|`if`| [bool](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/bool.md) | no | true | runs the command only if condition is true |
|`timeout`| [variable](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Special-Characters/variable.md) | no | [♥timeoutcommand](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Variables/Special-Variables.md)  | specifies time in milliseconds for G1ANT.Robot to wait for the command to be executed |
|`errorjump` | [label](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/label.md) | no | | name of the label to jump to if given `timeout` expires |
|`errormessage`| [string](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/string.md) | no |  | message that will be shown in case error occurs and no `errorjump` argument is specified |

For more information about `if`, `timeout`, `errorjump` and `errormessage` arguments, please visit [Common Arguments](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Common-Arguments.md)  manual page.

This command is contained in **G1ANT.Addon.Ocr.AbbyyFineReader.dll**.
See: [https://github.com/G1ANT-Robot/G1ANT.Addon.Ocr.AbbyyFineReader](https://github.com/G1ANT-Robot/G1ANT.Addon.Ocr.AbbyyFineReader)

**Example 1:**

Opening file we want to read (process) data from:

```G1ANT
ocrabbyy.processfile path ‴♥environment⟦HOMEDRIVE⟧♥environment⟦HOMEPATH⟧\Tests\document6.jpg‴ result ♥file1
```

OCR will now get position of a word Sergio in file with **documentID** argument and value set to **♥file1** (file we have processed in previous line):

```G1ANT
ocrabbyy.gettableposition search documentid ♥file1 ‴Sergio‴ tableindex 0 result ♥position
```

Displaying result:

```G1ANT
dialog ♥position
```

