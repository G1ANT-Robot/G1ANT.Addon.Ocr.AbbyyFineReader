# ocrabbyy.fromscreen

**Syntax:**

```G1ANT
ocrabbyy.fromscreen  area ‴x0⫽y0⫽x1⫽y1‴
```

**Description:**

Command `ocrabbyy.fromscreen` captures part of the screen and recognises text from it.

| Argument | Type | Required | Default Value | Description |
| -------- | ---- | -------- | ------------- | ----------- |
|`area`| "area":{TOPIC-LINK+rectangle} | yes |  | specifies screen area to be captured in format x0⫽y0⫽x1⫽y1 (x0,y0 – coordinates of a top left corner; x1,y1 – coordinates of a right bottom corner of the area) |
|`relative`| [bool](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/bool.md)  | no | true | runs the command only if condition is true |
|`language`| [bool](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/bool.md)  | no | true | the language which should be considered trying to recognize text |
|`result`| [variable](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Special-Characters/variable.md)  | no | [♥result](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Common-Arguments.md)  | name of variable (of type AbbyyDocument) where command’s result will be stored  |
|`if`| [bool](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/bool.md)  | no | true | runs the command only if condition is true |
|`timeout`| [variable](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Special-Characters/variable.md)  | no | [♥timeoutcommand](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Variables/Special-Variables.md)  | specifies time in milliseconds for G1ANT.Robot to wait for the command to be executed |
|`errorjump` | [label](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/label.md)  | no | | name of the label to jump to if given `timeout` expires |
|`errormessage`| [string](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Structures/string.md)  | no |  | message that will be shown in case error occurs and no `errorjump` argument is specified |

For more information about `if`, `timeout`, `errorjump` and `errormessage` arguments, please visit [Common Arguments](https://github.com/G1ANT-Robot/G1ANT.Manual/blob/master/G1ANT-Language/Common-Arguments.md)  manual page.

This command is contained in **G1ANT.Addon.Ocr.AbbyyFineReader.dll**.
See: [https://github.com/G1ANT-Robot/G1ANT.Addon.Ocr.AbbyyFineReader](https://github.com/G1ANT-Robot/G1ANT.Addon.Ocr.AbbyyFineReader)

**Example 1:**

```G1ANT
ie.open url ‴g1ant.com‴
window title ‴✱Internet✱‴ style ‴maximize‴
ocrabbyy.fromscreen area ‴170⫽250⫽1500⫽800‴ relative false language ‴English‴ result ♥text
dialog ♥text
```

The first part of the script
`ie.open url ‴g1ant.com‴
window title ‴✱Internet✱‴ style ‴maximize‴`
opens G1ANT website, then `window title ‴✱Internet✱‴ style ‴maximize‴` maximizes the window. It is necessary to make sure where the area we later want to extract text from is.
`window` command takes **title** argument with value of ✱Internet✱. It means, G1ANT.Robot will search for any phrase containing 'Internet' word in all opened windows.
The ctrl+W shortcut will show a list of all opened windows in a pop up dialog box.

The second part of the script
`ocrabbyy.fromscreen area ‴170⫽250⫽1500⫽800‴ relative false language ‴English‴ result ♥text
dialog ♥text` tells G1ANT.Robot to capture chosen area and extract text from it. Thanks to assigning captured text to **result** argument, we are  later able to dialog its value.

