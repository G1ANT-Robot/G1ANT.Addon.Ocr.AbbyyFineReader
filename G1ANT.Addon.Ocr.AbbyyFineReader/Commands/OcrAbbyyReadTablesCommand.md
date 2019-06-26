# ocrabbyy.readtables

**Syntax:**

```G1ANT
ocrabbyy.readtables 
```

**Description:**

Command `ocrabbyy.readtables` allows to read the content of all tables existing in a document and process it as a list.

| Argument | Type | Required | Default Value | Description |
| -------- | ---- | -------- | ------------- | ----------- |
|`documentid`| [integer](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/integer.md) | no |  | id of a processed document returned by a call to "ocrabbyy.processfile":{TOPIC-LINK+ocrabby-processfile} command, if not specified, the last processed document is used |
|`result`| [variable](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Special-Characters/variable.md) | no |  [♥result](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Common-Arguments.md)  | name of a variable (of type AbbyyDocument) where command’s result will be stored  |
|`if`| [bool](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/bool.md) | no | true | runs the command only if condition is true |
|`timeout`| [variable](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Special-Characters/variable.md) | no | [♥timeoutcommand](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Variables/Special-Variables.md)  | specifies time in milliseconds for G1ANT.Robot to wait for the command to be executed |
|`errorjump` | [label](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/label.md) | no | | name of the label to jump to if given `timeout` expires |
|`errormessage`| [string](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/string.md) | no |  | message that will be shown in case error occurs and no `errorjump` argument is specified |

For more information about `if`, `timeout`, `errorjump` and `errormessage` arguments, please visit [Common Arguments](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Common-Arguments.md)  manual page.

This command is contained in **G1ANT.Addon.Ocr.AbbyyFineReader.dll**.
See: [https://github.com/G1ANT-Robot/G1ANT.Addon.Ocr.AbbyyFineReader](https://github.com/G1ANT-Robot/G1ANT.Addon.Ocr.AbbyyFineReader)

**Example 1:**

In order to use `ocrabbyy.readtables` command, we first need to type `ocrabbyy.processfile` which opens certain file.
Then `ocrabbyy.readtables` allows G1ANT.Robot to read every table data and store it in a variable using **result** argument or process it further (inject the information in some other program like Excel).
It is not necessary to specify **documentid**, if not given, G1ANT.Robot will perform tasks on recently processed file. In our example we are only using `dialog` command to see the content of the table. It will appear as a list separated by: ❚

```G1ANT
ocrabbyy.processfile path ‴♥environment⟦HOMEDRIVE⟧♥environment⟦HOMEPATH⟧\Tests\document6.jpg‴ result ♥file1
ocrabbyy.readtables result ♥tables
dialog ♥tables
```

