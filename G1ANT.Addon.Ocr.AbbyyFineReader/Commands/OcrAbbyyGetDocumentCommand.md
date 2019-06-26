# ocrabbyy.getdocument

**Syntax:**

```G1ANT
ocrabbyy.getdocument
```

**Description:**

Command `ocrabbyy.getdocument` allows to assign project information to a variable in order to extract different types of data from it.

| Argument | Type | Required | Default Value | Description |
| -------- | ---- | -------- | ------------- | ----------- |
|`documentid`| [variable](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Special-Characters/variable.md) | no |  | Id of a processed document returned by a call to `processfile` command. If not specified, last processed document is used. |
|`result`| [variable](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Special-Characters/variable.md) | no |  [♥result](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Common-Arguments.md)   | name of variable (of type AbbyyDocument) where command’s result will be stored  |
|`if`| [bool](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/bool.md) | no | true | runs the command only if condition is true |
|`timeout`| [variable](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Special-Characters/variable.md) | no | [♥timeoutcommand](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Variables/Special-Variables.md)  | specifies time in milliseconds for G1ANT.Robot to wait for the command to be executed |
|`errorjump` | [label](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/label.md) | no | | name of the label to jump to if given `timeout` expires |
|`errormessage`| [string](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/string.md) | no |  | message that will be shown in case error occurs and no `errorjump` argument is specified |

For more information about `if`, `timeout`, `errorjump` and `errormessage` arguments, please visit [Common Arguments](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Common-Arguments.md)  manual page.

This command is contained in **G1ANT.Addon.Ocr.AbbyyFineReader.dll**.
See: [https://github.com/G1ANT-Robot/G1ANT.Addon.Ocr.AbbyyFineReader](https://github.com/G1ANT-Robot/G1ANT.Addon.Ocr.AbbyyFineReader)

**Example 1:**

In order to use `ocrabbyy.getdocument` command, we first need to type `ocrabbyy.processfile` which opens certain file. It is important to specify **result** argument and assign a variable to it, because we later have to use this variable while specifying **documentid** for `ocrabbyy.getdocument` command.
**documentid** argument needs to take **file1** as input, and then we also need to specify **result** argument to later extract data from it.
Please check "ocrabby commands":{TOPIC-LINK+ocrabby-commands} in order to see how to extract types of data.

```G1ANT
ocrabbyy.processfile path ‴♥environment⟦HOMEDRIVE⟧♥environment⟦HOMEPATH⟧\Tests\document6.jpg‴ result ♥file1
ocrabbyy.getdocument documentid ♥file1 result ♥document (main strucutre)
♥pagesCount = ♥document⟦count⟧
♥firstPage = ♥document⟦0⟧
♥rowsCount = ♥firstpage⟦count⟧
dialog message ‴Number of pages: ♥pagesCount Number of rows on a page: ♥rowsCount‴
```

