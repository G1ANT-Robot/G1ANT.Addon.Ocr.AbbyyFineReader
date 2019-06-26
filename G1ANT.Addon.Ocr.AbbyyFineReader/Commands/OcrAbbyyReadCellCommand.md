# ocrabbyy.readcell

**Syntax:**

```G1ANT
ocrabbyy.readcell  documentid ‴‴  tableindex ‴‴  position ‴‴
```

**Description:**

Command `ocrabbyy.readcell` allows to read row column indexed cell from specific table in the document.

| Argument | Type | Required | Default Value | Description |
| -------- | ---- | -------- | ------------- | ----------- |
|`documentid`| [variable](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Special-Characters/variable.md) | yes |  | id of a processed document returned by a call to "ocrabbyy.processfile":{TOPIC-LINK+ocrabby-processfile} command; if not specified, last processed document is used |
|`tableindex`| [integer](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/integer.md) | yes | | index of a table in document |
|`position`| [string](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/string.md) | yes |  | position of the cell in the table in format row, column |
|`offset`| [string](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/string.md) | no |  | offset to be added to get proper value in format row, column |
|`result`| [variable](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Special-Characters/variable.md) | no | [♥result](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Common-Arguments.md)  | name of variable (of type AbbyyDocument) where command’s result will be stored  |
|`if`| [bool](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/bool.md) | no | true | runs the command only if condition is true |
|`timeout`| [variable](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Special-Characters/variable.md) | no | [♥timeoutcommand](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Variables/Special-Variables.md)  | specifies time in milliseconds for G1ANT.Robot to wait for the command to be executed |
|`errorjump` | [label](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/label.md) | no | | name of the label to jump to if given `timeout` expires |
|`errormessage`| [string](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/string.md) | no |  | message that will be shown in case error occurs and no `errorjump` argument is specified |

For more information about `if`, `timeout`, `errorjump` and `errormessage` arguments, please visit [Common Arguments](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Common-Arguments.md)  manual page.

This command is contained in **G1ANT.Addon.Ocr.AbbyyFineReader.dll**.
See: [https://github.com/G1ANT-Robot/G1ANT.Addon.Ocr.AbbyyFineReader](https://github.com/G1ANT-Robot/G1ANT.Addon.Ocr.AbbyyFineReader)

**Example 1:**

In order to use `ocrabbyy.readcell` command, we first need to type `ocrabbyy.processfile` which opens certain file.
Then `ocrabbyy.readcell` allows G1ANT.Robot to read every cell data and store it in a variable using **result** argument or process it further (inject the information in some other program like Excel). In our example we are only using `dialog` command to see the content of chosen cell's position. It will appear as a list separated by: ❚ The **position** argument takes value in form of ‴1,2‴- in this case 1 is the position of rows and 2 is the position of columns.

```G1ANT
ocrabbyy.processfile path ‴♥environment⟦HOMEDRIVE⟧♥environment⟦HOMEPATH⟧\Tests\document6.jpg‴ result ♥file1
ocrabbyy.readcell tableindex 1 result ♥res position ‴1,2‴
dialog ♥res
```

This is the file we are processing, position  ‴1,2‴ marked with red:

This is the result of `ocrabbyy.readcell tableindex 1 result ♥res position ‴1,2‴`

